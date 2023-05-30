using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public interface IDataBase
    {
        bool InsertFile(string fileName, byte[] fileData);
        Dictionary<string, byte[]> GetFileData(string fileBeginsWith);
    }
}
