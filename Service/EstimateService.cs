using Application;
using Common;
using FileSystemManipulation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public delegate Load MyDelegate(string query);
    public class EstimateService : IEstimate
    {
        MyDelegate Del;
        private string path;

        public EstimateService()
        {
            Del += GetValue;
            this.path = ConfigurationManager.AppSettings["path"];
            FileDirUtil.CheckCreatePath(path);
        }

        public FileManipulationResults GetFiles(FileManipulationOptions options)
        {
            throw new NotImplementedException();
        }

        public Load GetValue(string query)
        {
            throw new NotImplementedException();
        }

        public FileManipulationResults SendFile(FileManipulationOptions options)
        {
            Console.WriteLine($"Receiving file with name: \"{ options.FileName}\"");
            return new InsertFileHandler(GetInsertFileCommand(options)).InsertFile();
        }

        private ICommand GetInsertFileCommand(FileManipulationOptions options)
        {
            return new FileSystemInsertFileCommand(options, path);
        }
    }
}
