using System.Collections.Generic;
using System.IO;

namespace Prabir.VisualStudio.CustomTool
{
    public abstract class BaseVsFileGenerator : BaseCodeGeneratorWithSite
    {
        string defaultExtension = ".auto";
        public override string GetDefaultExtension()
        {
            return defaultExtension;
        }

        public EnvDTE.DTE DTE { get; private set; }

        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
        {
            if (string.IsNullOrEmpty(inputFileName))
                return null;

            IVsFile primaryFile = null;
            IList<IVsFile> secondaryFiles = null;
            DTE = null;

            object serviceObject = GetService(typeof(EnvDTE.ProjectItem));
            EnvDTE.Project containingProject;
            EnvDTE.ProjectItem item = null;

            if (serviceObject != null)
            {
                item = (EnvDTE.ProjectItem)serviceObject;
                containingProject = ((EnvDTE.ProjectItem)serviceObject).ContainingProject;
                DTE = containingProject.DTE;
            }

            Generate(inputFileName, out primaryFile, out secondaryFiles);

            #region Preparation

            // Set the default extension from primaryFile.Extension.
            // Check for . in extension also
            defaultExtension = AddDotForExtension(primaryFile.Extension);
            string srcDir = Path.GetDirectoryName(inputFileName);

            #endregion

            #region Generate Secondary Files

            List<string> secondaryFileNames = new List<string>();

            if (secondaryFiles != null || secondaryFileNames.Count != 0) // if there are secondary files
            {
                foreach (IVsFile sf in secondaryFiles)
                {
                    string currentSecondaryFile = sf.FileName + AddDotForExtension(sf.Extension);
                    string secondaryFileName = Path.Combine(srcDir, currentSecondaryFile);

                    if (!File.Exists(secondaryFileName)) // if file doesn't exist just ignore it, instead of giving error
                        continue;

                    secondaryFileNames.Add(currentSecondaryFile);

                    // skip file if it exists and the OverwriteIfExists property is false
                    if (File.Exists(secondaryFileName) && !sf.OverwriteIfExists)
                        continue;

                    string sfFullPath = Path.Combine(srcDir, currentSecondaryFile);

                     if (sf is IVsFileGenerator)
                         WriteToFile((sf as IVsFileGenerator).GenerateContent(), sfFullPath);
                     /* else just add the filename only */

                     item.ProjectItems.AddFromFile(sfFullPath);
                }
            }

            #endregion

            #region Clean Up Old Files

            // Need to perfrom cleanup here, to remove old target files.
            foreach (EnvDTE.ProjectItem childItem in item.ProjectItems)
            {
                if (!(childItem.Name.EndsWith(defaultExtension) || secondaryFileNames.Contains(childItem.Name)))
                    childItem.Delete(); // then delete it
            }

            #endregion

            #region Generate Primary Files

            byte[] generatedData = null;

            if (primaryFile is IVsFileGenerator)
            {
                generatedData = (primaryFile as IVsFileGenerator).GenerateContent();
            }
            else
                generatedData = ReadFromFile(Path.Combine(srcDir, primaryFile.FileName + primaryFile.Extension));

            #endregion

            return generatedData;
        }

        public abstract void Generate(string inputFileName, out IVsFile primaryFile, out IList<IVsFile> secondaryFiles);

        #region HelperMethods

        private static byte[] ReadFromFile(string path)
        {
            byte[] buffer = null;

            if (!File.Exists(path))
                return null;

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                int length = (int)fs.Length;
                buffer = new byte[length];

                int count;
                int sum = 0;

                while ((count = fs.Read(buffer, sum, length - sum)) > 0)
                    sum += count;
            }

            return buffer;
        }

        private static void WriteToFile(byte[] buffer, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(buffer);
                }
            }
        }

        private static string AddDotForExtension(string extension)
        {
            if (!extension.StartsWith("."))
                return "." + extension;
            else
                return extension;
        }

        #endregion
    }
}