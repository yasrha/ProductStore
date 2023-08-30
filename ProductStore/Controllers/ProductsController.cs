using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        /**
         * This method maps to a URI that does not contain an "id" segment in the path, since it returns 
         * the entire list of products.
         */
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        /**
         * To get a product by id we must take the parameter "id" from the method call. The id parameter is 
         * mapped to the id segment of the URI path. 
         * 
         * The ASP.NET Web API framwork will automatically convert the ID to the correct data type (in this case int)
         * for the parameter.
         * 
         * If the id is not valid, the method throws an exception of type HttpStatusCode, which will be translated by
         * the framework into a 404 (Not Found) error.
         */
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        /**
         * If the request URI has a query string, the Web API tries to match the query parameters to parameters on
         * the controller method
         */
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }
        
        /**
         * In order to create a new product, the client sents an HTTP POST requst.
         * This POST request will be sent in a serialized representation of a product object in 
         * either XML or JSON format.
         * 
         * When a POST request creates a resource, the server should reply with status 201 (created).
         * And when the server creates a resource, it should include the URI of the new resource in the 
         * location header of the response.
         * 
         * By returning type HttpResponseMessage, we control the details of the HTTP respose message.
         * 
         */
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            String uri = Url.Link("DefaultApi", new { id =  item.Id });
            response.Headers.Location = new Uri(uri);

            return response;
        }

        /**
         * This method takes the product ID, which is taken from the URI path, and the product parameter,
         * which is deserialized from the request body.
         * 
         * Note: by default, the ASP.NET Web API framework takes simple prameter types from the route and 
         * complex types from the request body.
         */
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if(!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /**
         * If the delete request succeeds, it will return status
         *  - 200 (OK) with an entity-body that describes the status
         *  - 202 (Accepted) if the deletion is pending
         *  - 204 (No Content) with no entity body
         *  
         *  For this method, ASP.NET Web API will automatically translate this into status 204.
         */
        public void DeleteProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            repository.Remove(id);
        }

    }
}