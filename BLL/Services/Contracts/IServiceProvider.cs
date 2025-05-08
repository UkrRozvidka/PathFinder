namespace BLL.Services.Contracts
{
    public interface IServiceProvider
    {
        T GetService<T>() where T : class;
    }
}
