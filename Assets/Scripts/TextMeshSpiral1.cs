using TMPro;
using UnityEngine;
namespace BlueNoah
{

    public class TextMeshSpiral1 : TextMeshBase
    {
        public Vector3 offset = Vector3.up;
        public float radius = 20;
        public float maxRadius = 1000;
        public float ratateSpeed = 200;
        public float spiralUpSpeed = 0.2f;
        public float spiralCharacterInterval =20;
        public float spiralRadiuRadio = 0.5f;
        public string text = "シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装シャウト画面機能実装";

        public override void StartMotion()
        {
            Reset();
            coroutine = StartCoroutine(Spiral());
        }

        System.Collections.IEnumerator Spiral()
        {
            float height = 0;
            meshPro.text = text;
            yield return null;
            cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
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
                    var myVector = Quaternion.Euler(0, spiralCharacterInterval * i + Time.time * ratateSpeed / Mathf.Max(radius, 1), 0) * Vector3.forward;
                    if (height - i > 0)
                    {
                        destinationVertices[vertexIndex + 0] = myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                        destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
                    }
                }
                // Push changes into meshes
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
                yield return null;
            }
        }
    }
}