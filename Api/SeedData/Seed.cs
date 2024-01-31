using System.Text.Json;
using Domain.Entities;
using Domain.Entities.Cashier.Setup;
using Domain.Entities.HR.Setup;
using Domain.Entities.Identity;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.LookUps;
using Domain.Entities.Setup;
using Domain.Entities.Supplier.Setup;
using Microsoft.AspNetCore.Identity;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;

namespace Api;

public static class Seed
{
	public static async Task SeedData(this IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();

		var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

		#region Phase 1
		var cityRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<City>>();

		await SeedData(unitOfWork, cityRepo, "./SeedData/Setup/cities.json");

        var applicationRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<ApplicationTbl>>();
        await SeedData(unitOfWork, applicationRepo, "./SeedData/applications.json");

        var appModuleRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<AppModule>>();
        await SeedData(unitOfWork, appModuleRepo, "./SeedData/appModules.json");

		var countryRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Country>>();
		await SeedData(unitOfWork, countryRepo, "./SeedData/Setup/countries.json");
		
		var departmentRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Department>>();
		await SeedData(unitOfWork, departmentRepo, "./SeedData/Setup/departments.json");
		
        var moduleRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Module>>();
        await SeedData(unitOfWork, moduleRepo, "./SeedData/Setup/modules.json");

        var branchRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Branch>>();
        await SeedData(unitOfWork, branchRepo, "./SeedData/Setup/branches.json");

        var itemType = scope.ServiceProvider.GetRequiredService<IGenericRepository<ItemType>>();
		await SeedData(unitOfWork, itemType, "./SeedData/Inventory/Setup/itemTypes.json");

		var itemCategory = scope.ServiceProvider.GetRequiredService<IGenericRepository<ItemCategory>>();
		await SeedData(unitOfWork, itemCategory, "./SeedData/Inventory/Setup/itemCategories.json");

		var itemClassification = scope.ServiceProvider.GetRequiredService<IGenericRepository<ItemClassification>>();
		await SeedData(unitOfWork, itemClassification, "./SeedData/Inventory/Setup/itemClassifications.json");

		var contactRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<ContactTypes>>();
		await SeedData(unitOfWork, contactRepo, "./SeedData/Inventory/Setup/contactType.json");

		var supplierRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<SupplierType>>();
		await SeedData(unitOfWork, supplierRepo, "./SeedData/Supplier/Setup/supplierType.json");

		var paymentTypesRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<PaymentModesType>>();
		await SeedData(unitOfWork, paymentTypesRepo, "./SeedData/Cashier/Setup/PaymentModeTypes.json");

		var DeliveryRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<Delivery>>();
		await SeedData(unitOfWork, DeliveryRepo, "./SeedData/Supplier/Setup/delivery.json");

        var paymentGroup = scope.ServiceProvider.GetRequiredService<IGenericRepository<PaymentGroup>>();
        await SeedData(unitOfWork, paymentGroup, "./SeedData/Cashier/Setup/PaymentGroup.json");
		
		var paymentModes = scope.ServiceProvider.GetRequiredService<IGenericRepository<PaymentModes>>();
        await SeedData(unitOfWork, paymentModes, "./SeedData/Cashier/Setup/PaymentMode.json");
		
		var news = scope.ServiceProvider.GetRequiredService<IGenericRepository<New>>();
        await SeedData(unitOfWork, news, "./SeedData/Setup/news.json");
		
		var companies = scope.ServiceProvider.GetRequiredService<IGenericRepository<Company>>();
        await SeedData(unitOfWork, companies, "./SeedData/Setup/companies.json");
		
		var hrFiles = scope.ServiceProvider.GetRequiredService<IGenericRepository<HRFile>>();
		await SeedData(unitOfWork, hrFiles, "./SeedData/HR/Setup/hrFiles.json");
        
        #endregion

        #region Phase 2
        var currencyRepo = scope.ServiceProvider.GetRequiredService<IGenericRepository<CurrencySetup>>();
		await SeedData(unitOfWork, currencyRepo, "./SeedData/Setup/currencySetup.json");

		var item = scope.ServiceProvider.GetRequiredService<IGenericRepository<Item>>();
		await SeedData(unitOfWork, item, "./SeedData/Inventory/Setup/item.json");

        var governorates = scope.ServiceProvider.GetRequiredService<IGenericRepository<Governorate>>();
        await SeedData(unitOfWork, governorates, "./SeedData/Setup/governorates.json");

        var empoyees = scope.ServiceProvider.GetRequiredService<IGenericRepository<Employee>>();
        await SeedData(unitOfWork, empoyees, "./SeedData/HR/Setup/employees.json");

        var employeeFiles = scope.ServiceProvider.GetRequiredService<IGenericRepository<EmployeeFiles>>();
        await SeedData(unitOfWork, employeeFiles, "./SeedData/HR/Setup/employeeFiles.json");
		
		//var stores = scope.ServiceProvider.GetRequiredService<IGenericRepository<Store>>();
		//await SeedData(unitOfWork, stores, "./SeedData/Inventory/Setup/stores.json");

        #endregion

        #region Identity
        var usermanager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppUserRole>>();

        await AppIdentityDbContextSeed.SeedAsync(usermanager, roleManager);
        #endregion
    }

    private static async Task SeedData<T>(IUnitOfWork unitOfWork, IGenericRepository<T> genericRepository, string filePath) where T : BaseEntity
	{
		if (await genericRepository.CountAsync() > 0)
			return;

		var userData = await File.ReadAllTextAsync(filePath);

		var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		var data = JsonSerializer.Deserialize<List<T>>(userData, options);

		await genericRepository.InsertRangeAsync(data);
		await unitOfWork.Commit();
	}
}
