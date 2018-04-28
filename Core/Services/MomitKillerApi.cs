using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using MomitKiller.Models;
using System.Globalization;

namespace MomitKiller.Services
{
    public class MomitKillerApi
    {
        HttpClient _httpClient;

        public MomitKillerApi(string baseUrl)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _httpClient
                .DefaultRequestHeaders
                .Add("api-key", Settings.ApiKey);
            _httpClient
                .DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Thermostat

        public async Task<ThermostatStatus> ThermostatGetStatusAsync()
        {
            using (HttpResponseMessage res = await _httpClient.GetAsync("api/thermostat/status"))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred getting thermostat status",
                                        new Exception(data));
                }

                return Serialize<ThermostatStatus>(data);
            }
        }

        public async Task<ThermostatStatus> ThermostatSetSetpointAsync(decimal setpoint)
        {
            var stringContent = new StringContent(Math.Round(setpoint, 1).ToString("0.#", CultureInfo.InvariantCulture),
                                                    Encoding.UTF8,
                                                  "application/json");
            
            using (HttpResponseMessage res = await _httpClient
                                                    .PutAsync("api/thermostat/setpoint",
                                                    stringContent))
            using (HttpContent content = res.Content)
            {
                var data = await content.ReadAsStringAsync();

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred setting thermostat setpoint",
                                        new Exception(data));
                }

                return Serialize<ThermostatStatus>(data);
            }
        }

        public async Task<ThermostatStatus> ThermostatSetModeAsync(ThermostatModes mode)
        {
            var stringContent = new StringContent(((int)mode).ToString(),
                                                    Encoding.UTF8,
                                                  "application/json");
            
            using (HttpResponseMessage res = await _httpClient
                                                    .PutAsync("api/thermostat/mode",
                                                              stringContent))
                
            using (HttpContent content = res.Content)
            {
                var data = await content.ReadAsStringAsync();

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred setting thermostat mode",
                                        new Exception(data));
                }

                return Serialize<ThermostatStatus>(data);
            }
        }

        public async Task<decimal> ThermostatGetHysteresisAsync()
        {
            using (HttpResponseMessage res = await _httpClient.GetAsync("api/thermostat/hysteresis"))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                decimal hysteresis;
                if (decimal.TryParse(data, out hysteresis))
                {
                    return hysteresis;
                }

                throw new Exception("An error has ocurred getting hysteresis",
                                    new Exception(data));
            }
        }

        public async Task ThermostatSetHysteresisAsync(decimal hysteresis)
        {
            var stringContent = new StringContent(Math.Round(hysteresis, 1).ToString("0.#", CultureInfo.InvariantCulture),
                                                  Encoding.UTF8,
                                                  "application/json");
            
            using (HttpResponseMessage res = await _httpClient
                                                    .PutAsync("api/thermostat/hysteresis",
                                                              stringContent))
            using (HttpContent content = res.Content)
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    var data = await content.ReadAsStringAsync();
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred setting thermostat hysteresis",
                                        new Exception(data));
                }
            }
        }

        public async Task<decimal> ThermostatGetCalibrationAsync()
        {
            using (HttpResponseMessage res = await _httpClient.GetAsync("api/thermostat/calibration"))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                decimal calibration;
                if (decimal.TryParse(data, out calibration))
                {
                    return calibration;
                }

                throw new Exception("An error has ocurred getting calibration",
                                    new Exception(data));
            }
        }

        public async Task ThermostatSetCalibrationAsync(decimal calibration)
        {
            var stringContent = new StringContent(Math.Round(calibration, 1).ToString("0.#", CultureInfo.InvariantCulture),
                                                  Encoding.UTF8,
                                                  "application/json");
            
            using (HttpResponseMessage res = await _httpClient
                                                    .PutAsync("api/thermostat/calibration",
                                                              stringContent))
            using (HttpContent content = res.Content)
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    var data = await content.ReadAsStringAsync();
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred setting thermostat calibration",
                                        new Exception(data));
                }
            }
        }
        #endregion

        #region Relay

        public async Task RelayPowerOffAsync()
        {
            using (HttpResponseMessage res = await _httpClient.PutAsync("api/relay/off", null))
            using (HttpContent content = res.Content)
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    var data = await content.ReadAsStringAsync();
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred powering off the relay",
                                        new Exception(data));
                }
            }
        }

        public async Task RelayPowerOnAsync()
        {
            using (HttpResponseMessage res = await _httpClient.PutAsync("api/relay/on", null))
            using (HttpContent content = res.Content)
            {
                if (res.StatusCode != HttpStatusCode.OK)
                {
                    var data = await content.ReadAsStringAsync();
                    Debug.WriteLine(data);
                    throw new Exception("An error has ocurred powering on the relay",
                                        new Exception(data));
                }
            }
        }

        public async Task<bool> RelayGetStatusAsync()
        {
            using (HttpResponseMessage res = await _httpClient.GetAsync("api/relay"))
            using (HttpContent content = res.Content)
            {
                string data = await content.ReadAsStringAsync();
                int state;
                if (int.TryParse(data, out state))
                {
                    return state == 1;
                }

                Debug.WriteLine(data);
                throw new Exception("An error has ocurred getting relay status",
                                    new Exception(data));
            }
        }

        #endregion

        private T Serialize<T>(string json) where T : class
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                return ser.ReadObject(ms) as T;
            }
        }

        private async Task IsServiceReachable()
        {
            var timeout = Task.Delay(3000); 
            var request = new HttpClient().GetAsync("http://192.168.1.91:5000");

            await Task.WhenAny(timeout, request);

            if (timeout.IsCompleted)
            {
                throw new Exception("The service is not reachable, please verify connections.");
            }
        }

        /*
        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(data)))
        {
            ThermostatStatus status = new ThermostatStatus();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(status.GetType());  
            status = ser.ReadObject(ms) as ThermostatStatus;  
            return status;  
        }
        */
    }
}
