using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Repositories;
using MongoDB.Bson;
using static Domain.Entities.MqttDeviceData;


namespace Application.Commands
{
    public interface IDeviceDataService
    {
        Task<object> GetDeviceData(string serialNumber);

        Task<List<Temperatures>> GetTemperatureDataBySerialNumber(string serialNumber);

        Task<List<Motors>> GetMotorDataBySerialNumber(string serialNumber);

        Task<List<Dictionary<string, object>>> GetMotorDataBySerialNumberToday(string serialNumber);

        Task<List<Dictionary<string, object>>> GetTempDataBySerialNumberToday(string serialNumber);

        Task<List<Pressures>> GetPressuresDataBySerialNumber(string serialNumber);
        Task<List<Power>> GetPowerDataBySerialNumber(string serialNumber);
        Task<List<Standstill>> GetStandStillCode(string serialNumber);
    }

    public class DeviceDataService : IDeviceDataService
    {
        private readonly IDeviceDataRepository _deviceDataRepository;

        public DeviceDataService(IDeviceDataRepository deviceDataRepository)
        {
            _deviceDataRepository = deviceDataRepository;
        }

        public async Task AddDeviceData(BsonDocument data)
        {
            await _deviceDataRepository.AddDeviceDataBson(data);
        }


        public async Task<object> GetDeviceData(string serialNumber)
        {
            return await _deviceDataRepository.GetDeviceDataDb(serialNumber);
        }

        public async Task<List<Temperatures>> GetTemperatureDataBySerialNumber(string serialNumber)
        {
            return await _deviceDataRepository.GetTemperatureDataBySerialAsync(serialNumber);
        }

        public async Task<List<Motors>> GetMotorDataBySerialNumber(string serialNumber)
        {
            return await _deviceDataRepository.GetMotorDataBySerialNumber(serialNumber);
        }

        public async Task<List<Pressures>> GetPressuresDataBySerialNumber(string serialNumber) 
        { 
            return await _deviceDataRepository.GetPressuresBySerialNumber(serialNumber);
        }

        public async Task<List<Power>> GetPowerDataBySerialNumber(string serialNumber)
        { 
            return await _deviceDataRepository.GetPowerDataBySerialAsync(serialNumber);
        }

        public async Task<List<Standstill>> GetStandStillCode(string serialNumber) 
        {
            return await _deviceDataRepository.GetActualStatusBySerialNumber(serialNumber);
        }

        public async Task<List<Dictionary<string, object>>> GetMotorDataBySerialNumberToday(string serialNumber)
        {
            return await _deviceDataRepository.GetMotorDataBySerialNumberToday(serialNumber);
        }

        public async Task<List<Dictionary<string, object>>> GetTempDataBySerialNumberToday(string serialNumber)
        {
            return await _deviceDataRepository.GetTempDataBySerialNumberToday(serialNumber);
        }

        


    }
}
