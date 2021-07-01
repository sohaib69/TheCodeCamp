using System;
using System.Web.Http;
using System.Configuration;

namespace TheCodeCamp.Controllers
{
    public class OperationalController : ApiController
    {
        [HttpOptions]
        [Route("api/refreshconfig")]
        public IHttpActionResult RefreshAppSettings()
        {
            try
            {
                ConfigurationManager.RefreshSection("AppSettings");
                return Ok();
            }
            catch (Exception ex) { return InternalServerError(ex); }
        }
    }
}