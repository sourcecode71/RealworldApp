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

        public async Task<ResultModel> CallLogin(EmployeeModel loginUser)
        {
            string userJson = JsonConvert.SerializeObject(loginUser);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{EmployeeEndpoint}/login", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            EmployeeModel returnEmployee = new EmployeeModel();

            if (response.IsSuccessStatusCode)
                returnEmployee = JsonConvert.DeserializeObject<EmployeeModel>(apiResponse);

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = returnEmployee
            };
        }

        public async Task<ResultModel> CallRegisterUser(EmployeeModel employee)
        {
            string employeeJson = JsonConvert.SerializeObject(employee);
            StringContent content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{EmployeeEndpoint}/register", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            EmployeeModel returnEmployee = new EmployeeModel();
            
            if(response.IsSuccessStatusCode)
                returnEmployee = JsonConvert.DeserializeObject<EmployeeModel>(apiResponse);

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = returnEmployee
            };
        }

        public async Task<List<string>> CallGetEmployeesNames()
        {
            HttpResponseMessage response = await client.GetAsync($"{EmployeeEndpoint}/get-all-employees-names");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<string> employees = JsonConvert.DeserializeObject<List<string>>(apiResponse);

            return employees;
        }

        public async Task<ResultModel> CallCreateProject(ProjectModel employee)
        {
            string employeeJson = JsonConvert.SerializeObject(employee);
            StringContent content = new StringContent(employeeJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{EmployeeEndpoint}/register", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            EmployeeModel returnEmployee = new EmployeeModel();

            if (response.IsSuccessStatusCode)
                returnEmployee = JsonConvert.DeserializeObject<EmployeeModel>(apiResponse);

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = returnEmployee
            };
        }
    }
}