using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SmartSupply.Infrastructure
{
    public class SmartSupplyDbContextFactory : IDesignTimeDbContextFactory<SmartSupplyDbContext>
    {
        public SmartSupplyDbContext CreateDbContext(string[] args)
        {
            // cherche appsettings.json en remontant depuis le répertoire d'exécution
            var baseDir = AppContext.BaseDirectory;
            var directory = new DirectoryInfo(baseDir);
            string? configFolder = null;

            while (directory != null)
            {
                var candidate = Path.Combine(directory.FullName, "appsettings.json");
                if (File.Exists(candidate))
                {
                    configFolder = directory.FullName;
                    break;
                }
                directory = directory.Parent;
            }

            var configBuilder = new ConfigurationBuilder();

            if (configFolder != null)
            {
                configBuilder.SetBasePath(configFolder)
                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                             .AddEnvironmentVariables();
            }
            else
            {
                // fallback : variables d'environnement seulement
                configBuilder.AddEnvironmentVariables();
            }

            var configuration = configBuilder.Build();

            // Utilise la clé "smartSupply" (comme dans ton appsettings.json)
            var connectionString = configuration.GetConnectionString("smartSupply")
                                   ?? Environment.GetEnvironmentVariable("ConnectionStrings__smartSupply");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // fallback de dev local (adapté à ton environnement)
                connectionString = "Data Source=ZouariAhmed;Initial Catalog=smartSupply;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;";
            }

            var optionsBuilder = new DbContextOptionsBuilder<SmartSupplyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SmartSupplyDbContext(optionsBuilder.Options);
        }
    }
}
