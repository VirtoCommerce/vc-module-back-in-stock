
## Package manager
Add-Migration Initial -Context VirtoCommerce.BackInStockModule.Data.Repositories.BackInStockDbContext  -Verbose -OutputDir Migrations -Project VirtoCommerce.BackInStockModule.Data.PostgreSql -StartupProject VirtoCommerce.BackInStockModule.Data.PostgreSql  -Debug



### Entity Framework Core Commands
```
dotnet tool install --global dotnet-ef --version 6.*
```

**Generate Migrations**

```
dotnet ef migrations add Initial -- "{connection string}"
dotnet ef migrations add Update1 -- "{connection string}"
dotnet ef migrations add Update2 -- "{connection string}"
```

etc..

**Apply Migrations**

`dotnet ef database update -- "{connection string}"`
