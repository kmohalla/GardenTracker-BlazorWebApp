using GardenData;
using GardenTracker.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// register GardenContext with the dependency injection container-- must be done before the app is built
var connectionString =
	builder.Configuration.GetConnectionString("gardening")
		?? throw new InvalidOperationException("Connection string"
		+ "'gardening' not found.");

builder.Services.AddDbContextFactory<GardenContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("gardening")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
