using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightMobileApp.Models;
using FlightSimulatorApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ISimulatorModel mySimulatorModel;

        public CommandController(ISimulatorModel mySimulatorModel)
        {
            this.mySimulatorModel = mySimulatorModel;
        }

        // POST: api/Command
        [HttpPost]
        public ActionResult<string> Post([FromBody] PlaneMoveInfo planeMoveInfo)
        {
            try
            {
                mySimulatorModel.SetAileron(planeMoveInfo.Aileron.ToString());
                mySimulatorModel.SetRudder(planeMoveInfo.Rudder.ToString());
                mySimulatorModel.SetElevator(planeMoveInfo.Elevator.ToString());
                mySimulatorModel.SetThrottle(planeMoveInfo.Throttle.ToString());
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("Post Problem!" + e.Message);
            }
        }
    }
}
