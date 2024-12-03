using System;
using BasicSaasTenent.Middleware;
using BasicSaasTenent.Models;
using BasicSaasTenent.Repository;
using BasicSaasTenent.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ITenancyManager, TenancyManager>();
builder.Services.AddScoped<Tenant>();
builder.Services.AddScoped<ITenantGetter>(s=>s.GetRequiredService<Tenant>());
builder.Services.AddScoped<ITenantSetter>(s => s.GetRequiredService<Tenant>());

builder.Services.AddScoped<EnrollmentRepository>();
builder.Services.AddScoped<EnrollmentService>();

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//    .AddUserStore<ApplicationDbContext>()
//    .AddDefaultTokenProviders();



//builder.Services.AddAuthentication(options =>
//{
//    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<TenancyMiddleware>();
app.UseRouting();

app.UseAuthorization();
app.UseRouting();

app.MapControllers();
app.MapGet("/tenantInfo", (ITenantGetter tenant) => tenant);

app.Run();
