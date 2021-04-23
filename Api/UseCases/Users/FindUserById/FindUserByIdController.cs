using System;
using Api.Filters;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.UseCases.Users.FindUserById
{
    [ApiController]
    [Route("FindUserById")]
    public class FindUserByIdController : Controller
    {
        private IUserRepository _repository;
        public FindUserByIdController(IUserRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        [WebAuthorize]
        public IActionResult Execute(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest(new { message = "No id provided." });

            try
            {
                var user = this._repository.FindById(id);

                if (user == null)
                    return BadRequest(new { message = "User not found." });

                user.Password = "";

                return Ok(new { user });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}