using System.IO;
using ApprovalUtilities.Persistence;

namespace DeveloperTimer
{
    public interface ILoaderFile : ILoader<StreamReader>
    {
        bool CreateFile { get; set; }
    }
}