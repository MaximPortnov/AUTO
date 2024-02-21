using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static async Task<List<Country>> GetCountry()
        {

            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/Country";
            string responseBody = "asd";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                List<Country> country = JsonConvert.DeserializeObject<List<Country>>(responseBody);
                Console.WriteLine(responseBody);
                return country;
            }
            else
            {
            }
            return new List<Country>();
        }
    }
}
