using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurora.Commands
{
	internal class CommandNamespace
	{
		public string Name { get; }
		public Regex Pattern { get; }
		public List<KeyValuePair<Regex, Command>> Commands { get; }

		public CommandNamespace(string name, string pattern)
			: this(name, new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
		{

		}

		public CommandNamespace(string name, Regex pattern)
		{
			Name = name;
			Pattern = pattern;
			Commands = new List<KeyValuePair<Regex, Command>>();
		}
	}
}
