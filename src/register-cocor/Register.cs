using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Prabir.Cocor
{
    /// <summary>
    /// Summary description for Register.
    /// </summary>
    public class Register
    {
        const string dllName = "Prabir.Cocor.VisualStudio.dll";
        const string generatorName = "Cocor";
        const string guid = "{fc6e643b-f4a3-4c13-99f1-33f6be0ff31f}";
        const string defaultKeyValue = "Prabir's Coco/R Addin For Visual Studio";

        static int Main(string[] args)
        {
            try
            {
                return CompleteRegistration(dllName, generatorName, guid, defaultKeyValue);
            }
            catch (Exception e)
            {
                msg("An unespected error occured while registering vsCoco\n" + e.Message, true);
                return 1;
            }
        }

        /// <summary>
        /// Actually performs the registration using regasm.exe and regedit.exe
        /// </summary>
        /// <param name="dllName">Name of the Dll being registered as a VS.NET Custom tool</param>
        /// <param name="generatorName">Name to use as the custom tool</param>
        /// <param name="guid">Guid of the assembly class that is the custom tool</param>
        /// <param name="defaultKeyValue">Description of the custom tool to be entered into the registry</param>
        /// <returns>0 (Zero) for successful registration</returns>
        static private int CompleteRegistration(string dllName, string generatorName, string guid, string defaultKeyValue)
        {
            //This app must be run in the same directory as the Dll to be registered. 
            //Of course, this could be changed, but I haven't seen the need!
            string full = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string dir = Path.GetDirectoryName(full);

            //Set the root path for the .NET Runtime (where you can find regasm.exe)
            string dotNetRoot = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            string currentDir = System.AppDomain.CurrentDomain.BaseDirectory;

            Process prc = new Process();
            ListViewItem item = new ListViewItem();

            //Make sure regasm.exe is where it's supposed to be!
            if (!File.Exists(dotNetRoot + "regasm.exe"))
            {

                msg("Unable to locate regasm.exe", true);
                throw new ApplicationException("Install Failed");
            }

            //Make sure the Assembly to register is present and accounted for
            if (!File.Exists(currentDir + dllName))
            {
                msg("Unable to locate " + currentDir + dllName + "\n" + "** Installation Failed **", true);
                throw new ApplicationException("Install Failed");
            }

            //Get started

            //Register the Type library for the Assembly
            prc.StartInfo.FileName = dotNetRoot + "regasm.exe";
            prc.StartInfo.Arguments = "/tlb \"" + currentDir + dllName + "\"";
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.StartInfo.WorkingDirectory = dir;
            prc.Start();
            prc.WaitForExit();

            if (prc.ExitCode != 0)
            {
                msg("Failed to Register Type Library. Error code:" + prc.ExitCode.ToString() + "\n"
                    + "Installation Failed", true);
                return 1;
            }


            //Register the Assembly code base

            prc.StartInfo.FileName = dotNetRoot + "regasm.exe";
            prc.StartInfo.Arguments = "/codebase \"" + currentDir + dllName + "\"";
            prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            prc.StartInfo.WorkingDirectory = dir;
            prc.Start();
            prc.WaitForExit();

            if (prc.ExitCode != 0)
            {
                msg("Failed. Error code:" + prc.ExitCode.ToString() + "\n"
                    + "Install Failed", true);
                return 1;
            }

            bool badExitCode = false;

            if (File.Exists(Path.Combine(currentDir, "vcsexpress2010.reg")))
            {
                //If we need to perform the registry entry, then do it, otherwise exit.
                prc.StartInfo.FileName = "regedit";
                prc.StartInfo.Arguments = "/s \"" + currentDir + "vcsexpress2010.reg\"";
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.StartInfo.WorkingDirectory = dir;
                prc.Start();
                prc.WaitForExit();

                if (prc.ExitCode != 0) badExitCode = true;
            }

            if (File.Exists(Path.Combine(currentDir, "vcsexpress2008.reg")))
            {
                //If we need to perform the registry entry, then do it, otherwise exit.
                prc.StartInfo.FileName = "regedit";
                prc.StartInfo.Arguments = "/s \"" + currentDir + "vcsexpress2008.reg\"";
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.StartInfo.WorkingDirectory = dir;
                prc.Start();
                prc.WaitForExit();

                if (prc.ExitCode != 0) badExitCode = true;
            }

            if (File.Exists(Path.Combine(currentDir, "vs2010.reg")))
            {
                prc.StartInfo.FileName = "regedit";
                prc.StartInfo.Arguments = "/s \"" + currentDir + "vs2010.reg\"";
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.StartInfo.WorkingDirectory = dir;
                prc.Start();
                prc.WaitForExit();

                if (prc.ExitCode != 0) badExitCode = true;
            }


            if (File.Exists(Path.Combine(currentDir, "vs2008.reg")))
            {
                prc.StartInfo.FileName = "regedit";
                prc.StartInfo.Arguments = "/s \"" + currentDir + "vs2008.reg\"";
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.StartInfo.WorkingDirectory = dir;
                prc.Start();
                prc.WaitForExit();

                if (prc.ExitCode != 0) badExitCode = true;
            }

            if (badExitCode)
            {
                msg("Install Failed", true);
                return 1;
            }

            msg("Prabir's Coco/R Addin For Visual Studio\n\nRegistration succeeded", false);
            return 0;
        }

        /// <summary>
        /// Helper method to make the list view item entries
        /// </summary>
        /// <param name="verbage">Text to display</param>
        /// <param name="appendToLast">Whether or not to make a new list item or add this to the last item as a new sub-item</param>
        /// <param name="isError">Flag for an error message (makes the color Red)</param>
        static private void msg(string verbage, bool isError)
        {
            if (isError)
                MessageBox.Show(verbage, "CocoRegistration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                MessageBox.Show(verbage, "CocoRegistration", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
