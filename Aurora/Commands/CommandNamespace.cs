using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aurora.Commands
{
	internal class CommandNamespace : CommandHeirarchyElement, IEnumerable
	{
		public List<Command> Commands { get; }
		public List<CommandNamespace> Namespaces { get; }

		public CommandNamespace(string name, string description)
			: base(name, description, $@"^{name}\.")
		{
			Commands = new List<Command>();
			Namespaces = new List<CommandNamespace>();

			Add(new Global.DirCommand(this));
		}

		public void Add(CommandNamespace ns)
		{
			if (ns.Parent != null)
				throw new InvalidOperationException("Namespace has already been inserted into the hierarchy!");

			ns.Parent = this;
			Namespaces.Add(ns);
		}

		public void Add(Command c)
		{
			var ch = c as CommandHeirarchyElement;

			if (ch.Parent != null)
				throw new InvalidOperationException("Command has already been inserted into the hierarchy!");

			c.Parent = this;
			Commands.Add(c);
		}

		public CommandHeirarchyElement Resolve(string idFragment)
		{
			CommandHeirarchyElement cmd = Commands.FirstOrDefault(c => c.Matcher.IsMatch(idFragment));

			if (cmd == null)
			{
				foreach (var nsEntry in Namespaces)
				{
					var m = nsEntry.Matcher.Match(idFragment);
					if (!m.Success)
						continue;

					if (m.Length == idFragment.Length)
						cmd = nsEntry;
					else
						cmd = nsEntry.Resolve(idFragment.Substring(m.Length));

					if (cmd != null)
						break;
				}
			}

			return cmd;
		}

		public Command GetCommand(string idFragment)
		{
			return Resolve(idFragment) as Command;
		}

		public override IEnumerable<string> GetHelp()
		{
			yield return $"Namespace {Name} - {Description}";
		}

		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}
