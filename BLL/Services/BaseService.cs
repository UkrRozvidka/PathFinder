using AutoMapper;
using DAL.Entities;
using DAL.Repository.Contracts;

namespace BLL.Services
{
    public abstract class BaseService<T> where T : BaseEntity
    {
        protected readonly IMapper _mapper;
        protected readonly IGenericRepository<T> _repository;

        protected BaseService(IMapper mapper, IGenericRepository<T> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task Delete(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
