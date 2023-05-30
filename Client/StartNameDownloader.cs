using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class StartNameDownloader : IDownloader
    {
        private readonly IEstimate proxy;
        private readonly string path;

        public StartNameDownloader(IEstimate proxy, string path)
        {
            this.proxy = proxy;
            this.path = path;
        }

        public void Download(string fileNameTemplate)
        {
            var fileResults = proxy.GetFiles(new FileManipulationOptions(null, fileNameTemplate));
            ResultTypes resultType = fileResults.ResultType;
            switch (resultType)
            {
                case ResultTypes.Failed:
                    Console.WriteLine($"Error: {fileResults.ResultMessage}");
                    break;
                case ResultTypes.Warning:
                    Console.WriteLine($"Warning: {fileResults.ResultMessage}");
                    break;
                default:
                    Console.WriteLine(fileResults.ResultMessage);
                    DownloadFiles(fileResults.MSs);
                    break;
            }
            fileResults.Dispose();
        }

        private void DownloadFiles(Dictionary<string, MemoryStream> MSs)
        {
            foreach (KeyValuePair<string, MemoryStream> stream in MSs)
            {
                DownloadFile(stream.Key, stream.Value);
            }
        }

        private void DownloadFile(string fileName, MemoryStream stream)
        {
            // IMPLEMENTIRATI
            var fs = new FileStream($"{ path }\\{fileName}", FileMode.Create, FileAccess.Write);
            stream.WriteTo(fs);
            fs.Close();
            fs.Dispose();
            stream.Dispose();
            Console.WriteLine($"Downloaded file {fileName}");
        }
    }
}
