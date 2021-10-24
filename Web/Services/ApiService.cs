using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Web.Models;

namespace Web.Services
{
    public class ApiService
    {
        private HttpClient client;
        private IConfiguration _configuration;
        private string EmployeeEndpoint;
        private string ProjectEndpoint;

        public ApiService(IConfiguration configuration)
        {
            _configuration = configuration;
            client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetValue<string>("Client:BaseAddress"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            EmployeeEndpoint = _configuration.GetValue<string>("Client:Employee");
            ProjectEndpoint = _configuration.GetValue<string>("Client:Project");
        }

        public async Task<bool> CallRegisterUser(AddEmployeeModel employee)
        {
            string employeeJson = JsonConvert.SerializeObject(employee);
            StringContent content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{EmployeeEndpoint}/Login", content);
            string apiResponse = await response.Content.ReadAsStringAsync();


        }
    }
}