using System;
using System.IO;
using ApprovalUtilities.Persistence;

namespace DeveloperTimer
{
    public class LoaderFile : ILoader<StreamReader>
    {
        private readonly string path;

        public LoaderFile(string path)
        {
            this.path = path;
        }

        public StreamReader Load()
        {
            return new StreamReader(path);
        }
    }
}