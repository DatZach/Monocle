using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private void Program()
		{
			// Emit program header
			Emit("format PE");
			Emit("entry EntryPoint");
			Emit("include 'win32a.inc'");
		}
	}
}
