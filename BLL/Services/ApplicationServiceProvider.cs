using Microsoft.Extensions.DependencyInjection;


namespace BLL.Services
{
    public class ApplicationServiceProvider : BLL.Services.Contracts.IServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
