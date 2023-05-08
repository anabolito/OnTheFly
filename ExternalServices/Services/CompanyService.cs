using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CompanyService
    {
        static readonly HttpClient companyClient = new HttpClient();
        HttpResponseMessage responseMessage = new HttpResponseMessage();

        public async Task<List<Company>> Get()
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync("https://localhost:7099/api/Company");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Company>>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Company> GetByCnpj(string cnpj)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync($"https://localhost:7099/api/Company/{cnpj}");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<Company> Insert(Company c)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.PostAsJsonAsync("https://localhost:7099/api/Aircraft/", c);
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        // Atualiza
        public async Task<Company> Update(string cnpj)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync($"https://localhost:7099/api/Company/{cnpj}");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        // Deleta
        public async Task<Company> Delete(string cnpj)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.DeleteAsync($"https://localhost:7099/api/Company?cnpj={cnpj}");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

    }
}
