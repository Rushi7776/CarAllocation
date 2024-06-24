using CarAllocation.Model;
using CarAllocation.Service;
using Microsoft.AspNetCore.Mvc;

namespace CarAllocation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParkingController : ControllerBase
    {
        private readonly ParkingService _parkingService;

        public ParkingController(ParkingService parkingService)
        {
            _parkingService = parkingService ?? throw new ArgumentNullException(nameof(parkingService));
        }

        [HttpPost("allocate")]
        public IActionResult AllocateParking([FromBody] ParkingAllocation request)
        {
            try
            {
                var allocatedParking = _parkingService.AllocateParking(request.CarPlateNumber);
                return Ok(allocatedParking);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetParkingList(int pageNumber = 1, int pageSize = 10)
        {
            var parkingList = _parkingService.GetParkingList(pageNumber, pageSize);
            return Ok(parkingList);
        }
    }
}
