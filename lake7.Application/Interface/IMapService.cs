using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace lake7.Application.Interface
{
    public interface IMapService
    {
        Task<string> GetDirectionsAsync(string origin, string destination);
    }
}
