using System;

namespace FriceEngine.Utils.Message
{
	public sealed class FLog
	{
		public static void Error(string s) => Console.WriteLine(s);
		public static void Warning(string s) => Console.WriteLine(s);
		public static void Info(string s) => Console.WriteLine(s);
		public static void Debug(string s) => Console.WriteLine(s);
		public static void Verbose(string s) => Console.WriteLine(s);

		public static void E(string s) => Error(s);
		public static void W(string s) => Warning(s);
		public static void I(string s) => Info(s);
		public static void D(string s) => Debug(s);
		public static void V(string s) => Verbose(s);
	}
}
