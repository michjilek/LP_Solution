using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using OP_Db.Server;
using Op_LP;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add logging
Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .WriteTo.File(@".\Logs\log.txt")
               .CreateLogger();

Log.Information($"[ONLY SERVER APP]: START...");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//Db Prepare
builder.Services.AddDbContext<OP_Db_Context>(options => options.UseSqlite("Data Source=OP_db.db"));
builder.Services.AddHttpClient();

#region Installers
new OP_Pages_Library.Installer().Install(builder.Services);
new OP_Shared_Library.Installer().Install(builder.Services);
new Installer().Install(builder);
#endregion

//builder.WebHost.UseStaticWebAssets();
//builder.WebHost.UseWebRoot("wwwroot").UseStaticWebAssets();
var app = builder.Build();

// Db Initialize
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OP_Db_Context>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Tady je tøeba vydefinovat pøímo .yaml, protože jinak ho ve wwroot po pøekladu nenajdeme (json ano, i bez definice) 
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings = { [".yaml"] = "application/x-yaml" }
    }
});

//app.UseStaticFiles();

app.UseRouting();

//app.UseMiddleware<EshopRedirectMiddleware>();

app.MapBlazorHub();
app.MapRazorPages();
app.MapControllers();
//app.MapFallbackToPage("index.html");
app.MapFallbackToPage("/_Host");

app.Run();
