using Application;
using Common;
using DataBase;
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
using System.Xml.Linq;

namespace Service
{
    public delegate Load MyDelegate(string query);
    public class EstimateService : IEstimate
    {
        MyDelegate Del;
        private string path;
        //IEnumerable<Load> objects;

        public EstimateService()
        {
            Del += GetValue;
            this.path = ConfigurationManager.AppSettings["path"];
            FileDirUtil.CheckCreatePath(path);
            //objects = CreateObjects(@"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Service\fileMeasurements.csv");
            //Console.WriteLine(objects.Count());
        }

        public void CreateObjects(string csv_file_path)
        {
            //List<Load> objects = new List<Load>();

            using (var reader = new StreamReader(csv_file_path))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Load>();
                    // Pozvati metodu iz DataBase
                    List<Load> loadObjects = records.ToList();
                    DataBase.Class1.UpdateDB(loadObjects);

                    //List<Load> loadObjects = records.ToList();
                    //XElement xmlElements = new XElement("DataBase.TBL_LOAD", loadObjects.Select(i => new XElement("objekat", i)));
                }
            }

        }

        public FileManipulationResults GetFiles(FileManipulationOptions options)
        {
            Console.WriteLine($"Geting files starting with: \"{ options.FileName}\"");
            return new GetFilesHandler(GetFilesQuery(options)).GetFiles()
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

        private IQuery GetFilesQuery(FileManipulationOptions options)
        {
            return new FileSystemGetFilesQuery(options, path);
        }
    }
}
