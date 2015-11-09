using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Radiation
{
	internal static class CecilHelpers
	{
		public static TypeDefinition GetTypeDef(this ModuleDefinition module, string name)
		{
			var def = module.Types.FirstOrDefault(t => t.FullName == name);
			if (def == null)
				throw new Exception($"Can't find type {name}");

			return def;
		}

		public static MethodDefinition GetMethod(this TypeDefinition type, string name)
		{
			var def = type.Methods.FirstOrDefault(m => m.Name == name);
			if (def == null)
				throw new Exception($"Can't find method {name}");

			return def;
		}

		public static MethodDefinition GetMethod(this ModuleDefinition module, string typeName, string methodName)
		{
			return GetMethod(GetTypeDef(module, typeName), methodName);
		}

		public static MethodDefinition GetMethod(this ModuleDefinition module, string methodFullName)
		{
			var def = module.Types.SelectMany(t => t.Methods).FirstOrDefault(m => m.FullName == methodFullName);
			if (def == null)
				throw new Exception($"Can't find method {methodFullName}");

			return def;
		}

		/// <summary>
		/// Inserts a series of instructions after the given one, but in the proper order.
		/// </summary>
		/// <param name="proc">The proc.</param>
		/// <param name="target">The target.</param>
		/// <param name="code">The code.</param>
		public static void InsertAfter(this ILProcessor proc, Instruction target, params Instruction[] code)
		{
			foreach (var i in code.Reverse())
				proc.InsertAfter(target, i);
		}

		public static MethodReference Resolve<T>(this ModuleDefinition module, string name, params Type[] args)
		{
			return Resolve(module, typeof(T), name, args);
		}

		public static MethodReference Resolve(this ModuleDefinition module, Type type, string name, params Type[] args)
		{
			var method = args.Length == 0 ? type.GetMethod(name) : type.GetMethod(name, args);

			return module.Import(method);
		}
	}
}
