using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private void Emit(string value)
		{
			assemblyStream.AppendLine(value);
		}
	}
}
