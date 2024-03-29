﻿using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace UnoApp.Skia.Tizen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new UnoApp.App(), args);
            host.Run();
        }
    }
}
