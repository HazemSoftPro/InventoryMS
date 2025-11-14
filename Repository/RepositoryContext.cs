using Entities.Identity;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repository.Configuration;

namespace Repository
{
    public class RepositoryContext : IdentityDbContext<User, UserRole, string>
    {
        public RepositoryContext(DbContextOptions options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new SupplierConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleAssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new OfficeConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());

            // Existing relationships
            modelBuilder.Entity<Device>()
           .HasOne(d => d.Category)
           .WithMany(c => c.Devices)
           .HasForeignKey(d => d.CategoryId);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Brand)
                .WithMany(b => b.Devices)
                .HasForeignKey(d => d.BrandId);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.Supplier)
                .WithMany(s => s.Devices)
                .HasForeignKey(d => d.SupplierId);

            modelBuilder.Entity<Device>()
                .HasOne(d => d.CurrentAssignment)
                .WithOne(da => da.Device)
                .HasForeignKey<DeviceAssignment>(da => da.DeviceId);

            modelBuilder.Entity<DeviceAssignment>()
                .HasOne(da => da.Employee)
                .WithMany(e => e.DeviceAssignments)
                .HasForeignKey(da => da.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeviceAssignment>()
                .HasOne(da => da.Office)
                .WithMany(o => o.DeviceAssignments)
                .HasForeignKey(da => da.OfficeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MaintenanceSchedule>()
                .HasOne(ms => ms.Device)
                .WithMany(d => d.MaintenanceSchedules)
                .HasForeignKey(ms => ms.DeviceId);

            modelBuilder.Entity<ServiceHistory>()
                .HasOne(sh => sh.Device)
                .WithMany(d => d.ServiceHistories)
                .HasForeignKey(sh => sh.DeviceId);

            // New relationships for enhanced inventory management
            
            // InventoryTransaction relationships
            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.Device)
                .WithMany()
                .HasForeignKey(t => t.DeviceId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.FromUser)
                .WithMany()
                .HasForeignKey(t => t.FromUserId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.ToUser)
                .WithMany()
                .HasForeignKey(t => t.ToUserId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.FromOffice)
                .WithMany()
                .HasForeignKey(t => t.FromOfficeId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.ToOffice)
                .WithMany()
                .HasForeignKey(t => t.ToOfficeId);

            modelBuilder.Entity<InventoryTransaction>()
                .HasOne(t => t.ApprovedBy)
                .WithMany()
                .HasForeignKey(t => t.ApprovedById);

            // StockLevel relationships
            modelBuilder.Entity<StockLevel>()
                .HasOne(s => s.Device)
                .WithMany()
                .HasForeignKey(s => s.DeviceId);

            modelBuilder.Entity<StockLevel>()
                .HasOne(s => s.Warehouse)
                .WithMany(w => w.StockLevels)
                .HasForeignKey(s => s.WarehouseId);

            // Warehouse relationships
            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.CreatedBy)
                .WithMany()
                .HasForeignKey(w => w.CreatedById);

            modelBuilder.Entity<Warehouse>()
                .HasOne(w => w.UpdatedBy)
                .WithMany()
                .HasForeignKey(w => w.UpdatedById);

            // PurchaseOrder relationships
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Supplier)
                .WithMany()
                .HasForeignKey(po => po.SupplierId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Office)
                .WithMany()
                .HasForeignKey(po => po.OfficeId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Warehouse)
                .WithMany()
                .HasForeignKey(po => po.WarehouseId);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.CreatedBy)
                .WithMany()
                .HasForeignKey(po => po.CreatedById);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.ApprovedBy)
                .WithMany()
                .HasForeignKey(po => po.ApprovedById);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.PurchaseOrder)
                .WithMany(po => po.Items)
                .HasForeignKey(poi => poi.PurchaseOrderId);

            modelBuilder.Entity<PurchaseOrderItem>()
                .HasOne(poi => poi.Device)
                .WithMany()
                .HasForeignKey(poi => poi.DeviceId);

            // InventoryAlert relationships
            modelBuilder.Entity<InventoryAlert>()
                .HasOne(a => a.Device)
                .WithMany()
                .HasForeignKey(a => a.DeviceId);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(a => a.Category)
                .WithMany()
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(a => a.Office)
                .WithMany()
                .HasForeignKey(a => a.OfficeId);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(a => a.AcknowledgedBy)
                .WithMany()
                .HasForeignKey(a => a.AcknowledgedById);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(a => a.ResolvedBy)
                .WithMany()
                .HasForeignKey(a => a.ResolvedById);

            // AuditLog relationships
            modelBuilder.Entity<AuditLog>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);

            // InventoryReport relationships
            modelBuilder.Entity<InventoryReport>()
                .HasOne(r => r.GeneratedByUser)
                .WithMany()
                .HasForeignKey(r => r.GeneratedById);
        }

        // Existing DbSets
        public DbSet<Device> Devices { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Office> Offices { get; set; }
        public DbSet<DeviceAssignment> DeviceAssignments { get; set; }
        public DbSet<MaintenanceSchedule> MaintenanceSchedules { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<Report> Reports { get; set; }

        // New DbSets for enhanced inventory management
        public DbSet<InventoryTransaction> InventoryTransactions { get; set; }
        public DbSet<StockLevel> StockLevels { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<InventoryAlert> InventoryAlerts { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<InventoryReport> InventoryReports { get; set; }
    }
}
