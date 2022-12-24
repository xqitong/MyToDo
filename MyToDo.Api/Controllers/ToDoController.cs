﻿using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyToDo.Api.Context;
using MyToDo.Api.Service;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Threading.Tasks;

namespace MyToDo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController :ControllerBase
    {
        private readonly IToDoService service;

        public ToDoController(IToDoService toDoService)
        {
            this.service = toDoService;
        }
         [HttpGet]
         public async Task<ApiResponse> Get(int id)  => await service.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] ToDoParameter parameter) => await service.GetAllAsync(parameter);

        [HttpPost]
        public async Task<ApiResponse> Add([FromBody]ToDoDto model) => await service.AddAsync(model);


        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] ToDoDto model) => await service.UpdateAsync(model);


        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) => await service.DeleteAsync(id);

    }
}
