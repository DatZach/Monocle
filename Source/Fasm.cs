using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Monocle
{
	class Fasm
	{
		private readonly string cachePath;
		private readonly List<string> filenames; 

		public Fasm()
		{
			filenames = new List<string>();

			// Get a new temp directory
			cachePath = CreateTemporaryPath();

			// Set path so that FASM will work
			Environment.SetEnvironmentVariable("PATH", Directory.GetCurrentDirectory());
		}

		public void Add(string value)
		{
			string filename = cachePath + "\\" + Path.GetRandomFileName().Split(new[] { '.' }).First() + ".asm";
			filenames.Add(filename);

			File.WriteAllText(filename, value);
		}

		public void Assemble(string filename)
		{
			Process process = new Process
			{
				StartInfo =
				{
					UseShellExecute = false,
					RedirectStandardOutput = true,
					FileName = "fasm.exe",
					Arguments = filenames.Aggregate((s, n) => s + " " + n) + " " + filename
				}
			};

			Console.WriteLine("fasm.exe {0}", process.StartInfo.Arguments);

			process.Start();

			string result = process.StandardOutput.ReadToEnd();

			process.WaitForExit();

			if (result.Select((c, i) => result.Substring(i)).Count(sub => sub.StartsWith("error")) != 0)
				throw new Exception(result);
		}

		private string CreateTemporaryPath()
		{
			string tempDirectory = Path.GetTempPath();
			string tempExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
			string path = Path.Combine(tempDirectory, tempExtension ?? "fasm");

			Directory.CreateDirectory(path);

			return path;
		}
	}
}
