using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace System.Web.Mvc
{
	public class ZipStreamResult : FileResult
	{
		private const int _bufferSize = 4096;
		private static readonly string ContextType = "application/x-zip-compressed";
		private static readonly string FileExtension = ".zip";
		private string[] files;
		public ZipStreamResult(string file, string fileDownloadName) : this(new string[]
		{
			file
		}, fileDownloadName)
		{
		}
		public ZipStreamResult(string file) : this(file, "")
		{
		}
		public ZipStreamResult(System.Collections.Generic.IEnumerable<string> files, string fileDownloadName) : base(ZipStreamResult.ContextType)
		{
			this.files = files.ToArray<string>();
			string name = fileDownloadName ?? "";
			if (name.Length == 0 && files.Count<string>() == 1)
			{
				name = System.IO.Path.GetFileNameWithoutExtension(files.First<string>());
			}
			if (!name.ToLower().EndsWith(ZipStreamResult.FileExtension))
			{
				name += ZipStreamResult.FileExtension;
			}
			base.FileDownloadName = HttpUtility.UrlEncode(name);
		}
		protected override void WriteFile(System.Web.HttpResponseBase response)
		{
			ZipOutputStream outputStream = new ZipOutputStream(response.OutputStream);
			outputStream.IsStreamOwner = false;
			byte[] buffer = new byte[4096];
			string[] array = this.files;
			for (int i = 0; i < array.Length; i++)
			{
				string file = array[i];
				using (System.IO.FileStream stream = new System.IO.FileStream(file, System.IO.FileMode.Open))
				{
					ZipEntry zipEntry = new ZipEntry(System.IO.Path.GetFileName(file));
					outputStream.PutNextEntry(zipEntry);
					while (true)
					{
						int count = stream.Read(buffer, 0, 4096);
						if (count == 0)
						{
							break;
						}
						outputStream.Write(buffer, 0, count);
					}
					outputStream.CloseEntry();
				}
			}
			outputStream.Close();
		}
	}
}
