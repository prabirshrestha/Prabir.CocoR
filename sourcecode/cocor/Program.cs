using System;

namespace Prabir.Cocor
{
    public class Program
    {
        public static int Main(string[] arg)
        {
            return CocorGenerator.Generate(
                CocorGenerator.CocorProvider.Original, Console.Out, arg);
        } 
    }
} 