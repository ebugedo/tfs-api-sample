# tfs-api-sample

A .NET 8 console application demonstrating how to interact with the **TFS / Azure DevOps REST API** using the official Microsoft client libraries.

## Features

| Sample | Description |
|--------|-------------|
| List Projects | Retrieves all team projects visible to the authenticated account |
| List Repositories | Lists all Git repositories inside a specified project |
| Query Work Items | Runs a WIQL query and prints the 10 most-recently-changed active work items |

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Azure DevOps Services **or** an on-premises TFS instance (≥ TFS 2017)
- A **Personal Access Token (PAT)** with at minimum the following scopes:
  - `vso.project` – read projects
  - `vso.code_read` – read repositories
  - `vso.work` – read work items

## Quick Start

### 1. Clone and configure

```bash
git clone https://github.com/ebugedo/tfs-api-sample.git
cd tfs-api-sample
```

Edit `TfsApiSample/appsettings.json` and replace the placeholder values:

```json
{
  "Tfs": {
    "CollectionUrl": "https://dev.azure.com/<your-organization>",
    "PersonalAccessToken": "<your-pat>",
    "ProjectName": "<your-project-name>"
  }
}
```

For on-premises TFS the URL format is `http(s)://<server>/<collection>`.

> **Note:** Never commit a real PAT to source control.  
> You can alternatively set the values via environment variables:
> ```
> Tfs__CollectionUrl=https://dev.azure.com/myorg
> Tfs__PersonalAccessToken=<pat>
> Tfs__ProjectName=MyProject
> ```

### 2. Build and run

```bash
dotnet run --project TfsApiSample/TfsApiSample.csproj
```

Example output:

```
=== TFS / Azure DevOps REST API Sample ===
Collection : https://dev.azure.com/myorg

--- Projects ---
  [WellFormed] MyProject  (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx)

--- Git Repositories in 'MyProject' ---
  MyProject  (https://myorg@dev.azure.com/myorg/MyProject/_git/MyProject)

--- Recent Active Work Items in 'MyProject' ---
  #42     [Active] Fix login bug
  #41     [Active] Add dark mode
  ...

Done.
```

## Project Structure

```
TfsApiSample/
├── TfsApiSample.csproj        # Project file
├── appsettings.json           # Connection settings (replace placeholders)
├── TfsClientConfig.cs         # Configuration model
├── TfsApiClient.cs            # Reusable API client wrapper
└── Program.cs                 # Entry point – runs the three samples
```

## Key Classes

### `TfsApiClient`

Wraps `VssConnection` and exposes async methods:

| Method | Description |
|--------|-------------|
| `GetProjectsAsync()` | Returns all team projects |
| `GetWorkItemAsync(id)` | Returns a single work item by ID |
| `GetWorkItemsAsync(ids)` | Returns multiple work items by IDs |
| `QueryWorkItemsAsync(wiql, project)` | Runs a WIQL query and returns matching items |
| `GetRepositoriesAsync(project)` | Returns all Git repos in a project |

### `TfsClientConfig`

Maps the `Tfs` section from `appsettings.json` (or environment variables prefixed with `Tfs__`).

## License

This sample is released under the [MIT License](LICENSE).

