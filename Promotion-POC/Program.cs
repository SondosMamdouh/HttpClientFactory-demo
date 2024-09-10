using Promotion_POC.CustomExceptions;
using Promotion_POC.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information() 
	.WriteTo.Console()          
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day) 
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers(options =>
{
	options.Filters.Add(new ApiExceptionFilter());
});


builder.Services.AddControllers();

builder.Services.AddTransient<FakeApiService>();
builder.Services.AddTransient<WeatherService>();

builder.Services.AddHttpClient<JsonPlaceholderClientService>();



//this one will alywas fail becasue the end point not exists
builder.Services.AddHttpClient("FakeApiClient", client =>
{
	client.BaseAddress = new Uri("https://fake-api.com/");
	client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("WeatherClient", client =>
{
	client.BaseAddress = new Uri("https://api.open-meteo.com/v1/");
	client.DefaultRequestHeaders.Add("Accept", "application/json");
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
