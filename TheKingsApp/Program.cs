using TheKingsApp.UseCases.Interfaces;
using TheKingsApp.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = WebApplicationBuilderExtensions.BuildApplication(args);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.MapGet("/monarchs", async (IMonarchAnswersService monarchService) =>
{
    var monarchList = await monarchService.GetAnswersAboutMonarchsAsync();
    return monarchList != null ? Results.Ok(monarchList) : Results.NotFound();
});


app.Run();
