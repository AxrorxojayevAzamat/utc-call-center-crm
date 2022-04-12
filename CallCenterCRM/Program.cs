using CallCenterCRM.Data;
using CallCenterCRM.Features.Identity;
using CallCenterCRM.HttpHandlers;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var provider = builder.Services.BuildServiceProvider();
var configuration = provider.GetRequiredService<IConfiguration>();

//Http client
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddTransient<AuthenticationDelegatingHandler>();
builder.Services.AddHttpClient("IdentityAPI", client =>
{
    client.BaseAddress = new Uri(configuration.GetValue<string>("Identity:ApiUrl")); // API GATEWAY URL
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
}).AddHttpMessageHandler<AuthenticationDelegatingHandler>();

//Add UTC Identity Service
builder.Services.AddSingleton<IdentityService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CallcentercrmContext>(options =>
              //options.UseMySql("server=localhost;port=3306;database=callcentercrm;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.33-mysql"), x => x.UseNetTopologySuite())
              options.UseNpgsql("Server=localhost;Port=54331;Database=uzcloud;Username=postgres;Password=c065e76a148975b90f407ac2a065b48a;",(x)=>{})
              );
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>(); 
builder.Services.AddScoped<IApplicationService, ApplicationService>();
//builder.Services.AddDbContext<CallcentercrmContext>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
    {

        options.Authority = configuration.GetValue<string>("Identity:Url");
        options.GetClaimsFromUserInfoEndpoint = true;

        options.ClientId = configuration.GetValue<string>("Identity:ClientId");
        //options.ClientSecret = configuration.GetValue<string>("Identity:ClientSecret");
        options.Scope.Add("roles");
        options.Scope.Add("openid");
        options.Scope.Add("Auth_api");
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.TokenValidationParameters.RoleClaimType = "role";
        options.ResponseType = "code";

        options.SignInScheme = "Cookies";
        options.SaveTokens = true;

    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#region IDENTITY



#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();
