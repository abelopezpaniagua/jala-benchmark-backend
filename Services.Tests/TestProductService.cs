using Services.Implementations;
using Domain.Abstractions;
using System;
using Xunit;
using Moq;
using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Tests
{
    public class TestProductService
    {
        private ProductService _productService;
        private Mock<IProductRepository> _productRepositoryMock;

        public TestProductService()
        {
            _productRepositoryMock = new Mock<IProductRepository>();

            _productService = new ProductService(_productRepositoryMock.Object);
        }

        [Fact]
        public async Task GetProducts_Return_GetPagedProducts()
        {
            var filters = new FilterParams()
            {
                SearchFilter = "",
                PageNumber = 1,
                PageSize = 10
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductsAsync(It.IsAny<FilterParams>()))
                .ReturnsAsync((FilterParams filters) =>
                {
                    return new PagedResult<Product>() {
                        Results = new List<Product>()
                        {
                            new Product() { Id = 1, Code = "PROD01", Name = "Product dummy", Price = 12, InStock = true}
                        },
                        CurrentPage = filters.PageNumber,
                        PageSize = filters.PageSize,
                        PageCount = 1,
                        RowCount = 1
                    };
                });

            var pagedProducts = await _productService.GetProducts(filters);

            Assert.IsType<PagedResult<Product>>(pagedProducts);
            Assert.Single(pagedProducts.Results);
        }

        [Fact]
        public async Task GetProductById_Return_Product()
        {
            var mockId = 1;

            _productRepositoryMock
                .Setup(repo => repo.GetProductByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return new Product()
                    {
                        Id = id,
                        Code = "PROD01",
                        Name = "Product dummy one",
                        Price = 15,
                        InStock = true
                    };
                });

            var product = await _productService.GetProductById(mockId);

            Assert.IsType<Product>(product);
            Assert.Equal(mockId, product.Id);
        }

        [Fact]
        public async Task CreateProduct_Return_Product()
        {
            var mockId = 1;

            var newProduct = new Product()
            {
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) => null);

            _productRepositoryMock
                .Setup(repo => repo.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) =>
                {
                    return new Product()
                    {
                        Id = mockId,
                        Code = product.Code,
                        Name = product.Name,
                        Price = product.Price,
                        DiscountPrice = product.DiscountPrice,
                        InStock = product.InStock
                    };
                });

            var productCreated = await _productService.CreateProduct(newProduct);

            Assert.IsType<Product>(productCreated);
            Assert.Equal(mockId, productCreated.Id);
            Assert.Equal(newProduct.Code, productCreated.Code);
        }

        [Fact]
        public async Task CreateProduct_Throws_ApplicationException_On_DuplicatedProduct()
        {
            var newProduct = new Product()
            {
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) =>
                {
                    return newProduct;
                });


            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _productService.CreateProduct(newProduct);
            });
        }

        [Fact]
        public async Task UpdateProduct_Return_True()
        {
            var mockId = 1;

            var existingProduct = new Product()
            {
                Id = mockId,
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) => null);

            _productRepositoryMock
                .Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => true);

            var productUpdated = await _productService.UpdateProduct(existingProduct);

            Assert.IsType<bool>(productUpdated);
            Assert.True(productUpdated);
        }

        [Fact]
        public async Task UpdateProduct_With_Same_Code_Return_True()
        {
            var mockId = 1;

            var existingProduct = new Product()
            {
                Id = mockId,
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) =>
                {
                    return existingProduct;
                });

            _productRepositoryMock
                .Setup(repo => repo.UpdateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => true);

            var productUpdated = await _productService.UpdateProduct(existingProduct);

            Assert.IsType<bool>(productUpdated);
            Assert.True(productUpdated);
        }

        [Fact]
        public async Task UpdateProduct_Throws_ApplicationException_On_DuplicatedProduct()
        {
            var mockId = 1;

            var existingProduct = new Product()
            {
                Id = mockId,
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.GetProductByCodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string code) =>
                {
                    return new Product()
                    {
                        Id = 2,
                        Code = code,
                        Name = "Product 02",
                        Price = 26,
                        InStock = false
                    };
                });


            await Assert.ThrowsAsync<ApplicationException>(async () =>
            {
                await _productService.UpdateProduct(existingProduct);
            });
        }

        [Fact]
        public async Task DeleteTask_Return_True()
        {
            var mockId = 1;

            var existingProduct = new Product()
            {
                Id = mockId,
                Code = "PROD01",
                Name = "Product 01",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productRepositoryMock
                .Setup(repo => repo.DeleteProductAsync(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => true);

            var result = await _productService.DeleteProduct(existingProduct);

            Assert.IsType<bool>(result);
            Assert.True(result);
        }
    }
}
