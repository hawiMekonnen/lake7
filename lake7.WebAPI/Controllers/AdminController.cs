using lake7.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly IDeliveryService _deliveryService;
        private readonly IPaymentService _paymentService;

        public AdminController(
            IDriverService driverService,
            IRideService rideService,
            IDeliveryService deliveryService,
            IPaymentService paymentService)
        {
            _driverService = driverService;
            _rideService = rideService;
            _deliveryService = deliveryService;
            _paymentService = paymentService;
        }

        [HttpPost("drivers/{driverId}/approve")]
        public async Task<IActionResult> ApproveDriver(Guid driverId)
        {
            var driver = await _driverService.SetApprovalStatusAsync(driverId, true);
            if (driver == null) return NotFound();
            return Ok(driver);
        }

        [HttpPost("drivers/{driverId}/suspend")]
        public async Task<IActionResult> SuspendDriver(Guid driverId)
        {
            var driver = await _driverService.SetApprovalStatusAsync(driverId, false);
            if (driver == null) return NotFound();
            return Ok(driver);
        }


        //View all rides
        [HttpGet("rides")]
        public async Task<IActionResult> GetAllRides()
        {
            var rides = await _rideService.GetAllRidesAsync();
            return Ok(rides);
        }

        //View all deliveries
        [HttpGet("deliveries")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }

        //View all payments
        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            return Ok(payments);
        }

        //Handle disputes/cancellations
        [HttpPost("rides/{rideId}/cancel")]
        public async Task<IActionResult> CancelRide(Guid rideId)
        {
            var cancelledRide = await _rideService.UpdateRideStatusAsync(rideId, Domain.Enums.RideStatus.Cancelled);
            if (cancelledRide == null) return NotFound();
            return Ok(cancelledRide);
        }
    }
}
