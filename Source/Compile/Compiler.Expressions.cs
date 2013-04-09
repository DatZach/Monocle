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
						Term();
						
						Emit("pop	edx");
						Emit("add	eax, edx");
						break;

					case "-":
						Term();
						
						Emit("pop	edx");
						Emit("sub	eax, edx");
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

				bool spaceOpcode = true;
				while (!stream.Accept(TokenType.Delimiter, "}"))
				{
					Token token = stream.Read();
					if (token.Line != stream.Peek().Line)
					{
						asmSource.AppendLine(token.Value);
						spaceOpcode = true;
					}
					else
					{
						asmSource.Append(token.Value);
						if (spaceOpcode)
							asmSource.Append(" ");

						spaceOpcode = false;
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
