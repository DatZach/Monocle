﻿using System;
using System.Collections.Generic;
using System.Text;
using Monocle.Lexer;

namespace Monocle.Compile
{
	partial class Compiler
	{
		private readonly StringBuilder assemblyStream;
		private List<string> localStrings;
		private TokenStream stream;

		public Compiler()
		{
			assemblyStream = new StringBuilder();
			localStrings = null;
			stream = null;
		}

		public bool Compile(TokenStream tokenStream, string filename)
		{
			Fasm fasm = new Fasm();

			stream = tokenStream;

			try
			{
				Program();
				fasm.Add(assemblyStream.ToString());
			}
			catch (Exception e)
			{
				Console.WriteLine("Compile Error: {0}", e.Message);
				return false;
			}

			try
			{
				fasm.Assemble(filename);
			}
			catch (Exception e)
			{
				Console.WriteLine("Assembler Error: {0}", e.Message);
				return false;
			}

			Console.Write(assemblyStream.ToString());

			return true;
		}
	}
}
