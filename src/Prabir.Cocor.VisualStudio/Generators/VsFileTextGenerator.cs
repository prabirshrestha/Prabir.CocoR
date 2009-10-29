using System.IO;
using System.Text;

namespace Prabir.VisualStudio.CustomTool
{
    public class VsFileTextGenerator : IVsFileGenerator
    {
        #region Constructors

        public VsFileTextGenerator()
        {
            OverwriteIfExists = true;
            Extension = ".txt";
            Encoding = System.Text.Encoding.ASCII;
        }

        public VsFileTextGenerator(string inputFilePath)
            : this()
        {
            FileName = Path.GetFileNameWithoutExtension(inputFilePath);
        }

        #endregion

        public string Text { get; set; }
        public Encoding Encoding { get; set; }

        #region IVsFile Members

        public bool OverwriteIfExists { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        #endregion

        #region IVsFileGenerator Members

        public byte[] GenerateContent()
        {
            return Encoding.GetBytes(Text);
        }

        #endregion

    }
}