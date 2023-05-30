using Application;
using Common;
using CsvHelper;
using FileSystemManipulation;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
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
        IEnumerable<Load> objects;

        public EstimateService()
        {
            Del += GetValue;
            this.path = ConfigurationManager.AppSettings["path"];
            FileDirUtil.CheckCreatePath(path);
            objects = CreateObjects(@"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Service\fileMeasurements.csv");
        }

        public IEnumerable<Load> CreateObjects(string csv_file_path)
        {
            //List<Load> objects = new List<Load>();

            using (var reader = new StreamReader(csv_file_path))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Load>();
                    return records;
                }
            }

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
