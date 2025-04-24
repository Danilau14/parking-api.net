using Microsoft.AspNetCore.Mvc;

namespace ParkingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ParkingsLotsController : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public IActionResult CrearteParkinLot()
    {

    }

}
