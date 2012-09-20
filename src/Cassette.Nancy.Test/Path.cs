using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cassette.Nancy.Test
{
    public static class Path
    {
        /// <summary>
        /// Removes elements of a file path according to special strings inserted into that path.
        /// 
        /// Wraps the shell PathCanonicalize function
        /// </summary>
        /// <param name="src">Path to canonicalize</param>
        /// <returns></returns>
        public static string Canonicalize(string src)
        {
            var sb = new StringBuilder(2048);
            PathCanonicalize(sb, src);
            return sb.ToString();
        }

        /// <summary>
        /// Concatenates two strings that represent properly formed paths into one path; also concatenates any relative path elements.
        /// 
        /// Wraps the shell PathCombine function
        /// </summary>
        /// <param name="src1">first part of the path to combine</param>
        /// <param name="src2">second part of the path to combine</param>
        /// <returns></returns>
        public static string Combine(string src1, string src2)
        {
            var sb = new StringBuilder(2048);
            PathCombine(sb, src1, src2);
            return sb.ToString();
        }

        /// <summary>
        /// Determines whether a path to a file system object such as a file or directory is valid.
        /// </summary>
        /// <param name="path">the full path of the object to verify</param>
        /// <returns>Returns TRUE if the file exists, or FALSE otherwise.</returns>
        public static bool FileExists(string path)
        {
            return PathFileExists(path);
        }

        /// <summary>
        /// Searches a path and determines if it is relative.
        /// </summary>
        /// <param name="path">the path to search</param>
        /// <returns>Returns TRUE if the path is relative, or FALSE if it is absolute.</returns>
        public static bool IsRelative(string path)
        {
            return PathIsRelative(path);
        }

        /// <summary>
        /// Parses a path to determine if it is a directory root.
        /// </summary>
        /// <param name="path">the path to be validated</param>
        /// <returns>Returns TRUE if the specified path is a root, or FALSE otherwise.</returns>
        public static bool IsRoot(string path)
        {
            return PathIsRoot(path);
        }


        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PathCanonicalize([Out] StringBuilder dst, string src);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr PathCombine([Out] StringBuilder lpszDest, string lpszDir, string lpszFile);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PathFileExists(string pszPath);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PathIsRelative(string pPath);

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool PathIsRoot(string pPath);
    }
}
