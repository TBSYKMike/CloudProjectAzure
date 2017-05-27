using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace CouldProjectAzureV2
{

    public class Entity : TableEntity
        {

            public Entity(string partitionkey, string rowkey)
            : base(partitionkey, rowkey)
            { }

            public Entity() { }
                       
            public string SensorAccelerometerX { get; set; }
            public string SensorAccelerometerY { get; set; }
            public string SensorAccelerometerZ { get; set; }
            public string SensorLight { get; set; }
            public string SensorProximity { get; set; }
            public string METAData { get; set; }
            public string BatteryLevel { get; set; }
    }
    }
