using Authelia.Server.Helpers;
using Authelia.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

Setup.ConfigureServices(builder.Services, builder.Configuration);
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseAutheliaExceptionHandler();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Setup.Configure(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapRazorPages();
app.Run();
