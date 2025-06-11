using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity; // Necessário para IdentityUser e IdentityRole
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TecnusAPI.Data;
using TecnusAPI.Models; // Necessário para configuração do Swagger

var builder = WebApplication.CreateBuilder(args);

// --- Configuração dos Serviços ---

// 1. Adicionar o DbContext para o seu aplicativo (TecnusDBContext)
builder.Services.AddDbContext<TecnusDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Adicionar ASP.NET Core Identity
// Configura o Identity para usar IdentityUser (usuário padrão) e IdentityRole (para roles)
// e o seu TecnusDBContext para persistir os dados do Identity.
// `AddIdentityApiEndpoints` já inclui `AddEntityFrameworkStores` e `AddDefaultTokenProviders`.
builder.Services.AddIdentityApiEndpoints<AppUsuario>(options => // <--- Mude de IdentityUser para UsuarioModel
{
    // *** RECOMENDAÇÃO DE SEGURANÇA: Configurações de senha mais fortes ***
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
.AddEntityFrameworkStores<TecnusDBContext>() // Isso está correto se TecnusDBContext herda de IdentityDbContext<UsuarioModel>
.AddDefaultTokenProviders();

// 3. Adicionar o serviço de CORS (removido duplicidade)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // Permite qualquer origem (BOM PARA DEV, RUIM PARA PROD - RESTRINJA!)
              .AllowAnyMethod()  // Permite qualquer método HTTP (GET, POST, PUT, DELETE)
              .AllowAnyHeader(); // Permite qualquer cabeçalho HTTP
    });
});

// 4. Adicionar Controllers (para seus próprios endpoints)
builder.Services.AddControllers();

// 5. Adicionar Endpoints de API Explorer (para Swagger)
builder.Services.AddEndpointsApiExplorer();

// 6. Configuração do Swagger/OpenAPI para suportar Bearer Token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "TecnusAPI", Version = "v1" }); // Título e versão da API

    // Adiciona a definição de segurança para Bearer Token no Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Exemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Adiciona o requisito de segurança global para todas as operações no Swagger UI
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
            Array.Empty<string>() // Define que não há escopos específicos
        }
    });
});

var secretKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(secretKey))
    throw new Exception("Chave secreta JWT não configurada.");

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

// --- Construção do Aplicativo ---
var app = builder.Build();

// --- Configuração do Pipeline de Requisições HTTP ---

// 1. Habilita o Swagger e a interface do Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 2. Redireciona requisições HTTP para HTTPS
app.UseHttpsRedirection();

// 3. Habilita o CORS (deve vir antes de UseAuthentication e UseAuthorization)
app.UseCors("AllowAll");

// 4. Habilita a autenticação (Identity)
app.UseAuthentication();

// 5. Habilita a autorização
app.UseAuthorization();

// 6. Mapeia os controladores (seus próprios endpoints)
app.MapControllers();

// 7. Mapeia os endpoints de autenticação do Identity
// Isso irá expor rotas como /Usuario/register, /Usuario/login, etc.
app.MapGroup("/Usuario").MapIdentityApi<AppUsuario>();

// 8. Executa o aplicativo
app.Run();