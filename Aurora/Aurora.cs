using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aura.Channel.Scripting.Scripts;

namespace Aurora
{
    public class Aurora : GeneralScript
    {
	    private const string ByLine = "Aurora loaded!";
	    private const int LogoWidth = 76;
	    public static readonly string[] Logo =
	    {
			@"  ` : | | | |:  ||  :     `  :  |  |+|: | : : :|   .        `              .",
			@"      ` : | :|  ||  |:  :    `  |  | :| : | : |:   |  .                    :",
			@"         .' ':  ||  |:  |  '       ` || | : | |: : |   .  `           .   :.",
			@"                `'  ||  |  ' |   *    ` : | | :| |*|  :   :               :|",
			@"        *    *       `  |  : :  |  .      ` ' :| | :| . : :         *   :.||",
			@"             .`            | |  |  : .:|       ` | || | : |: |          | ||",
			@"      '          .         + `  |  :  .: .         '| | : :| :    .   |:| ||",
			@"         .                 .    ` *|  || :       `    | | :| | :      |:| |",
			@" .                .          .        || |.: *          | || : :     :|||",
			@"        .            .   . *    .   .  ` |||.  +        + '| |||  .  ||`",
			@"     .             *              .     +:`|!             . ||||  :.||`",
			@" +                      .                ..!|*          . | :`||+ |||`",
			@"     .                         +      : |||`        .| :| | | |.| ||`     .",
			@"       *     +   '               +  :|| |`     :.+. || || | |:`|| `",
			@"                            .      .||` .    ..|| | |: '` `| | |`  +",
			@"  .       +++                      ||        !|!: `       :| |",
			@"              +         .      .    | .      `|||.:      .||    .      .    `",
			@"          '                           `|.   .  `:|||   + ||'     `",
			@"  __    +      *                         `'       `'|.    `:",
			@"""'  `---""""""----....____,..^---`^``----.,.___          `.    `.  .    ____,.,-",
			@"    ___,--'""""`---""'   ^  ^ ^        ^       """"""'---,..___ __,..---""""'",
			@"--""'                           ^                         ``--..,__",

		};

		private static void PrintLogo()
		{
			var cf = Console.ForegroundColor;
			var cb = Console.BackgroundColor;

			Console.ForegroundColor = ConsoleColor.Green;
			Console.BackgroundColor = ConsoleColor.Black;

			Console.WriteLine();

			var padding = new string(' ', (Console.WindowWidth - LogoWidth) / 2);

			foreach (var line in Logo)
			{
				Console.Write(padding);
				Console.WriteLine(line);
			}

			Console.WriteLine();
			Console.WriteLine(ByLine.PadLeft((Console.WindowWidth - ByLine.Length) / 2 + ByLine.Length));
			Console.WriteLine();

			Console.ForegroundColor = cf;
			Console.BackgroundColor = cb;
		}

		public override void Load()
		{
			PrintLogo();
		}
    }
}
