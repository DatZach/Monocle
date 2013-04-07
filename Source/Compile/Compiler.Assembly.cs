using System;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private void Emit(string value, params object[] parameters)
		{
			assemblyStream.AppendLine(String.Format(value, parameters));
		}
	}
}
