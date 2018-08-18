using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Collections;

namespace Assets
{
    // https://www.red-gate.com/simple-talk/dotnet/c-programming/calling-restful-apis-unity3d/

    public class WeatherController : MonoBehaviour
    {
        [SerializeField] GameObject snowSystem;

        private const string API_KEY = "c74bbc2e2b9d89c92a12a12107eb4851";
        private const float API_CHECK_MAXTIME = 10 * 60.0f; //10 minutes

        public string cityId;
        private float apiCheckCountdown = API_CHECK_MAXTIME;

        void Start()
        {
            StartCoroutine(GetWeather(CheckSnowStatus));
        }

        void Update()
        {
            apiCheckCountdown -= Time.deltaTime;
            if (apiCheckCountdown <= 0)
            {
                apiCheckCountdown = API_CHECK_MAXTIME;
                StartCoroutine(GetWeather(CheckSnowStatus));
            }
        }

        public void CheckSnowStatus(WeatherInfo weatherObj)
        {
            bool snowing = weatherObj.weather[0].main.Equals("Snow");

            print(weatherObj.weather[0].main);
            if (snowing)
                snowSystem.SetActive(true);
            else
                snowSystem.SetActive(false);
        }

        IEnumerator GetWeather(Action<WeatherInfo> onSuccess)
        {
            using (UnityWebRequest req = UnityWebRequest.Get($"http://api.openweathermap.org/data/2.5/weather?id={cityId}&APPID={API_KEY}"))
            {
                yield return req.SendWebRequest(); // the point where the method will be resumed from the next frame

                while (!req.isDone) // makes sure that the response arrived before continuing with any other 
                    yield return null;

                byte[] result = req.downloadHandler.data;
                string weatherJSON = System.Text.Encoding.Default.GetString(result);
                WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
                onSuccess(info);
            }
        }

        //private async Task<WeatherInfo> GetWeather()
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest
        //        .Create($"http://api.openweathermap.org/data/2.5/weather?id={cityId}&APPID={API_KEY}");

        //    HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());
        //    StreamReader reader = new StreamReader(response.GetResponseStream());
        //    string jsonResponse = reader.ReadToEnd();
        //    WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(jsonResponse);
        //    return info;
        //}
    }
}
