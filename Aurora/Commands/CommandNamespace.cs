using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurora.Commands
{
	internal class CommandNamespace : IEnumerable
	{
		public string Name { get; }
		public string Description { get; }

		public List<KeyValuePair<Regex, Command>> Commands { get; }
		public List<KeyValuePair<Regex, CommandNamespace>> Namespaces { get; }

		public CommandNamespace(string name, string description)
		{
			Name = name;
			Description = description;

			Commands = new List<KeyValuePair<Regex, Command>>();
			Namespaces = new List<KeyValuePair<Regex, CommandNamespace>>();

			Add(@"^\??$", new Global.DirCommand(this));
		}

		public void Add(string pattern, CommandNamespace ns)
		{
			Add(new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), ns);
		}

		public void Add(Regex pattern, CommandNamespace ns)
		{
			Namespaces.Add(new KeyValuePair<Regex, CommandNamespace>(pattern, ns));
		}

		public void Add(string pattern, Command c)
		{
			Add(new Regex(pattern, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), c);
		}

		public void Add(Regex pattern, Command c)
		{
			Commands.Add(new KeyValuePair<Regex, Command>(pattern, c));
		}

		public Command GetCommand(string idFragment)
		{
			var cmd = Commands.FirstOrDefault(kvp => kvp.Key.IsMatch(idFragment)).Value;

			if (cmd == null)
			{
				foreach (var nsEntry in Namespaces)
				{
					var m = nsEntry.Key.Match(idFragment);
					if (!m.Success)
						continue;

					cmd = nsEntry.Value.GetCommand(idFragment.Substring(m.Length));
					if (cmd != null)
						break;
				}
			}

			return cmd;
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
