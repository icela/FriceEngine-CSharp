using System;
using JetBrains.Annotations;

namespace FriceEngine.Utils.Message
{
	public static class FLog
	{
		public static void Error([NotNull] string s) => Console.Error.WriteLine(s);
		public static void Warning([NotNull] string s) => Console.WriteLine(s);
		public static void Info([NotNull] string s) => Console.WriteLine(s);
		public static void Debug([NotNull] string s) => Console.WriteLine(s);
		public static void Verbose([NotNull] string s) => Console.WriteLine(s);

		public static void E([NotNull] string s) => Error(s);
		public static void W([NotNull] string s) => Warning(s);
		public static void I([NotNull] string s) => Info(s);
		public static void D([NotNull] string s) => Debug(s);
		public static void V([NotNull] string s) => Verbose(s);
	}
}