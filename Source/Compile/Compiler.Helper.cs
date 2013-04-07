using Monocle.Lexer;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private bool IsAddOp()
		{
			Token token = stream.Peek();
			return token.Type == TokenType.Delimiter && (token.Value == "+" || token.Value == "-");
		}
	}
}
