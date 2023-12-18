using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace AmonicAirLines
{
    internal class Office
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }

        public static async Task<List<Office>> GetOffices()
        {

            string url = $"{App.PROTOCOL}://localhost:{App.PORT}/Offices";
            string responseBody = "asd";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                responseBody = response.Content.ReadAsStringAsync().Result;
                List<Office> offices = JsonConvert.DeserializeObject<List<Office>>(responseBody);
                return offices;
            }
            else
            {
            }
            return new List<Office>();
        }
    }
}
