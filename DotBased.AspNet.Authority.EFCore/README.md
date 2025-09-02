# EF Core database

## Add migration project
```csharp
options.UseSqlite("Data Source=dev-dotbased.db", c => c.MigrationsAssembly("PROJECT-NAME"));
```

## EF Tool

Add migration
```shell
dotnet ef migrations add MIGRATION-NAME --project PROJECT-NAME
```

Remove migrations
```shell
dotnet ef migrations remove --project PROJECT-NAME
```

Update database
```shell
dotnet ef database update --project PROJECT-NAME
```
