using Common;
using CsvHelper;
using Service;
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
    //public delegate void MyDelegate(string query);
    class Program
    {
        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

        }

        private static void GetValues()
        {
            string downloadPath = ConfigurationManager.AppSettings["downloadPath"];
            FileDirUtil.CheckCreatePath(downloadPath);

            ChannelFactory<IEstimate> channel = new ChannelFactory<IEstimate>("EstimateService");
            IEstimate proxy = channel.CreateChannel();
            //proxy.Del()
            string s;
            IDownloader downloader = GetDownloader(proxy, downloadPath);
            do
            {
                Console.WriteLine("Type min, max or stdDev and Enter to download. Press only Enter for exit");
                s = Console.ReadLine();
                if (s != null && !string.IsNullOrEmpty(s))
                {
                    //if (s == "min")
                    //{
                    //    downloader.Download(s);
                    //}
                    string vreme = DateTime.Now.ToString();
                    proxy.GetValue(s, vreme);
                    downloader.Download("Measurements.txt");
                    Console.WriteLine($"Ime preuzete datoteke: Measurements\nPutanja do datoteke: {downloadPath}Measurements.txt");
                }


            } while (s != null && !string.IsNullOrEmpty(s));
        }

        private static IDownloader GetDownloader(IEstimate proxy, string path)//, StorageTypes storageType)
        {
            //return new(proxy, path);//, storageType);
            return new StartNameDownloader(proxy, path);
        }

        private static void SendCsvFile()
        {
            var uploadPath = ConfigurationManager.AppSettings["uploadPath"];
            FileDirUtil.CheckCreatePath(uploadPath);
            
            ChannelFactory<IEstimate> channel = new ChannelFactory<IEstimate>("EstimateService");
            IEstimate proxy = channel.CreateChannel();
            IUploader uploader = GetUploader(GetFileSender(proxy, GetFileInUseChecker(), uploadPath), uploadPath);
            uploader.Start();
            proxy.CreateObjects(@"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Service\fileMeasurements.csv");

            // dodano za probu, IZBRISATI
            //proxy.GetValue("string test", "string test");
            

            Console.WriteLine("Uploader Client is running. Press any key to return to menu.");
            Console.ReadLine();
            //Environment.Exit(0);
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

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("'Send' to send the CSV file");
            Console.WriteLine("'Get' to select which value to receive");
            Console.WriteLine("'Exit' to Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "Send":
                    SendCsvFile();
                    return true;
                case "Get":
                    GetValues();
                    return true;
                case "Exit":
                    return false;
                default:
                    return true;
            }
        }
    }
}
