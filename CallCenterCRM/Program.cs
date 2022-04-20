using CallCenterCRM.Data;
using CallCenterCRM.Features.Identity;
using CallCenterCRM.HttpHandlers;
using CallCenterCRM.Interfaces;
using CallCenterCRM.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
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
              options.UseNpgsql(configuration.GetConnectionString("CallCenterCRMContext"), (x) => { })
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
    .AddCookie("Cookies", options =>
   {
       options.LoginPath = "/signIn";
       options.LogoutPath = "/signOut";
       options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
       if (builder.Environment.IsDevelopment())
           options.Cookie.SecurePolicy = CookieSecurePolicy.None;
   })
    .AddOpenIdConnect("oidc", options =>
    {

        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.SignOutScheme = "Cookies";

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
        options.SaveTokens = true;

       options.SignInScheme = "Cookies";

    });

builder.Services.AddAuthorization();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CallcentercrmContext>();
    db.Database.Migrate();
}


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
