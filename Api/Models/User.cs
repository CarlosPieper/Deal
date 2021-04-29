using System;
using Api.UseCases.Users.CreateUser;
using Api.UseCases.Users.UpdateUser;

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
        public DateTime LastUpdateDate { get; set; }
        public int UserIsActive { get; set; }
        public User(CreateUserDTO user)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = user.Password;
            this.DefaultDeliveryAdress = "";
            this.CreationDate = DateTime.Now;
            this.LastUpdateDate = DateTime.Now;
            this.UserIsActive = 1;
        }

        public User(UpdateUserDTO user)
        {
            this.Id = user.Id;
            this.Name = user.Name;
            this.Email = user.Email;
            this.Password = user.NewPassword;
            this.DefaultDeliveryAdress = user.DefaultDeliveryAdress;
            this.CreationDate = user.CreationDate;
            this.LastUpdateDate = DateTime.Now;
            this.UserIsActive = user.UserIsActive;
        }

        public User()
        {

        }
    }
}