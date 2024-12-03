using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DppMachineController : ControllerBase
    {
        private readonly DppMachineService _service;

        public DppMachineController(DppMachineService service)
        {
            _service = service;
        }

        // API per ottenere tutte le macchine
        [HttpGet("MachineList")]
        public async Task<IActionResult> GetAll()
        {
            var machinesDto = await _service.GetAllMachinesAsync();
            return Ok(machinesDto);
        }
    }
}
