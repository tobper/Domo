using System.Threading.Tasks;

namespace Domo.Extensions
{
    public static class TaskExtensions
    {
         public static Task<T> AsTaskResult<T>(this T result)
         {
             return Task.FromResult(result);
         }
    }
}