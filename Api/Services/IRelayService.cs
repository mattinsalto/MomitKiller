using System;
using System.Threading.Tasks;

namespace MomitKiller.Api.Services
{
    public interface IRelayService
    {
        Task<RelayStatuses> GetStatusAsync();
		Task PowerOnAsync();
        Task PowerOffAsync();
    }
}
