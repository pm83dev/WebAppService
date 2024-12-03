using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application;
using Application.Services;
using Application.Commands;


namespace Application
{
    public static class InjectAppServices
    {
        public static IServiceCollection InjectApplicationServices(this IServiceCollection self) 
        {
            self.AddScoped<IAccountService, AccountService>();
            self.AddScoped<IDeviceDataService, DeviceDataService>();
            self.AddScoped<DppMachineService>();
            self.AddAutoMapper(typeof(MappingProfile));
            self.AddHostedService<MqttBackgroundService>();
            return self;
        
        }
    }
}
