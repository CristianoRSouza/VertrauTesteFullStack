using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Domain.Entities;
using Domain.Enums;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Api.Extensions;

public static class DatabaseExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<AppDbContext>>();

        try
        {
            if (!context.Database.IsInMemory())
            {
                logger.LogInformation("Aplicando migrations...");
                await context.Database.MigrateAsync();
            }
            else
            {
                logger.LogInformation("Usando InMemory database - criando schema...");
                await context.Database.EnsureCreatedAsync();
            }
            
            logger.LogInformation("Executando seed de dados...");
            await SeedDataAsync(context);
            
            logger.LogInformation("Database inicializado com sucesso");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao inicializar database");
            throw;
        }
    }

    private static async Task SeedDataAsync(AppDbContext context)
    {
        await SeedUsuarios(context);
    }

    private static async Task SeedUsuarios(AppDbContext context)
    {
        if (await context.Usuarios.AnyAsync()) return;

        var usuarios = new[]
        {
            new Usuario 
            { 
                Nome = "Administrador",
                Sobrenome = "Sistema",
                Email = "admin@vertrau.com",
                Senha = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = Role.Admin,
                Genero = Genero.Masculino
            },
            new Usuario 
            { 
                Nome = "Cliente",
                Sobrenome = "Teste",
                Email = "cliente@vertrau.com",
                Senha = BCrypt.Net.BCrypt.HashPassword("cliente123"),
                Role = Role.Client,
                Genero = Genero.Feminino
            }
        };

        await context.Usuarios.AddRangeAsync(usuarios);
        await context.SaveChangesAsync();
    }
}
