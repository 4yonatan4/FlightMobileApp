using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FlightMobileWeb.Models;
using FlightSimulatorApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;

namespace FlightMobileWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        private readonly ISimulatorModel mySimulatorModel;
        private BlockingCollection<PlaneMoveInfo> _queue;

        public double Aileron { get; set; }
        public double Rudder { get; set; }
        public double Elevator { get; set; }
        public double Throttle { get; set; }

        public CommandController(ISimulatorModel mySimulatorModel)
        {
            this.mySimulatorModel = mySimulatorModel;
            _queue = new BlockingCollection<PlaneMoveInfo>();
            Task producerThread = Task.Factory.StartNew(() =>
            {
                this.ConsumerThreadStart();
            });
                
        }

        // POST: api/Command
        [HttpPost]
        public ActionResult<string> Post([FromBody] PlaneMoveInfo planeMoveInfo)
        {
            _queue.Add(planeMoveInfo);
            return Ok();
        }


        void ConsumerThreadStart()
        {
            foreach(PlaneMoveInfo info in _queue.GetConsumingEnumerable())
            {
                Consume(info);
            }
        }

        void Consume(PlaneMoveInfo info)
        {
            try
            {
                mySimulatorModel.SetAileron(info.Aileron.ToString());
                mySimulatorModel.SetRudder(info.Rudder.ToString());
                mySimulatorModel.SetElevator(info.Elevator.ToString());
                mySimulatorModel.SetThrottle(info.Throttle.ToString());
            }
            catch (Exception)
            {
                
            }
        }
    }
}
