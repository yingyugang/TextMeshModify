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

        private void Awake()
        {
            meshPro = GetComponent<TextMeshPro>();
            tmpText = GetComponent<TMP_Text>();
            textInfo = tmpText.textInfo;
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
                Color32 c0 = tmpText.color;
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
                    var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                    if (textInfo.characterInfo[i].isVisible)
                    {
                        newVertexColors[vertexIndex + 0] = c0;
                        newVertexColors[vertexIndex + 1] = c0;
                        newVertexColors[vertexIndex + 2] = c0;
                        newVertexColors[vertexIndex + 3] = c0;
                    }
                }
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
                tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }
        }
        /// <inheritdoc/>
        public abstract void StartMotion(string text);
    }
}