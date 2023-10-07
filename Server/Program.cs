using Microsoft.AspNetCore.ResponseCompression;
using ProjectBlazor.Client.Handlers;
using ProjectBlazor.Client.Providers;
using ProjectBlazor.Server.Data;
using ProjectBlazor.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.Headers["Location"] = context.RedirectUri;
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5053") });
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://astonishing-moxie-cc17b8.netlify.app/") });
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddScoped<CookieHandler>();

            /*builder.Services.AddHttpClient("ProjectBlazor.ServerAPI", client => client.BaseAddress = new Uri("http://localhost:5053"))
               .AddHttpMessageHandler<CookieHandler>();*/
               builder.Services.AddHttpClient("ProjectBlazor.ServerAPI", client => client.BaseAddress = new Uri("https://astonishing-moxie-cc17b8.netlify.app/"))
               .AddHttpMessageHandler<CookieHandler>();


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

//app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();


app.UseRouting();
app.UseAuthorization();



app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
