using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace FlightMobileWeb.Models
{
    /// <summary>
    /// Json object, hold all the parameters of the joystick that tha client send
    /// </summary>
    public class PlaneMoveInfo
    {
        [JsonProperty("aileron")]
        [JsonPropertyName("aileron")]
        public double Aileron { get; set; }

        [JsonProperty("rudder")]
        [JsonPropertyName("rudder")]
        public double Rudder { get; set; }

        [JsonProperty("elevator")]
        [JsonPropertyName("elevator")]
        public double Elevator { get; set; }

        [JsonProperty("throttle")]
        [JsonPropertyName("throttle")]
        public double Throttle { get; set; }

    }
}
