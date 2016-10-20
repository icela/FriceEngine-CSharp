using System.IO;

namespace FriceEngine.Utils.File
{
	public static class FileUtils
	{
		public static void String2File(string path, string content)
		{
			var s = new FileInfo(path).CreateText();
			s.WriteLine(content);
			s.Close();
		}

		public static string File2String(string path)
		{
			var s = new FileInfo(path).OpenText();
			var ret = s.ReadToEnd();
			s.Close();
			return ret;
		}
	}
}
