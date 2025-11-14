# Deployment Guide - Comprehensive Inventory Management System

## Overview

This guide provides step-by-step instructions for deploying the Comprehensive Inventory Management System in various environments, including development, staging, and production.

## Prerequisites

### System Requirements

#### **Minimum Requirements**
- **CPU**: 2 cores
- **RAM**: 4GB
- **Storage**: 50GB SSD
- **OS**: Windows Server 2019+ / Ubuntu 20.04+ / CentOS 8+

#### **Recommended Requirements**
- **CPU**: 4+ cores
- **RAM**: 8GB+
- **Storage**: 100GB+ SSD
- **OS**: Windows Server 2022 / Ubuntu 22.04 / CentOS 9

#### **Software Dependencies**
- **.NET 8.0 SDK** or later
- **Node.js 20.0** or later
- **Angular CLI 18.0** or later
- **SQL Server 2019** or later / PostgreSQL 13+ / MySQL 8.0+
- **Git** for source control

## Database Setup

### SQL Server Configuration

#### **1. Database Creation**
```sql
CREATE DATABASE InventoryMSDb;
GO

USE InventoryMSDb;
GO
```

#### **2. User Configuration**
```sql
CREATE LOGIN [InventoryMSUser] WITH PASSWORD = 'YourStrongPassword123!';
GO

USE InventoryMSDb;
CREATE USER [InventoryMSUser] FOR LOGIN [InventoryMSUser];
ALTER ROLE db_owner ADD MEMBER [InventoryMSUser];
GO
```

#### **3. Enable Required Features**
```sql
-- Enable identity insert for GUID columns
ALTER DATABASE InventoryMSDb SET ALLOW_SNAPSHOT_ISOLATION ON;
GO
```

### Database Migrations

#### **Apply Migrations via Package Manager Console**
```powershell
Update-Database -Context RepositoryContext
```

#### **Apply Migrations via .NET CLI**
```bash
dotnet ef database update --context RepositoryContext
```

## Backend Deployment

### 1. Application Configuration

#### **appsettings.json Configuration**
```json
{
  "ConnectionStrings": {
    "sqlConnection": "Server=your-server;Database=InventoryMSDb;User Id=InventoryMSUser;Password=YourStrongPassword123!;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "ValidIssuer": "InventoryMS",
    "ValidAudience": "InventoryMS",
    "Secret": "your-super-secret-jwt-key-with-at-least-32-characters",
    "ExpiresInMinutes": 60
  },
  "EmailSettings": {
    "SmtpServer": "smtp.yourcompany.com",
    "Port": 587,
    "UseSSL": true,
    "Username": "inventory-system@yourcompany.com",
    "Password": "your-email-password"
  },
  "FileStorage": {
    "UploadPath": "C:\\InventoryMS\\Uploads",
    "MaxFileSize": 10485760,
    "AllowedExtensions": ".jpg,.jpeg,.png,.pdf,.doc,.docx"
  }
}
```

#### **appsettings.Production.json**
```json
{
  "ConnectionStrings": {
    "sqlConnection": "Server=prod-server;Database=InventoryMSDb;User Id=InventoryMSProdUser;Password=ProductionPassword!;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "JWT": {
    "Secret": "production-super-secret-key-minimum-32-characters"
  }
}
```

### 2. Build and Publish

#### **Development Build**
```bash
dotnet build InventrySystem.sln --configuration Debug
```

#### **Production Build**
```bash
dotnet build InventrySystem.sln --configuration Release
```

#### **Publish for Deployment**
```bash
dotnet publish InventrySystem/InventrySystem.csproj \
  --configuration Release \
  --output ./publish \
  --self-contained false \
  --runtime win-x64
```

### 3. IIS Deployment

#### **Create IIS Site**
1. Open **IIS Manager**
2. Right-click **Sites** → **Add Website**
3. Configure:
   - **Site name**: InventoryMS
   - **Physical path**: C:\inetpub\InventoryMS
   - **Port**: 443 (HTTPS)
   - **Host name**: inventory.yourcompany.com

