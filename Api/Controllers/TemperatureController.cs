using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MomitKiller.Api.Services;
using MomitKiller.Api.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MomitKiller.Api.Controllers
{
    [Route("api/[controller]")]
    public class TemperatureController : Controller
    {
        private ITemperatureService _temperatureService;
        private IMailService _mailService;

        public TemperatureController(ITemperatureService temperatureService, IMailService mailService)
        {
            _temperatureService = temperatureService;
            _mailService = mailService;
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            try
            {
                await Task.FromResult(false);
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                await _mailService.SendEmailAsync("Error in TemperatureController ListAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
