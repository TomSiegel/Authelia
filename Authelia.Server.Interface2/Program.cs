using Authelia.Server.Helpers;
using Authelia.Server.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();


Setup.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseAutheliaExceptionHandler();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.GetFullPath("frontend/wwwroot")),
    RequestPath = ""
});
app.UseRouting();
app.MapRazorPages();
Setup.Configure(app);
app.Run();