#### **Configure Application Pool**
1. Create new Application Pool: **InventoryMSPool**
2. **.NET CLR version**: **No Managed Code**
3. **Managed pipeline mode**: **Integrated**
4. **Identity**: **ApplicationPoolIdentity**

#### **SSL Certificate**
1. Install SSL certificate for your domain
2. Bind certificate to the site
3. Enforce HTTPS in web.config:
```xml
<system.webServer>
  <rewrite>
    <rules>
      <rule name="HTTP to HTTPS redirect" stopProcessing="true">
        <match url="(.*)" />
        <conditions>
          <add input="{HTTPS}" pattern="off" ignoreCase="true" />
        </conditions>
        <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
      </rule>
    </rules>
  </rewrite>
</system.webServer>
```

### 4. Docker Deployment

#### **Dockerfile for Backend**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["InventrySystem/InventrySystem.csproj", "InventrySystem/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["Contracts/Contracts.csproj", "Contracts/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "InventrySystem/InventrySystem.csproj"
COPY . .
WORKDIR "/src/InventrySystem"
RUN dotnet build "InventrySystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InventrySystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InventrySystem.dll"]
```

#### **docker-compose.yml**
```yaml
version: '3.8'

services:
  inventory-api:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "5001:80"
      - "5002:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__sqlConnection=Server=sql-server;Database=InventoryMSDb;User Id=sa;Password=YourPassword123!
    depends_on:
      - sql-server
    networks:
      - inventory-network

  sql-server:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
    ports:
      - "1433:1433"
    volumes:
      - sql-data:/var/opt/mssql
    networks:
      - inventory-network

volumes:
  sql-data:

networks:
  inventory-network:
    driver: bridge
```

## Frontend Deployment

### 1. Angular Configuration

#### **environment.prod.ts**
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://inventory.yourcompany.com',
  enableDebug: false,
  version: '1.0.0'
};
```

#### **angular.json Build Configuration**
```json
{
  "projects": {
    "inventoryui": {
      "architect": {
        "build": {
          "configurations": {
            "production": {
              "fileReplacements": [
                {
                  "replace": "src/environments/environment.ts",
                  "with": "src/environments/environment.prod.ts"
                }
              ],
              "optimization": true,
              "outputHashing": "all",
              "sourceMap": false,
              "extractCss": true,
              "namedChunks": false,
              "extractLicenses": true,
              "vendorChunk": false,
              "buildOptimizer": true,
              "budgets": [
                {
                  "type": "initial",
                  "maximumWarning": "2mb",
                  "maximumError": "5mb"
                }
              ]
            }
          }
        }
      }
    }
  }
}
```

### 2. Build for Production

```bash
# Install dependencies
npm install

# Build production version
ng build --configuration production

# The output will be in dist/inventoryui/
```

### 3. Web Server Configuration

#### **IIS Configuration**
1. Create new site for frontend
2. Point to dist/inventoryui/ folder
3. Configure URL rewrite for SPA routing:

**web.config** (in dist/inventoryui/)
```xml
<?xml version="1.0" encoding="UTF-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
    <staticContent>
      <remove fileExtension=".woff" />
      <mimeMap fileExtension=".woff" mimeType="application/x-font-woff" />
      <remove fileExtension=".woff2" />
      <mimeMap fileExtension=".woff2" mimeType="application/font-woff2" />
    </staticContent>
  </system.webServer>
</configuration>
```

#### **Nginx Configuration**
```nginx
server {
    listen 80;
    server_name inventory.yourcompany.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name inventory.yourcompany.com;

    ssl_certificate /path/to/certificate.crt;
    ssl_certificate_key /path/to/private.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers HIGH:!aNULL:!MD5;

    root /var/www/inventory/dist/inventoryui;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api/ {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }

    # Cache static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

## Security Configuration

### 1. HTTPS and SSL

#### **SSL Certificate Setup**
```bash
# Generate self-signed certificate (development)
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout inventory.key \
  -out inventory.crt \
  -subj "/C=US/ST=State/L=City/O=Company/CN=inventory.yourcompany.com"

