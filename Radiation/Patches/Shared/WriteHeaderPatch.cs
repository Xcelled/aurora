using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

namespace Radiation.Patches.Shared
{
	internal class WriteHeaderPatch : Patch
	{
		public WriteHeaderPatch() : base("Write Header")
		{
		}

		public override void Apply(ModuleDefinition module)
		{
			var method = module.GetMethod("System.Void Aura.Shared.Util.CliUtil::WriteHeader(System.String,System.ConsoleColor)");

			method.Body.SimplifyMacros();

			var bylineWrite = method.Body.Instructions.First(i => i.OpCode == OpCodes.Ldstr && i.Operand.ToString().Contains("by the"));

			do
			{
				bylineWrite = bylineWrite.Next;
			} while (!bylineWrite.Operand.ToString().Contains("System.Console::Write"));

			var il = method.Body.GetILProcessor();

			il.InsertAfter(bylineWrite,
				il.Create(OpCodes.Ldc_I4, (int)ConsoleColor.Yellow),
				il.Create(OpCodes.Call, module.Resolve(typeof(Console), "set_ForegroundColor")),
				il.Create(OpCodes.Ldstr, "                            == RADIATION Edition ==                             "),
				il.Create(OpCodes.Call, module.Resolve(typeof(Console), "Write", typeof(string)))
			);

			method.Body.OptimizeMacros();
		}
	}
}
