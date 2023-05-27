﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class Load
    {
        private int id;
        private DateTime timeStamp;
        private double measuredValue;

        public Load()
        {
        }

        public Load(int id, DateTime timeStamp, double measuredValue)
        {
            this.id = id;
            this.timeStamp = timeStamp;
            this.measuredValue = measuredValue;
        }

        [DataMember]
        public int Id { get => id; set => id = value; }

        [DataMember]
        public DateTime TimeStamp { get => timeStamp; set => timeStamp = value; }

        [DataMember]
        public double MeasuredValue { get => measuredValue; set => measuredValue = value; }

    }
}