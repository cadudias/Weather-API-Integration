using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class Weather
    {
        public int id;
        public string main; // field from json response
    }

    [Serializable]
    public class WeatherInfo
    {
        public int id;
        public string name;
        public List<Weather> weather;       
    }
}
