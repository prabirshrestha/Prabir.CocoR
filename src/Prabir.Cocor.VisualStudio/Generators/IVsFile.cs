
namespace Prabir.VisualStudio.CustomTool
{
    public interface IVsFile
    {
        bool OverwriteIfExists { get; }
        string FileName { get; }
        string Extension { get; }
    }
}
