using AutoScales.Core.Configuration;
using AutoScales.Data;
using AutoScales.Data.Configuration;
using AutoScales.Data.Entities;
using AutoScales.Server;
using AutoScales.Server.Healthcheck;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ScalesDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<ScalesDbContext>();
builder.Services.AddAuthentication(options =>
{
    options.RequireAuthenticatedSignIn = true;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddAutoMapper(typeof(AppMapperConfiguration));

builder.Services.AddHealthChecks()
                .AddCheck<LivenessCheck>("liveness", tags: new[] { "liveness" })
                .AddCheck<ReadinessCheck>("readiness", tags: new[] { "readiness" });

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health/liveness", new HealthCheckOptions { Predicate = x => x.Name == "liveness" });
    endpoints.MapHealthChecks("/health/readiness", new HealthCheckOptions { Predicate = x => x.Name == "readiness" });
    endpoints.MapRazorPages();
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("index.html");
});

await SeedData.PopulateDb(builder);

app.Run();
