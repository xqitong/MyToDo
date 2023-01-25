using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    internal class LoginService : ILoginService
    {
        private readonly HttpRestClient client;
        private readonly string serviceName = "Login";
        public LoginService(HttpRestClient client)
        {
            this.client = client;
        }

        public async Task<ApiResponse> LoginAsync(UserDto dto)
        {
            BaseRequest request= new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Login";
            request.Parameter = dto;
            return await client.ExecuteAsync(request);
        }

        public async Task<ApiResponse> RegisterAsync(UserDto dto)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Post;
            request.Route = $"api/{serviceName}/Register";
            request.Parameter = dto;
            return await client.ExecuteAsync(request);
        }
    }
}
