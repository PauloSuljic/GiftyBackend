using System.Text;
using Gifty.Application.Services;
using Gifty.Data;
using Gifty.Data.Repositories;
using Gifty.Domain.Models;
using Gifty.Domain.Repositories;
using Gifty.Domain.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options => {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
        options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Register application services
builder.Services.AddTransient<EmailService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistItemRepository, WishlistItemRepository>();
builder.Services.AddScoped<IWishlistItemService, WishlistItemService>();
builder.Services.AddScoped<BirthdayReminderService, BirthdayReminderService>();

// Add CORS services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder =>
        {
            corsBuilder.WithOrigins("http://localhost:4200") // Replace with your frontend domain
                .AllowAnyHeader()
                .AllowAnyMethod();
        });

    // Add a policy for development/testing that allows any origin
    options.AddPolicy("AllowAllOrigins",
        corsBuilder =>
        {
            corsBuilder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHangfire(x => x.UseSqlServerStorage("DefaultConnection"));
builder.Services.AddHangfireServer();

// Configure JWT Authentication
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
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });

// Add Hangfire services
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddHangfireServer();

// Build the application
var app = builder.Build();

// Step 2: Enable CORS in the request pipeline

// Enable the specific CORS policy for production
app.UseCors("AllowAllOrigins");

// For development, use the more permissive policy (optional)
// app.UseCors("AllowAllOrigins");

// Configure Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Enable Swagger middleware
    app.UseSwaggerUI(); // Enable Swagger UI
    app.MapSwagger(); // Add this if you want to expose the Swagger UI
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();

app.MapControllers();


app.Run();