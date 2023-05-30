using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataBase
{
    public class Class1
    {
        public static void UpdateDB(List<Load> lista)
        {
            string savePath = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\DataBase\TBL_Load.xml";
            var xmlSavePath = new XElement("TBL_Load", from obj in lista select new XElement("Object", new XElement("Id", obj.Id),
                                                                                                       new XElement("TimeStamp", obj.TimeStamp),
                                                                                                       new XElement("MeasuredValue", obj.MeasuredValue)));
            xmlSavePath.Save(savePath);
        }

        public static void UpdateDBAudit(List<Audit> lista)
        {
            string savePath = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\DataBase\TBL_Audit.xml";
            var xmlSavePath = new XElement("TBL_Audit", from obj in lista
                                                       select new XElement("Object", new XElement("Id", obj.Id),
                                                                                     new XElement("TimeStamp", obj.TimeStamp),
                                                                                     new XElement("MessageType", obj.MessageType),
                                                                                     new XElement("Message", obj.Message)));
            xmlSavePath.Save(savePath);
        }
    }
}
