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
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            client = new HttpClient(clientHandler);
            client.BaseAddress = new Uri(_configuration.GetValue<string>("Client:BaseAddress"));
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            EmployeeEndpoint = _configuration.GetValue<string>("Client:Employee");
            ProjectEndpoint = _configuration.GetValue<string>("Client:Project");
        }

        public async Task<ResultModel> CallArchiveProject(ProjectModel project)
        {
            string userJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(userJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/archive", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = response
            };
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

      

        public async Task<bool> CallLogout(EmployeeModel employeeModel)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(employeeModel), Encoding.UTF8, "application/json");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", employeeModel.Token);
            HttpResponseMessage response = await client.PostAsync($"{EmployeeEndpoint}/logout", content);

            string apiResponse = await response.Content.ReadAsStringAsync();

            return apiResponse == "true";
        }

        public async Task<ResultModel> CallRegisterUser(EmployeeModel employee)
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

        public async Task<List<string>> CallGetEmployeesNames()
        {
            HttpResponseMessage response = await client.GetAsync($"{EmployeeEndpoint}/get-all-employees-names");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<string> employees = JsonConvert.DeserializeObject<List<string>>(apiResponse);

            return employees;
        }


        public async Task<List<EmployeeModel>> CallGetEmployees()
        {
            HttpResponseMessage response = await client.GetAsync($"{EmployeeEndpoint}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<EmployeeModel> employees = JsonConvert.DeserializeObject<List<EmployeeModel>>(apiResponse);

            return employees;
        }


        public async Task<ResultModel> CallCreateProject(ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<List<ProjectModel>> CallGetProjects()
        {
            HttpResponseMessage response = await client.GetAsync($"{ProjectEndpoint}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<ProjectModel> projects = JsonConvert.DeserializeObject<List<ProjectModel>>(apiResponse);

            return projects;
        }

        public async Task<ProjectModel> CallGetProject(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{ProjectEndpoint}/byId?selfProjectId={id}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            ProjectModel project = JsonConvert.DeserializeObject<ProjectModel>(apiResponse);

            return project;
        }

        public async Task<ResultModel> CallAssignEmployee(ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/assign", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<ResultModel> CallSaveDelayedComment(ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/savedelayed", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<ResultModel> SaveModifiedComment(ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/savemodified", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<ResultModel> CallAddActivity(ActivityModel activity)
        {
            string projectJson = JsonConvert.SerializeObject(activity);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/activity", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<ResultModel> CallCompleteProject(CompleteProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/complete", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<ResultModel> CallSavePaid(ProjectModel project)
        {
            string projectJson = JsonConvert.SerializeObject(project);
            StringContent content = new StringContent(projectJson, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync($"{ProjectEndpoint}/save-paid", content);
            string apiResponse = await response.Content.ReadAsStringAsync();

            return new ResultModel
            {
                IsSuccess = response.IsSuccessStatusCode,
                ErrorMessage = response.IsSuccessStatusCode ? string.Empty : $"Error: {apiResponse}",
                Result = apiResponse
            };
        }

        public async Task<List<ActivityModel>> CallGetActivities(string currentEmail)
        {
            HttpResponseMessage response = await client.GetAsync($"{EmployeeEndpoint}/activity-by-employee?email={currentEmail}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<ActivityModel> activities = JsonConvert.DeserializeObject<List<ActivityModel>>(apiResponse);

            return activities;
        }


        public async Task<List<ActivityModel>> CallGetActivitiesForProject(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"{EmployeeEndpoint}/activity-by-project?id={id}");

            string apiResponse = await response.Content.ReadAsStringAsync();

            List<ActivityModel> activities = JsonConvert.DeserializeObject<List<ActivityModel>>(apiResponse);

            return activities;
        }

    }
}