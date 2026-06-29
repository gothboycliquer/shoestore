using AutoMapper;
using ShoeStore.API.Services.Interfaces;
using ShoeStore.Data.Repositories.Interfaces;
using ShoeStore.Shared.DTOs.Manufacturers;

namespace ShoeStore.API.Services;

public class ManufacturerService : IManufacturerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ManufacturerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ManufacturerDto>> GetAllAsync()
    {
        var manufacturers = await _unitOfWork.Manufacturers.GetAllAsync();
        return _mapper.Map<IEnumerable<ManufacturerDto>>(manufacturers);
    }
}