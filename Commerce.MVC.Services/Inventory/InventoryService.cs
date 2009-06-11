using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commerce.Data;

namespace Commerce.Services {
    public class InventoryService:IInventoryService {

        IInventoryRepository _inventoryRepository;
        ICatalogService _catalogService;

        public InventoryService(IInventoryRepository inventoryRepository, ICatalogService catalogService) {
            _inventoryRepository = inventoryRepository;
            _catalogService = catalogService;
        }


        public void IncrementInventory(int productID, int amount, string note) {
            _inventoryRepository.Increment(productID, amount, note);
        }

        public IList<Commerce.Data.InventoryRecord> GetInventory(int productID) {
            return _inventoryRepository.GetInventory().ForProduct(productID).ToList();
        }

        public int GetAmountOnHand(int productID) {
            return _inventoryRepository.GetInventory().ForProduct(productID).Sum(x => x.Increment);
        }

        public void SetProductInventoryStatus(Product product) {

            //pull the amount on hand
            int amountOnHand = GetAmountOnHand(product.ID);
            InventoryStatus status;

            //this is where the inventorying logic lives; how you set it depends on 
            //your business requirements.

            if (amountOnHand > 0) {
                status = InventoryStatus.InStock;
                product.EstimatedDelivery = "2-3 Days";
            } else {

                if (product.AllowBackOrder) {
                    status = InventoryStatus.BackOrder;
                    product.EstimatedDelivery = "2-3 Weeks";
                } else {
                    status = InventoryStatus.CurrentlyUnavailable;
                    product.EstimatedDelivery = " -- ";
                }
            }

            //save the status
            SetProductInventoryStatus(product, status);

        }

        public void SetProductInventoryStatus(Product product, InventoryStatus status) {

            product.Inventory = status;
            _catalogService.SaveProduct(product);
            
            
        }
    }
}
