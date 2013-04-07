using System;

namespace Monocle
{
	class Program
	{
		public static void Main(string[] args)
		{
			Fasm fasm = new Fasm();

			fasm.Add(
				";\n" +
				";      console.asm\n" +
				";      Write \"Hello, World!\" to console from assembly\n" +
				";\n" +
				"\n" +
				"format PE\n" +
				"entry EntryPoint\n" +
				"\n" +
				"include 'win32a.inc'\n" +
				"\n" +
				"StandardCallTest:\n" +
				"       push    ebp\n" +
				"       mov             ebp, esp\n" +
				"       \n" +
				"       invoke  GetStdHandle, 0xFFFFFFF5\n" +
				"       invoke  WriteConsole, eax, [ebp + 8], [ebp + 12], [charsWritten], 0\n" +
				"       \n" +
				"       pop             ebp\n" +
				"       ret             8\n" +
				"\n" +
				"EntryPoint:\n" +
				"       push    17\n" +
				"       push    message\n" +
				"       call    StandardCallTest\n" +
				"       \n" +
				"       invoke  ExitProcess, 0\n" +
				"\n" +
				"message        db      'Hello, World!', 10, 13, 0\n" +
				"charsWritten dd ?\n" +
				"       \n" +
				"data import\n" +
				"       library kernel, 'KERNEL32.DLL',\\\n" +
				"                       user, 'USER32.DLL'\n" +
				"       \n" +
				"       import kernel,\\\n" +
				"               GetStdHandle, 'GetStdHandle',\\\n" +
				"               WriteConsole, 'WriteConsoleA',\\\n" +
				"               ExitProcess, 'ExitProcess'\n" +
				"       \n" +
				"       import user,\\\n" +
				"               MessageBox, 'MessageBoxA'\n" +
				"end data\n"
			);

			try
			{
				fasm.Assemble("testasm.exe");
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message);
			}

			Console.ReadKey();
		}
	}
}
