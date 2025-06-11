using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity; // Necess�rio para IdentityUser e IdentityRole
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TecnusAPI.Data;
using TecnusAPI.Models; // Necess�rio para configura��o do Swagger

var builder = WebApplication.CreateBuilder(args);

// --- Configura��o dos Servi�os ---

// 1. Adicionar o DbContext para o seu aplicativo (TecnusDBContext)
builder.Services.AddDbContext<TecnusDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adicionar ASP.NET Core Identity
// Configura o Identity para usar IdentityUser (usu�rio padr�o) e IdentityRole (para roles)
// e o seu TecnusDBContext para persistir os dados do Identity.
// `AddIdentityApiEndpoints` j� inclui `AddEntityFrameworkStores` e `AddDefaultTokenProviders`.
builder.Services.AddIdentityApiEndpoints<AppUsuario>(options => // <--- Mude de IdentityUser para UsuarioModel
{
    // *** RECOMENDA��O DE SEGURAN�A: Configura��es de senha mais fortes ***
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 2;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<TecnusDBContext>() // Isso est� correto se TecnusDBContext herda de IdentityDbContext<UsuarioModel>
.AddDefaultTokenProviders();

// 3. Adicionar o servi�o de CORS (removido duplicidade)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permite qualquer origem (BOM PARA DEV, RUIM PARA PROD - RESTRINJA!)
              .AllowAnyMethod()  // Permite qualquer m�todo HTTP (GET, POST, PUT, DELETE)
              .AllowAnyHeader(); // Permite qualquer cabe�alho HTTP
    });
});

// 4. Adicionar Controllers (para seus pr�prios endpoints)
builder.Services.AddControllers();

// 5. Adicionar Endpoints de API Explorer (para Swagger)
builder.Services.AddEndpointsApiExplorer();

// 6. Configura��o do Swagger/OpenAPI para suportar Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TecnusAPI", Version = "v1" }); // T�tulo e vers�o da API

    // Adiciona a defini��o de seguran�a para Bearer Token no Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Adiciona o requisito de seguran�a global para todas as opera��es no Swagger UI
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>() // Define que n�o h� escopos espec�ficos
        }
    });
});

var secretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(secretKey))
    throw new Exception("Chave secreta JWT n�o configurada.");

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// --- Constru��o do Aplicativo ---
var app = builder.Build();

// --- Configura��o do Pipeline de Requisi��es HTTP ---

// 1. Habilita o Swagger e a interface do Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 2. Redireciona requisi��es HTTP para HTTPS
app.UseHttpsRedirection();

// 3. Habilita o CORS (deve vir antes de UseAuthentication e UseAuthorization)
app.UseCors("AllowAll");

// 4. Habilita a autentica��o (Identity)
app.UseAuthentication();

// 5. Habilita a autoriza��o
app.UseAuthorization();

// 6. Mapeia os controladores (seus pr�prios endpoints)
app.MapControllers();

// 7. Mapeia os endpoints de autentica��o do Identity
// Isso ir� expor rotas como /Usuario/register, /Usuario/login, etc.
app.MapGroup("/Usuario").MapIdentityApi<AppUsuario>();

// 8. Executa o aplicativo
app.Run();