# For production, use Let's Encrypt or purchase from CA
certbot --nginx -d inventory.yourcompany.com
```

### 2. Firewall Configuration

#### **Windows Firewall**
```powershell
# Allow HTTP/HTTPS traffic
New-NetFirewallRule -DisplayName "InventoryMS HTTP" -Direction Inbound -Protocol TCP -LocalPort 80 -Action Allow
New-NetFirewallRule -DisplayName "InventoryMS HTTPS" -Direction Inbound -Protocol TCP -LocalPort 443 -Action Allow

# Allow database access (if needed)
New-NetFirewallRule -DisplayName "InventoryMS SQL" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
```

#### **Linux Firewall (UFW)**
```bash
# Allow SSH, HTTP, HTTPS
sudo ufw allow 22/tcp
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Enable firewall
sudo ufw enable
```

### 3. Application Security

#### **Security Headers in ASP.NET Core**
```csharp
// Program.cs
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseHsts();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self' 'unsafe-inline'; style-src 'self' 'unsafe-inline'");
    
    await next();
});
```

## Monitoring and Logging

### 1. Application Monitoring

#### **Health Checks**
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddDbContextCheck<RepositoryContext>()
    .AddCheck("self", () => HealthCheckResult.Healthy());

app.UseHealthChecks("/health");
```

#### **Application Insights Integration**
```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();

app.UseApplicationInsightsRequestTelemetry();
app.UseApplicationInsightsExceptionTelemetry();
```

### 2. Logging Configuration

#### **Serilog Configuration**
```csharp
// Program.cs
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/inventory-ms-.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Seq("http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();
```

## Performance Optimization

### 1. Database Optimization

#### **Indexes**
```sql
-- Create indexes for performance
CREATE INDEX IX_InventoryTransactions_DeviceId ON InventoryTransactions(DeviceId);
CREATE INDEX IX_InventoryTransactions_TransactionDate ON InventoryTransactions(TransactionDate);
CREATE INDEX IX_StockLevels_DeviceId_WarehouseId ON StockLevels(DeviceId, WarehouseId);
CREATE INDEX IX_PurchaseOrders_Status ON PurchaseOrders(Status);
CREATE INDEX IX_StockLevels_NeedsReorder ON StockLevels(QuantityAvailable, ReorderLevel) WHERE IsActive = 1;
```

#### **Query Optimization**
```sql
-- Partition large tables by date if needed
CREATE PARTITION FUNCTION TransactionDateRangePF (datetime2)
AS RANGE RIGHT FOR VALUES ('2023-01-01', '2024-01-01', '2025-01-01');

CREATE PARTITION SCHEME TransactionDateRangePS
AS PARTITION TransactionDateRangePF
ALL TO ([PRIMARY]);
```

### 2. Caching Strategy

#### **Redis Cache Integration**
```csharp
// Program.cs
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Cache frequently accessed data
public class WarehouseService
{
    private readonly IMemoryCache _cache;
    private readonly IWarehouseRepository _repository;

    public async Task<IEnumerable<Warehouse>> GetActiveWarehousesAsync()
    {
        var cacheKey = "active_warehouses";
        if (!_cache.TryGetValue(cacheKey, out IEnumerable<Warehouse> warehouses))
        {
            warehouses = await _repository.GetActiveWarehousesAsync(false);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));
            _cache.Set(cacheKey, warehouses, cacheOptions);
        }
        return warehouses;
    }
}
```

## Backup and Recovery

### 1. Database Backup Strategy

