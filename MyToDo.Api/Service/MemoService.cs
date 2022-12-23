﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Parameters;
using System.Linq;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public class MemoService : IMemoService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public MemoService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }                                         
        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var todo = mapper.Map<Memo>(model);
                await unitOfWork.GetRepository<Memo>().InsertAsync(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse{ Status = true, Result = model };
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
                var repository = unitOfWork.GetRepository<Memo>();
                var todo= await repository.GetFirstOrDefaultAsync(predicate: t=> t.Id == id);
                repository.Delete(todo);
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse { Status = true };
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
                var repository = unitOfWork.GetRepository<Memo>();
                var todos = await repository.GetPagedListAsync(predicate:
                x => string.IsNullOrWhiteSpace(parameter.Search) ? true : x.Title.Equals(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: source => source.OrderByDescending(t => t.CreateTime)
                ); ;
                return new ApiResponse{ Status = true, Result = todos };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = unitOfWork.GetRepository<Memo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate:t => t.Id == id);
                if (todo !=null)
                {
                    return new ApiResponse{ Status = true, Result = todo };
                }
                return new ApiResponse{ Message = "查找的数据不存在" };

            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message };
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var dbTodo = mapper.Map<Memo>(model);
                var repository = unitOfWork.GetRepository<Memo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: t => t.Id == dbTodo.Id);

                repository.Update(dbTodo);
 
                
                if (await unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse{ Status = true, Result = todo };
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