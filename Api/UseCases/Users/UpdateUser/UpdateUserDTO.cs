using System;

namespace Api.UseCases.Users.UpdateUser
{
    public class UpdateUserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
        public string DefaultDeliveryAdress { get; set; }

    }
}