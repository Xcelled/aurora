using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Commands
{
	/// <summary>
	/// Utility class to preform sh-like tokenizing of
	/// argument strings into string[].
	/// </summary>
	/// <remarks>
	/// Based on Dr. Java's implementation:
	/// http://www.drjava.org/
	/// </remarks>
	internal class ArgumentTokenizer
	{
		public List<string> Tokenize(string argString)
		{
			var args = new List<string>();
			var current = new StringBuilder(argString.Length, argString.Length);

			var iter = argString.GetEnumerator();

			while (iter.MoveNext())
			{
				switch (iter.Current)
				{
					case '\'':
						ParseSingleQuoteToken(iter, current);
						break;
					case '"':
						ParseDoubleQuoteToken(iter, current);
						break;
					case '\\':
						ParseEscapedToken(iter, current);
						break;
					default:
						if (char.IsWhiteSpace(iter.Current))
						{
							if (current.Length != 0)
								args.Add(current.ToString());
							current.Clear();
						}
						else
						{
							current.Append(iter.Current);
						}
						break;
				}
			}

			if (current.Length != 0)
				args.Add(current.ToString());

			return args;
		}

		private void ParseSingleQuoteToken(IEnumerator<char> iter, StringBuilder sb)
		{
			while (iter.MoveNext())
			{
				if (iter.Current == '\'')
					return;
				sb.Append(iter.Current);
			}

			throw new FormatException("Unexpected end of input");
		}

		private void ParseDoubleQuoteToken(IEnumerator<char> iter, StringBuilder sb)
		{
			while (iter.MoveNext())
			{
				if (iter.Current == '\"')
					return;
				if (iter.Current == '\\')
					ParseEscapedToken(iter, sb);
				else
					sb.Append(iter.Current);
			}

			throw new FormatException("Unexpected end of input");
		}

		private void ParseEscapedToken(IEnumerator<char> iter, StringBuilder sb)
		{
			if (!iter.MoveNext())
				throw new FormatException("Unexpected end of input");

			switch (iter.Current)
			{
				case 'n':
					sb.Append("\n");
					break;
				case 'r':
					sb.Append("\r");
					break;
				case 't':
					sb.Append("\t");
					break;
				default:
					sb.Append(iter.Current);
					break;
			}
		}
	}
}
