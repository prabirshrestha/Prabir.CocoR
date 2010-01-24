using System.Configuration.Install;

namespace Prabir.Cocor.VisualStudio
{
    public class CocorInstaller : Installer
    {
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            string regasmPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + @"regasm.exe";
            string componentPath = typeof(CocorInstaller).Assembly.Location;
            System.Diagnostics.Process.Start(regasmPath, "/codebase \"" + componentPath + "\"");
        }

        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            base.Uninstall(savedState);
            string regasmPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory() + @"regasm.exe";
            string componentPath = typeof(CocorInstaller).Assembly.Location;
            System.Diagnostics.Process.Start(regasmPath, "/unregister \"" + componentPath + "\"");
        }
    }
}
