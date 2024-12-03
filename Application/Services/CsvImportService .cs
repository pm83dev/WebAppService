using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Domain.Entities;
using Application.Dtos;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Repositories;



namespace Application
{
    public class CsvImportService
    {
        private readonly IDppMachineRepository _repository;
        private readonly IMapper _mapper;
        private readonly string _csvFilePath;

        public CsvImportService(IDppMachineRepository repository, string csvFilePath, IMapper mapper)
        {
            _repository = repository;
            _csvFilePath = csvFilePath;
            _mapper = mapper;
        }

        public async Task ImportCsvToDatabase(string csvFilePath)
        {
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                BadDataFound = context => { /* handle bad data */ },
                MissingFieldFound = null,
                HeaderValidated = null,
                IgnoreBlankLines = true
            };

            try
            {
                using (var reader = new StreamReader(csvFilePath))
                using (var csv = new CsvReader(reader, csvConfig))
                {
                    // Usa la mappatura per il DTO
                    csv.Context.RegisterClassMap<DppMachineDtoMap>();

                    // Leggi i record dal CSV e mappa ai DTO
                    var records = csv.GetRecords<DppMachineDTO>().ToList();

                    var dppMachines = new List<DppMachine>();

                    foreach (var record in records)
                    {
                        if (!await _repository.ExistsAsync(record.SerialNumber))
                        {
                            // Mappa il DTO in entità
                            var dppMachine = _mapper.Map<DppMachine>(record);
                            dppMachines.Add(dppMachine);
                        }
                    }

                    // Salva i dati nel database
                    await _repository.AddRangeAsync(dppMachines);
                    await _repository.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public sealed class DppMachineDtoMap : ClassMap<DppMachineDTO>
        {
            public DppMachineDtoMap()
            {
                Map(m => m.SerialNumber).Name("SerialNumber");
                Map(m => m.JobNr).Name("JobNr");
                Map(m => m.Model).Name("Model");
                Map(m => m.Description).Name("Description");
                Map(m => m.Customer).Name("Customer");
                Map(m => m.FinalUser).Name("FinalUser");
                Map(m => m.Country).Name("Country");
                Map(m => m.ManufactureDate).Name("ManufactureDate");
                Map(m => m.InstallationDate).Name("InstallationDate");
                Map(m => m.MaterialType).Name("MaterialType");
                Map(m => m.ProductionRate).Name("ProductionRate");
                Map(m => m.TotalPower).Name("TotalPower");
                Map(m => m.Certification).Name("Certification");
            }
        }
    }
}
