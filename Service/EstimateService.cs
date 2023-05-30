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
using CsvHelper.Configuration;
using System.Xml.Serialization;
using System.Xml;

namespace Service
{
    public delegate void MyDelegate(string query);
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

        public void GetValue(string query)
        {
            string path = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\DataBase\TBL_Load.xml";

            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            //foreach
            List<Load> objekti = new List<Load>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Load));
                using (var sReader = new StreamReader(path))
                {

                    using (var reader = XmlReader.Create(sReader))
                    {
                        Load objekat = (Load)serializer.Deserialize(reader);
                        objekti.Add(objekat);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            List<double> values = new List<double>();
            foreach (Load o in objekti)
            {
                values.Add(o.MeasuredValue);
            }

            var minValue = objekti.Aggregate((min, next) => next.MeasuredValue < min.MeasuredValue ? next : min);
            var maxValue = objekti.Aggregate((max, next) => next.MeasuredValue > max.MeasuredValue ? next : max);
            double avg = values.Average();
            var stdDev = Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        public void CreateObjects(string csv_file_path)
        {
            //List<Load> objects = new List<Load>();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, MissingFieldFound = null };
            

            using (var reader = new StreamReader(csv_file_path))
            {
                using (var csv = new CsvReader(reader, configuration))
                {
                    List<Load> loadObjects = csv.GetRecords<Load>().ToList();
                    //foreach (Load objekat in loadObjects)
                    //{
                    //    if (!double.TryParse(objekat.MeasuredValue.ToString(), out double result))
                    //    {
                    //        // DataBase.Class1.UpdateDBAudit(loadObjects);
                    //    }
                    //    else
                    //    {
                    //        //DataBase.Class1.UpdateDB(loadObjects);
                    //    }
                    //}
                    DataBase.Class1.UpdateDB(loadObjects);
                }
            }

        }

        public FileManipulationResults GetFiles(FileManipulationOptions options)
        {
            Console.WriteLine($"Geting files starting with: \"{ options.FileName}\"");
            return new GetFilesHandler(GetFilesQuery(options)).GetFiles();
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
