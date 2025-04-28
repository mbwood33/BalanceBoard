using BalanceBoard.Components;
using BalanceBoard.Components.Account;  // Contains Identity Razor components
using BalanceBoard.Data;    // Contains ApplicationDbContext
using BalanceBoard.Models;  // Contains custom ApplicationUser
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;   // Needed for accessing configuration
using Syncfusion.Blazor;    // Needed for Syncfusion services
using Npgsql;

// Using custom services (commenting out user management parts later)
// using BalanceBoard.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- Register Syncfusion Blazor services with the license key ---
var syncfusionLicenseKey = builder.Configuration["SyncfusionLicenseKey"];   // Retrieve the Syncfusion license key from environment variables or other configuration sources
if (!string.IsNullOrEmpty(syncfusionLicenseKey))
{
    // Register Syncfusion Blazor services with the license key
    Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);
    builder.Services.AddSyncfusionBlazor();  // <-- Add Syncfusion service
}
else
{
    // Handle the case where the license key is not found
    Console.WriteLine("Warning: SyncfusionLicenseKey environment variable is not set.");
    // Still add the Syncfusion service, but it will run in trial mode.
    builder.Services.AddSyncfusionBlazor();
}
// --- End Syncfusion Blazor registration ---

// Setup services fro ASP.NET Core Identity and Blazor Authentication State
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>(); // Helps access user info in components
builder.Services.AddScoped<IdentityRedirectManager>();  // Manages redirects during auth operations
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>(); // Provides the AuthenticationState based on Identity

// Configure Authentication using Identity Cookies
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;    // Sets the default authentication scheme
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme; // Used for external logins (like Google, Facebook)
    })
    .AddIdentityCookies();  // Configures Identity to use cookie-based authentication

// Configure the DbContext for ASP.NET Core Identity to use PostgreSQL
// Retrieves the connection string from configuration (e.g., appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Registers the ApplicationDbContext with Entity Framework Core, configured to use PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));   // Using Npgsql for PostgreSQL

// Adds services for database developer page exceptions (useful in development)
builder.Services.AddDatabaseDeveloperPageExceptionFilter(); 

// Configures ASP.NET Core Identity, using the custom ApplicationUser and ApplicationDbContext
builder.Services.AddIdentityCore<BalanceBoard.Data.ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true) // Configure Identity options (e.g., email confirmation)
    .AddEntityFrameworkStores<ApplicationDbContext>()   // Configures Identity to use EF Core with ApplicationDbContext for data storage
    .AddSignInManager() // Adds the SignInManager for handling user logins
    .AddDefaultTokenProviders();    // Adds default token providers (for password reset, email confirmation tokens)

// Registers a placeholder email sender for Identity (replace with a real one for production)
// Identity requires an IEmailSender, even if email confirmation is not strictly enforced initially
builder.Services.AddSingleton<IEmailSender<BalanceBoard.Data.ApplicationUser>, IdentityNoOpEmailSender>();

// --- Custom Services - The user management parts will be replaced by Identity ---
// Your custom DatabaseService. The user management parts will be replaced by Identity.
// You will need to keep or refactor the weight log methods from this service.
// For now, keep it registered if other parts of your app still depend on it,
// but be aware its user management methods will become obsolete.
// builder.Services.AddScoped<DatabaseService>();

// Your custom UserSessionService. This will be replaced by Blazor's AuthenticationStateProvider
// integrated with Identity. You can likely remove this service entirely after refactoring.
// builder.Services.AddScoped<UserSessionService>();
// --- End Custom Services ---

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // In development, use the migrations endpoint page for applying EF Core migrations
    app.UseMigrationsEndPoint();
}
else
{
    // In production, use a standard error handler and HSTS.
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();  // Enforce HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Map Blazor Razor Components and enable interactive rendering
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();  // Configured for Interactive Server rendering

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
