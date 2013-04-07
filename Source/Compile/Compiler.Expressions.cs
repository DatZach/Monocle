using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monocle.Lexer;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private void Expression()
		{
			Term();

			while(IsAddOp())
			{
				Emit("push	eax");
				switch(stream.Read().Value)
				{
					case "+":
						stream.Accept(TokenType.Delimiter, "+");

						Term();
						
						Emit("pop	edx");
						Emit("add	eax, edx");
						break;

					case "-":
						Term();
						break;

					default:
						stream.Expected("add-op");
						break;
				}
			}
		}

		private void Term()
		{
			Factor();
		}

		private void Factor()
		{
			if (stream.Accept(TokenType.Word, "_asm"))
			{
				StringBuilder asmSource = new StringBuilder();
				stream.Expect(TokenType.Delimiter, "{");

				while (!stream.Accept(TokenType.Delimiter, "}"))
				{
					Token token = stream.Read();
					if (token.Line != stream.Peek().Line)
						asmSource.AppendLine(token.Value);
					else
					{
						asmSource.Append(token.Value);
						asmSource.Append(" ");
					}
				}

				Emit(asmSource.ToString());
			}
			else if (stream.Pass(TokenType.Number))
				Emit("mov	eax, {0}", stream.Read().Value);
			else if (stream.Pass(TokenType.String))
			{
				Emit("mov	eax, .lstr{0}", localStrings.Count);
				localStrings.Add(stream.Read().Value);
			}
		}
	}
}
