using DAL;
using DLL;
using DLL.Interfaces;
using Erdogan_Backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IGameHubClient, GameHubClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Uncomment this line if you want to use https, but http for websockets at the moment
app.UseHttpsRedirection();

app.MapControllers();
app.MapHub<GameHub>("/gamehub");

app.Run();