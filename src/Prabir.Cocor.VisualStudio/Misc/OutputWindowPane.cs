using EnvDTE;

namespace Prabir.VisualStudio
{
    public class OutputWindowPane
    {
        private readonly DTE _dte;
        private readonly string _paneName;
        private readonly EnvDTE.OutputWindowPane _owpPane;

        public EnvDTE.OutputWindowPane VsOutputWindowPane { get { return _owpPane; } }
        public string PaneName { get { return _paneName; } }

        public OutputWindowPane(DTE dte, string paneName)
        {
            _dte = dte;
            _paneName = paneName;

            _owpPane = Init();
        }

        private EnvDTE.OutputWindowPane Init()
        {
            EnvDTE.Window win = _dte.Windows.Item(EnvDTE.Constants.vsWindowKindOutput);
            EnvDTE.OutputWindow OW = (EnvDTE.OutputWindow)win.Object;

            EnvDTE.OutputWindowPane OWp = null;

            for (int i = 1; i <= OW.OutputWindowPanes.Count; i++)
            {
                if (OW.OutputWindowPanes.Item(i).Name == _paneName)
                {
                    OWp = OW.OutputWindowPanes.Item(i);
                    OWp.Clear();
                }
            }

            if (OWp == null)
                OWp = OW.OutputWindowPanes.Add(_paneName);

            OWp.Activate();

            return OWp;

        }
    }
}
