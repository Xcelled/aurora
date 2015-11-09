using System;
using System.Collections.Generic;
using System.Linq;
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

			var bylineWrite = method.Body.Instructions
			                        .First(i => i.OpCode == OpCodes.Ldstr && i.Operand.ToString().Contains("by the"));

			do
			{
				bylineWrite = bylineWrite.Next;
			} while (!bylineWrite.Operand.ToString().Contains("System.Console::Write"));

			var il = method.Body.GetILProcessor();
			var consoleWrite = typeof(Console).GetMethod("Write", new[] { typeof(string) });

			var setYellow = il.Create(OpCodes.Ldc_I4, (int)ConsoleColor.Yellow);
			var callSet = il.Create(OpCodes.Call, method.Module.Import(typeof(Console).GetMethod("set_ForegroundColor")));
			var ldStr = il.Create(OpCodes.Ldstr, "                            == RADIATION Edition ==                             ");

			il.InsertAfter(bylineWrite, il.Create(bylineWrite.OpCode, method.Module.Import(consoleWrite)));
			il.InsertAfter(bylineWrite, ldStr);
			il.InsertAfter(bylineWrite, callSet);
			il.InsertAfter(bylineWrite, setYellow);

			method.Body.OptimizeMacros();
		}
	}
}
