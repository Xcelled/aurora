using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Pdb;

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
			Console.WriteLine($"Loading {Name} from {path}");

			var readerParameters = new ReaderParameters();

			if (File.Exists(Path.ChangeExtension(path, "pdb")))
			{
				readerParameters.SymbolReaderProvider = new PdbReaderProvider();
				readerParameters.ReadSymbols = true;
			}

			Module = ModuleDefinition.ReadModule(path, readerParameters);

			Console.WriteLine($"{Name} loaded");
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
			Console.WriteLine($"Saving {Name} to {path}");

			var writerParameters = new WriterParameters();

			if (File.Exists(Path.ChangeExtension(path, "pdb")))
			{
				writerParameters.WriteSymbols = true;
			}

			Module.Write(path, writerParameters);

			Console.WriteLine($"{Name} saved");
		}
	}
}
