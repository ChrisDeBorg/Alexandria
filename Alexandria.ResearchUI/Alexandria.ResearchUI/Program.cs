using Alexandria.Application.Abstractions;
using Alexandria.Application.Entities;
using Alexandria.Application.Relations;
using Alexandria.Infrastructure.Persistence.PostgreSql;
using Alexandria.ResearchUI.Client.Pages;
using Alexandria.ResearchUI.Components;
using Microsoft.EntityFrameworkCore;

namespace Alexandria.ResearchUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            var connectionString =
                builder.Configuration.GetConnectionString("Alexandria")
                ?? throw new InvalidOperationException(
                    "Connection string 'Alexandria' is missing.");

            builder.Services.AddDbContext<AlexandriaDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddScoped<IEntityStore, EntityStore>();
            builder.Services.AddScoped<IEntityService, EntityService>();
            builder.Services.AddScoped<IRelationStore, RelationStore>();
            builder.Services.AddScoped<IRelationService, RelationService>();

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

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
