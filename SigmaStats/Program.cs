using Microsoft.EntityFrameworkCore;
using SigmApi.Models.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



builder.Services.AddDbContext<SteamContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("SteamContext") ?? throw new InvalidOperationException("Connection string 'SteamContext' not found.")));



var app = builder.Build();

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
app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Steam}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "gameDetails",
    pattern: "games/details/{appId?}",
    defaults: new {controller = "Games", action = "Details" });



app.Run();
