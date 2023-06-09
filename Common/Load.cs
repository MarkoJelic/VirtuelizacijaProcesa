﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common
{
    [XmlType("Object")]
    [DataContract]
    public class Load
    {
        //private int id;
        //private DateTime timeStamp;
        //private double measuredValue;

        ////public Load()
        ////{
        ////}

        //public Load(int id, DateTime timeStamp, double measuredValue)
        //{
        //    this.id = new Random().Next();
        //    this.timeStamp = timeStamp;
        //    this.measuredValue = measuredValue;
        //}

        //[DataMember]
        //public int Id { get => id; set => id = value; }
        [XmlElement("Id")]
        [DataMember]
        public int Id { get; set; }

        //[DataMember]
        //public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }
        [XmlElement("TimeStamp")]
        [DataMember]
        public DateTime TimeStamp { get; set; }

        //[DataMember]
        //public double MeasuredValue { get => measuredValue; set => measuredValue = value; }
        [XmlElement("MeasuredValue")]
        [DataMember]
        public double MeasuredValue { get; set; }

    }
}
