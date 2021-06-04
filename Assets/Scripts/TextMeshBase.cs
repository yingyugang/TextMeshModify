using System.Collections;
using TMPro;
using UnityEngine;

namespace BlueNoah
{
    /// <summary>
    /// テキストを変形
    /// </summary>
    [RequireComponent(typeof(TextMeshPro))]
    public abstract class TextMeshMotionBase : MonoBehaviour, ITextMeshMotion
    {
        protected TextMeshPro meshPro;
        protected TMP_Text tmpText;
        protected TMP_TextInfo textInfo;
        protected TMP_MeshInfo[] cachedMeshInfo;
        protected Coroutine coroutine;
        protected Coroutine colorCoroutine;

        protected const float MaxWideScale = 10f;
        protected const float EarthRadiu = 637;
        protected const float WideScaleFactor = 0.02f;
        protected const float MoveUpdateInterval = 0.033f;
        protected const float ColorUpdateInterval = 0.033f;
        protected System.Action onTerminal { get; set; }

        public bool isLoop { get; set; }

        public bool isTerminal { get; private set; }

        private void Awake()
        {
            meshPro = GetComponent<TextMeshPro>();
            tmpText = GetComponent<TMP_Text>();
            textInfo = tmpText.textInfo;
        }
        /// <summary>
        /// メッシューの色の変更
        /// </summary>
        protected IEnumerator AnimateVertexColors()
        {
            TMP_TextInfo textInfo = tmpText.textInfo;
            int currentCharacter = 0;
            Color32[] newVertexColors;
            Color32 c0 = tmpText.color;
            int characterCount = textInfo.characterCount;
            if (characterCount == 0)
            {
                yield return new WaitForSeconds(ColorUpdateInterval);
            }
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                if (textInfo.characterInfo[i].isVisible)
                {
                    c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), newVertexColors[vertexIndex].a);
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;
                }
            }
            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            while (true)
            {
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), newVertexColors[vertexIndex].a);
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;
                }
                currentCharacter = (currentCharacter + 1) % characterCount;
                tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(ColorUpdateInterval);
            }
        }
        /// <summary>
        /// テキストをリセット
        /// </summary>
        protected void Reset()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            if (colorCoroutine != null)
            {
                StopCoroutine(colorCoroutine);
            }
            if (cachedMeshInfo != null)
            {
                ResetVertics();
                ResetColors();
            }
        }

        protected void ResetVertics()
        {
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex];
                destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1];
                destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2];
                destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3];
            }
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }

        protected void ResetColors()
        {
            Color32 c0 = tmpText.color;
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                if (textInfo.characterInfo[i].isVisible)
                {
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;
                }
            }
            tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }

        /// <inheritdoc/>
        public abstract void StartMotion(string text, bool isLoop, System.Action onTerminal);
    }
}