using Application.DTOs;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User Test Web API",
        Version = "v1",
        Description = "API for User Management with JWT Authentication"
    });

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Configure CORS
var corsSettings = builder.Configuration.GetSection("Cors:Policies").GetChildren().FirstOrDefault();
if (corsSettings != null)
{
    var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
    var allowedMethods = corsSettings.GetSection("AllowedMethods").Get<string[]>() ?? new[] { "GET", "POST", "PUT", "DELETE", "OPTIONS" };
    var allowedHeaders = corsSettings.GetSection("AllowedHeaders").Get<string[]>() ?? new[] { "*" };
    var allowCredentials = corsSettings.GetValue<bool>("AllowCredentials");

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Development", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .WithMethods(allowedMethods)
                  .WithHeaders(allowedHeaders)
                  .AllowCredentials();
        });
    });
}

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatShouldBeAtLeast32CharactersLongForSecurity!";
var issuer = jwtSettings["Issuer"] ?? "UsertestWebAPI";
var audience = jwtSettings["Audience"] ?? "UsertestWebAPI";

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
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("TeacherOrAdmin", policy => policy.RequireRole("Teacher", "Admin"));
    options.AddPolicy("StudentOrAbove", policy => policy.RequireRole("Student", "Teacher", "Admin"));
    
    options.AddPolicy("ViewUsers", policy => policy.RequireClaim("Permission", "ViewUsers"));
    options.AddPolicy("CreateUser", policy => policy.RequireClaim("Permission", "CreateUser"));
    options.AddPolicy("EditUser", policy => policy.RequireClaim("Permission", "EditUser"));
    options.AddPolicy("DeleteUser", policy => policy.RequireClaim("Permission", "DeleteUser"));
    
    options.AddPolicy("ViewStudents", policy => policy.RequireClaim("Permission", "ViewStudents"));
    options.AddPolicy("CreateStudent", policy => policy.RequireClaim("Permission", "CreateStudent"));
    options.AddPolicy("EditStudent", policy => policy.RequireClaim("Permission", "EditStudent"));
    options.AddPolicy("DeleteStudent", policy => policy.RequireClaim("Permission", "DeleteStudent"));
    
    options.AddPolicy("ViewTeachers", policy => policy.RequireClaim("Permission", "ViewTeachers"));
    options.AddPolicy("CreateTeacher", policy => policy.RequireClaim("Permission", "CreateTeacher"));
    options.AddPolicy("EditTeacher", policy => policy.RequireClaim("Permission", "EditTeacher"));
    options.AddPolicy("DeleteTeacher", policy => policy.RequireClaim("Permission", "DeleteTeacher"));
    
    options.AddPolicy("ManageRoles", policy => policy.RequireClaim("Permission", "ManageRoles"));
    options.AddPolicy("ManagePermissions", policy => policy.RequireClaim("Permission", "ManagePermissions"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Student>, StudentRepository>();
builder.Services.AddScoped<IRepository<Teacher>, TeacherRepository>();
builder.Services.AddScoped<IService<User, UserResponse, UserModel>, UserService>();
builder.Services.AddScoped<IService<Student, StudentResponse, StudentModel>, StudentService>();
builder.Services.AddScoped<IService<Teacher, TeacherResponse, TeacherModel>, TeacherService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS before Authentication
app.UseCors("Development");

// Authentication & Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
