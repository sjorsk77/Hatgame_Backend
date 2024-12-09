using System.Text;
using DAL;
using DLL;
using Erdogan_Backend.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Replace with your React app's URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // Required for WebSocket connections
    });
});


builder.Services.AddControllers();

builder.Services.AddSignalR(options =>
{
    options.KeepAliveInterval = TimeSpan.FromSeconds(10);
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if(connectionString == null) throw new Exception("Connection string is null");

builder.Services.AddDal(connectionString);
builder.Services.AddDLL();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(secretKey),
            RoleClaimType = "typ"
        };
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Uncomment this line if you want to use https, but http for websockets at the moment
// app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<GameHub>("/gamehub");

app.Run();