using AttendanceTracker.Application.Interfaces;
using AttendanceTracker.Application.Services;
using AttendenceTracker.Domain.Entity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add services
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddControllers();

// ❌ REMOVE THIS
// builder.Services.AddOpenApi();

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ DB
var cs = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AttendanceDbContext>(x =>
    x.UseSqlServer(cs));

var app = builder.Build();

// ✅ Enable Swagger (FOR ALL ENVIRONMENTS)
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = string.Empty; // 👉 opens at root URL
});

// ✅ Middleware
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();