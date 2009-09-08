using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Prabir.VisualStudio.CustomTool;
using System.IO;

namespace Prabir.Cocor.VisualStudio
{
    [Guid("e6fc16fd-54b9-4165-bd0b-7d0edf6422bf")]
    [ComVisible(true)]
    [CustomTool("CocoR", "Prabir's Coco/R Addin For Visual Studio")]
    public class CocorCustomTool : VsFileGenerator
    {
        public override void Generate(string inputFilePath, string defaultNameSpace, out IVsFile primaryFile, out IList<IVsFile> secondaryFiles)
        {
            TextWriter writer = new CocorVsOutputWindowPaneTextWriter(base.DTE, "Coco/R");

            CocorGenerator.Generate(CocorGenerator.CocorProvider.Prabir, writer,
                new string[] { inputFilePath });

            DTE.StatusBar.Highlight(true);

            // Our Primary File is _____.Parser.cs and rest are secondaryFiles.

            string srcDir = Path.GetDirectoryName(inputFilePath);

            #region Add Files

            primaryFile = new VsFileInclude(inputFilePath) { Extension = ".Parser.cs" };

            secondaryFiles = new List<IVsFile>();
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".Scanner.cs" });
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".Parser.cs.old" });
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".Scanner.cs.old" });
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".trace.txt" });
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".Parser.Frame" });
            secondaryFiles.Add(new VsFileInclude(inputFilePath) { Extension = ".Scanner.Frame" });

            #endregion
        }

        #region COM-Related Stuffs

        internal static Guid CSharpCategoryGuid = new Guid("FAE04EC1-301F-11D3-BF4B-00C04F79EFBC");
        private const string VisualStudioVersion2008 = "9.0";
        // Version 2005,2003,2002 hasn't been tested yet.
        private const string VisualStudioVersion2005 = "8.0";
        private const string VisualStudioVersion2003 = "7.1";
        private const string VisualStudioVersion2002 = "7.0";

        [ComRegisterFunction]
        public static void RegisterClass(Type t)
        {
            GuidAttribute guidAttribute = getGuidAttribute(t);
            CustomToolAttribute customToolAttribute = getCustomToolAttribute(t);

            // For VS2008
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
              GetKeyName2008(CSharpCategoryGuid, customToolAttribute.Name)))
            {
                key.SetValue("", customToolAttribute.Description);
                key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2008(CSharpCategoryGuid, ".atg")))
            {
                key.SetValue("", "CocoR");
            }
            // For VS2008 Express
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
              GetKeyName2008Express(CSharpCategoryGuid, customToolAttribute.Name)))
            {
                key.SetValue("", customToolAttribute.Description);
                key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2008Express(CSharpCategoryGuid, ".atg")))
            {
                key.SetValue("", "CocoR");
            }

            // For VS2005
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
            GetKeyName2008(CSharpCategoryGuid, customToolAttribute.Name)))
            {
                key.SetValue("", customToolAttribute.Description);
                key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2005(CSharpCategoryGuid, ".atg")))
            {
                key.SetValue("", "CocoR");
            }

            // For VS2003
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
            GetKeyName2003(CSharpCategoryGuid, customToolAttribute.Name)))
            {
                key.SetValue("", customToolAttribute.Description);
                key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2002(CSharpCategoryGuid, ".atg")))
            {
                key.SetValue("", "CocoR");
            }

            // For VS2002
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2002(CSharpCategoryGuid, customToolAttribute.Name)))
            {
                key.SetValue("", customToolAttribute.Description);
                key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
                key.SetValue("GeneratesDesignTimeSource", 1);
            }
            using (RegistryKey key = Registry.LocalMachine.CreateSubKey(
                GetKeyName2002(CSharpCategoryGuid, ".atg")))
            {
                key.SetValue("", "CocoR");
            }
        }

        [ComUnregisterFunction]
        public static void UnregisterClass(Type t)
        {
            CustomToolAttribute customToolAttribute = getCustomToolAttribute(t);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2008(
              CSharpCategoryGuid, customToolAttribute.Name), false);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2008(
              CSharpCategoryGuid, ".atg"), false);

            Registry.LocalMachine.DeleteSubKey(GetKeyName2008Express(
              CSharpCategoryGuid, customToolAttribute.Name), false);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2008Express(
              CSharpCategoryGuid, ".atg"), false);

            Registry.LocalMachine.DeleteSubKey(GetKeyName2005(
              CSharpCategoryGuid, customToolAttribute.Name), false);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2005(
              CSharpCategoryGuid, ".atg"), false);

            Registry.LocalMachine.DeleteSubKey(GetKeyName2003(
              CSharpCategoryGuid, customToolAttribute.Name), false);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2003(
              CSharpCategoryGuid, ".atg"), false);

            Registry.LocalMachine.DeleteSubKey(GetKeyName2002(
              CSharpCategoryGuid, customToolAttribute.Name), false);
            Registry.LocalMachine.DeleteSubKey(GetKeyName2002(
              CSharpCategoryGuid, ".atg"), false);
        }

        internal static GuidAttribute getGuidAttribute(Type t)
        {
            return (GuidAttribute)getAttribute(t, typeof(GuidAttribute));
        }

        internal static CustomToolAttribute getCustomToolAttribute(Type t)
        {
            return (CustomToolAttribute)getAttribute(t, typeof(CustomToolAttribute));
        }

        internal static Attribute getAttribute(Type t, Type attributeType)
        {
            object[] attributes = t.GetCustomAttributes(attributeType, /* inherit */ true);
            if (attributes.Length == 0)
                throw new Exception(
                  String.Format("Class '{0}' does not provide a '{1}' attribute.",
                  t.FullName, attributeType.FullName));
            return (Attribute)attributes[0];
        }

        internal static string GetKeyName2008(Guid categoryGuid, string toolName)
        {
            return
              String.Format("SOFTWARE\\Microsoft\\VisualStudio\\" + VisualStudioVersion2008 +
                "\\Generators\\{{{0}}}\\{1}\\", categoryGuid, toolName);
        }

        internal static string GetKeyName2008Express(Guid categoryGuid, string toolName)
        {
            return
              String.Format("SOFTWARE\\Microsoft\\VCSExpress\\" + VisualStudioVersion2008 +
                "\\Generators\\{{{0}}}\\{1}\\", categoryGuid, toolName);
        }

        internal static string GetKeyName2005(Guid categoryGuid, string toolName)
        {
            return
              String.Format("SOFTWARE\\Microsoft\\VisualStudio\\" + VisualStudioVersion2005 +
                "\\Generators\\{{{0}}}\\{1}\\", categoryGuid, toolName);
        }

        internal static string GetKeyName2003(Guid categoryGuid, string toolName)
        {
            return
              String.Format("SOFTWARE\\Microsoft\\VisualStudio\\" + VisualStudioVersion2003 +
                "\\Generators\\{{{0}}}\\{1}\\", categoryGuid, toolName);
        }

        internal static string GetKeyName2002(Guid categoryGuid, string toolName)
        {
            return
              String.Format("SOFTWARE\\Microsoft\\VisualStudio\\" + VisualStudioVersion2002 +
                "\\Generators\\{{{0}}}\\{1}\\", categoryGuid, toolName);
        }

        #endregion

    }
}
