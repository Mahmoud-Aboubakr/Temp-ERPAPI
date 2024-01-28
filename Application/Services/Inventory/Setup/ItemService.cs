using Application.Contracts.Handlers;
using Application.Contracts.IServices;
using Application.Contracts.Models;
using Application.Contracts.Persistence;
using Application.Contracts.Specifications;
using Application.Dtos.Inventory.Setup.Item;
using Application.Handlers;
using Application.Specifications;
using Application.Specifications.Inventory.Setup;
using AutoMapper;
using Domain.Entities.Inventory.Setup;
using Domain.Entities.Setup;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Inventory.Setup
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork uof;
        private readonly IMapper mapper;
        private readonly IResponseModelHandler responseModelHandler;
        private readonly IPaginatedModelHandler paginatedModelHandler;

        public ItemService(IUnitOfWork unitOfWork,
            IMapper _mapper,
            IResponseModelHandler _responseModelHandler,
            IPaginatedModelHandler _paginatedModelHandler)
        {
            uof = unitOfWork;
            mapper = _mapper;
            responseModelHandler = _responseModelHandler;
            paginatedModelHandler = _paginatedModelHandler;
        }
        public async Task<IPaginatedModelHandler> GetItemsAsync(int pageSize, int pageNumber, string term, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Item> ItemRepository = uof.GetRepository<Item>();
            using (ISpecification<Item> specifications = new ItemSpec(pageSize, pageNumber, term))
            {
                specifications.IncludeStrings.AddRange(new List<string>() { "ItemCategory", "ItemType", "ItemClassification", "CostPriceCurrency", "RetailCurrency", "WholesaleCurrency" });
                IEnumerable<ReadItemDto> Items = mapper.Map<IEnumerable<ReadItemDto>>(await ItemRepository.GetAllAsync(specifications));
                var ItemSearch = new BaseSpecification<Item>(a => a.ServiceName.Contains(term) || a.EnglishName.Contains(term) || a.ArabicName.Contains(term));
                IPaginatedModelHandler responseModel = paginatedModelHandler.Create(Items, pageNumber, pageSize, await ItemRepository.CountAsync(ItemSearch, cts.Token)).WithResponseModel("DONE", StatusCodes.Status200OK, Lang);

                return responseModel;
            }
        }
        public async Task<IResponseModel> GetItemAsync(int ItemId, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Item> ItemRepository = uof.GetRepository<Item>();
            using (ISpecification<Item> specifications = new ItemSpec(ItemId))
            {
                Item Item = await ItemRepository.GetByIdAsync(ItemId, cts.Token);

                if (Item is null || Item.IsDeleted)
                    return responseModelHandler.GetResponseModel(null, "NOT_FOUND", 404, Lang);

                return responseModelHandler.GetResponseModel(mapper.Map<ReadItemDto>(Item), "DONE", StatusCodes.Status200OK, Lang);
            }
        }
        public async Task<IResponseModel> AddItemAsync(CreateItemDto Item, string Lang, CancellationTokenSource cts)
        {

            IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();
            var findItemCategory = await ItemCategoryRepository.GetByIdAsync(Item.ItemCategoryId);
            if (findItemCategory == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CATEGORY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
            var findItemType = await ItemTypeRepository.GetByIdAsync(Item.ItemTypeId);
            if (findItemType == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_TYPE_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();
            var findItemClassification = await ItemClassificationRepository.GetByIdAsync(Item.ItemClassificationId);
            if (findItemClassification == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CLASSIFICATION_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
            var findCostPriceCurrency = await CurrencyRepository.GetByIdAsync(Item.CostPriceCurrencyId);
            if (findCostPriceCurrency == null)
                return responseModelHandler.GetResponseModel(Item, "COST_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //validate that retail exist but currency not exist or the opposit
            if (Item.RetailCurrencyId > 0 && Item.Retail is null)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_RETAIL_PRICE", StatusCodes.Status404NotFound, Lang);

            if (Item.RetailCurrencyId is null && Item.Retail > 0)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_RETAIL_PRICE_Currency", StatusCodes.Status404NotFound, Lang);

            var findRetailCurrency = await CurrencyRepository.GetByIdAsync(Item.RetailCurrencyId ?? 0);
            if (findRetailCurrency == null && Item.RetailCurrencyId > 0)
                return responseModelHandler.GetResponseModel(Item, "RETAIL_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //validate that wholesale exist but currency not exist or the opposit
            if (Item.WholesaleCurrencyId > 0 && Item.Wholesale is null)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_WholeSale_PRICE", StatusCodes.Status404NotFound, Lang);

            if (Item.WholesaleCurrencyId is null && Item.Wholesale > 0)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_WholeSale_PRICE_Currency", StatusCodes.Status404NotFound, Lang);

            var findWholesaleCurrency = await CurrencyRepository.GetByIdAsync(Item.WholesaleCurrencyId ?? 0);
            if (findWholesaleCurrency == null && Item.WholesaleCurrencyId > 0)
                return responseModelHandler.GetResponseModel(Item, "WHOLESALE_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Item> ItemRepository = uof.GetRepository<Item>();

            var ItemSpe = new BaseSpecification<Item>(a => a.ItemCode == Item.ItemCode);
            var checkItemCode = await ItemRepository.GetAsync(ItemSpe);
            if (checkItemCode != null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CODE_EXSIT", StatusCodes.Status409Conflict, Lang);


            Item newItem = mapper.Map<Item>(Item);

            newItem.IsActive = true;
            newItem.CreateDate = DateTime.Now;

            await ItemRepository.InsertAsync(newItem, cts.Token);
            await uof.Commit();
            return responseModelHandler.GetResponseModel(mapper, "SAVE_SUCCESS", StatusCodes.Status201Created, Lang);

        }

        public async Task<IResponseModel> UpdateItemAsync(int id, UpdateItemDto Item, string Lang)
        {
            if (id != Item.Id)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ItemCategory> ItemCategoryRepository = uof.GetRepository<ItemCategory>();
            var findItemCategory = await ItemCategoryRepository.GetByIdAsync(Item.ItemCategoryId);
            if (findItemCategory == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CATEGORY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ItemType> ItemTypeRepository = uof.GetRepository<ItemType>();
            var findItemType = await ItemTypeRepository.GetByIdAsync(Item.ItemTypeId);
            if (findItemType == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_TYPE_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<ItemClassification> ItemClassificationRepository = uof.GetRepository<ItemClassification>();
            var findItemClassification = await ItemClassificationRepository.GetByIdAsync(Item.ItemClassificationId);
            if (findItemClassification == null)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CLASSIFICATION_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<CurrencySetup> CurrencyRepository = uof.GetRepository<CurrencySetup>();
            var findCostPriceCurrency = await CurrencyRepository.GetByIdAsync(Item.CostPriceCurrencyId);
            if (findCostPriceCurrency == null)
                return responseModelHandler.GetResponseModel(Item, "COST_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //validate that retail exist but currency not exist or the opposit
            if (Item.RetailCurrencyId > 0 && Item.Retail is null)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_RETAIL_PRICE", StatusCodes.Status404NotFound, Lang);

            if (Item.RetailCurrencyId is null && Item.Retail > 0)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_RETAIL_PRICE_Currency", StatusCodes.Status404NotFound, Lang);

            var findRetailCurrency = await CurrencyRepository.GetByIdAsync(Item.RetailCurrencyId ?? 0);
            if (findRetailCurrency == null && Item.RetailCurrencyId > 0)
                return responseModelHandler.GetResponseModel(Item, "RETAIL_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            //validate that wholesale exist but currency not exist or the opposit
            if (Item.WholesaleCurrencyId > 0 && Item.Wholesale is null)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_WholeSale_PRICE", StatusCodes.Status404NotFound, Lang);

            if (Item.WholesaleCurrencyId is null && Item.Wholesale > 0)
                return responseModelHandler.GetResponseModel(Item, "YOU_MUST_ADD_WholeSale_PRICE_Currency", StatusCodes.Status404NotFound, Lang);

            var findWholesaleCurrency = await CurrencyRepository.GetByIdAsync(Item.WholesaleCurrencyId ?? 0);
            if (findWholesaleCurrency == null && Item.WholesaleCurrencyId > 0)
                return responseModelHandler.GetResponseModel(Item, "WHOLESALE_PRICE_CURRENCY_NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            IGenericRepository<Item> ItemRepository = uof.GetRepository<Item>();
            var ItemSpe = new BaseSpecification<Item>(a => a.ItemCode == Item.ItemCode);
            var checkItemCode = await ItemRepository.GetAsync(ItemSpe);
            if (checkItemCode != null && checkItemCode.Id != id)
                return responseModelHandler.GetResponseModel(Item, "ITEM_CODE_EXSIT", StatusCodes.Status409Conflict, Lang);

            if (checkItemCode != null)
                ItemRepository.Detach(checkItemCode);

            Item updateItem = mapper.Map<UpdateItemDto, Item>(Item);
            updateItem.EditDate = DateTime.Now;


            ItemRepository.Update(updateItem);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "EDIT_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }

        public async Task<IResponseModel> DeleteItemAsync(int id, string Lang, CancellationTokenSource cts)
        {
            IGenericRepository<Item> ItemRepository = uof.GetRepository<Item>();

            Item currentItem = await ItemRepository.GetByIdAsync(id, cts.Token);

            if (currentItem is null || currentItem.IsDeleted)
                return responseModelHandler.GetResponseModel(null, "NOT_FOUND", StatusCodes.Status404NotFound, Lang);

            currentItem.DeleteDate = DateTime.Now;
            ItemRepository.SoftDelete(currentItem);
            await uof.Commit();

            return responseModelHandler.GetResponseModel(null, "DELETE_SUCCESS", StatusCodes.Status204NoContent, Lang);

        }
    }
}
