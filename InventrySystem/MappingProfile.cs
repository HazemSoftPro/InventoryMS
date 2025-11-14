using AutoMapper;
using Entities.Identity;
using Entities.Models;
using Shared.DTO.Brand;
using Shared.DTO.Category;
using Shared.DTO.Device;
using Shared.DTO.DeviceAssignment;
using Shared.DTO.Employee;
using Shared.DTO.MaintenanceSchedule;
using Shared.DTO.Office;
using Shared.DTO.ServiceHistory;
using Shared.DTO.Supplier;
using Shared.DTO.User;
using Shared.DTO.InventoryTransaction;
using Shared.DTO.StockLevel;
using Shared.DTO.PurchaseOrder;
using Shared.DTO.Warehouse;

namespace InventrySystem
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<UserForRegistrationDto, User>()
                        .ForMember(u => u.UserName, opt => opt.MapFrom(x => GenerateValidUserName(x.Email)));

            CreateMap<Device, DeviceDto>()
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(d => d.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name));
            CreateMap<DeviceForCreationDto, Device>();
            CreateMap<DeviceForUpdateDto, Device>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryForCreationDto, Category>();
            CreateMap<CategoryForUpdateDto, Category>();

            CreateMap<Brand, BrandDto>();
            CreateMap<BrandForCreationDto, Brand>();
            CreateMap<BrandForUpdateDto, Brand>();

            CreateMap<Supplier, SupplierDto>();
            CreateMap<SupplierForCreationDto, Supplier>();
            CreateMap<SupplierForUpdateDto, Supplier>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreationDto, Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();

            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeForCreationDto, Office>();
            CreateMap<OfficeForUpdateDto, Office>();

            CreateMap<DeviceAssignment, DeviceAssignmentDto>()
                .ForMember(d => d.SerialNumber, opt => opt.MapFrom(src => src.Device.SerialNumber))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.Device.Name))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(src => src.Device.Category.Name))
                .ForMember(d => d.BrandName, opt => opt.MapFrom(src => src.Device.Brand.Name));
            CreateMap<DeviceAssignmentForCreationDto, DeviceAssignment>();
            CreateMap<DeviceAssignmentForOfficeDto, DeviceAssignment>();
            CreateMap<DeviceAssignmentForUpdateDto, DeviceAssignment>();

            CreateMap<MaintenanceSchedule, MaintenanceScheduleDto>();
            CreateMap<MaintenanceScheduleForCreationDto, MaintenanceSchedule>();
            CreateMap<MaintenanceScheduleForUpdateDto, MaintenanceSchedule>();

            CreateMap<ServiceHistory, ServiceHistoryDto>();
            CreateMap<ServiceHistoryForCreationDto, ServiceHistory>();
            CreateMap<ServiceHistoryForUpdateDto, ServiceHistory>();

            CreateMap<UserForRegistrationDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<UserRoleForCreationDto, UserRole>();
            CreateMap<UserRoleForUpdateDto, UserRole>();

            // New mappings for enhanced inventory management
            
            // InventoryTransaction mappings
            CreateMap<InventoryTransaction, InventoryTransactionDto>()
                .ForMember(d => d.DeviceName, opt => opt.MapFrom(src => src.Device != null ? src.Device.Name : null))
                .ForMember(d => d.DeviceSerialNumber, opt => opt.MapFrom(src => src.Device != null ? src.Device.SerialNumber : null))
                .ForMember(d => d.FromUserName, opt => opt.MapFrom(src => src.FromUser != null ? src.FromUser.UserName : null))
                .ForMember(d => d.ToUserName, opt => opt.MapFrom(src => src.ToUser != null ? src.ToUser.UserName : null))
                .ForMember(d => d.FromOfficeName, opt => opt.MapFrom(src => src.FromOffice != null ? src.FromOffice.Name : null))
                .ForMember(d => d.ToOfficeName, opt => opt.MapFrom(src => src.ToOffice != null ? src.ToOffice.Name : null))
                .ForMember(d => d.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? src.ApprovedBy.UserName : null))
                .ForMember(d => d.TransactionTypeDisplay, opt => opt.MapFrom(src => src.TransactionType.ToString()))
                .ForMember(d => d.StatusDisplay, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<InventoryTransactionForCreationDto, InventoryTransaction>();
            CreateMap<InventoryTransactionForUpdateDto, InventoryTransaction>();

            // StockLevel mappings
            CreateMap<StockLevel, StockLevelDto>()
                .ForMember(d => d.DeviceName, opt => opt.MapFrom(src => src.Device != null ? src.Device.Name : null))
                .ForMember(d => d.DeviceSerialNumber, opt => opt.MapFrom(src => src.Device != null ? src.Device.SerialNumber : null))
                .ForMember(d => d.CategoryName, opt => opt.MapFrom(src => src.Device != null &amp;&amp; src.Device.Category != null ? src.Device.Category.Name : null))
                .ForMember(d => d.BrandName, opt => opt.MapFrom(src => src.Device != null &amp;&amp; src.Device.Brand != null ? src.Device.Brand.Name : null))
                .ForMember(d => d.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : null));
            CreateMap<StockLevelForCreationDto, StockLevel>();
            CreateMap<StockLevelForUpdateDto, StockLevel>();

            // Warehouse mappings
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(d => d.UtilizationPercentage, opt => opt.MapFrom(src => src.Capacity > 0 ? (src.CurrentUtilization / src.Capacity) * 100 : 0))
                .ForMember(d => d.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : null))
                .ForMember(d => d.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedBy != null ? src.UpdatedBy.UserName : null))
                .ForMember(d => d.DeviceCount, opt => opt.MapFrom(src => src.Devices != null ? src.Devices.Count : 0))
                .ForMember(d => d.StockLevelCount, opt => opt.MapFrom(src => src.StockLevels != null ? src.StockLevels.Count : 0))
                .ForMember(d => d.TotalInventoryValue, opt => opt.MapFrom(src => src.StockLevels != null ? src.StockLevels.Sum(s => s.TotalValue) : 0));
            CreateMap<WarehouseForCreationDto, Warehouse>();
            CreateMap<WarehouseForUpdateDto, Warehouse>();

            // PurchaseOrder mappings
            CreateMap<PurchaseOrder, PurchaseOrderDto>()
                .ForMember(d => d.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
                .ForMember(d => d.OfficeName, opt => opt.MapFrom(src => src.Office != null ? src.Office.Name : null))
                .ForMember(d => d.WarehouseName, opt => opt.MapFrom(src => src.Warehouse != null ? src.Warehouse.Name : null))
                .ForMember(d => d.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : null))
                .ForMember(d => d.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? src.ApprovedBy.UserName : null))
                .ForMember(d => d.StatusDisplay, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<PurchaseOrderForCreationDto, PurchaseOrder>();
            CreateMap<PurchaseOrderForUpdateDto, PurchaseOrder>();

            // PurchaseOrderItem mappings
            CreateMap<PurchaseOrderItem, PurchaseOrderItemDto>()
                .ForMember(d => d.DeviceName, opt => opt.MapFrom(src => src.Device != null ? src.Device.Name : null))
                .ForMember(d => d.DeviceSerialNumber, opt => opt.MapFrom(src => src.Device != null ? src.Device.SerialNumber : null));
            CreateMap<PurchaseOrderItemForCreationDto, PurchaseOrderItem>();
        }
        private string GenerateValidUserName(string email)
        {
            var atIndex = email.IndexOf('@');
            if (atIndex > 0)
            {
                return email.Substring(0, atIndex);
            }
            return email;
        }
    }
}