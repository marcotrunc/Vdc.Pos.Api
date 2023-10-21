using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vdc.Pos.Business.Mappers;
using Vdc.Pos.Business.Services;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Business.Validators;
using Vdc.Pos.Infrastructure.Service;
using Vdc.Pos.Infrastructure.Service.Interfaces;
using Vdc.Pos.Infrastructure.Settings;
using Vdc.Pos.Persistence.DataContext;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Binding For Settings
builder.Services.Configure<EmailSmtpSettings>(builder.Configuration.GetSection("EmailSmtpSettings"));
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Validator
builder.Services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();

// Add Mapping
builder.Services.AddAutoMapper(typeof(UserMapperProfiler).Assembly);

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDataContext>(options =>
                    options.UseSqlServer(connectionString));

// Add LifeTime Service
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IEmailService, EmailGoogleServices>();
builder.Services.AddScoped<OtpServices>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOtpRepository,OtpRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
