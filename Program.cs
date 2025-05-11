using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using edusync.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<EduSyncDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<BlobStorageSettings>(
    builder.Configuration.GetSection("AzureBlobStorage"));
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost5173", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Allow only localhost:5173
              .AllowAnyMethod() // Allow any HTTP method (GET, POST, etc.)
              .AllowAnyHeader() // Allow any headers
              .AllowCredentials(); // Allow credentials if needed
    });
});
builder.Services.AddLogging();

// Add Authentication services for JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS globally
app.UseCors("AllowLocalhost5173");
app.UseStaticFiles();

// Enable Authentication and Authorization
app.UseAuthentication(); // Adds JWT authentication middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
