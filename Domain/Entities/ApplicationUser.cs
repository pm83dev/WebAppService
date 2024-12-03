using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class ApplicationUser:IdentityUser
    {
        /* Se è necessario aggiungere altri campi inserirli qui
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        // Altri campi personalizzati
        */
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Company { get; set; }
        public string PhoneNumber { get; set; }
    }
}
