using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MomitKiller.Api.Models;
using MomitKiller.Api.Services;

namespace MomitKiller.Api.Controllers
{
    public class ThermostatController : Controller
    {
        private IThermostatService _thermostatService;
        private IMailService _mailService;

        public ThermostatController(IThermostatService thermostatService, IMailService mailService)
        {
            _thermostatService = thermostatService;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("api/thermostat/status")]
        public async Task<IActionResult> GetStatusAsync()
        {
            try
            {
                var status = await _thermostatService.GetStatusAsync();
                return Ok(status);
            }
            catch(Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController GetStatusAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/thermostat/setpoint")]
        public async Task<IActionResult> SetSetpointAsync([FromBody]decimal setpoint)
        {
            try
            {
                var status = await _thermostatService.SetSetpointAsync(setpoint);
                return Ok(status);
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController SetSetpointAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/thermostat/mode")]
        public async Task<IActionResult> SetModeAsync([FromBody]ThermostatModes mode)
        {
            try
            {
                var status = await _thermostatService.SetModeAsync(mode);
                return Ok(status);
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController SetModeAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        [Route("api/thermostat/hysteresis")]
        public async Task<IActionResult> GetHysteresisAsync()
        {
            try
            {
                var hysteresis = await _thermostatService.GetHysteresisAsync();
                return Ok(hysteresis);
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController GetHysteresisAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/thermostat/hysteresis")]
        public async Task<IActionResult> SetHysteresisAsync([FromBody]decimal hysteresis)
        {
            try
            {
                await _thermostatService.SetHysteresisAsync(hysteresis);
                return Ok();
            }
            catch(Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController SetHysteresisAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpGet]
        [Route("api/thermostat/calibration")]
        public async Task<IActionResult> GetCalibrationAsync()
        {
            try
            {
                var calibration = await _thermostatService.GetCalibrationAsync();
                return Ok(calibration);
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController GetCalibrationAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/thermostat/calibration")]
        public async Task<IActionResult> SetCalibrationAsync([FromBody]decimal calibration)
        {
            try
            {
                await _thermostatService.SetCalibrationAsync(calibration);
                return Ok();
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in ThermostatController SetCalibrationAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
