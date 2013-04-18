namespace Domo.Data
{
    public interface IServiceLoader<out T>
    {
        T LoadService();
    }
}