using System;
using System.Net.Http;
using System.Threading.Tasks;
using MomitKiller.Api.Models;

namespace MomitKiller.Api.Services
{
    public class RelayService : IRelayService
    {
        private const string _baseUrl = "http://192.168.1.93/cm?cmnd=";
        private const string _offCommand = "power off";
        private const string _onCommand = "power on";
        private const string _stateCommand = "power";
        private const string _auth = "&user=admin&password=Sanroker42.";
        private const string _onState = "ON";
        private const string _offState = "OFF";

        public RelayService()
        {
        }

        public async Task PowerOffAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl+_offCommand+_auth))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data == null || data.Substring(data.Length-3) != _offState)
                    {
                        throw new Exception($"Error powering off the relay: {data}");
                    }
                }
            }
        }

        public async Task PowerOnAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl + _onCommand + _auth))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data == null || data.Substring(data.Length - 2) != _onState)
                    {
                        throw new Exception($"Error powering on the relay: {data}");
                    }
                }
            }
        }

        public async Task<RelayStatuses> GetStatusAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.GetAsync(_baseUrl + _stateCommand + _auth))
                using (HttpContent content = res.Content)
                {
                    string data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        try
                        {
                            var state = data.Substring(data.Length - 3).Trim();
                            switch(state)
                            {
                                case "ON":
                                    return RelayStatuses.On;
                                case "OFF":
                                    return RelayStatuses.Off;
                            }
                        }
                        catch(Exception ex)
                        {
                            throw new Exception($"Error reading relay state: {data}", ex);
                        }
                    }

                    throw new Exception($"Error reading relay state: {data}");
                }
            }
        }
    }


}
