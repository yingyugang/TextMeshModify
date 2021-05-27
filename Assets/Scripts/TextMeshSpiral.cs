using TMPro;
using UnityEngine;
namespace BlueNoah
{
	/// <summary>
	/// テキストを螺旋に変形
	/// 参考：
	/// TMPro.Examples.VertexJitter.cs
	/// TMPro.Examples.VertexColorCycler.cs
	/// </summary>
	public class TextMeshSpiral : TextMeshMotionBase
	{
		public Vector3 offset = Vector3.up;
		public float maxRadius = 1000;
		public float rotateSpeed = 200;
		public float spiralUpSpeed = 0.2f;
		public float spiralCharacterInterval = 20;
		public float spiralRadiuRadio = 0.5f;
		public float displayStartHeight = 1f;
		public float displayEndHeight = 10f;
		public float vanishStartHeight = 40f;
		public float vanishEndHeight = 50f;
		const float MinWideScale = 2.3f;
		/// <inheritdoc/>
		public override void StartMotion(string text)
		{
			Reset();
			coroutine = StartCoroutine(Spiral(text));
		}

		System.Collections.IEnumerator Spiral(string text)
		{
			float height = 0;
			var angleWithCamera = Vector3.Angle(Camera.main.transform.forward, transform.forward);  
			meshPro.text = text;
			yield return null;
			//メッシュ情報をキャシュー
			cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			Color32 c0 = tmpText.color;
			while (true)
			{
				//var wideScale = ((Mathf.Abs(Camera.main.transform.localPosition.z) - 637) / 2) * 0.04f;
				//wideScale = Mathf.Min(wideScale, MinWideScale);
				//transform.localScale = new Vector3(wideScale, wideScale, wideScale);
				for (int i = 0; i < textInfo.characterCount; i++)
				{
					//各キャラクターの頂点のポジションを螺旋の形に変更。（基本をテストしながら、パラメータを調整する）
					int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[i].vertexIndex;
					Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
					Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
					height += Time.deltaTime * spiralUpSpeed;
					var radius = Mathf.Clamp((height - i) * spiralRadiuRadio, 0, maxRadius);
					var myVector = Quaternion.Euler(0, spiralCharacterInterval * i + Time.time * rotateSpeed, 0) * Vector3.forward;
					if (height - i > 0)
					{
						destinationVertices[vertexIndex + 0] = myVector * radius + new Vector3(0, height - i, 0);
						destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
						destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
						destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - sourceVertices[vertexIndex] + myVector * radius + new Vector3(0, height - i, 0);
						var axis = (destinationVertices[vertexIndex + 3] - destinationVertices[vertexIndex + 0]).normalized;
						destinationVertices[vertexIndex + 2] = Quaternion.AngleAxis(angleWithCamera, axis) * (destinationVertices[vertexIndex + 2] - destinationVertices[vertexIndex + 0]) + destinationVertices[vertexIndex + 0];
						destinationVertices[vertexIndex + 1] = Quaternion.AngleAxis(angleWithCamera, axis) * (destinationVertices[vertexIndex + 1] - destinationVertices[vertexIndex + 0]) + destinationVertices[vertexIndex + 0];
					}
					var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
					//各キャラクターの頂点の色を頂点透明度を高さの元ついてスムーズに変更
					if (textInfo.characterInfo[i].isVisible)
					{
						float alpha = 1f;
						float characterHeight = Mathf.Max(height - i, 0);
						//高さが一番下未満と一番上を超えた場合。
						if (characterHeight <= displayStartHeight || characterHeight > vanishEndHeight)
						{
							alpha = 0;
						}
						//displayStartHeightからdisplayEndHeight徐々に表示
						else if (characterHeight > displayStartHeight && characterHeight < displayEndHeight)
						{
							alpha = (characterHeight - displayStartHeight) / (displayEndHeight - displayStartHeight);
						}
						//vanishStartHeightからvanishEndHeight徐々に非表示
						else if (characterHeight > vanishStartHeight && characterHeight < vanishEndHeight)
						{
							alpha = (vanishEndHeight - characterHeight) / (vanishEndHeight - vanishStartHeight);
						}
						c0 = new Color32(c0.r, c0.g, c0.b, (byte)(alpha * 255));
						newVertexColors[vertexIndex + 0] = c0;
						newVertexColors[vertexIndex + 1] = c0;
						newVertexColors[vertexIndex + 2] = c0;
						newVertexColors[vertexIndex + 3] = c0;
					}
				}
				//変形した頂点をTextMeshProに入れ
				for (int i = 0; i < textInfo.meshInfo.Length; i++)
				{
					textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
					tmpText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
				}
				//TextMeshProの色更新
				tmpText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				yield return null;
			}
		}
	}
}