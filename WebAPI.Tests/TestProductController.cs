using AutoMapper;
using BenchmarkItemAPI.Controllers;
using BenchmarkItemAPI.Dtos;
using BenchmarkItemAPI.Mappers;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace WebAPI.Tests
{
    public class TestProductController
    {
        private ProductsController _productsController;
        private Mock<IProductService> _productServiceMock;
        private IMapper _mapper;

        public TestProductController()
        {
            _productServiceMock = new Mock<IProductService>();

            var productProfile = new ProductProfile();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(productProfile));

            _mapper = new Mapper(mapperConfig);

            _productsController = new ProductsController(_productServiceMock.Object, _mapper);
        }

        [Fact]
        public async Task GetProducts_ReturnEmpty_With_NotMatch_SearchFilter()
        {
            var filterParams = new FilterParams()
            {
                SearchFilter = "dummy search string",
                PageNumber = 1,
                PageSize = 10
            };

            _productServiceMock
                .Setup(service => service.GetProducts(It.IsAny<FilterParams>()))
                .ReturnsAsync(new PagedResult<Product>()
                {
                    Results = new List<Product>(),
                    CurrentPage = filterParams.PageNumber,
                    PageSize = filterParams.PageSize,
                    RowCount = 0
                });

            var paginatedResult = await _productsController.GetProducts(filterParams);

            Assert.IsType<OkObjectResult>(paginatedResult.Result);
            Assert.IsType<PagedResult<Product>>(((OkObjectResult)paginatedResult.Result).Value);
            Assert.Equal(0, ((PagedResult<Product>)((OkObjectResult)paginatedResult.Result).Value).RowCount);
            Assert.Empty(((PagedResult<Product>)((OkObjectResult)paginatedResult.Result).Value).Results);
        }

        [Fact]
        public async Task GetProducts_WithPageSize_Return_PaginatedProducts()
        {
            var filterParams = new FilterParams()
            {
                SearchFilter = "",
                PageNumber = 1,
                PageSize = 5
            };

            var productsMockResult = GetMockProducts().GetRange(0, filterParams.PageSize);

            _productServiceMock
                .Setup(service => service.GetProducts(It.IsAny<FilterParams>()))
                .ReturnsAsync(new PagedResult<Product>()
                {
                    Results = productsMockResult,
                    PageCount = 1,
                    PageSize = filterParams.PageSize,
                    CurrentPage = filterParams.PageNumber,
                    RowCount = GetMockProducts().Count
                });

            var paginatedResult = await _productsController.GetProducts(filterParams);

            Assert.IsType<OkObjectResult>(paginatedResult.Result);
            Assert.IsType<PagedResult<Product>>(((OkObjectResult)paginatedResult.Result).Value);
            Assert.Equal(filterParams.PageNumber, ((PagedResult<Product>)((OkObjectResult)paginatedResult.Result).Value).CurrentPage);
            Assert.Equal(productsMockResult, ((PagedResult<Product>)((OkObjectResult)paginatedResult.Result).Value).Results);

        }

        [Fact]
        public async Task GetProduct_With_Id_Returns_Product()
        {
            var randomInt = new Random();
            var randomPosition = randomInt.Next(0, GetMockProducts().Count - 1);
            var mockId = GetMockProducts()[randomPosition].Id;

            _productServiceMock
                .Setup(service => service.GetProductById(It.IsAny<int>()))
                .ReturnsAsync(() =>
                {
                    return GetMockProducts()[randomPosition];
                });

            var result = await _productsController.GetProductById(mockId);

            Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsType<Product>(((OkObjectResult)result.Result).Value);
        }

        [Fact]
        public async Task GetProduct_With_Wrong_Id_Returns_NotFound()
        {
            var mockId = 99;

            _productServiceMock
                .Setup(service => service.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return null;
                });

            var result = await _productsController.GetProductById(mockId);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateProduct_Returns_CreatedAtRoute_Result()
        {
            var product = new CreationProduct()
            {
                Code = "PRODDUMMY",
                Name = "Product dummy",
                Price = 10,
                DiscountPrice = 2,
                InStock = true
            };

            _productServiceMock
                .Setup(service => service.CreateProduct(It.IsAny<Product>()))
                .ReturnsAsync((Product product) =>
                {
                    return new Product()
                    {
                        Id = 1,
                        Code = product.Code,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        DiscountPrice = product.DiscountPrice,
                        InStock = product.InStock
                    };
                });

            var result = await _productsController.CreateProduct(product);

            Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.IsType<Product>(((CreatedAtRouteResult)result.Result).Value);
        }

        [Fact]
        public async Task UpdateProduct_Returns_OkObjectResult()
        {
            var product = new UpdateProduct()
            {
                Id = 1,
                Code = "PRODDUMMYUPD",
                Name = "Product dummy updated",
                Price = 12,
                DiscountPrice = 5,
                InStock = true
            };

            _productServiceMock
                .Setup(service => service.UpdateProduct(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => true);

            var result = await _productsController.UpdateProduct(product.Id, product);

            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task DeleteProduct_Returns_NoContentResult()
        {
            var mockProducId = 1;

            _productServiceMock
                .Setup(service => service.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    return new Product()
                    {
                        Id = id,
                        Code = "PROD01",
                        Name = "Product dummy",
                        Price = 10,
                        DiscountPrice = 5,
                        InStock = true
                    };
                });

            _productServiceMock
                .Setup(service => service.DeleteProduct(It.IsAny<Product>()))
                .ReturnsAsync((Product product) => true);

            var result = await _productsController.DeleteProductById(mockProducId);

            Assert.IsType<NoContentResult>(result);
        }

        private List<Product> GetMockProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 1, Code = "PROD01", Name = "Product dummy one", Price = 25, DiscountPrice = 12.5m, InStock = true },
                new Product() { Id = 1, Code = "PROD02", Name = "Product dummy two", Price = 15, InStock = true },
                new Product() { Id = 1, Code = "PROD03", Name = "Product dummy three", Price = 22, InStock = true },
                new Product() { Id = 1, Code = "PROD04", Name = "Product dummy four", Price = 25, DiscountPrice = 12.5m, InStock = true },
                new Product() { Id = 1, Code = "PROD05", Name = "Product dummy five", Price = 12, InStock = true },
                new Product() { Id = 1, Code = "PROD06", Name = "Product dummy six", Price = 18, DiscountPrice = 9.5m, InStock = true },
                new Product() { Id = 1, Code = "PROD07", Name = "Product dummy seven", Price = 8, InStock = true },
                new Product() { Id = 1, Code = "PROD08", Name = "Product dummy eight", Price = 11, InStock = true },
                new Product() { Id = 1, Code = "PROD09", Name = "Product dummy nine", Price = 10, DiscountPrice = 5.5m, InStock = true },
                new Product() { Id = 1, Code = "PROD10", Name = "Product dummy ten", Price = 19, InStock = true }
            };
        }
    }
}
