using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    //public delegate void MyDelegate(string query, string timeStamp);

    [ServiceContract]
    public interface IEstimate
    {
        [OperationContract]
        void GetValue(string query, string timeStamp);

        [OperationContract]
        FileManipulationResults SendFile(FileManipulationOptions options);

        [OperationContract]
        FileManipulationResults GetFiles(FileManipulationOptions options);

        [OperationContract]
        void CreateObjects(string csv_file_path);

    }
}
