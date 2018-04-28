using System;
using System.Threading.Tasks;
using MomitKiller.Api.Models;
namespace MomitKiller.Api.Services
{
    public interface ITemperatureService
    {
        Task<decimal> GetCurrentAsync();
    }
}
