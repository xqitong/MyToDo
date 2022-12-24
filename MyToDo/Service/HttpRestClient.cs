using MyToDo.Shared.Contact;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class HttpRestClient
    {
        private readonly string apiUrl;
        protected readonly RestClient client;

        public HttpRestClient(string apiUrl)
        {
            this.apiUrl = apiUrl;
            client = new RestClient();


    }
        public async Task<ApiResponse> ExecuteAsync(BaseRequest baseRequest)
        {

            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;
            request.AddHeader("Content-Type",baseRequest.ContentType);
            if (baseRequest.Parameter != null)
            {
                request.AddJsonBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            }
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);

            else
                return new ApiResponse()
                {
                    Status = false,
                    Result = null,
                    Message = response.ErrorMessage
                };
        }
        public async Task<ApiResponse<T> > ExecuteAsync<T>(BaseRequest baseRequest)
        {

  
            var request = new RestRequest(apiUrl + baseRequest.Route);
            request.Method = baseRequest.Method;
            request.AddHeader("Content-Type", baseRequest.ContentType);
            if (baseRequest.Parameter != null)
            {
                request.AddJsonBody( JsonConvert.SerializeObject(baseRequest.Parameter));
            }
            var response = await client.ExecuteAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);

            else
                return new ApiResponse<T>()
                {
                    Status = false,
                    Message = response.ErrorMessage
                };
        }

    }
}
