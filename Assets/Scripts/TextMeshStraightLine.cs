using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace BlueNoah
{
    public class TextMeshStraightLine : TextMeshMotionBase, ITextMeshMotion
    {
		public float spiralUpSpeed = 40;
		public float displayStartHeight = 1f;
		public float displayEndHeight = 10f;
		public float vanishStartHeight = 40f;
		public float vanishEndHeight = 50f;

		public override void StartMotion(string text)
		{
			Reset();
			coroutine = StartCoroutine(Spiral(text));
			//SpiralAsync(text);
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
			var angleWithCamera = Vector3.Angle(Camera.main.transform.forward, transform.forward);
			meshPro.text = text;
			meshPro.ForceMeshUpdate();
			//��å������򥭥㥷��`
			cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			Color32 c0 = tmpText.color;
			while (true)
			{
				//var wideScale = ((Mathf.Abs(Camera.main.transform.localPosition.z) - 637) / 2) * 0.04f;
				//wideScale = Mathf.Min(wideScale, MinWideScale);
				//transform.localScale = new Vector3(wideScale, wideScale, wideScale);
				height += Time.deltaTime * spiralUpSpeed;
				for (int i = 0; i < textInfo.characterCount; i++)
				{
					int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[i].vertexIndex;
					Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
					Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
					if (height - i > 0)
					{
						destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex] + new Vector3(0, (height - i) , 0);
						destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] +  new Vector3(0, (height - i), 0);
						destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + new Vector3(0, (height - i), 0);
						destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + new Vector3(0, (height - i), 0);
					}
					var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
					//������饯���`��픵��ɫ��픵�͸���Ȥ�ߤ���Ԫ�Ĥ��ƥ���`���ˉ��
					if (textInfo.characterInfo[i].isVisible)
					{
						float alpha = 1f;
						float characterHeight = Mathf.Max(destinationVertices[vertexIndex + 0].y, 0);
						//�ߤ���һ����δ����һ���Ϥ򳬤������ϡ�
						if (characterHeight <= displayStartHeight || characterHeight > vanishEndHeight)
						{
							alpha = 0;
						}
						//displayStartHeight����displayEndHeight�졩�˱�ʾ
						else if (characterHeight > displayStartHeight && characterHeight < displayEndHeight)
						{
							alpha = (characterHeight - displayStartHeight) / (displayEndHeight - displayStartHeight);
						}
						//vanishStartHeight����vanishEndHeight�졩�˷Ǳ�ʾ
						else if (characterHeight > vanishStartHeight && characterHeight < vanishEndHeight)
						{
							alpha = (vanishEndHeight - characterHeight) / (vanishEndHeight - vanishStartHeight);
						}
						c0 = new Color32(newVertexColors[vertexIndex + 0].r, newVertexColors[vertexIndex + 0].g, newVertexColors[vertexIndex + 0].b, (byte)(alpha * 255));
						newVertexColors[vertexIndex + 0] = c0;
						newVertexColors[vertexIndex + 1] = c0;
						newVertexColors[vertexIndex + 2] = c0;
						newVertexColors[vertexIndex + 3] = c0;
					}
				}
				//���Τ���픵��TextMeshPro�����
				for (int i = 0; i < textInfo.meshInfo.Length; i++)
				{
					textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
					tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
				}
				//TextMeshPro��ɫ����
				tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				yield return new WaitForSeconds(0.033f);
			}
		}
	}
}
