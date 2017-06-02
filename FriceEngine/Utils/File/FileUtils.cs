using System.IO;

namespace FriceEngine.Utils.File
{
	public static class FileUtils
	{
		public static void String2File(string path, string content)
		{
		    using (var s = new FileInfo(path).CreateText())
		    {
		        s.WriteLine(content);
            }
		}

		public static string File2String(string path)
		{
		    using (var s = new FileInfo(path).OpenText())
		    {
		        var ret = s.ReadToEnd();
		        return ret;
            }
		}
	}
}