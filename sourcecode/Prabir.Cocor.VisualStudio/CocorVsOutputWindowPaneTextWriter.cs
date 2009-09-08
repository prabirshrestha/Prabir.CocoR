using System;
using System.IO;

namespace Prabir.Cocor.VisualStudio
{
    public class CocorVsOutputWindowPaneTextWriter : TextWriter
    {
        readonly EnvDTE.OutputWindowPane _owp;
        readonly string _paneName;

        public EnvDTE.OutputWindowPane OWP { get { return _owp; } }
        public string PaneName { get { return _paneName; } }

        public CocorVsOutputWindowPaneTextWriter(EnvDTE.OutputWindowPane owp, string paneName)
        {
            _owp = owp;
            _paneName = paneName;
        }

        public CocorVsOutputWindowPaneTextWriter(EnvDTE.DTE dte, string paneName)
        {
            _paneName = paneName;
            _owp = new Prabir.VisualStudio.OutputWindowPane(dte, paneName).VsOutputWindowPane;
        }

        // Just implement only these; coz Coco uses only them.

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            this.WriteLine(string.Format(format, arg0, arg1, arg2));
        }

        public override void WriteLine(string value)
        {
            _owp.OutputString(value);
            _owp.OutputString(Environment.NewLine);
        }

        public override System.Text.Encoding Encoding
        {
            get { return System.Text.Encoding.ASCII; }
        }
    }
}