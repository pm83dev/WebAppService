using Application.Dtos;
using AutoMapper;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DppMachineService
    {
        private readonly IDppMachineRepository _repository;
        private readonly IMapper _mapper;

        
        public DppMachineService(IDppMachineRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<DppMachineDTO>> GetAllMachinesAsync()
        {
            var machines = await _repository.GetAllMachinesAsync();
            return _mapper.Map<IEnumerable<DppMachineDTO>>(machines);
        }

    }
}
