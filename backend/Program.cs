using Microsoft.EntityFrameworkCore;
using TgiControl.Data;
using TgiControl.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDbContext<TgiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IAuthService, DemoAuthService>();
builder.Services.AddScoped<IPermitService, PermitService>();
builder.Services.AddScoped<IShiftService, ShiftService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Middleware
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map endpoints
app.MapAuthEndpoints();
app.MapPermitEndpoints();
app.MapShiftEndpoints();
app.MapHealthCheck("/health", "TGI Control Operacional API");

app.Run();