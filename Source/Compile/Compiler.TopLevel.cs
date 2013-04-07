using Monocle.Lexer;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private void Program()
		{
			stream.Expect(TokenType.Word, "program");
			string entryPointName = stream.GetWord();
			stream.Expect(TokenType.Delimiter, ";");

			EmitHeader(entryPointName);

			do
			{
				Function();
			} while (!stream.EndOfStream);

			EmitFooter();
		}

		private void Function()
		{
			stream.Expect(TokenType.Word, "function");
			
			// TODO Ignore type for now
			stream.Read();

			string functionName = stream.GetWord();
			Emit("{0}:", functionName);

			stream.Expect(TokenType.Delimiter, "(");
			// TODO Ignore variables
			stream.Expect(TokenType.Delimiter, ")");

			Block();
		}

		private void Block()
		{
			stream.Expect(TokenType.Delimiter, "{");
			Emit("push	ebp");
			Emit("mov	ebp, esp");

			while(!stream.Accept(TokenType.Delimiter, "}"))
			{
				Expression();
				stream.Expect(TokenType.Delimiter, ";");
			}

			Emit("pop	ebp");
			Emit("ret");
		}

		private void EmitHeader(string entryPointName)
		{
			// Emit program header
			Emit("format PE");
			Emit("entry __EntryPoint");
			Emit("include 'win32a.inc'");

			// Emit entry stub
			Emit("__EntryPoint:");
			Emit("call	{0}", entryPointName);
			Emit("invoke	ExitProcess, 0");
			Emit("ret");
		}

		private void EmitFooter()
		{
			Emit("data import");
			Emit("	library kernel, 'KERNEL32.DLL'");
			Emit("	import kernel,\\");
			Emit("		ExitProcess, 'ExitProcess'");
			Emit("end data");
		}
	}
}
