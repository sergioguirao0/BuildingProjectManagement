using BuildingProjectManagementAPI.Data;
using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 4, 32))));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddIdentityCore<IdentityUser>()        // Agrega el sistema de usuario con IdentityUser
    .AddEntityFrameworkStores<ApplicationDbContext>()   // Configura EFC como el almacenamiento de datos de usuarios con ApplicationDbContext
    .AddDefaultTokenProviders();                        // Habilita la generación de tokens

builder.Services.AddScoped<UserManager<IdentityUser>>();    // Gestiona los usuarios
builder.Services.AddScoped<SignInManager<IdentityUser>>();  // Gestiona la autenticación de usuarios
builder.Services.AddTransient<IUserRepository, UserService>();
builder.Services.AddHttpContextAccessor();                  // Permite acceder al contexto Http actual

builder.Services.AddScoped<IUserRepository, UserService>();

builder.Services.AddAuthentication().AddJwtBearer(options => // Agrega la autenticación por tokens JWT
{
    options.MapInboundClaims = false;                        // Deshabilita la asignación automática de claims de usuarios
    options.TokenValidationParameters = new TokenValidationParameters 
    {
        ValidateIssuer = false,             // No valida el emisor del token
        ValidateAudience = false,           // No valida la audiencia del token
        ValidateLifetime = true,            // Verifica que el token no esté expirado
        ValidateIssuerSigningKey = true,    // Verifica la clave de firma del token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtKey"]!)), // Usa una clave secreta para firmar los tokens
        ClockSkew = TimeSpan.Zero           // Elimina el tiempo de tolerancia de la expiración del token
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors();
app.MapControllers();

app.Run();
