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
			Emit("mov	eax, {0}", stream.Read().Value);
		}
	}
}
