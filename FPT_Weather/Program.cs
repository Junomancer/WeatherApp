using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FPT_Weather
{
    class Program
    {
        static HttpClient http = new HttpClient();
        static Weather weather;
        static async Task Main(string[] args)
        {
            string selectedOption;
            Console.WriteLine("Enter your Zip Code");
            string zipCode = Console.ReadLine();
            await GetWeather(zipCode);


            Console.WriteLine("What would you like to do Today?");
            Console.WriteLine("1. Should I go Outside?");
            Console.WriteLine("2. Should I wear Sunscreen?");
            Console.WriteLine("3. Can I fly my kite?");

            Console.WriteLine("");
            Console.Write("Please enter your selection: ");

            selectedOption = Console.ReadLine();

            switch (selectedOption) {

                case "1":
                    getRainStatus(weather.current.weather_descriptions[0]);
                    break;
                case "2":
                    getUVStatus(weather.current.uv_index);
                    break;
                case "3":
                    getWindStatus(weather.current.wind_speed, weather.current.weather_descriptions[0]);
                    break;
                default:
                    Console.WriteLine("Out of the Option");
                    break;
            }
            Console.ReadLine();
        }
        public static async Task GetWeather(string qry)
        {
            string resp;
            try
            {
                resp = await http.GetStringAsync("http://api.weatherstack.com/current?access_key=44f276254fe45f8edd17c68e2074e281&query=" + qry + "");
                weather = JsonConvert.DeserializeObject<Weather>(resp);
                Console.WriteLine(weather.request.query);
           
            }
            catch (Exception err)
            {
                Console.WriteLine("Something Went Wrong!");
                Console.ReadLine();
                Environment.Exit(0);
            }

        }

        private static void getRainStatus(string forecast) {
            Console.WriteLine(forecast);
            if (forecast.Contains("Sunny"))
            {
                Console.WriteLine("Yes! It is Sunny");
            }
            else if (forecast.Contains("Rain") || forecast.Contains("Rainy"))
            {
                Console.WriteLine("No! It is " + forecast + "");
            }
            else {
                Console.WriteLine("Up to you! It is " + forecast + "");
            }
            
        }
        private static void getUVStatus(int UV) { 
            if(UV > 3)
            {
                Console.WriteLine("Yes! Wear Sunscreen");
            }
            else
            {
                Console.WriteLine("No need for Sunscreen!");
            }
        }
        private static void getWindStatus(int wind, string forecast) {
            if (wind > 15 && !forecast.Contains("Rain")){
                Console.WriteLine("Yes! You can fly the Kite");
            }
            else
            {
                Console.WriteLine("Not a good time to fly the kite");
            }
        }
    }
}