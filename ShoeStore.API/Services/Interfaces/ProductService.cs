using AutoMapper;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Data.Repositories.Interfaces;
using ShoeStore.Shared.DTOs.Products;

namespace ShoeStore.API.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = _mapper.Map<ShoeStore.Data.Product>(dto);
        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var created = await _unitOfWork.Products.GetByIdWithDetailsAsync(product.Id);
        return _mapper.Map<ProductDto>(created!);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return null;

        _mapper.Map(dto, product);
        await _unitOfWork.Products.UpdateAsync(product);
        await _unitOfWork.SaveChangesAsync();

        var updated = await _unitOfWork.Products.GetByIdWithDetailsAsync(id);
        return _mapper.Map<ProductDto>(updated!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
            return false;

        bool isInOrders = await _unitOfWork.Products.IsProductInOrdersAsync(id);
        if (isInOrders)
            throw new InvalidOperationException("Нельзя удалить товар, который есть в заказах.");

        await _unitOfWork.Products.DeleteAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}