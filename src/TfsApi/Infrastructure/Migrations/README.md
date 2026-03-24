Remove-Migration -Force -Context TfsApiDbContext -Project Infrastructure -StartupProject WebApi -Verbose
Add-Migration -Name XXXXX -OutputDir Migrations -Context TfsApiDbContext -Project Infrastructure -StartupProject WebApi -Verbose
Update-Database -Context TfsApiDbContext -Project Infrastructure -StartupProject WebApi -Verbose