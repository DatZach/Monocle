using System;
using System.Collections.Generic;
using System.Linq;
using Monocle.Compile;

namespace Monocle.Lexer
{
	class TokenStream
	{
		private readonly Stack<int> positionStack;
		private readonly List<Token> tokens;
		public int Position;

		public bool EndOfStream
		{
			get { return Position >= tokens.Count || tokens[Position].Type == TokenType.EndOfStream; }
		}

		public string Filename
		{
			get
			{
				return tokens.First() == null ? "" : tokens.First().Filename;
			}
		}

		public uint CurrentLine
		{
			get
			{
				return Peek() == null ? tokens.Last().Line : Peek().Line;
			}
		}

		public TokenStream(List<Token> tokens)
		{
			positionStack = new Stack<int>();
			this.tokens = tokens;
			Position = 0;
		}

		public Token Peek()
		{
			return EndOfStream ? new Token(TokenType.EndOfStream, tokens.First().Filename, 0) : tokens[Position];
		}

		public Token PeekAhead(int offset)
		{
			return Position + offset < tokens.Count ? tokens[Position + offset] : null;
		}

		public Token Read()
		{
			return EndOfStream ? null : tokens[Position++];
		}

		public void Expect(TokenType type)
		{
			if (!EndOfStream && Read().Type == type)
				return;

			Expected(type.ToString());
		}

		public void Expect(TokenType type, string value)
		{
			if (!EndOfStream)
			{
				Token token = Read();
				if (token.Type == type && token.Value == value)
					return;
			}

			Expected(value);
		}

		public bool Accept(TokenType type)
		{
			if (EndOfStream)
				return false;

			Token token = Peek();
			if (token.Type == type)
			{
				Read();
				return true;
			}

			return false;
		}

		public bool Accept(TokenType type, string value)
		{
			if (EndOfStream)
				return false;

			Token token = Peek();
			if (token.Type == type && token.Value == value)
			{
				Read();
				return true;
			}

			return false;
		}

		public bool Pass(TokenType type)
		{
			return Peek().Type == type;
		}

		public bool Pass(TokenType type, string value)
		{
			return Peek().Type == type && Peek().Value == value;
		}

		public bool Pass(string value)
		{
			return Peek().Type == TokenType.Word && Peek().Value == value;
		}

		public string GetWord()
		{
			Token token = Read();
			if (token.Type != TokenType.Word)
			{
				Expected("identifier");
				return "";
			}

			return token.Value;
		}

		public void PushPosition()
		{
			positionStack.Push(Position);
		}

		public void PopPosition()
		{
			Position = positionStack.Pop();
		}

		// TODO This really shouldn't be here...
		public void Error(string message, params object[] args)
		{
			string parsedError = String.Format(message, args);
			string errorMessage = String.Format("Error on line {0}: \n\t", CurrentLine);
			errorMessage += parsedError.Replace("\n", "\n\t");

			throw new CompilerException(errorMessage);
		}

		public void Expected(string value)
		{
			Error("Expected \"{0}\" got \"{1}\" instead.", value, Peek().Value);
		}
	}
}
