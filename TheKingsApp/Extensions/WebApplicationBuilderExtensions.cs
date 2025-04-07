using TheKingsApp.Core.Interfaces;
using TheKingsApp.UseCases.Interfaces;
using TheKingsApp.UseCases.Services;

namespace TheKingsApp.Web.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplication BuildApplication(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<IMonarchService, MonarchService>();
            builder.Services.AddScoped<IMonarchAnswersService, MonarchAnswersService>();
            builder.Services.AddHttpClient<MonarchService>();
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            return builder.Build();
        }
    }
}
