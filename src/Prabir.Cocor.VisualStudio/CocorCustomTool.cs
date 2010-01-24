using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Prabir.VisualStudio.CustomTool;

namespace Prabir.Cocor.VisualStudio
{
    [Guid("fc6e643b-f4a3-4c13-99f1-33f6be0ff31f")]
    [ComVisible(true)]
    public class CocorCustomTool : BaseVsFileGenerator
    {
        public override void Generate(string inputFileName, out IVsFile primaryFile, out System.Collections.Generic.IList<IVsFile> secondaryFiles)
        {
            TextWriter writer = System.Console.Out;
            if (base.DTE != null)
                writer = new CocorVsOutputWindowPaneTextWriter(base.DTE, "Coco/R");

            CocorGenerator.Generate(CocorGenerator.CocorProvider.Prabir, writer,
                new string[] { inputFileName }); //arguments passed

            DTE.StatusBar.Highlight(true);

            string srcDir = Path.GetDirectoryName(inputFileName);

            #region Add Files

            primaryFile = new VsFileInclude(inputFileName) { Extension = ".Parser.cs" };

            secondaryFiles = new List<IVsFile>();
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".Scanner.cs" });
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".Parser.cs.old" });
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".Scanner.cs.old" });
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".trace.txt" });
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".Parser.Frame" });
            secondaryFiles.Add(new VsFileInclude(inputFileName) { Extension = ".Scanner.Frame" });

            #endregion
        }
    }

}
