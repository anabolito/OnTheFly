using Newtonsoft.Json;
using PassengerAPI.DTO;

namespace PassengerAPI.AddressService
{
    public class PostOffice
    {
        static readonly HttpClient endereco = new HttpClient();
        public async Task<AddressDTO> GetAddress(string cep)
        {
            try
            {
                HttpResponseMessage response = await endereco.GetAsync("https://viacep.com.br/ws/" + cep + "/json/");
                response.EnsureSuccessStatusCode(); string ender = await response.Content.ReadAsStringAsync();
                var end = JsonConvert.DeserializeObject<AddressDTO>(ender); return end;
            }
            catch (HttpRequestException e)
            {
                throw new(e.Message);
            }
        }
    }
}
