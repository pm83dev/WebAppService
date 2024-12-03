using Application.Commands;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Text.Json;
using static Domain.Entities.MqttDeviceData;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceDataController : ControllerBase
    {
        private readonly IDeviceDataService _deviceDataService;

        public DeviceDataController(IDeviceDataService deviceDataService)
        {
            _deviceDataService = deviceDataService;
        }

        [HttpGet("{serialNumber}")]
        public async Task<ActionResult<MqttDeviceData>> GetDeviceData(string serialNumber)
        {
            var data = await _deviceDataService.GetDeviceData(serialNumber);
            
            if (data == null)
            {
                return NotFound();
            }
            
            return Ok(data);
        }


        [HttpGet("temperature/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetTemperatureDataBySN(string serialNumber)
        {
            try
            {
                var dataTemp = await _deviceDataService.GetTemperatureDataBySerialNumber(serialNumber);
                return Ok(dataTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("motor/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetMotorDataBySN(string serialNumber)
        {
            try
            {
                var dataMotor= await _deviceDataService.GetMotorDataBySerialNumber(serialNumber);
                return Ok(dataMotor);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("pressure/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetPressureDataBySN(string serialNumber) 
        {
            try
            {
                var dataPressures = await _deviceDataService.GetPressuresDataBySerialNumber(serialNumber);
                return Ok(dataPressures);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        
        }

        [HttpGet("power/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetPowerDataBySN(string serialNumber) 
        {
            try
            {
                var dataPower = await _deviceDataService.GetPowerDataBySerialNumber(serialNumber);
                return Ok(dataPower);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

        [HttpGet("status/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetStatusDataBySN(string serialNumber) 
        {
            try
            {
                var dataStatus = await _deviceDataService.GetStandStillCode(serialNumber);
                return Ok(dataStatus);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("motor/today/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetMotorDataBySNTodayFilter(string serialNumber)
        {
            try
            {
                var dataTemp = await _deviceDataService.GetMotorDataBySerialNumberToday(serialNumber);
                return Ok(dataTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("temperature/today/{serialNumber}")]
        public async Task<ActionResult<List<Dictionary<string, object>>>> GetTempDataBySNToday(string serialNumber)
        {
            try
            {
                var dataTemp = await _deviceDataService.GetTempDataBySerialNumberToday(serialNumber);
                return Ok(dataTemp);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
            
        }

    }
}
