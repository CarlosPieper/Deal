using Microsoft.AspNetCore.Mvc;

namespace Api.Filters
{
    public class WebAuthorizeAttribute : TypeFilterAttribute
    {
        public WebAuthorizeAttribute() : base(typeof(WebAuthorizeFilter))
        {

        }
    }
}