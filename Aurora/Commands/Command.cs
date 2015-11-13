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
	internal abstract class Command
	{
		private readonly int _defaultSelfAuth;
		private readonly int _defaultTargetAuth;
		public string Name { get; }
		public string Description { get; }

		public int SelfAuth => ChannelServer.Instance.Conf.Commands.GetAuth(Name, _defaultSelfAuth, _defaultTargetAuth).Auth;

		public int TargertAuth
			=> ChannelServer.Instance.Conf.Commands.GetAuth(Name, _defaultSelfAuth, _defaultTargetAuth).CharAuth;

		public bool CanBeTargeted => TargertAuth > 0;

		protected Command(string name, string description, int selfAuth = 99, int targetAuth = 99)
		{
			Name = name;
			Description = description;
			_defaultSelfAuth = selfAuth;
			_defaultTargetAuth = targetAuth;
		}

		public abstract void Execute(ChannelClient client, Creature sender, Creature target, IList<string> args);

		protected void EnsureCanExecute(Creature sender, Creature target)
		{
			var isSelfCommand = sender == target;
			EnsureAuth(sender, isSelfCommand);

			if (isSelfCommand)
				EnsureTargeted();
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
