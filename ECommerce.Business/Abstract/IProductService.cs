using ECommerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.Abstract
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllByCategory(int categoryId);
        List<Product> GetAllByFilterAZ(List<Product> list, bool az = false); 
        List<Product> GetAllByFilterHigherToLower(List<Product> list, bool higher = false);

        Task Add(Product product);
        Task Update(Product product);
        Task Delete(int id);
        Task<Product> GetById(int id);
    }
}
