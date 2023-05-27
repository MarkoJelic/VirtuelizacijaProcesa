using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class FileManipulationOptions : IDisposable
    {
        public FileManipulationOptions(MemoryStream ms, string fileName)//, StorageTypes storageType)
        {
            this.MS = ms;
            this.FileName = fileName;
            //this.StorageType = storageType;
        }

        /// <summary>
        /// IMPLEMENTIRATI oslobadjanje memorije
        /// </summary>
        [DataMember]
        public MemoryStream MS { get; set; }

        [DataMember]
        public string FileName { get; set; }

        //[DataMember]
        //public StorageTypes StorageType { get; set; }

        public void Dispose()
        {
            if (MS == null)
                return;
            try
            {
                MS.Dispose();
                MS.Close();
                MS = null;
            }
            catch (System.Exception)
            {
                Console.WriteLine("Unsuccesful disposing!");
            }
        }
    }
}
