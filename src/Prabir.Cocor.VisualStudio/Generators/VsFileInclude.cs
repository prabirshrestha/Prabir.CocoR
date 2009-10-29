using System.IO;

namespace Prabir.VisualStudio.CustomTool
{
    public class VsFileInclude : IVsFile
    {
        #region Constructors

        public VsFileInclude()
        {
            OverwriteIfExists = true;
            Extension = "txt";
        }

        public VsFileInclude(string inputFilePath)
            : this()
        {
            FileName = Path.GetFileNameWithoutExtension(inputFilePath);
        }

        #endregion

        #region IVsFile Members

        public bool OverwriteIfExists { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        #endregion
    }
}