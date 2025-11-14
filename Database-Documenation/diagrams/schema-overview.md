# Database Schema Overview

This document provides a visual overview of the Inventory Management System database schema with detailed explanations of the relationships and data flow.

## High-Level Architecture

The database follows a domain-driven design pattern with clear separation of concerns:

```mermaid
graph TB
    subgraph "Core Business Domain"
        Device[Devices]
        Category[Categories]
        Brand[Brands]
        Supplier[Suppliers]
    end
    
    subgraph "Assignment Domain"
        DeviceAssignment[DeviceAssignments]
        Employee[Employees]
        Office[Offices]
    end
    
    subgraph "Maintenance Domain"
        MaintenanceSchedule[MaintenanceSchedules]
        ServiceHistory[ServiceHistories]
    end
    
    subgraph "Reporting Domain"
        Report[Reports]
    end
    
    subgraph "Identity Domain"
        User[AspNetUsers]
        Role[AspNetRoles]
        UserRole[AspNetUserRoles]
    end
    
    Category --> Device
    Brand --> Device
    Supplier --> Device
    Device --> DeviceAssignment
    DeviceAssignment --> Employee
    DeviceAssignment --> Office
    Device --> MaintenanceSchedule
    Device --> ServiceHistory
    
    User --> UserRole
    Role --> UserRole
    
    classDef business fill:#e1f5fe
    classDef assignment fill:#f3e5f5
    classDef maintenance fill:#e8f5e8
    classDef reporting fill:#fff3e0
    classDef identity fill:#fce4ec
    
    class Device,Category,Brand,Supplier business
    class DeviceAssignment,Employee,Office assignment
    class MaintenanceSchedule,ServiceHistory maintenance
    class Report reporting
    class User,Role,UserRole identity
```

## Detailed Entity Relationships

### Device-Centric View

```mermaid
erDiagram
    Device ||--o{ MaintenanceSchedule : "schedules"
    Device ||--o{ ServiceHistory : "records"
    Device ||--o| DeviceAssignment : "assigned_as"
    Device }o--|| Category : "belongs_to"
    Device }o--|| Brand : "manufactured_by"
    Device }o--|| Supplier : "provided_by"
    
    DeviceAssignment }o--|| Employee : "assigned_to"
    DeviceAssignment }o--|| Office : "located_at"
    
    Device {
        uniqueidentifier Id PK
        string Name
        string SerialNumber
        uniqueidentifier CategoryId FK
        uniqueidentifier BrandId FK
        uniqueidentifier SupplierId FK
        boolean IsFaulty
        boolean IsAvailable
    }
    
    DeviceAssignment {
        uniqueidentifier Id PK
        uniqueidentifier DeviceId FK,UK
        uniqueidentifier EmployeeId FK
        uniqueidentifier OfficeId FK
        datetime AssignedDate
        datetime ReturnedDate
        boolean IsActive
    }
```

### Assignment Flow Diagram

```mermaid
sequenceDiagram
    participant Admin
    participant Device as Device Table
    participant Assignment as DeviceAssignment Table
    participant Employee as Employee Table
    
    Admin->>Device: Check availability
    Device-->>Admin: Available: true
    
    Admin->>Assignment: Create new assignment
    Assignment->>Device: Update IsAvailable = false
    Device-->>Assignment: Updated
    
    Assignment-->>Admin: Assignment created
    
    Note over Assignment: Device is now assigned and unavailable
    
    Admin->>Assignment: Return device
    Assignment->>Device: Update IsAvailable = true
    Device-->>Assignment: Updated
    Assignment->>Assignment: Set IsActive = false, ReturnedDate = now
    Assignment-->>Admin: Device returned
```

### Maintenance Workflow

```mermaid
stateDiagram-v2
    [*] --> Scheduled
    Scheduled --> InProgress: Start Maintenance
    InProgress --> Completed: Finish Successfully
    InProgress --> Failed: Maintenance Fails
    Failed --> Scheduled: Reschedule
    Completed --> [*]
    
    Scheduled --> ServiceRecord: Create Service History
    Completed --> ServiceRecord: Update Service History
    Failed --> ServiceRecord: Log Failure
```

## Data Flow Patterns

### Device Lifecycle

```mermaid
flowchart TD
    A[New Device Added] --> B{Available?}
    B -->|Yes| C[Available Pool]
    B -->|No| D[Faulty/In Maintenance]
    
    C --> E[Assigned to Employee]
    E --> F[Device in Use]
    F --> G{Status Change}
    
    G -->|Return| H[Device Returned]
    G -->|Report Fault| I[Marked Faulty]
    G -->|Scheduled Maintenance| J[Maintenance Scheduled]
    
    H --> C
    I --> D
    J --> K[Maintenance Performed]
    K --> L{Maintenance Result}
    
    L -->|Success| M[Service Record Created]
    L -->|Failed| I
    
    M --> C
    D --> N[Repair Process]
    N --> O[Device Repaired]
    O --> C
    
    style A fill:#e1f5fe
    style C fill:#e8f5e8
    style D fill:#ffebee
    style F fill:#fff3e0
    style M fill:#f3e5f5
```

### User Access Flow

