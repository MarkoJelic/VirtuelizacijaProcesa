using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IEstimate
    {
        [OperationContract]
        void GetValue(string query);

        [OperationContract]
        FileManipulationResults SendFile(FileManipulationOptions options);

        [OperationContract]
        FileManipulationResults GetFiles(FileManipulationOptions options);

        [OperationContract]
        void CreateObjects(string csv_file_path);

        //[OperationContract]
        //Load GetMax();

        //[OperationContract]
        //Load GetStand();
    }
}
