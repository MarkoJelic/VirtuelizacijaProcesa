using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileDirUtil
    {
        public static bool CreateDirIfNotExists(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists)
                {
                    di.Create();
                }
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public static bool IsPathValid(string path)
        {
            return Path.IsPathRooted(path);
        }

        public static void CheckCreatePath(string path)
        {
            if (!IsPathValid(path))
            {
                throw new CustomException($"Invalid path: {path}");
            }
            CreateDirIfNotExists(path);
        }

        public static string[] GetAllFiles(string path)
        {
            return Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
        }
    }
}
