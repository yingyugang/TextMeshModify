using System.Collections;
using TMPro;
using UnityEngine;

namespace BlueNoah
{

	/// <summary>
	/// 直線のシャウトテキスト
	/// </summary>
	public class TextMeshStraightLine : TextMeshMotionBase, ITextMeshMotion
	{
		public float spiralUpSpeed;
		public float displayStartHeight = 1f;
		public float displayEndHeight = 10f;
		public float vanishStartHeight = 40f;
		public float vanishEndHeight = 50f;

		public override void StartMotion(string text, bool isLoop, System.Action onTerminal)
		{
			Reset();
			this.isLoop = isLoop;
			coroutine = StartCoroutine(StraightLine(text));
			colorCoroutine = StartCoroutine(AnimateVertexColors());
		}

		IEnumerator StraightLine(string text)
		{
			float height = 0;
			meshPro.text = text;
			meshPro.ForceMeshUpdate();
			//メッシュ情報をキャシュー
			cachedMeshInfo = textInfo.CopyMeshInfoVertexData();
			Color32 c0 = tmpText.color;
			float terminalTime = 0;
			float lifeCycle = 10 + text.Length * 0.1f;
			while (terminalTime < lifeCycle)
			{
				//文字をカメラに向いて
				/*
				transform.LookAt(-Camera.main.transform.position, transform.position.normalized);
				var wideScale = (Mathf.Abs(Camera.main.transform.localPosition.z) - EarthRadiu) * WideScaleFactor;
				wideScale = Mathf.Min(wideScale, MaxWideScale);
				transform.localScale = new Vector3(wideScale, wideScale, wideScale);*/
				height += MoveUpdateInterval * spiralUpSpeed;
				for (int i = 0; i < textInfo.characterCount; i++)
				{
					int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = textInfo.characterInfo[i].vertexIndex;
					Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;
					Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
					destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex] + new Vector3(0, (height), 0);
					destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] + new Vector3(0, (height), 0);
					destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] + new Vector3(0, (height), 0);
					destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] + new Vector3(0, (height ), 0);
					var newVertexColors = textInfo.meshInfo[materialIndex].colors32;
					//各キャラクターの頂点の色を頂点透明度を高さの元ついてスムーズに変更
					if (textInfo.characterInfo[i].isVisible)
					{
						float alpha = 1f;
						float characterHeight = Mathf.Max(destinationVertices[vertexIndex + 0].y, 0);
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
						c0 = new Color32(newVertexColors[vertexIndex].r, newVertexColors[vertexIndex].g, newVertexColors[vertexIndex].b, (byte)(alpha * 255));
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
				yield return new WaitForSeconds(MoveUpdateInterval);
				terminalTime += MoveUpdateInterval;
				if (terminalTime >= lifeCycle)
				{
					if (isLoop)
					{
						ResetVertics();
						height = 0;
						terminalTime = 0;
					}
				}
			}
			onTerminal?.Invoke();
			Destroy(gameObject);
		}
	}
}