using Arch.EntityFrameworkCore.UnitOfWork.Collections;
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
    public interface IToDoService:IBaseService<ToDoDto>
    {
        Task<ApiResponse<PagedList<ToDoDto>>> GetAllAsync(ToDoParameter parameter);
        Task<ApiResponse<SummaryDto>> SummaryAsync();
    }
}
