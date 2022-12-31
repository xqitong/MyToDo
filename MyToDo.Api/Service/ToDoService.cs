using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Migrations;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;

namespace MyToDo.Api.Service
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ToDoService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }                                         
        public async Task<ApiResponse> AddAsync(ToDoDto model)
        {
            try
            {
                var dbModel = mapper.Map<ToDo>(model);
                await unitOfWork.GetRepository<ToDo>().InsertAsync(dbModel);
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse{ Status = true, Result = mapper.Map<ToDoDto>(dbModel) };
                }
                return new ApiResponse{ Message = "添加数据失败" };
            }                                                                       
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }

        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var dbModel= await repository.GetFirstOrDefaultAsync(predicate: t=> t.Id == id);
                repository.Delete(dbModel);
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse{ Status = true };
                }
                return new ApiResponse{ Message = "删除数据失败" };
            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }   
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var dbModels = await repository.GetPagedListAsync(predicate:
                x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateTime)
                ); ;
                return new ApiResponse{ Status = true, Result = dbModels };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }
        }

        public async Task<ApiResponse> GetAllAsync(ToDoParameter parameter)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var dbModels = await repository.GetPagedListAsync(predicate:
                x => (string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Contains(parameter.Search))
                &&(parameter.Status == null?true: x.Status.Equals(parameter.Status)),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateTime)
                ); ;
                return new ApiResponse { Status = true, Result = dbModels };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse { Message = ex.Message };
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<ToDo>();
                var dbModel = await repository.GetFirstOrDefaultAsync(predicate:t => t.Id == id);
                if (dbModel !=null)
                {
                    return new ApiResponse{ Status = true, Result = dbModel };
                }
                return new ApiResponse{ Message = "查找的数据不存在" };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }
        }

        public async Task<ApiResponse> Summary()
        {
            try
            {
                var todos = await unitOfWork.GetRepository<ToDo>().GetAllAsync(
                orderBy: source => source.OrderByDescending(t=> t.CreateTime));

                var memos = await unitOfWork.GetRepository<Memo>().GetAllAsync(
                orderBy: source => source.OrderByDescending(t => t.CreateTime));

                SummaryDto summary = new SummaryDto();
                summary.Sum = todos.Count();
                summary.CompletedCount  = todos.Where(t => t.Status == 1).Count();
                summary.CompletedRatio = (summary.CompletedCount / (double)summary.Sum).ToString("0%");
                summary.MemoCount = memos.Count();
                summary.ToDoList = new ObservableCollection<ToDoDto>(mapper.Map<List<ToDoDto>>(todos.Where(t => t.Status == 0)));
                summary.MemoList = new ObservableCollection<MemoDto>(mapper.Map<List<MemoDto>>(memos));
                return new ApiResponse{ Status = true,Result = summary};
                }
            catch (Exception ex)
            {

                return new ApiResponse { Message = ex.Message };
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto model)
        {
            try
            {
                var updateModel = mapper.Map<ToDo>(model);
                var repository = unitOfWork.GetRepository<ToDo>();
                var dbModel = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id == updateModel.Id);

                dbModel.Title = updateModel.Title;
                dbModel.Content = updateModel.Content;
                dbModel.Status = updateModel.Status;
                dbModel.UpdateTime = DateTime.Now;
                repository.Update(dbModel);
 
                
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse{ Status = true, Result = dbModel };
                }
                return new ApiResponse{ Message = "更新失败" };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }
        }
    }
}
