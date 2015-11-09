using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radiation.Patches;

namespace Radiation
{
	class Program
	{
		private const string ByLine = "Radiation by Xcelled";
        private const int RadiationSymbolWidth = 40;
		private static readonly string[] RadiationSymbol =
		{
			@"             =+$HM####@H%;,",
			@"          /H###############M$,",
			@"          ,@################+",
			@"           .H##############+",
			@"             X############/",
			@"              $##########/",
			@"               %########/",
			@"                /X/;;+X/",
			@"",
			@"                 -XHHX-",
			@"                ,######,",
			@"#############X  .M####M.  X#############",
			@"##############-   -//-   -##############",
			@"X##############%,      ,+##############X",
			@"-##############X        X##############-",
			@" %############%          %############%",
			@"  %##########;            ;##########%",
			@"   ;#######M=              =M#######;",
			@"    .+M###@,                ,@###M+.",
			@"       :XH.                  .HX:",
		};

		static void Main(string[] args)
		{
			Console.Title = "☢ Radiation ☢";
            PrintLogo();

			Environment.CurrentDirectory = @"D:\Documents\Programming\Visual Studio\Projects\aura\bin\Debug\";

			var shared = new SharedAssembly();
			shared.Load("Shared2.dll");
			shared.Patch();
			shared.Save("Shared.dll");

			var channel = new ChannelServerAssembly();
			channel.Load("ChannelServer2.exe");
			channel.Patch();
			channel.Save("ChannelServer.exe");

			Console.ReadKey();
		}

		private static void PrintLogo()
		{
			var cf = Console.ForegroundColor;
			var cb = Console.BackgroundColor;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.BackgroundColor = ConsoleColor.Black;

			Console.WriteLine();

			var padding = new string(' ', (Console.WindowWidth - RadiationSymbolWidth) / 2);

			foreach (var line in RadiationSymbol)
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
	}
}
