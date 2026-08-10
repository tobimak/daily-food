using System.Text;
using Aplicacion.Interfaces;
using Aplicacion.Services;
using Dominio.Interfaces_repository.Command;
using Dominio.Interfaces_repository.Query;
using Daily_food.Middleware;
using Infraestructura.Persistence;
using Infraestructura.Repository.Command;
using Infraestructura.Repository.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext con SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== REGISTRAR REPOSITORIOS (Puertos → Adaptadores) =====

// Commands
builder.Services.AddScoped<IUsuarioCommandRepository, UsuarioCommandRepository>();
builder.Services.AddScoped<IPlatoCommandRepository, PlatoCommandRepository>();
builder.Services.AddScoped<IDiaCommandRepository, DiaCommandRepository>();

// Queries
builder.Services.AddScoped<IUsuarioQueryRepository, UsuarioQueryRepository>();
builder.Services.AddScoped<IPlatoQueryRepository, PlatoQueryRepository>();
builder.Services.AddScoped<IDiaQueryRepository, DiaQueryRepository>();

// ===== REGISTRAR SERVICIOS DE APLICACIÓN =====
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPlatoService, PlatoService>();
builder.Services.AddScoped<IDiaService, DiaService>();

// ===== AUTENTICACIÓN JWT =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
        };
    });
builder.Services.AddAuthorization();

// ===== CORS =====
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [])
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseDeveloperExceptionPage();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
//app.UseHttpsRedirection();
app.UseAuthentication();        // autentica antes de autorizar
app.UseAuthorization();
//app.UseMiddleware<ExceptionMiddleware>();  // nuestras excepciones → códigos HTTP

app.MapControllers();

app.Run();