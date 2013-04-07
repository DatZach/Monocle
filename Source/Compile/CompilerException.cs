using System;

namespace Monocle.Compile
{
	class CompilerException : Exception
	{
		public CompilerException()
		{

		}

		public CompilerException(string message)
			: base(message)
		{

		}
	}
}