#### **Automated Backup Script**
```sql
-- Full backup
BACKUP DATABASE InventoryMSDb 
TO DISK = 'C:\Backups\InventoryMSDb_Full_' + FORMAT(GETDATE(), 'yyyyMMddHHmmss') + '.bak'
WITH COMPRESSION, STATS = 10;

-- Differential backup
BACKUP DATABASE InventoryMSDb 
TO DISK = 'C:\Backups\InventoryMSDb_Diff_' + FORMAT(GETDATE(), 'yyyyMMddHHmmss') + '.bak'
WITH DIFFERENTIAL, COMPRESSION, STATS = 10;

-- Transaction log backup
BACKUP LOG InventoryMSDb 
TO DISK = 'C:\Backups\InventoryMSDb_Log_' + FORMAT(GETDATE(), 'yyyyMMddHHmmss') + '.trn'
WITH COMPRESSION, STATS = 10;
```

#### **PowerShell Backup Script**
```powershell
$backupPath = "C:\Backups"
$databaseName = "InventoryMSDb"
$timestamp = Get-Date -Format "yyyyMMddHHmmss"

# Full backup
$sqlcmd = "BACKUP DATABASE [$databaseName] TO DISK = '$backupPath\$databaseName`_Full_$timestamp.bak' WITH COMPRESSION, STATS = 10"
Invoke-Sqlcmd -Query $sqlcmd -ServerInstance "localhost"

# Clean up old backups (keep last 30 days)
Get-ChildItem $backupPath -Filter "*.bak" | Where-Object CreationTime -lt (Get-Date).AddDays(-30) | Remove-Item -Force
```

### 2. Application Backup

#### **File System Backup**
```bash
#!/bin/bash
# Linux backup script
BACKUP_DIR="/backups/inventory-ms"
APP_DIR="/var/www/inventory"
DATE=$(date +%Y%m%d_%H%M%S)

# Create backup directory
mkdir -p $BACKUP_DIR/$DATE

# Backup application files
tar -czf $BACKUP_DIR/$DATE/app_backup_$DATE.tar.gz $APP_DIR

# Backup configuration
cp /etc/nginx/sites-available/inventory.yourcompany.com $BACKUP_DIR/$DATE/nginx_config_$DATE
cp -r $APP_DIR/ssl $BACKUP_DIR/$DATE/

# Clean up old backups (keep 30 days)
find $BACKUP_DIR -type d -mtime +30 -exec rm -rf {} \;
```

## Troubleshooting

### Common Issues and Solutions

#### **1. Database Connection Issues**
```
Error: A network-related or instance-specific error occurred
```
**Solution:**
- Check connection string configuration
- Verify SQL Server is running
- Check firewall settings
- Validate user permissions

#### **2. CORS Issues**
```
Error: No 'Access-Control-Allow-Origin' header is present
```
**Solution:**
```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("https://inventory.yourcompany.com")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

app.UseCors("AllowFrontend");
```

#### **3. File Upload Issues**
```
Error: Request body too large
```
**Solution:**
```csharp
// Program.cs
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600; // 100MB
});
```

#### **4. Performance Issues**
**Diagnosis Steps:**
1. Check database query performance
2. Monitor memory usage
3. Review application logs
4. Use profiling tools

## Maintenance

### Regular Maintenance Tasks

#### **Daily**
- Check application health
- Review error logs
- Monitor system resources

#### **Weekly**
- Database maintenance (rebuild indexes, update statistics)
- Review backup success
- Security patch updates

#### **Monthly**
- Performance tuning
- Capacity planning
- Security audit
- Documentation updates

### Update Process

#### **Application Updates**
1. Backup current version
2. Deploy new version to staging
3. Perform testing
4. Schedule downtime window
5. Deploy to production
6. Monitor performance

#### **Database Updates**
1. Create backup before migration
2. Test migrations on staging
3. Apply migrations during maintenance window
4. Validate data integrity
5. Update application connection strings if needed

This comprehensive deployment guide provides everything needed to successfully deploy and maintain the Inventory Management System in production environments.