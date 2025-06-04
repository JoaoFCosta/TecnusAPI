using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Repositorio;
using TecnusAPI.Repositorio.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkSqlServer()
                .AddDbContext<TecnusDBContext>(
                   options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                );

builder.Services.AddScoped<IUsuariosRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<ICursoRepositorio, CursoRepositorio>();
builder.Services.AddScoped<IFeedbackRepositorio, FeedbackRepositorio>();
builder.Services.AddScoped<IQuizRepositorio, QuizRepositorio>();
builder.Services.AddScoped<IEmail, Email>();
builder.Services.AddScoped<IVideoRepositorio, VideoRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
