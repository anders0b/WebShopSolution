using DbUp;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WebShop.Repository;

public static class MigrationHelper
{
    public static void EnsureDatabaseIsAvailableAndUpToDate(string connectionString, ILogger logger)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgradeEngine =
            DeployChanges.To
            .SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
            .LogToConsole()
            .Build();
        if (upgradeEngine.GetScriptsToExecute().Any())
        {
            foreach (var script in upgradeEngine.GetScriptsToExecute())
            {
                logger.LogInformation("Will run {name} to keep database up to date", script.Name);
            }
            var dbUpgradeResult = upgradeEngine.PerformUpgrade();
            if (!dbUpgradeResult.Successful)
            {
                logger.LogError(dbUpgradeResult.Error, "Could not update database");
                throw new Exception($"Could not update database. ${dbUpgradeResult.Error.Message}", dbUpgradeResult.Error);
            }
        }
    }
}

