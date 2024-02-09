using Infrastructure;
using ManagementApp.Components;
using ManagementApp.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Repositories;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<JobSightDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Development"))); // I don't like that the database connection string has to be matching in both Infrastructure and THIS project. They should exist only in one file. Look into changing this at some point, please!! It will really inconvenience development once deployed to production. 
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJobService, JobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
