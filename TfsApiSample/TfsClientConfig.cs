namespace TfsApiSample;

/// <summary>
/// Holds the configuration settings required to connect to a TFS / Azure DevOps organization.
/// </summary>
public class TfsClientConfig
{
    /// <summary>
    /// Gets or sets the Azure DevOps / TFS collection URL.
    /// For Azure DevOps Services: https://dev.azure.com/{organization}
    /// For on-premises TFS: http(s)://{server}/{collection}
    /// </summary>
    public string CollectionUrl { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Personal Access Token (PAT) used for authentication.
    /// The token must have read access for Work Items and Code scopes.
    /// </summary>
    public string PersonalAccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the default project name used when listing repositories
    /// and querying work items.
    /// </summary>
    public string ProjectName { get; set; } = string.Empty;
}
