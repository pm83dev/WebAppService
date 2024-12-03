using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ValueObject;

namespace Domain.Entities
{
    public class MotorEnt
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public MotorState State { get; private set; }
        public Speed Speed { get; private set; }
        public Current Current { get; private set; }
        public Torque Torque { get; private set; }

        public MotorEnt(int id, string name)
        {
            Id = id;
            Name = name;
            State = new MotorState("Stopped");
            Speed = new Speed(0, 0); // velocità attuale e setpoint
            Current = new Current(0);
            Torque = new Torque(0);
        }

        // Metodi per aggiornare il motore
        public void UpdateState(string state)
        {
            State = new MotorState(state);
        }

        public void UpdateSpeed(double sp, double pv)
        {
            Speed = new Speed(sp, pv);
        }

        public void UpdateCurrent(double current)
        {
            Current = new Current(current);
        }

        public void UpdateTorque(double torque)
        {
            Torque = new Torque(torque);
        }
    }
}
