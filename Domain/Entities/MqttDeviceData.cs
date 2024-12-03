using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MqttDeviceData
    {
        // Costanti per identificare i prefissi dei campi
        public const string TempPvPropertyName = "Temp_PV_zone";
        public const string TempSpPropertyName = "Temp_SP_zone";
        public const string CurrentPvPropertyName = "Current_PV_zone";
        public const string MotorSpeedSpPropertyName = "Speed_SP";
        public const string MotorSpeedPvPropertyName = "Speed_PV";
        public const string PowerPvPropertyName = "Power_PV";
        public const string MotorTorquePvPropertyName = "Torque_PV";
        public const string MotorCurrentPvPropertyName = "Current_PV";
        public const string PressPvRampackPropertyName = "Press_PV_Rampack";
        public const string PressPvInletPropertyName = "Press_PV_Inlet";
        public const string PressPvOutletPropertyName = "Press_PV_Outlet";
        public const string StandstillCodePropertyName = "StandStill";



        // Classi per organizzare i dati
        public required Temperatures TemperatureData { get; set; }
        public required Motors MotorData { get; set; }
        public required Pressures PressureData { get; set; }

        // Classi interne per raggruppare i dati
        public class Temperatures
        {
            public required string SerialNumber { get; set; }
            public DateTime Timestamp { get; set; } // Timestamp
            public Dictionary<string, object> TempPv { get; set; } = new();
            public Dictionary<string, object> TempSp { get; set; } = new();
        }

        public class Motors
        {
            public required string SerialNumber { get; set; }
            public DateTime Timestamp { get; set; } // Timestamp
            public Dictionary<string, object> SpeedPv { get; set; } = new();
            public Dictionary<string, object> SpeedSp { get; set; } = new();
            public Dictionary<string, object> TorquePv { get; set; } = new();
            public Dictionary<string, object> CurrentPv { get; set; } = new();
        }

        public class Power 
        {
            public required string SerialNumber { get; set; }
            public DateTime Timestamp { get; set; } // Timestamp
            public Dictionary<string, object> PowerPv { get; set; } = new();
        }

        public class Pressures
        {
            public required string SerialNumber { get; set; }
            public DateTime Timestamp { get; set; } // Timestamp
            public Dictionary<string, object> PressRampack { get; set; } = new();
            public Dictionary<string, object> PressInlet { get; set; } = new();
            public Dictionary<string, object> PressOutlet { get; set; } = new();
        }

        public class Standstill
        {
            public required string SerialNumber { get; set; }
            public DateTime Timestamp { get; set; } // Timestamp
            public Dictionary<string, object> codeValue { get; set; } = new();
        }
    }
}