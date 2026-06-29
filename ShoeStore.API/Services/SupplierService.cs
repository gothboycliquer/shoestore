using AutoMapper;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Data.Repositories.Interfaces;
using ShoeStore.Shared.DTOs.Suppliers;

namespace ShoeStore.API.Services;

public class SupplierService : ISupplierService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _unitOfWork.Suppliers.GetAllAsync();
        return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
    }
}