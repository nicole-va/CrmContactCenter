using CrmContactCenter.Server.Data;
using CrmContactCenter.Server.Services;

using CrmContactCenter.Server.Services.Interfaces;

//using CrmContactCenter.Server.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
//  1. BASE DE DATOS — MySQL con EF Core
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CrmDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// ============================================================
//  2. AUTENTICACIÓN JWT
// ============================================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];
if (string.IsNullOrWhiteSpace(secretKey))
    throw new InvalidOperationException("JwtSettings:SecretKey no está configurado. Debe tener al menos 32 caracteres (256 bits) para HS256.");

var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
if (secretKeyBytes.Length < 32)
    throw new InvalidOperationException($"JwtSettings:SecretKey es demasiado corto para HS256. Mínimo 32 bytes (256 bits). Actual: {secretKeyBytes.Length} bytes.");

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
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes)
    };
});

builder.Services.AddAuthorization();

// ============================================================
//  3. CONTROLADORES
// ============================================================
builder.Services.AddControllers();

// ============================================================
//  4. SWAGGER
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CRM Contact Center API",
        Version = "v1",
        Description = "API para gestión de clientes, interacciones y seguimientos"
    });

    // Agregar soporte para JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token así: Bearer {tu token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ============================================================
//  5. CORS — permite peticiones desde Vue.js
// ============================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVue", policy =>
    {
        // En dev el front puede correr en distintos puertos (Vite elige uno si está ocupado).
        policy.WithOrigins(
                "https://localhost:5173", "http://localhost:5173",
                "https://localhost:63759", "http://localhost:63759"
            )
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ============================================================
//  6. SERVICIOS (se agregarán aquí conforme avancemos)
// ============================================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IInteractionService, InteractionService>();
builder.Services.AddScoped<IFollowUpService, FollowUpService>();

// ============================================================
//  BUILD
// ============================================================
var app = builder.Build();

// ============================================================
//  7. MIDDLEWARE PIPELINE
// ============================================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM API v1");
    });
}

app.UseDefaultFiles();
app.UseStaticFiles();

// Middleware global de errores (lo crearemos en Middleware/)
// app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowVue");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");

app.Run();