using Microsoft.EntityFrameworkCore;
using DIYshopAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy(name: "DIYshopOrigins",
    policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
    }));

builder.Services.AddDbContext<UserdbContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<CustomerContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<ProductContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<OrderContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<OrderItemContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddDbContext<PromotionContext>(options =>
{
    options.UseSqlServer(
       builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration.GetSection("AppSettings:Token").Value!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DIYshopOrigins");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
