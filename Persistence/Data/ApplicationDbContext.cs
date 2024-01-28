using Domain;
using Domain.Entities.Cashier.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Identity;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.LookUps;
using Domain.Entities.Setup;
using Domain.Entities.Supplier.Setup;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppUserRole, string>
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		#region Setup
		public DbSet<Nationality> Nationalities { get; set; }
		public DbSet<New> News { get; set; }
		public DbSet<Country> Countries { get; set; }
		public DbSet<Governorate> Governorates { get; set; }
		public DbSet<City> Cities { get; set; }
		public DbSet<Branch> Branches { get; set; }
		public DbSet<Company> Companies { get; set; }
		//public DbSet<User> Users { get; set; }
		public DbSet<UserBranch> UsersBranches { get; set; }
		public DbSet<ApplicationPagePrefix> ApplicationPagePrefixs { get; set; }
		public DbSet<UserType> UserTypes { get; set; }
		public DbSet<ApplicationTbl> ApplicationsTbl { get; set; }
		public DbSet<AppModule> AppModules { get; set; }
		public DbSet<AppPage> AppPages { get; set; }
		public DbSet<CurrencySetup> CurrencySetup { get; set; }
		public DbSet<Department> Departments { get; set; }
		public DbSet<Domain.Entities.Setup.Module> Modules { get; set; }
		public DbSet<UserModule> UserModules { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitTemplate> UnitTemplates { get; set; }
        public DbSet<StoreAdjustment> StoreAdjustments { get; set; }
        #endregion
        #region HR
        public DbSet<Employee> Employees { get; set; }
		public DbSet<HRFile> HRFiles { get; set; }
		public DbSet<EmployeeFiles> EmployeesFiles { get; set; }
		public DbSet<IdentityType> IdentityTypes { get; set; }
		#endregion
		#region Inventory
		#region Setup
		public DbSet<ItemType> ItemTypes { get; set; }
		public DbSet<ItemCategory> ItemCategories { get; set; }
		public DbSet<ItemClassification> ItemClassifications { get; set; }
		public DbSet<Item> Items { get; set; }
		public DbSet<Store> Stores { get; set; }
		public DbSet<ContactTypes> ContactTypes { get; set; }
		#endregion
		#endregion
		#region Supplier
		#region Setup
		public DbSet<SupplierType> SupplierTypes { get; set; }
		public DbSet<Delivery> Deliveryterms { get; set; }
        #endregion
        #endregion
        #region Cashier
        #region Setup
        //public DbSet<PaymentType> paymentTypes { get; set; }
        #endregion
        #endregion

        #region Identity
        public DbSet<ChangePasswordReason> ChangePasswordReasons { get; set; }
        public DbSet<AppUserChangePasswordReason> AppUserChangePasswordReasons { get; set; }
        public DbSet<AppUserRolePrivilege> AppUserRolePrivileges { get; set; }
        #endregion

        public DbSet<PaymentModesType> paymentModesType { get; set; }
        public DbSet<PaymentGroup> PaymentGroups { get; set; }
		public DbSet<PaymentModes> PaymentModes { get; set; }
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}
	}
}