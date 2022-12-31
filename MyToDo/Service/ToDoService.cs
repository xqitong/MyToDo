﻿using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Service
{
    public class ToDoService : BaseService<ToDoDto>, IToDoService
    {
        public ToDoService(HttpRestClient client) : base(client, "ToDo")
        {
        }

        public async Task<ApiResponse<PagedList<ToDoDto>>> GetAllAsync(ToDoParameter parameter)
        {
            BaseRequest request = new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = $"api/ToDo/GetAll?PageIndex={parameter.PageIndex}" +
            $"&PageSize={parameter.PageSize}" +
            $"&Search={parameter.Search}"+
            $"&Status={parameter.Status}";
            return await client.ExecuteAsync<PagedList<ToDoDto>>(request);
        }

        public async Task<ApiResponse<SummaryDto>> SummaryAsync()
        {
            BaseRequest request= new BaseRequest();
            request.Method = RestSharp.Method.Get;
            request.Route = "api/ToDo/Summary";
            return await client.ExecuteAsync<SummaryDto>(request);
        }
    }
}
