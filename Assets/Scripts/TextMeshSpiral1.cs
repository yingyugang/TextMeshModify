using System.Collections;
using TMPro;
using UnityEngine;
namespace BlueNoah
{
    public class TextMeshSpiral1 : TextMeshMotionBase
    {
        public Vector3 offset = Vector3.up;
        public float radius = 20;
        public float maxRadius = 1000;
        public float rotateSpeed = 200;
        public float spiralUpSpeed = 0.2f;
        public float spiralCharacterInterval =20;
        public float spiralRadiuRadio = 0.5f;
        public string text = "シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装";
        public float displayStartHeight = 1f;
        public float displayEndHeight = 10f;
        public float vanishStartHeight = 40f;
        public float vanishEndHeight = 50f;
        public override void StartMotion(string text,bool isLoop, System.Action onTerminal)
        {
            Reset();
            coroutine = StartCoroutine(Spiral(text));
            colorCoroutine = StartCoroutine(AnimateVertexColors());
        }

        IEnumerator AnimateVertexColors()
        {
            // Force the text object to update right away so we can have geometry to modify right from the start.
            TMP_TextInfo textInfo = tmpText.textInfo;
            int currentCharacter = 0;
            Color32[] newVertexColors;
            Color32 c0 = tmpText.color;
            while (true)
            {
                int characterCount = textInfo.characterCount;
                if (characterCount == 0)
                {
                    yield return new WaitForSeconds(0.25f);
                    continue;
                }
                int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;
                newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;
                if (textInfo.characterInfo[currentCharacter].isVisible)
                {
                    c0 = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), newVertexColors[vertexIndex + 0].a);
                    newVertexColors[vertexIndex + 0] = c0;
                    newVertexColors[vertexIndex + 1] = c0;
                    newVertexColors[vertexIndex + 2] = c0;
                    newVertexColors[vertexIndex + 3] = c0;
                }
                currentCharacter = (currentCharacter + 1) % characterCount;
                tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return new WaitForSeconds(0.02f);
                //yield return new WaitForSeconds(0.05f);
            }
        }

        IEnumerator Spiral(string text)
        {
            float height = 0;
            meshPro.text = text;
            meshPro.ForceMeshUpdate();
            cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
            Color32 c0 = tmpText.color;
            while (true)
            {
                for (int i = 0; i < textInfo.characterCount; i++)
                {
                    // Get the index of the material used by the current character.
                    int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                    // Get the index of the first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                    // Get the cached vertices of the mesh used by this text element (character or sprite).
                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
                    height += Time.deltaTime * spiralUpSpeed;
                    var radius = Mathf.Clamp((height - i) * spiralRadiuRadio, 0, maxRadius);
                    var myVector = Quaternion.Euler(0, spiralCharacterInterval * i + Time.time *  rotateSpeed / Mathf.Max(radius, 1), 0) * Vector3.forward;
                    if (height - i > 0)
                    {
                        destinationVertices[vertexIndex + 0] = myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                    }
                    // Get the vertex colors of the mesh used by this text element (character or sprite).
                    var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                    // Only change the vertex color if the text element is visible.
                    if (textInfo.characterInfo[i].isVisible)
                    {
                        float alpha = 1f;
                        float characterHeight = Mathf.Max( height - i ,0);
                        if (characterHeight <= displayStartHeight)
                        {
                            alpha = 0;
                        }
                        else if (characterHeight > displayStartHeight && characterHeight < displayEndHeight)
                        {
                            alpha = (characterHeight - displayStartHeight) / (displayEndHeight - displayStartHeight);
                        }
                        else if (characterHeight > vanishStartHeight && characterHeight < vanishEndHeight)
                        {
                            alpha = (vanishEndHeight - characterHeight) / (vanishEndHeight - vanishStartHeight);
                        }
                        else if (characterHeight > vanishEndHeight)
                        {
                            alpha = 0;
                        }
                        c0 = new Color32(newVertexColors[vertexIndex + 0].r, newVertexColors[vertexIndex + 0].g, newVertexColors[vertexIndex + 0].b, (byte)(alpha * 255));
                        newVertexColors[vertexIndex + 0] = c0;
                        newVertexColors[vertexIndex + 1] = c0;
                        newVertexColors[vertexIndex + 2] = c0;
                        newVertexColors[vertexIndex + 3] = c0;
                    }
                }
                // Push changes into meshes
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
                tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                yield return null;
            }
        }
    }
}