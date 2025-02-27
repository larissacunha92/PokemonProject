using Microsoft.EntityFrameworkCore;
using PokemonProject;
using PokemonProject.Data;
using PokemonProject.Services;
using PokemonProject.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IPokemonApiService, PokemonApiService>(client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/");
});

builder.Services.AddScoped<IBattleHistoryService, BattleHistoryService>();

builder.Services.AddScoped<IFavoritePokemonService, FavoritePokemonService>();

builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite("Data Source=pokemon.db;",
        sqlOptions => sqlOptions.MigrationsAssembly("PokemonProject")));


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
