using System;
using System.Threading.Tasks;

namespace MomitKiller.Api.Services
{
    public class RelayServiceFake:IRelayService
    {
        public RelayServiceFake()
        {
        }

        public async Task<RelayStatuses> GetStatusAsync()
        {
            var random = new System.Random();
            var state = RelayStatuses.On;

            if (random.Next(0, 2) == 0)
                state = RelayStatuses.Off;

            return await Task.FromResult<RelayStatuses>(state);
        }

        public async Task PowerOffAsync()
        {
            await Task.FromResult(false);
        }

        public async Task PowerOnAsync()
        {
            await Task.FromResult(false);
        }
    }
}
