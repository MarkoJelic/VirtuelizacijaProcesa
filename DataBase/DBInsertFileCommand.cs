﻿using Application;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class DBInsertFileCommand : ICommand
    {
        private readonly IDataBase dataBase;
        private readonly FileManipulationOptions options;

        public DBInsertFileCommand(IDataBase dataBase, FileManipulationOptions options)
        {
            this.dataBase = dataBase;
            this.options = options;
        }

        public void Execute()
        {
            if (options.MS == null || options.MS.Length == 0)
            {
                throw new IncorrectDataException("Memory stream does not contain data!");
            }
            InsertFile(options.MS, options.FileName);
            options.Dispose();
        }

        private void InsertFile(MemoryStream ms, string fileName)
        {
            var bytes = GetFileData(ms);
            dataBase.InsertFile(fileName, bytes);
        }

        /// <summary>
        /// Metod treba da kreira i vrati niz bajtova od prosledjenog memory stream-a
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        private byte[] GetFileData(MemoryStream ms)
        {
            // IMPLEMENTIRATI
            return ms.ToArray();
        }
    }
}
