using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class DppMachine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string SerialNumber { get; private set; }

        [Required]
        public string JobNr { get; private set; }

        [Required]
        public string Model { get; private set; }

        [Required]
        public string Description { get; private set; }

        [Required]
        public string Customer { get; private set; }

        [Required]
        public string FinalUser { get; private set; }

        public string Country { get; private set; }
        public string ManufactureDate { get; private set; }
        public string InstallationDate { get; private set; }
        public string MaterialType { get; private set; }
        public string? ProductionRate { get; private set; }
        public string? TotalPower { get; private set; }
        public string? Certification { get; private set; }

        // Constructor for DppMachine entity
        public DppMachine(string serialNumber, string jobNr, string model, string description,
                          string customer, string finalUser, string country, string manufactureDate,
                          string installationDate, string materialType, string productionRate,
                          string totalPower, string certification)
        {
            SerialNumber = serialNumber;
            JobNr = jobNr;
            Model = model;
            Description = description;
            Customer = customer;
            FinalUser = finalUser;
            Country = country;
            ManufactureDate = manufactureDate;
            InstallationDate = installationDate;
            MaterialType = materialType;
            ProductionRate = productionRate;
            TotalPower = totalPower;
            Certification = certification;
        }

        // Example method for updating the production rate
        public void UpdateProductionRate(string newRate)
        {
            ProductionRate = newRate;
        }

        // Example validation method
        public bool IsCertificationValid()
        {
            // Logica per validare la certificazione
            return !string.IsNullOrWhiteSpace(Certification);
        }
    }
}
