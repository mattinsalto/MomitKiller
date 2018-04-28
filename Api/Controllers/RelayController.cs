using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MomitKiller.Api.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MomitKiller.Api.Controllers
{
    
    public class RelayController : Controller
    {
        private IRelayService _relayService;
        private IMailService _mailService;

        public RelayController(IRelayService relayservice, IMailService mailService)
        {
            _relayService = relayservice;
            _mailService = mailService;
        }

        [HttpGet]
        [Route("api/relay")]
        public async Task<IActionResult> GetStatusAsync()
        {
            try
            {
                var state = await _relayService.GetStatusAsync();
                return Ok(state);
            }
            catch(Exception ex)
            {
                await _mailService.SendEmailAsync("Error in RelayController GetStatusAsync", ex.ToString());
                return StatusCode(500, ex.ToString()); 
            }

        }

        [HttpPut]
        [Route("api/relay/off")]
        public async Task<IActionResult> PowerOffAsync()
        {
            try
            {
                await _relayService.PowerOffAsync();
                return Ok();
            }
            catch(Exception ex)
            {
                await _mailService.SendEmailAsync("Error in RelayController PowerOffAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }

        [HttpPut]
        [Route("api/relay/on")]
        public async Task<IActionResult> PowerOnAsync()
        {
            try
            {
                await _relayService.PowerOnAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                await _mailService.SendEmailAsync("Error in RelayController PowerOnAsync", ex.ToString());
                return StatusCode(500, ex.ToString());
            }
        }
    }
}
