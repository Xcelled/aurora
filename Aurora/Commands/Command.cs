using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Aura.Channel;
using Aura.Channel.Network;
using Aura.Channel.Network.Sending;
using Aura.Channel.World.Entities;
using Aura.Shared.Util;

namespace Aurora.Commands
{
	internal interface ICommand
	{
		bool IsNameMatch(string name);
		void Execute(ChannelClient client, Creature sender, Creature target, IEnumerable<string> args);
	}

	internal abstract class Command : ICommand
	{
		private readonly int _defaultSelfAuth;
		private readonly int _defaultTargetAuth;
		protected readonly Regex NamePatternRegex;
		public string Name { get; }

		public int SelfAuth => ChannelServer.Instance.Conf.Commands.GetAuth(Name, _defaultSelfAuth, _defaultTargetAuth).Auth;

		public int TargertAuth
			=> ChannelServer.Instance.Conf.Commands.GetAuth(Name, _defaultSelfAuth, _defaultTargetAuth).CharAuth;

		public bool CanBeTargeted => TargertAuth > 0;

		protected Command(string name, int selfAuth = 99, int targetAuth = 99, string namePattern = null)
			: this(name, new Regex($"^{namePattern ?? name}$", RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), selfAuth, targetAuth)
		{
		}

		protected Command(string name, Regex namePattern, int selfAuth = 99, int targetAuth = 99)
		{
			Name = name;
			_defaultSelfAuth = selfAuth;
			_defaultTargetAuth = targetAuth;
			NamePatternRegex = namePattern;
		}

		public virtual void Execute(ChannelClient client, Creature sender, Creature target, IEnumerable<string> args)
		{
			var isSelfCommand = sender == target;
			EnsureAuth(sender, isSelfCommand);

			if (isSelfCommand)
				EnsureTargeted();
		}

		public bool IsNameMatch(string name)
		{
			return NamePatternRegex.IsMatch(name);
		}

		protected bool HasAuth(Creature invoker, bool isSelfCommand)
		{
			var req = isSelfCommand ? SelfAuth : TargertAuth;
			return req < invoker.Client.Account?.Authority;
		}

		protected void EnsureAuth(Creature invoker, bool isSelfCommand)
		{
			if (!HasAuth(invoker, isSelfCommand))
			{
				throw new Exception(string.Format(Localization.Get("You're not authorized to use '{0}'."), Name));
			}
		}

		protected void EnsureTargeted()
		{
			if (!CanBeTargeted)
			{
				throw new Exception(string.Format(Localization.Get("Command '{0}' cannot be used on another character."), Name));
			}
		}
	}
}
