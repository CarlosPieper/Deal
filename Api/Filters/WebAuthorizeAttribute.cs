using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters
{
    public class WebAuthorizeAttribute : TypeFilterAttribute
    {
        public WebAuthorizeAttribute() : base(typeof(WebAuthorizeFilter))
        {
            Arguments = new object[] { };
        }
    }
}