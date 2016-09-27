namespace FriceEngine.Utils.Misc
{
	/// <summary>
	/// TF 是 Type first,别想成某青少年明星组合了
	/// 我不是四叶草		——ice1000
	/// </summary>
	/// <typeparam name="TF">第一个参数的类型</typeparam>
	/// <typeparam name="TS">第二个参数的类型</typeparam>
	public class Pair<TF, TS>
	{
		public TF First { get; set; }
		public TS Second { get; set; }

		public Pair(TF first, TS second)
		{
			Second = second;
			First = first;
		}
	}
}
