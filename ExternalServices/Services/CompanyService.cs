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
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync("https://localhost:7099/api/Company/Companies");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Company>>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Company>> GetRestrictedCompany() 
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync("https://localhost:7099/api/Company/RestrictedCompany");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Company>>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }

        public async Task<List<Company>> GetReleasedCompany() 
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync("https://localhost:7099/api/Company/ReleasedCompany");
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

        public async Task<Company> UpdateNameOptCompany(string cnpj, string nameOpt)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync($"https://localhost:7099/api/Company/{cnpj} StatusNameOpt?nameOpt={nameOpt}");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
        public async Task<Company> UpdateStatusCompany(string cnpj, bool status)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync($"https://localhost:7099/api/Company/{cnpj} Modificar Status da Companhia Aérea?status={status}");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
        public async Task<Company> UpdateAddressCompany(string cnpj)
        {
            try
            {
                HttpResponseMessage response = await CompanyService.companyClient.GetAsync($"https://localhost:7099/api/Company/{cnpj} Modificar Endereço da Companhia Aérea");
                response.EnsureSuccessStatusCode();
                string company = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Company>(company);
            }
            catch (HttpRequestException e)
            {
                throw;
            }
        }
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
