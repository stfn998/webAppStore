using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProduct();

        Task<IEnumerable<ProductDTO>> GetAllProductSeller(int idSeller);

        Task<IEnumerable<ProductDTO>> GetProductsInOrder(int idOrder);

        Task<bool> AddProduct(ProductDTO dto);

        Task<bool> UpdateProduct(ProductDTO dto);

        Task<ProductDTO> GetProduct(int idProduct);

        Task<bool> DeleteProduct(int idProduct);

        Task<string> GetImage(int idProduct);
    }
}