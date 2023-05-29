using AutoMapper;
using Common.DTOs;
using Common.Models;
using DataAcceess.IRepository;
using Services.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _genericRepository;
        private readonly IMapper _mapper;

        public ProductService(IGenericRepository<Product> genericRepository, IMapper mapper)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddProduct(ProductDTO dto)
        {
            Product product = _mapper.Map<Product>(dto);

            if (!String.IsNullOrEmpty(dto.ImageUrl))
            {
                string mimeType;
                string fileExtension = "";
                byte[] imageBytes;

                if (dto.ImageUrl.StartsWith("data:image"))
                {
                    var base64Data = dto.ImageUrl.Split(",").Last();
                    mimeType = dto.ImageUrl.Split(",").First().Split(";").First().Split(":").Last();
                    fileExtension = mimeType.Split("/").Last();
                    imageBytes = Convert.FromBase64String(base64Data);
                }
                else
                {
                    imageBytes = Convert.FromBase64String(dto.ImageUrl);
                }

                // Generate a unique file name or use any other logic as needed
                string imageName = $"picproduct_{product.Id}_name_{product.Name}.{fileExtension}";

                string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "Common", "Images");

                // Normalize the path
                imagesDirectory = Path.GetFullPath(imagesDirectory);

                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                string imagePath = Path.Combine(imagesDirectory, imageName);

                try
                {
                    await File.WriteAllBytesAsync(imagePath, imageBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing to file: " + ex.Message);
                }

                product.ImageUrl = imagePath;
            }

            await _genericRepository.Insert(product);
            await _genericRepository.Save();

            return true;
        }

        public async Task<bool> UpdateProduct(ProductDTO dto)
        {
            Product product = await _genericRepository.GetByObject(dto.Id);

            if (product == null)
            {
                throw new KeyNotFoundException("Product does not exists.");
            }

            if (dto.Name != null)
            {
                product.Name = dto.Name;
                await _genericRepository.Update(product);
            }
            if (dto.Description != null)
            {
                product.Description = dto.Description;
                await _genericRepository.Update(product);
            }
            if (dto.Price != product.Price)
            {
                product.Price = dto.Price;
                await _genericRepository.Update(product);
            }
            if (dto.Quantity != product.Quantity)
            {
                product.Quantity = dto.Quantity;
                await _genericRepository.Update(product);
            }
            if (dto.ImageUrl != product.ImageUrl)
            {
                string mimeType;
                string fileExtension = "";
                byte[] imageBytes;

                if (dto.ImageUrl.StartsWith("data:image"))
                {
                    var base64Data = dto.ImageUrl.Split(",").Last();
                    mimeType = dto.ImageUrl.Split(",").First().Split(";").First().Split(":").Last();
                    fileExtension = mimeType.Split("/").Last();
                    imageBytes = Convert.FromBase64String(base64Data);
                }
                else
                {
                    imageBytes = Convert.FromBase64String(dto.ImageUrl);
                }

                // Generate a unique file name or use any other logic as needed
                string imageName = $"picproduct_{product.Id}_name_{product.Name}.{fileExtension}";

                string imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "Common", "Images");

                // Normalize the path
                imagesDirectory = Path.GetFullPath(imagesDirectory);

                if (!Directory.Exists(imagesDirectory))
                {
                    Directory.CreateDirectory(imagesDirectory);
                }

                string imagePath = Path.Combine(imagesDirectory, imageName);

                try
                {
                    await File.WriteAllBytesAsync(imagePath, imageBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error writing to file: " + ex.Message);
                }

                product.ImageUrl = imagePath;
                await _genericRepository.Update(product);
            }
            await _genericRepository.Save();

            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            var products = await _genericRepository.GetAll();
            if (products == null)
            {
                throw new KeyNotFoundException("No products.");
            }

            IEnumerable<ProductDTO> productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            return productsDto;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductSeller(int idSeller)
        {
            var products = await _genericRepository.GetAll();
            if (products == null)
            {
                throw new KeyNotFoundException("No products.");
            }

            IEnumerable<ProductDTO> productsDto = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDTO>>(products);

            // Filter products by SellerId
            productsDto = productsDto.Where(p => p.SellerId == idSeller);

            return productsDto;
        }

        public async Task<ProductDTO> GetProduct(int idProduct)
        {
            Product product = await _genericRepository.GetByObject(idProduct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product does not exist.");
            }

            ProductDTO dto = _mapper.Map<ProductDTO>(product);

            return dto;
        }

        public async Task<bool> DeleteProduct(int idProduct)
        {
            Product product = await _genericRepository.GetByObject(idProduct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product does not exist.");
            }

            if (File.Exists(product.ImageUrl))
            {
                File.Delete(product.ImageUrl);
            }

            await _genericRepository.Delete(product);
            await _genericRepository.Save();

            return true;
        }

        public async Task<string> GetImage(int idProduct)
        {
            Product product = await _genericRepository.GetByObject(idProduct);

            if (product == null)
            {
                throw new KeyNotFoundException("User does not exist.");
            }
            return product.ImageUrl;
        }
    }
}