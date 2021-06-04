namespace BlueNoah
{
	/// <summary>
	/// テキストを変形
	/// </summary>
	public interface ITextMeshMotion
	{
		void StartMotion(string text, bool isLoop, System.Action onTerminal);
	}
}
