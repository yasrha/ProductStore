using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductStore.Models
{
    /**
     * A repository to store the list in local memory.
     * 
     * Note: for this application it's ok to store in local memory, but for a real application you should
     * store it externally in a database or some sort of cloud storage.
     */
    public class ProductRepository : IProductRepository
    {
        private List<Product> products = new List<Product>();
        private int _nextId = 1;

        public ProductRepository() 
        {
            Add(new Product { Name = "Tomato soup", Category = "Groceries", Price = 1.39M });
            Add(new Product { Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            Add(new Product { Name = "Hammer", Category = "Hardware", Price = 16.99M });
        }

        public IEnumerable<Product> GetAll() // Return the list of all the products
        {
            return products;
        }

        public Product Add(Product item) // Add a new product to the list
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            item.Id = _nextId++;
            products.Add(item);
            return item;
        }

        public Product Get(int id) // Get a product by id
        {
            return products.Find(p => p.Id == id);
        }

        public void Remove(int id) // Remove a product by id
        {
            products.RemoveAll(p => p.Id == id);
        }

        public bool Update(Product item) // Update an item
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            int index = products.FindIndex(p =>  p.Id == item.Id);
            if (index == -1)
            {
                return false;
            }
            products.RemoveAt(index);
            products.Add(item);
            return true;

        }
    }
}