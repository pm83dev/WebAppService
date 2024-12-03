using AutoMapper;
using Domain.Entities;
using Application.Dtos;

namespace Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mappatura da DppMachineDTO a DppMachine (già presente)
            CreateMap<DppMachineDTO, DppMachine>()
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.JobNr, opt => opt.MapFrom(src => src.JobNr))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.FinalUser, opt => opt.MapFrom(src => src.FinalUser))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ManufactureDate, opt => opt.MapFrom(src => src.ManufactureDate))
                .ForMember(dest => dest.InstallationDate, opt => opt.MapFrom(src => src.InstallationDate))
                .ForMember(dest => dest.MaterialType, opt => opt.MapFrom(src => src.MaterialType))
                .ForMember(dest => dest.ProductionRate, opt => opt.MapFrom(src => src.ProductionRate))
                .ForMember(dest => dest.TotalPower, opt => opt.MapFrom(src => src.TotalPower))
                .ForMember(dest => dest.Certification, opt => opt.MapFrom(src => src.Certification));

            // Aggiungi la mappatura da DppMachine a DppMachineDTO
            CreateMap<DppMachine, DppMachineDTO>()
                .ForMember(dest => dest.SerialNumber, opt => opt.MapFrom(src => src.SerialNumber))
                .ForMember(dest => dest.JobNr, opt => opt.MapFrom(src => src.JobNr))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Model))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.FinalUser, opt => opt.MapFrom(src => src.FinalUser))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.ManufactureDate, opt => opt.MapFrom(src => src.ManufactureDate))
                .ForMember(dest => dest.InstallationDate, opt => opt.MapFrom(src => src.InstallationDate))
                .ForMember(dest => dest.MaterialType, opt => opt.MapFrom(src => src.MaterialType))
                .ForMember(dest => dest.ProductionRate, opt => opt.MapFrom(src => src.ProductionRate))
                .ForMember(dest => dest.TotalPower, opt => opt.MapFrom(src => src.TotalPower))
                .ForMember(dest => dest.Certification, opt => opt.MapFrom(src => src.Certification));

            // Mappa ApplicationUser a UserDto
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString())) // Explicitly convert Guid to string
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            // Mappa RegisterDto a RegisterUser
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Surname, opt => opt.MapFrom(src => src.Surname))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));


        }
    }
}
