using System.Collections.Generic;
using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IMapService
    {
        Task<List<object>> GetDirectionsAsync(string origin, string destination);
    }
}