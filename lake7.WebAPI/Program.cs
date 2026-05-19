using System.Text;
using lake7.Application.Interface;
using lake7.Application.Services;
using lake7.Infrastructure.Context;
using lake7.Infrastructure.Repositories;
using lake7.Infrastructure.Repository;
using lake7.WebAPI.Hubs;
using lake7.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Lake7DbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// Repositories & Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IDriverService, DriverService>();

builder.Services.AddScoped<IRideRepository, RideRepository>();
builder.Services.AddScoped<IRideService, RideService>();

builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IDriverLocationRepository, DriverLocationRepository>();
builder.Services.AddScoped<IDriverLocationService, DriverLocationService>();

builder.Services.AddScoped<IMapService, MapService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();

builder.Services.AddScoped<INotificationService, NotificationService>();


builder.Services.AddSignalR();

//Backend Proxy HttpClient
builder.Services.AddHttpClient("MapsClient", client =>
{
    client.BaseAddress = new Uri("https://maps.googleapis.com/");
});

// JWT Auth
var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // important for local dev
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)),

            ClockSkew = TimeSpan.Zero // remove delay tolerance
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"]
                    .FirstOrDefault()?.Split(" ").Last();

                Console.WriteLine("🔑 RAW HEADER: " + context.Request.Headers["Authorization"]);
                Console.WriteLine("🔑 EXTRACTED TOKEN: " + token);

                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            },

            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("❌ AUTH FAILED: " + context.Exception.Message);
                return Task.CompletedTask;
            },

            OnTokenValidated = context =>
            {
                Console.WriteLine("✅ TOKEN VALIDATED");
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

// CORS (React Native friendly)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



var app = builder.Build();



app.UseCors("AllowAll");

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<DriverHub>("/driverHub"); // expose hub endpoint
app.MapHub<UserHub>("/userHub");

app.Run();