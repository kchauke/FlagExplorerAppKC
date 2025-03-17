using FlagExplorerAppKC.API.Services.Interfaces;
using FlagExplorerAppKC.API.Services.Logic;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Country API"
    });

    options.EnableAnnotations();
});

builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient("HttpMessageHandler")
    .ConfigureHttpClient(httpClient => { })
    .ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler() { UseDefaultCredentials = true });


var restCountriesUrl = builder.Configuration["ApiSettings:RestCountriesUrl"];
if (string.IsNullOrEmpty(restCountriesUrl))
{
    throw new ArgumentNullException(nameof(restCountriesUrl), "Country API Url cannot be null or empty.");
}

builder.Services.AddHttpClient<ICountryService, CountryService>(client =>
{
    client.BaseAddress = new Uri(restCountriesUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Country API");
});

app.Run();
