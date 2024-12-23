﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class DppMachineDTO
    {
        public string SerialNumber { get; set; }
        public string JobNr { get; set; }
        public string Model { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }
        public string FinalUser { get; set; }
        public string Country { get; set; }
        public string ManufactureDate { get; set; }
        public string InstallationDate { get; set; }
        public string MaterialType { get; set; }
        public string ProductionRate { get; set; }
        public string TotalPower { get; set; }
        public string Certification { get; set; }

    }
}
