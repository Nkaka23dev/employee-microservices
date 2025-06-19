using Microsoft.AspNetCore.Mvc;

namespace TimeTrackingService.Controllers
{
    [Route("api/t/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        [HttpPost]
        public ActionResult CheckInBoundConnection()
        {
            Console.WriteLine("--> In Bound #TimeTrackingService");
            return Ok("In bound test okay");
        }
    }
}
