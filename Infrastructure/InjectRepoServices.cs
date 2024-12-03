using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class InjectRepoServices
    {
        public static IServiceCollection InjectRepositoryServices(this IServiceCollection self)
        {
            self.AddScoped<IDeviceDataRepository, DeviceDataRepositoryMongoDb>();
            self.AddScoped<IDppMachineRepository, DppMachineRepositoryPGSql>();
            return self;
        }
    }
}
