using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperDigital.DigitalAccount.CrossCutting;
using System;
using System.Linq;

namespace SuperDigital.DigitalAccount.SuperDigital.DigitalAccount.Api
{
    [Authorize]
    [ApiController]
    public class BaseController : Controller
    {
        public Guid? UserId
        {
            get
            {
                return new Guid(HttpContext.User.Claims.First(x => x.Type == "IdUser").Value);
            }
        }

        public virtual ObjectResult Result<T>(BaseResponse<T> response)
        {
            if (response.Success)
                return base.Ok(response);

            return base.BadRequest(response);
        }
    }
}
