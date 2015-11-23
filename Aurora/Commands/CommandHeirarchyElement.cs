using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aura.Channel.Network.Sending;
using Aura.Channel.World.Entities;

namespace Aurora.Commands
{
	internal abstract class CommandHeirarchyElement
	{
		internal CommandHeirarchyElement Parent { get; set; }

		public string Name { get; }
		public string Description { get; }
		public bool Hide { get; }
		public Regex Matcher { get; }

		public string FullyQualifiedName
		{
			get
			{
				var levels = new Stack<string>();

				var c = this;
				while (c.Parent != null)
				{
					levels.Push(c.Name);
					c = c.Parent;
				}

				levels.Push("(global)");

				return string.Join(".", levels);
			}
		}

		protected CommandHeirarchyElement(string name, string description, string matcher, bool hide = false)
			: this(name, description, new Regex(matcher, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), hide)
		{
		}

		protected CommandHeirarchyElement(string name, string description, Regex matcher, bool hide = false)
		{
			Name = name;
			Hide = hide;
			Matcher = matcher;
			Description = description;
		}

		public abstract IEnumerable<string> GetHelp();

		protected void ShowHelp(Creature target, IEnumerable<string> help = null)
		{
			help = help ?? GetHelp();

			foreach (var line in help)
				Send.ServerMessage(target, line);
		}
	}
}