```mermaid
flowchart LR
    A[User Login] --> B{Authentication}
    B -->|Success| C[Load User Profile]
    B -->|Failed| D[Access Denied]
    
    C --> E{Check Roles}
    E -->|Admin| F[Full System Access]
    E -->|User| G[Limited Access]
    
    F --> H[Device Management]
    F --> I[User Management]
    F --> J[Reports]
    F --> K[System Settings]
    
    G --> L[View Assigned Devices]
    G --> M[Request Assignments]
    G --> N[View Service History]
    
    style B fill:#fff3e0
    style E fill:#e8f5e8
    style F fill:#e1f5fe
    style G fill:#f3e5f5
```

## Physical Database Layout

### Table Distribution

| Schema | Table | Primary Key | Foreign Keys | Indexes |
|--------|-------|-------------|--------------|---------|
| dbo | Categories | Id | - | PK |
| dbo | Brands | Id | - | PK |
| dbo | Suppliers | Id | - | PK |
| dbo | Employees | Id | - | PK |
| dbo | Offices | Id | - | PK |
| dbo | Devices | Id | CategoryId, BrandId, SupplierId | PK, FKx3 |
| dbo | DeviceAssignments | Id | DeviceId, EmployeeId, OfficeId | PK, UK, FKx3 |
| dbo | MaintenanceSchedules | Id | DeviceId | PK, FK |
| dbo | ServiceHistories | Id | DeviceId | PK, FK |
| dbo | Reports | Id | - | PK |
| dbo | AspNetUsers | Id | - | PK, UserName, Email |
| dbo | AspNetRoles | Id | - | PK, RoleName |
| dbo | AspNetUserRoles | UserId, RoleId | UserId, RoleId | PK, FKx2 |
| dbo | AspNetUserClaims | Id | UserId | PK, FK |
| dbo | AspNetUserLogins | LoginProvider, ProviderKey | UserId | PK, FK |
| dbo | AspNetUserTokens | UserId, LoginProvider, Name | UserId | PK, FK |
| dbo | AspNetRoleClaims | Id | RoleId | PK, FK |

### Data Volume Estimates

| Table | Expected Records | Record Size | Total Size |
|-------|------------------|-------------|------------|
| Categories | 10-50 | 100 bytes | < 1 MB |
| Brands | 20-100 | 100 bytes | < 1 MB |
| Suppliers | 10-50 | 200 bytes | < 1 MB |
| Employees | 100-1000 | 500 bytes | < 1 MB |
| Offices | 5-50 | 200 bytes | < 1 MB |
| Devices | 1000-10000 | 1 KB | 1-10 MB |
| DeviceAssignments | 5000-50000 | 1 KB | 5-50 MB |
| MaintenanceSchedules | 2000-20000 | 1 KB | 2-20 MB |
| ServiceHistories | 3000-30000 | 1 KB | 3-30 MB |
| Reports | 100-1000 | 10 KB | 1-10 MB |
| AspNetUsers | 100-1000 | 1 KB | < 1 MB |
| AspNetRoles | 2-10 | 100 bytes | < 1 MB |
| Other Identity Tables | 500-5000 | 500 bytes | < 5 MB |

## Performance Considerations

### Hot Tables (High Activity)
1. **Devices** - Frequent reads for availability checks
2. **DeviceAssignments** - High read/write for assignment tracking
3. **AspNetUsers** - Frequent authentication checks

### Warm Tables (Medium Activity)
1. **MaintenanceSchedules** - Daily maintenance checks
2. **ServiceHistories** - Service record lookups
3. **Reports** - Periodic report generation

### Cold Tables (Low Activity)
1. **Categories, Brands, Suppliers** - Reference data
2. **Employees, Offices** - HR data updates
3. **AspNetRoles** - Rare role changes

### Query Patterns

#### Most Common Queries
1. Device availability lookup
2. Current assignments by employee
3. Maintenance schedule by date
4. Service history by device
5. User authentication

#### Report Queries
1. Inventory summary by category
2. Device utilization reports
3. Assignment duration analysis
4. Maintenance compliance reports
5. User activity reports

## Security Model

### Access Control Matrix

| Role | Read | Write | Delete | Admin |
|------|------|-------|--------|-------|
| Admin | All | All | All | All |
| Manager | All | Limited | Limited | Limited |
| User | Personal | Personal | None | None |
| Auditor | Read | None | None | None |

### Data Classification

| Classification | Tables | Access Level |
|----------------|--------|--------------|
| Public | Categories, Brands, Suppliers | Read |
| Internal | Devices, Employees, Offices | Role-based |
| Confidential | Assignments, Service History | Restricted |
| Sensitive | User Credentials, Authentication | Admin Only |

## Integration Points

### External Systems
1. **HR System** - Employee data synchronization
2. **Procurement System** - Supplier and purchase information
3. **Asset Management** - Financial asset tracking
4. **Help Desk System** - Service request integration

### API Interfaces
1. **REST API** - Device management operations
2. **GraphQL** - Flexible reporting queries
3. **SOAP** - Legacy system integration
4. **OData** - Advanced filtering and querying

## Scalability Considerations

### Vertical Scaling
- Increase server CPU, RAM, and storage
- Optimize existing queries and indexes
- Implement caching strategies

### Horizontal Scaling
- Read replicas for reporting
- Database sharding by department/location
- Partitioning large tables by date

### Archive Strategy
- Archive completed assignments older than 5 years
- Archive service history older than 7 years
- Maintain current 3 years of data online

This schema overview provides a comprehensive understanding of the database structure, relationships, and operational patterns to support effective system management and future enhancements.