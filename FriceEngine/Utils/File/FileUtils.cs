using System.IO;
using JetBrains.Annotations;

namespace FriceEngine.Utils.File
{
	public static class FileUtils
	{
		public static void String2File(
			[NotNull] string path,
			[NotNull] string content)
		{
			using (var s = new FileInfo(path).CreateText())
				s.WriteLine(content);
		}

		[NotNull]
		public static string File2String([NotNull] string path)
		{
			using (var s = new FileInfo(path).OpenText())
			{
				var ret = s.ReadToEnd();
				return ret;
			}
		}
	}
}