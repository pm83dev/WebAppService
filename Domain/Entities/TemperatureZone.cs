using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TemperatureZone
    {
        public string Zone { get; private set; }
        public Temperature Temperature { get; private set; }
        public DateTime Timestamp { get; private set; }

        public TemperatureZone(string zone, double temperature, DateTime timestamp)
        {
            Zone = zone;
            Temperature = new Temperature(temperature);
            Timestamp = timestamp;
        }

        public void UpdateTemperature(double temperature, DateTime timestamp)
        {
            Temperature = new Temperature(temperature);
            Timestamp = timestamp;
        }
    }

    public class Temperature
    {
        public double Value { get; private set; }

        public Temperature(double value)
        {
            Value = value;
        }
    }
}
