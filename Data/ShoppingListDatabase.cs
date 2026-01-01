using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using CosteaAdrianaCristinaLab7.Models;

namespace CosteaAdrianaCristinaLab7.Data
{
    public class ShoppingListDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public ShoppingListDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<ShopList>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<ListProduct>().Wait();
        }

        public Task<List<ShopList>> GetShopListsAsync()
        {
            return _database.Table<ShopList>().ToListAsync();
        }

        public Task<ShopList> GetShopListAsync(int id)
        {
            return _database.Table<ShopList>()
                            .Where(i => i.ID == id)
                            .FirstOrDefaultAsync();
        }
        public Task<int> SaveShopListAsync(ShopList slist)
        {
            if (slist.ID != 0)
                return _database.UpdateAsync(slist);
            else
                return _database.InsertAsync(slist);
        }

        public Task<int> DeleteShopListAsync(ShopList slist)
        {
            return _database.DeleteAsync(slist);
        }

        public Task<List<Product>> GetProductsAsync()
        {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<int> SaveProductAsync(Product product)
        {
            if (product.ID != 0)
                return _database.UpdateAsync(product);
            else
                return _database.InsertAsync(product);
        }

        public Task<int> DeleteProductAsync(Product product)
        {
            return _database.DeleteAsync(product);
        }

        public Task<int> SaveListProductAsync(ListProduct listp)
        {
            if (listp.ID != 0)
                return _database.UpdateAsync(listp);
            else
                return _database.InsertAsync(listp);
        }

        public Task<int> DeleteListProductAsync(int shopListId, int productId)
        {
            return _database.ExecuteAsync(
                "DELETE FROM ListProduct WHERE ShopListID = ? AND ProductID = ?",
                shopListId, productId);
        }

        public Task<List<Product>> GetListProductsAsync(int shoplistid)
        {
            return _database.QueryAsync<Product>(
                "SELECT P.ID, P.Description FROM Product P " +
                "INNER JOIN ListProduct LP ON P.ID = LP.ProductID " +
                "WHERE LP.ShopListID = ?",
                shoplistid);
        }
    }
}