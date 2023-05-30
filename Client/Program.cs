using Common;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            //var uploadPath = ConfigurationManager.AppSettings["uploadPath"];
            //FileDirUtil.CheckCreatePath(uploadPath);

            //ChannelFactory<IEstimate> channel = new ChannelFactory<IEstimate>("EstimateService");
            //IEstimate proxy = channel.CreateChannel();
        }

        private static void SendCsvFile()
        {
            var uploadPath = ConfigurationManager.AppSettings["uploadPath"];
            FileDirUtil.CheckCreatePath(uploadPath);
            FileStream fs = new FileStream(@"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Client\bin\Debug\measured_today_09.csv",
                FileMode.Open, FileAccess.Read, FileShare.Read);

            StreamReader sr = new StreamReader(fs);
            var csvReader = new CsvReader(sr, CultureInfo.InvariantCulture);

            //var uploadPath = ConfigurationManager.AppSettings["uploadPath"];
            //FileDirUtil.CheckCreatePath(uploadPath);
            ChannelFactory<IEstimate> channel = new ChannelFactory<IEstimate>("EstimateService");
            IEstimate proxy = channel.CreateChannel();
            IUploader uploader = GetUploader(GetFileSender(proxy, GetFileInUseChecker(), uploadPath), uploadPath);
            uploader.Start();
            sr.Close();
            fs.Close();

            Console.WriteLine("Uploader Client is running. Press any key to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static IFileInUseChecker GetFileInUseChecker()
        {
            return new FileInUseCommonChecker();
        }

        private static IFileSender GetFileSender(IEstimate proxy, IFileInUseChecker fileInUseChecker, string uploadPath)
        {
            return new FileSender(proxy, fileInUseChecker, uploadPath);
        }

        private static IUploader GetUploader(IFileSender fileSender, string uploadPath)
        {
            
            Console.WriteLine("Event Uploader is being used.");
            return new EventUploader(fileSender, uploadPath);
        }

        //private static void CreateCsvFile()
        //{
        //    var random = new Random();
        //    var measurments = new List<Measurement>()
        //    {
        //        new Measurement { Value = random.Next(1,101), DateOfMeasurement = DateTime.Now},
        //        new Measurement { Value = random.Next(1,101), DateOfMeasurement = DateTime.Now.AddHours(1)},
        //        new Measurement { Value = random.Next(1,101), DateOfMeasurement = DateTime.Now.AddHours(2)},
        //        new Measurement { Value = random.Next(1,101), DateOfMeasurement = DateTime.Now.AddHours(3)},
        //        new Measurement { Value = random.Next(1,101), DateOfMeasurement = DateTime.Now.AddHours(4)}
        //    };

        //    using (var writer = new StreamWriter("fileMeasurements.csv"))
        //    {
        //        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        //        {
        //            csv.WriteRecords(measurments);
        //        }
        //    }
        //}

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Create a CSV file");
            Console.WriteLine("2) Send CSV file");
            Console.WriteLine("3) Get min value");
            Console.WriteLine("4) Get max value");
            Console.WriteLine("5) Get standard deviation");
            Console.WriteLine("6) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    //CreateCsvFile();
                    return true;
                case "2":
                    SendCsvFile();
                    return true;
                case "3":
                    return true;
                case "4":
                    return true;
                case "5":
                    return true;
                case "6":
                    return false;
                default:
                    return true;
            }
        }
    }
}
