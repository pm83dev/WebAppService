using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ValueObject

{
    // Oggetto valore per lo stato del motore
    public class MotorState
    {
        public string Value { get; private set; }

        public MotorState(string value)
        {
            Value = value;
        }
    }

    // Oggetto valore per la velocità
    public class Speed
    {
        public double SpeedSetPoint { get; private set; }
        public double SpeedProcessValue { get; private set; }

        public Speed(double sp, double pv)
        {
            SpeedSetPoint = sp;
            SpeedProcessValue = pv;
        }
    }

    // Oggetto valore per la corrente
    public class Current
    {
        public double Value { get; private set; }

        public Current(double value)
        {
            Value = value;
        }
    }

    // Oggetto valore per la coppia (torque)
    public class Torque
    {
        public double Value { get; private set; }

        public Torque(double value)
        {
            Value = value;
        }
    }

}
