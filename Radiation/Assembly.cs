﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Radiation
{
	internal abstract class Assembly
	{
		public List<Patch> Patches { get; }
		public string Name { get; }

		protected ModuleDefinition Module;

		protected Assembly(string name)
		{
			Name = name;
			Patches = new List<Patch>();
		}

		public void Load(string path)
		{
			Module = ModuleDefinition.ReadModule(path);
		}

		public void Patch()
		{
			foreach (var patch in  Patches)
			{
				Console.Write($"\tApplying {patch.Name} ...");
				patch.Apply(Module);
				Console.WriteLine($"\r\tApplying {patch.Name} ... done");
			}
		}

		public void Save(string path)
		{
			Module.Write(path);
		}
	}
}