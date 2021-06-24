using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nihilquest
{
    public static class VisualStudioProvider
    {
        //returns solution path
        //needed for testing
        public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory;
        }
    }
}
