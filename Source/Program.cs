using System;
using Monocle.Compile;
using Monocle.Lexer;

namespace Monocle
{
	class Program
	{
		public static void Main(string[] args)
		{
			Compiler compiler = new Compiler();
			compiler.Compile(new TokenStream(Tokenizer.ParseFile(args[0])), args[1]);

			Console.ReadKey();
		}
	}
}
