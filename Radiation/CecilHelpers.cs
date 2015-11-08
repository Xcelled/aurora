using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

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

	}
}
