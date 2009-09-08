using System;
using System.IO;

namespace Prabir.Cocor
{
    public class CocorConsoleTextWriter : TextWriter
    {
        // Just implement only these; coz Coco uses only them.

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Console.WriteLine(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(string value)
        {
            Console.WriteLine(value);
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.ASCII; }
        }
    }
}