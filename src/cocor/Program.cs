using System;

namespace Prabir.Cocor
{
    class Program
    {
        static int Main(string[] args)
        {
            return CocorGenerator.Generate(
               CocorGenerator.CocorProvider.Prabir, Console.Out, args);
        }
    }
}
