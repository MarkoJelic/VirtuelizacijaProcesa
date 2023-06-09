﻿using Application;
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
using System.Text.RegularExpressions;

namespace Service
{
    public delegate void MyDelegate(string query, string timeStamp);
    public class EstimateService : IEstimate
    {
        private MyDelegate del;
        private string path;

        public EstimateService()
        {
            //Del += GetValue;
            this.path = ConfigurationManager.AppSettings["path"];
            FileDirUtil.CheckCreatePath(path);
        }

        public void GetValue1(string query, string timeStamp)
        {
            del += GetValue;
            del(query, timeStamp);
        }

        public void GetValue(string query, string timeStamp)
        {
            string path = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\DataBase\TBL_Load.xml";

            List<Load> objekti = new XmlSerialize().ConvertXmlToObjects<Load>(path);

            List<double> values = new List<double>();
            foreach (Load o in objekti)
            {
                values.Add(o.MeasuredValue);
            }

            var minValue = objekti.Aggregate((min, next) => next.MeasuredValue < min.MeasuredValue ? next : min);
            var maxValue = objekti.Aggregate((max, next) => next.MeasuredValue > max.MeasuredValue ? next : max);
            double avg = values.Average();
            var stdDev = Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));

            //string timeStampTrimmed = String.Concat(timeStamp.Where(c => !Char.IsWhiteSpace(c)));

            string fileName = @"C:\Users\Marko\source\repos\VirtuelizacijaProcesa\Service\Measurements.txt";

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (FileStream fs = File.Create(fileName))
                {
                    Byte[] title = new UTF8Encoding(true).GetBytes("");
                    //List<string> queryParts = query.Split(' ').ToList();
                    //for (int i = 0; i < queryParts.Count; i++)
                    //{
                    //    title = new UTF8Encoding(true).GetBytes($"{queryParts[i]}: ");
                    //}

                    if (query.Contains("min"))
                    {
                        title = new UTF8Encoding(true).GetBytes("Min: " + minValue.MeasuredValue);
                        fs.Write(title, 0, title.Length);
                    }
                    if (query.Contains("max"))
                    {
                        title = new UTF8Encoding(true).GetBytes("\nMax: " + maxValue.MeasuredValue);
                        fs.Write(title, 0, title.Length);
                    }
                    if (query.Contains("stdDev"))
                    {
                        title = new UTF8Encoding(true).GetBytes("\nStdDev: " + stdDev);
                        fs.Write(title, 0, title.Length);
                    }
                    
                    //else //if (query == "stdDev")
                    //{
                    //    title = new UTF8Encoding(true).GetBytes("StdDev: " + stdDev);
                    //}
                    //Byte[] title = new UTF8Encoding(true).GetBytes("Min: " + minValue.MeasuredValue + "  Max: " + maxValue.MeasuredValue + "  StdDev:" + stdDev);
                    //fs.Write(title, 0, title.Length);
                }

                
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void CreateObjects(string csv_file_path)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, MissingFieldFound = null };

            using (var reader = new StreamReader(csv_file_path))
            {
                using (var csv = new CsvReader(reader, configuration))
                {
                    List<Audit> audits = new List<Audit>();

                    try
                    {
                        List<Load> loadObjects = csv.GetRecords<Load>().ToList();

                        foreach (var obj in loadObjects)
                        {
                            if(obj.MeasuredValue < 10 || obj.MeasuredValue > 60)
                            {
                                var audit = new Audit() { Id = new Random().Next(), TimeStamp = DateTime.Now, MessageType = MsgType.Warn, Message = "Van opsega!" };
                                audits.Add(audit);
                            }
                            else
                            {
                                var audit = new Audit() { Id = new Random().Next(), TimeStamp = DateTime.Now, MessageType = MsgType.Info, Message = "Validno!" };
                                audits.Add(audit);
                            }
                        }

                        DataBase.Class1.UpdateDB(loadObjects);
                    }
                    catch (Exception e)
                    {
                        var audit = new Audit() { Id = new Random().Next(), TimeStamp = DateTime.Now, MessageType = MsgType.Err, Message = e.Message };
                        audits.Add(audit);
                    }

                    DataBase.Class1.UpdateDBAudit(audits);

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
