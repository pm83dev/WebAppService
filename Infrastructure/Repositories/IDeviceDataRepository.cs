using Domain.Entities;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Domain.Entities.MqttDeviceData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    public interface IDeviceDataRepository
    {
        Task AddDeviceDataBson(BsonDocument bsonData);
        Task<object> GetDeviceDataDb(string serialNumber);
        Task<List<Temperatures>> GetTemperatureDataBySerialAsync(string serialNumber);
        Task<List<Power>>GetPowerDataBySerialAsync(string serialNumber);
        Task<List<Motors>> GetMotorDataBySerialNumber(string serialNumber);
        Task<List<Pressures>> GetPressuresBySerialNumber(string serialNumber);
        Task<List<Standstill>> GetActualStatusBySerialNumber(string serialNumber);
        Task<List<Dictionary<string, object>>> GetMotorDataBySerialNumberToday(string serialNumber);
        Task<List<Dictionary<string, object>>> GetTempDataBySerialNumberToday(string serialNumber);
    }

    public class DeviceDataRepositoryMongoDb : IDeviceDataRepository
    {
        //private readonly IMongoCollection<MqttDeviceData> _deviceDataCollection;
        private readonly IMongoCollection<BsonDocument> _deviceDataCollection;

        public DeviceDataRepositoryMongoDb(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("MqttDeviceDb");
            _deviceDataCollection = database.GetCollection<BsonDocument>("DeviceData");
        }


        public async Task AddDeviceDataBson(BsonDocument bsonData)
        {
            try
            {
                // Salva il documento nel database MongoDB
                await _deviceDataCollection.InsertOneAsync(bsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting data: {ex.Message}");
            }
        }

        public async Task<List<BsonDocument>> GetDeviceDataBySerialAsync(string serialNumber)
        {
            // Crea un filtro per cercare il SerialNumber
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber);

            // Recupera i documenti corrispondenti dalla collezione e li ordina per Timestamp
            var data = await _deviceDataCollection.Find(filter)
                                                  .SortBy(doc => doc["Timestamp"])
                                                  .ToListAsync();

            return data; // Restituisce una lista di documenti
        }

        public async Task<object> GetDeviceDataDb(string serialNumber)
        {
            var rawData = await GetDeviceDataBySerialAsync(serialNumber);

            // Ristruttura i dati per il frontend
            var structuredData = rawData.Select(doc => new
            {
                Id = doc["_id"].ToString(),
                Timestamp = doc["Timestamp"].ToUniversalTime(),
                Data = doc["d"].AsBsonDocument.ToDictionary(k => k.Name, v => v.Value.ToString())
            }).ToList();

            return structuredData;
        }

        public async Task<List<Temperatures>> GetTemperatureDataBySerialAsync(string serialNumber)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber) &
                         Builders<BsonDocument>.Filter.Or(
                             Builders<BsonDocument>.Filter.Ne(MqttDeviceData.TempPvPropertyName, new BsonDocument()),
                             Builders<BsonDocument>.Filter.Ne(MqttDeviceData.TempSpPropertyName, new BsonDocument())
                             );

            var documents = await _deviceDataCollection.Find(filter).ToListAsync();
            var tempList = new List<Temperatures>();

            foreach (var bsonDocument in documents)
            {
                var temperatures = new Temperatures
                {
                    SerialNumber = bsonDocument["SerialNumber"].AsString,
                    Timestamp = bsonDocument["Timestamp"].ToLocalTime()
                };

                var data = bsonDocument["d"].AsBsonDocument;

                foreach (var element in data.Elements)
                {
                    if (element.Name.StartsWith(TempPvPropertyName))
                    {
                        temperatures.TempPv[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(TempSpPropertyName))
                    {
                        temperatures.TempSp[element.Name] = element.Value.ToDouble();
                    }
                }

                // Aggiungi solo se i dizionari TempPv o TempSp contengono effettivamente dati
                if (temperatures.TempPv.Any() || temperatures.TempSp.Any())
                {
                    tempList.Add(temperatures);
                }
            }

            return tempList;
        }

        public async Task<List<Power>> GetPowerDataBySerialAsync(string serialNumber) 
        {
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber) &
                         Builders<BsonDocument>.Filter.Or(
                             Builders<BsonDocument>.Filter.Ne(MqttDeviceData.PowerPvPropertyName, new BsonDocument())
                             );

            var documents = await _deviceDataCollection.Find(filter).ToListAsync();
            var powerList = new List<Power>();

            foreach (var bsonDocument in documents)
            {
                var powers = new Power
                {
                    SerialNumber = bsonDocument["SerialNumber"].AsString,
                    Timestamp = bsonDocument["Timestamp"].ToLocalTime()
                };

                var data = bsonDocument["d"].AsBsonDocument;

                foreach (var element in data.Elements)
                {
                    if (element.Name.StartsWith(PowerPvPropertyName))
                    {
                        powers.PowerPv[element.Name] = element.Value.ToDouble();
                    }
                }

                // Aggiungi solo se i dizionari TempPv o TempSp contengono effettivamente dati
                if (powers.PowerPv.Any())
                {
                    powerList.Add(powers);
                }
            }

            return powerList;


        }



        public async Task<List<Motors>> GetMotorDataBySerialNumber(string serialNumber)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber);
                         
            var documents = await _deviceDataCollection.Find(filter).ToListAsync();
            var motorsList = new List<Motors>();

            foreach (var bsonDocument in documents)
            {
                var motors = new Motors
                {
                    SerialNumber = bsonDocument["SerialNumber"].AsString,
                    Timestamp = bsonDocument["Timestamp"].ToLocalTime()
                };

                var data = bsonDocument["d"].AsBsonDocument;

                foreach (var element in data.Elements)
                {

                    if (element.Name.StartsWith(MotorSpeedSpPropertyName))
                    {
                        motors.SpeedSp[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(MotorSpeedPvPropertyName))
                    {
                        motors.SpeedPv[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(MotorTorquePvPropertyName))
                    {
                        motors.TorquePv[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(MotorCurrentPvPropertyName))
                    {
                        motors.CurrentPv[element.Name] = element.Value.ToDouble();
                    }                    
                }

                // Aggiungi solo se i dizionari contengono effettivamente dati
                if (motors.SpeedSp.Any() || motors.SpeedPv.Any() || motors.CurrentPv.Any() || motors.TorquePv.Any())
                {
                    motorsList.Add(motors);
                }
            }
                
            return motorsList;
        }

        public async Task<List<Pressures>> GetPressuresBySerialNumber(string serialNumber) 
        {
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber);
                         
            var documents = await _deviceDataCollection.Find(filter).ToListAsync();
            var pressuresList = new List<Pressures>();

            foreach (var bsonDocument in documents)
            {
                var pressure = new Pressures
                {
                    SerialNumber = bsonDocument["SerialNumber"].AsString,
                    Timestamp = bsonDocument["Timestamp"].ToLocalTime()
                };

                var data = bsonDocument["d"].AsBsonDocument;

                foreach (var element in data.Elements)
                {

                    if (element.Name.StartsWith(PressPvInletPropertyName))
                    {
                        pressure.PressInlet[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(PressPvOutletPropertyName))
                    {
                        pressure.PressOutlet[element.Name] = element.Value.ToDouble();
                    }
                    else if (element.Name.StartsWith(PressPvRampackPropertyName))
                    {
                        pressure.PressRampack[element.Name] = element.Value.ToDouble();
                    }

                }

                // Aggiungi solo se i dizionari contengono effettivamente dati
                if (pressure.PressInlet.Any() || pressure.PressOutlet.Any() || pressure.PressRampack.Any())
                {
                    pressuresList.Add(pressure);
                }
            }
            return pressuresList;
        }

        public async Task<List<Standstill>> GetActualStatusBySerialNumber(string serialNumber) 
        {
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber);

            var documents = await _deviceDataCollection.Find(filter).ToListAsync();
            var standstillList = new List<Standstill>();

            foreach (var bsonDocument in documents)
            {
                var code = new Standstill
                {
                    SerialNumber = bsonDocument["SerialNumber"].AsString,
                    Timestamp = bsonDocument["Timestamp"].ToLocalTime()
                };

                var data = bsonDocument["d"].AsBsonDocument;

                foreach (var element in data.Elements)
                {

                    if (element.Name.StartsWith(StandstillCodePropertyName))
                    {
                        code.codeValue[element.Name] = element.Value.ToString();
                    }
                }

                // Aggiungi solo se i dizionari contengono effettivamente dati
                if (code.codeValue.Any())
                {
                    standstillList.Add(code);
                }
            }
            return standstillList;
        }

        public async Task<List<Dictionary<string, object>>> GetMotorDataBySerialNumberToday(string serialNumber)
        {
            var startDate = DateTime.UtcNow.Date;
            var endDate = DateTime.UtcNow;

            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber) &
                         Builders<BsonDocument>.Filter.Gte("Timestamp", startDate) &
                         Builders<BsonDocument>.Filter.Lte("Timestamp", endDate);

            var projection = Builders<BsonDocument>.Projection.Include("Speeds").Exclude("_id");

            var bsonResults = await _deviceDataCollection.Find(filter).Project<BsonDocument>(projection).ToListAsync();

            return bsonResults.Select(bson => bson.ToDictionary()).ToList();
        }


        public async Task<List<Dictionary<string, object>>> GetTempDataBySerialNumberToday(string serialNumber)
        {
            var startDate = DateTime.Today;
            var endDate = DateTime.Now;
            
            var filter = Builders<BsonDocument>.Filter.Eq("SerialNumber", serialNumber) &
                         Builders<BsonDocument>.Filter.Gte("Timestamp", startDate) &
                         Builders<BsonDocument>.Filter.Lte("Timestamp", endDate);
            
            var projection = Builders<BsonDocument>.Projection.Include("Temperatures").Exclude("_id");
            
            var results = await _deviceDataCollection
                .Find(filter)
                .Project<Dictionary<string, object>>(projection)
                .ToListAsync();

            return results;
        }





    }
}
