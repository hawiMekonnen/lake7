using lake7.Application.Interface;
using lake7.Domain.Entities;
using lake7.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace lake7.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IRideService _rideService;
        private readonly IDeliveryService _deliveryService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IConfiguration _configuration;

        public AdminController(
            IDriverService driverService,
            IRideService rideService,
            IDeliveryService deliveryService,
            IPaymentService paymentService,
            IUserService userService,
            IAdminService adminService,
            IConfiguration configuration)
        {
            _driverService = driverService;
            _rideService = rideService;
            _deliveryService = deliveryService;
            _paymentService = paymentService;
            _userService = userService;
            _adminService = adminService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var admin = await _adminService.LoginAsync(request.Email, request.Password);
            if (admin == null) return Unauthorized(new { message = "Invalid credentials" });

            var token = GenerateJwtToken(admin);
            return Ok(new { 
                token, 
                user = new { 
                    id = admin.Id, 
                    name = admin.FullName, 
                    email = admin.Email,
                    role = admin.Role
                } 
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AdminAccount admin)
        {
            var created = await _adminService.RegisterAsync(admin);
            return Ok(created);
        }

        [HttpGet("admins")]
        public async Task<IActionResult> GetAllAdmins()
        {
            // Note: In a real app, this should be super-admin only
            var admins = await _adminService.GetAllAdminsAsync();
            var result = admins.Select(a => new {
                id = a.Id,
                name = a.FullName,
                email = a.Email,
                role = a.Role,
                lastLogin = a.LastLogin.ToString("yyyy-MM-dd HH:mm")
            });
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GlobalSearch([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q)) return Ok(new { drivers = new List<object>(), users = new List<object>(), rides = new List<object>() });

            var query = q.ToLower();
            
            var drivers = (await _driverService.GetDriversAsync())
                .Where(d => d.Name.ToLower().Contains(query) || d.Email.ToLower().Contains(query) || d.LicensePlate.ToLower().Contains(query))
                .Select(d => new { id = d.Id, name = d.Name, type = "driver", detail = d.Email });

            var users = (await _userService.GetUserAsync())
                .Where(u => u.Name.ToLower().Contains(query) || u.Email.ToLower().Contains(query))
                .Select(u => new { id = u.Id, name = u.Name, type = "user", detail = u.Email });

            var rides = (await _rideService.GetAllRidesAsync())
                .Where(r => r.PickupLocation.ToLower().Contains(query) || r.DropoffLocation.ToLower().Contains(query))
                .Select(r => new { id = r.Id, name = $"{r.PickupLocation} -> {r.DropoffLocation}", type = "ride", detail = r.Status.ToString() });

            return Ok(new { drivers, users, rides });
        }

        [HttpPost("drivers/approve")]
        public async Task<IActionResult> ApproveDriver([FromBody] IdRequest request)
        {
            var driver = await _driverService.SetApprovalStatusAsync(request.Id, true);
            if (driver == null) return NotFound();
            return Ok(new { success = true, message = "Driver approved" });
        }

        [HttpPost("drivers/suspend")]
        public async Task<IActionResult> SuspendDriver([FromBody] IdRequest request)
        {
            var driver = await _driverService.SetApprovalStatusAsync(request.Id, false);
            if (driver == null) return NotFound();
            return Ok(new { success = true, message = "Driver suspended" });
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var rides = await _rideService.GetAllRidesAsync();
            var drivers = await _driverService.GetDriversAsync();
            var users = await _userService.GetUserAsync();
            var payments = await _paymentService.GetAllPaymentsAsync();

            var stats = new
            {
                totalRevenue = payments.Where(p => p.Status == PaymentStatus.Completed).Sum(p => p.Amount),
                activeDrivers = drivers.Count(d => d.IsApproved && d.IsAvailable),
                pendingApprovals = drivers.Count(d => !d.IsApproved),
                totalRides = rides.Count,
                totalUsers = users.Count,
                activeRides = rides.Count(r => r.Status == RideStatus.InProgress || r.Status == RideStatus.Pending)
            };

            return Ok(stats);
        }

        [HttpGet("drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            var drivers = await _driverService.GetDriversAsync();
            var result = drivers.Select(d => new {
                id = d.Id,
                name = d.Name,
                email = d.Email,
                status = d.IsApproved ? "approved" : "pending",
                totalRides = d.CompletedRides,
                rating = d.Rating,
                vehicleInfo = d.VehicleInfo,
                licensePlate = d.LicensePlate,
                vehicleType = d.VehicleType,
                phoneNumber = d.PhoneNumber
            });
            return Ok(result);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUserAsync();
            var result = users.Select(u => new {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                phoneNumber = u.PhoneNumber,
                totalRides = u.Rides.Count,
                totalDeliveries = u.Deliveries.Count
            });
            return Ok(result);
        }

        [HttpGet("rides")]
        public async Task<IActionResult> GetAllRides()
        {
            var rides = await _rideService.GetAllRidesAsync();
            var result = rides.Select(r => new {
                id = r.Id,
                passenger = r.User?.Name ?? "Unknown",
                driver = r.Driver?.Name ?? "Unassigned",
                start = r.PickupLocation,
                end = r.DropoffLocation,
                status = r.Status.ToString().ToLower(),
                fare = r.Fare,
                date = r.RequestedAt.ToString("yyyy-MM-dd")
            });
            return Ok(result);
        }

        [HttpGet("deliveries")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            var result = deliveries.Select(d => new {
                id = d.Id,
                sender = d.SenderName ?? d.User?.Name ?? "Unknown",
                receiver = d.ReceiverName ?? "Unknown",
                item = d.PackageDetails,
                status = d.Status.ToString().ToLower(),
                fee = d.Fare
            });
            return Ok(result);
        }

        [HttpGet("payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();
            var result = payments.Select(p => new {
                id = p.Id,
                amount = p.Amount,
                method = "Card", // Simplified
                date = p.CreatedAt.ToString("yyyy-MM-dd"),
                status = p.Status.ToString().ToLower()
            });
            return Ok(result);
        }

        [HttpPost("rides/cancel")]
        public async Task<IActionResult> CancelRide([FromBody] IdRequest request)
        {
            var cancelledRide = await _rideService.UpdateRideStatusAsync(request.Id, RideStatus.Cancelled);
            if (cancelledRide == null) return NotFound();
            return Ok(new { success = true, message = "Ride cancelled" });
        }

        private string GenerateJwtToken(AdminAccount admin)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, admin.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, admin.Role),
                new Claim("id", admin.Id.ToString()),
                new Claim("name", admin.FullName)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class IdRequest
    {
        public Guid Id { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

