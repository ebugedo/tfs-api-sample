using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;

namespace TfsApiSample;

/// <summary>
/// Provides high-level helper methods for common TFS / Azure DevOps REST API operations,
/// such as listing projects, querying work items, and enumerating Git repositories.
/// </summary>
public sealed class TfsApiClient : IDisposable
{
    private readonly VssConnection _connection;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of <see cref="TfsApiClient"/> and opens a connection
    /// to the TFS / Azure DevOps collection using Personal Access Token authentication.
    /// </summary>
    /// <param name="config">Connection settings including the collection URL and PAT.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="config"/> is <c>null</c>.</exception>
    public TfsApiClient(TfsClientConfig config)
    {
        ArgumentNullException.ThrowIfNull(config);

        var credentials = new VssBasicCredential(string.Empty, config.PersonalAccessToken);
        _connection = new VssConnection(new Uri(config.CollectionUrl), credentials);
    }

    /// <summary>
    /// Returns all team projects visible to the authenticated account.
    /// </summary>
    public async Task<IReadOnlyList<TeamProjectReference>> GetProjectsAsync(
        CancellationToken cancellationToken = default)
    {
        // GetProjects is a synchronous paged method without a CancellationToken overload;
        // cancellation applies to the client-acquisition step above.
        using var client = await _connection.GetClientAsync<ProjectHttpClient>(cancellationToken);
        var projects = await client.GetProjects(stateFilter: ProjectState.All);
        return projects.AsReadOnly();
    }

    /// <summary>
    /// Returns the details of a single work item by its numeric ID.
    /// </summary>
    /// <param name="workItemId">The ID of the work item to retrieve.</param>
    public async Task<WorkItem> GetWorkItemAsync(
        int workItemId,
        CancellationToken cancellationToken = default)
    {
        using var client = await _connection.GetClientAsync<WorkItemTrackingHttpClient>(cancellationToken);
        return await client.GetWorkItemAsync(workItemId, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Returns a batch of work items by their IDs.
    /// </summary>
    /// <param name="workItemIds">A collection of work-item IDs to retrieve.</param>
    public async Task<IReadOnlyList<WorkItem>> GetWorkItemsAsync(
        IEnumerable<int> workItemIds,
        CancellationToken cancellationToken = default)
    {
        using var client = await _connection.GetClientAsync<WorkItemTrackingHttpClient>(cancellationToken);
        var items = await client.GetWorkItemsAsync(workItemIds, cancellationToken: cancellationToken);
        return items.AsReadOnly();
    }

    /// <summary>
    /// Runs a WIQL query and returns the matching work items.
    /// </summary>
    /// <param name="wiqlQuery">A WIQL SELECT statement, e.g. <c>"SELECT [Id] FROM WorkItems WHERE ..."</c></param>
    /// <param name="project">
    /// Optional project scope.  When supplied the query is executed within that project context.
    /// </param>
    public async Task<IReadOnlyList<WorkItem>> QueryWorkItemsAsync(
        string wiqlQuery,
        string? project = null,
        CancellationToken cancellationToken = default)
    {
        using var client = await _connection.GetClientAsync<WorkItemTrackingHttpClient>(cancellationToken);

        var wiql = new Wiql { Query = wiqlQuery };
        var queryResult = await client.QueryByWiqlAsync(wiql, project, cancellationToken: cancellationToken);

        var ids = queryResult.WorkItems.Select(r => r.Id).ToArray();
        if (ids.Length == 0)
        {
            return [];
        }

        var items = await client.GetWorkItemsAsync(ids, cancellationToken: cancellationToken);
        return items.AsReadOnly();
    }

    /// <summary>
    /// Returns all Git repositories in the specified project.
    /// </summary>
    /// <param name="project">The name or ID of the project.</param>
    public async Task<IReadOnlyList<GitRepository>> GetRepositoriesAsync(
        string project,
        CancellationToken cancellationToken = default)
    {
        using var client = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
        var repos = await client.GetRepositoriesAsync(project, cancellationToken: cancellationToken);
        return repos.AsReadOnly();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _connection.Dispose();
        _disposed = true;
    }
}
