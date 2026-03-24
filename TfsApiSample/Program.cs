using Microsoft.Extensions.Configuration;
using TfsApiSample;

// ---------------------------------------------------------------------------
// Build configuration
// ---------------------------------------------------------------------------
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables()   // allows overriding settings in CI / containers
    .Build();

var tfsConfig = configuration.GetSection("Tfs").Get<TfsClientConfig>()
    ?? throw new InvalidOperationException(
        "The 'Tfs' configuration section is missing from appsettings.json.");

// ---------------------------------------------------------------------------
// Validate that placeholder values have been replaced
// ---------------------------------------------------------------------------
if (tfsConfig.CollectionUrl.Contains("your-organization", StringComparison.OrdinalIgnoreCase) ||
    tfsConfig.PersonalAccessToken.Equals("your-pat-here", StringComparison.OrdinalIgnoreCase))
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("⚠  Configuration placeholders detected in appsettings.json.");
    Console.WriteLine("   Update CollectionUrl, PersonalAccessToken and ProjectName, then re-run.");
    Console.ResetColor();
    return;
}

// ---------------------------------------------------------------------------
// Run samples
// ---------------------------------------------------------------------------
using var client = new TfsApiClient(tfsConfig);

Console.WriteLine("=== TFS / Azure DevOps REST API Sample ===");
Console.WriteLine($"Collection : {tfsConfig.CollectionUrl}");
Console.WriteLine();

// Sample 1 – list projects
Console.WriteLine("--- Projects ---");
try
{
    var projects = await client.GetProjectsAsync();
    foreach (var project in projects)
    {
        Console.WriteLine($"  [{project.State}] {project.Name}  ({project.Id})");
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  Error fetching projects: {ex.Message}");
    Console.ResetColor();
}

Console.WriteLine();

// Sample 2 – list Git repositories in the configured project
if (!string.IsNullOrWhiteSpace(tfsConfig.ProjectName))
{
    Console.WriteLine($"--- Git Repositories in '{tfsConfig.ProjectName}' ---");
    try
    {
        var repos = await client.GetRepositoriesAsync(tfsConfig.ProjectName);
        foreach (var repo in repos)
        {
            Console.WriteLine($"  {repo.Name}  ({repo.RemoteUrl})");
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Error fetching repositories: {ex.Message}");
        Console.ResetColor();
    }

    Console.WriteLine();

    // Sample 3 – query the 10 most-recently-updated active work items
    Console.WriteLine($"--- Recent Active Work Items in '{tfsConfig.ProjectName}' ---");
    try
    {
    // Escape single quotes in the project name to prevent WIQL injection.
    var safeProjectName = tfsConfig.ProjectName.Replace("'", "''");
    var wiql = $"SELECT [System.Id], [System.Title], [System.State] " +
               $"FROM WorkItems " +
               $"WHERE [System.TeamProject] = '{safeProjectName}' " +
               $"AND [System.State] <> 'Closed' " +
               $"ORDER BY [System.ChangedDate] DESC";

        var workItems = await client.QueryWorkItemsAsync(wiql, tfsConfig.ProjectName);
        var top10 = workItems.Take(10);
        foreach (var wi in top10)
        {
            var id = wi.Id;
            var title = wi.Fields.TryGetValue("System.Title", out var t) ? t?.ToString() : "(no title)";
            var state = wi.Fields.TryGetValue("System.State", out var s) ? s?.ToString() : "?";
            Console.WriteLine($"  #{id,-6} [{state}] {title}");
        }

        if (!workItems.Any())
        {
            Console.WriteLine("  (no work items found)");
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  Error querying work items: {ex.Message}");
        Console.ResetColor();
    }
}

Console.WriteLine();
Console.WriteLine("Done.");
