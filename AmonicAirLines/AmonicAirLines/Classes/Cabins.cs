using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AmonicAirLines.Classes
{
    internal class Cabins
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static async Task<List<Cabins>> GetCabinTypes()
        {

            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/CabinTypes";
            string responseBody = "asd";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                List<Cabins> cabins = JsonConvert.DeserializeObject<List<Cabins>>(responseBody);
                Console.WriteLine(responseBody);
                return cabins;
            }
            else
            {
            }
            return new List<Cabins>();
        }
    }
}
