using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace BlueNoah
{
    [RequireComponent(typeof(TextMeshPro))]
    public class TextMeshModify : MonoBehaviour
    {
        TMP_Text tmpText;
        TMP_TextInfo textInfo;
        TMP_MeshInfo[] cachedMeshInfo;
        public Vector3 offset = Vector3.up;
        public float radius = 20;
        public float maxRadius = 20;
        Coroutine coroutine;
        public float ratateSpeed = 10;

        public float spiralUpSpeed = 1;
        public float spiralCharacterInterval = 10;
        public float spiralRadiuRadio = 1;
        private void Awake()
        {
            tmpText = GetComponent<TMP_Text>();
            textInfo = tmpText.textInfo;
        }

        public void StartCircle()
        {
            if (coroutine!=null)
            {
                StopCoroutine(coroutine);
                Reset();
            }
            coroutine = StartCoroutine(Circle());
        }

        public void StartCircle1()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                Reset();
            }
            coroutine = StartCoroutine(Circle1());
        }

        public void StartSpiral()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                Reset();
            }
            coroutine = StartCoroutine(Spiral());
        }

        public void StartSpiral1()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                Reset();
            }
            coroutine = StartCoroutine(Spiral());
        }

        private void Reset()
        {
            if (cachedMeshInfo != null)
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
                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex];
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1];
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2];
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3];
                }
                // Push changes into meshes
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }
            }
        }

        System.Collections.IEnumerator Circle()
        {
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
                    var myVector = Quaternion.Euler(0, 10 * i + Time.time * ratateSpeed, 0) * Vector3.forward;
                    destinationVertices[vertexIndex + 0] = myVector * radius;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - sourceVertices[vertexIndex] + myVector * radius;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - sourceVertices[vertexIndex] + myVector * radius;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - sourceVertices[vertexIndex] + myVector * radius;
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

        System.Collections.IEnumerator Circle1()
        {
            float height = 0;
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
                    var myVector = Quaternion.Euler(0, spiralCharacterInterval * i + Time.time * ratateSpeed, 0) * Vector3.forward;
                    height += Time.deltaTime * spiralUpSpeed;
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

        System.Collections.IEnumerator Spiral()
        {
            float height = 0;
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
                    var myVector = Quaternion.Euler(0, spiralCharacterInterval * i + Time.time * ratateSpeed / Mathf.Max(radius,1), 0) * Vector3.forward;
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