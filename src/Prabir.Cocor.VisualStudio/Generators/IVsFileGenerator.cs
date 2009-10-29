
namespace Prabir.VisualStudio.CustomTool
{
    interface IVsFileGenerator : IVsFile
    {
        byte[] GenerateContent();
    }
}
