using System;
using Api.UseCases.Users.CreateUser;

namespace Api.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DefaultDeliveryAdress { get; set; }
        public DateTime CreationDate { get; set; }
        public User(CreateUserDTO user)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = user.Password;
            this.CreationDate = new DateTime();
        }
    }
}