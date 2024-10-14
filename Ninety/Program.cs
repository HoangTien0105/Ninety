using Microsoft.Extensions.Logging;
using Ninety.Business.Services;
using Ninety.Business.Services.Interfaces;
using Ninety.Data.Repositories;
using Ninety.Data.Repositories.Interfaces;
using Ninety.Models.Models;
using AutoMapper;
using Ninety.Business.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"}
            },
            Array.Empty<string>()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:3000") // Add the origin you want to allow
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication(options =>
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            //      ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = false,
            //      ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1c4890495b93b9e71fee12bf1880242771ad287f814d9553b120de5b82428b0b"))
        };
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var token = context.SecurityToken as JwtSecurityToken;
                if (token != null && !JWTGenerator.IsTokenValid(token.RawData))
                {
                    context.Fail("Token is invalid");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddScoped<NinetyContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<ISportRepository, SportRepository>();

builder.Services.AddScoped<ITournamentService, TournamentService>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();

builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IRankingRepository, RankingRepository>();

builder.Services.AddScoped<ITeamDetailsRepository, TeamDetailRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();

builder.Services.AddScoped<IBadmintonMatchDetailService, BadmintonMatchDetailService>();
builder.Services.AddScoped<IBadmintonMatchDetailRepository, BadmintonMatchDetailRepository>();

builder.Services.AddAutoMapper(typeof(Ninety.Business.Mapping.MappingProfiles));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowSpecificOrigin");

app.Run();
