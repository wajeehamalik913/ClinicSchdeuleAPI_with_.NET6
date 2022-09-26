using Clinic.Data;
using Clinic.Models;
using ClinicApi.Data;
using ClinicApi.Interfaces;
using ClinicApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// Add database context to app.
builder.Services.AddDbContext<ClinicContext>(
options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("ClinicDB"),
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.23-mysql"));
});

//Add Scoped services
builder.Services.AddScoped<IDoctor, DoctorServices>();
builder.Services.AddScoped<IAuth, AuthServices>();

//Add token validation services.
var tokenValidationParameters = new TokenValidationParameters()
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Secret"])),

    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:Issuer"],

    ValidateAudience = false,
    ValidAudience = builder.Configuration["JWT:Audience"],

    ValidateLifetime = true,
    ClockSkew=TimeSpan.Zero,
};

//Add single instance of the service to be used througout.
builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddMemoryCache();

//Add identity services
builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredLength = 6;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
    })
    .AddEntityFrameworkStores<ClinicContext>()
    .AddDefaultTokenProviders();

//Add JWT Authentcation service 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
//Add JWT Bearer
.AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata=false;
    options.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddMvc(options => {
    options.Filters.Add(typeof(ValidateModelStateAttribute));
});
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Authentication & Authurization

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Seed the datbase with roles
ClinicDbInitializer.SeedRolesToDb(app).Wait();

app.Run();
