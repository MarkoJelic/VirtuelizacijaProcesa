using Common;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class FileSender : IFileSender
    {
        private readonly ConcurrentBag<string> importedFiles = new ConcurrentBag<string>();
        private readonly string path;
        private readonly IEstimate proxy;
        private readonly IFileInUseChecker fileInUseChecker;

        public FileSender(IEstimate proxy, IFileInUseChecker fileInUseChecker, string path)
        {
            this.proxy = proxy;
            this.fileInUseChecker = fileInUseChecker;
            this.path = path;
        }



        public void SendFile(string filePath)
        {
            if (importedFiles.Contains(filePath))
            {
                Console.WriteLine($"File {Path.GetFileName(filePath)} has already been uploaded.");
                DeleteFile(filePath);
                return;
            }
            var fileName = Path.GetFileName(filePath);
            FileManipulationOptions options = new FileManipulationOptions(GetMemoryStream(filePath), fileName);//, storageType);
            var res = proxy.SendFile(options);
            options.Dispose();
            if (res.ResultType == ResultTypes.Failed)
            {
                Console.WriteLine($"Upload file {fileName} unsuccesful. Error message: {res.ResultMessage}");
            }
            else
            {
                if (res.ResultType == ResultTypes.Warning)
                {
                    Console.WriteLine($"Upload file {fileName} imported with warning: {res.ResultMessage}");
                }
                else
                {
                    Console.WriteLine($"Upload file {fileName} imported succesfully.");
                }
                importedFiles.Add(filePath);
            }
            DeleteFile(filePath);
        }

        public void SendFiles()
        {
            string[] files = FileDirUtil.GetAllFiles(path);
            foreach (string filePath in files)
            {
                SendFile(filePath);
            }
        }

        private MemoryStream GetMemoryStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            if (fileInUseChecker.IsFileInUse(filePath))
            {
                Console.WriteLine($"Cannot process the file {Path.GetFileName(filePath)}. It's being in use by another process or it has been deleted.");
                return ms;
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fileStream.CopyTo(ms);
                fileStream.Close();
            }
            return ms;
        }

        private void DeleteFile(string filePath)
        {
            if (fileInUseChecker.IsFileInUse(filePath))
            {
                Console.WriteLine($"Cannot delete the file {Path.GetFileName(filePath)}. It's being in use by another process or it has been already deleted.");
                return;
            }
            File.Delete(filePath);
        }
    }
}
