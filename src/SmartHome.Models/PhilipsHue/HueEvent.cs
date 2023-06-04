﻿using SmartHome.Models.JsonHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmartHome.Models.PhilipsHue
{
    public class HueEvent
    {

        //[JsonPropertyName("creationtime")]
        //public DateTime Creationtime { get; set; }

        [JsonPropertyName("data")]
        public IEnumerable<HueEventData> Data { get; set; }

        //[JsonPropertyName("id")]
        //public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

    }

    public class HueEventData
    {

        [JsonPropertyName("id")]
        public string Id { get; set; }

        //[JsonPropertyName("id_v1")]
        //public string Id_v1 { get; set; }

        //[JsonPropertyName("owner")]
        //public Owner Owner { get; set; }

        [JsonPropertyName("motion")]
        public HueMotion Motion { get; set; }

        [JsonPropertyName("button")]
        public HueButton Button { get; set; }

        //[JsonPropertyName("on")]
        //public HueOn On { get; set; }

        [JsonPropertyName("on")]
        [JsonConverter(typeof(HueOnJsonHandler))]
        public bool On { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class HueOn
    {

        [JsonPropertyName("on")]
        public bool On { get; set; }
    }

    public class HueButton
    {
        [JsonPropertyName("last_event")]
        public string LastEvent { get; set; }

    }

    public class HueMotion
    {
        [JsonPropertyName("motion_valid")]
        public bool MotionValid { get; set; }

        [JsonPropertyName("motion")]
        public bool Motion { get; set; }
    }

}
