using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BlueNoah
{
    [RequireComponent(typeof(TextMeshPro))]
    public abstract class TextMeshBase : MonoBehaviour, ITextMeshMotion
    {
        protected TextMeshPro meshPro;
        protected TMP_Text tmpText;
        protected TMP_TextInfo textInfo;
        protected TMP_MeshInfo[] cachedMeshInfo;
        protected Coroutine coroutine;

        private void Awake()
        {
            meshPro = GetComponent<TextMeshPro>();
            tmpText = GetComponent<TMP_Text>();
            textInfo = tmpText.textInfo;
        }

        protected void Reset()
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
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

        public abstract void StartMotion();
    }
}