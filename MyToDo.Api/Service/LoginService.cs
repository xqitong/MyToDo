using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Extensions;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public LoginService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(string account, string password)
        {
            try                                     
            {
                password = password.GetMD5();
                var model = await unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate:
                x=> (x.Account.Equals(account)&&(x.PassWord.Equals(password))) );
                if (model == null)
                {
                    return new ApiResponse { Message = "账号或密码错误，请重试" };
                }
                return new ApiResponse { Status =  true, Result = model };
            }
            catch (System.Exception ex)
            {

                return new ApiResponse { Message = ex.Message };
            }
        }

        public async Task<ApiResponse> Register(UserDto user)
        {
            try
            {
                var model = mapper.Map<User>(user);
                var repository =  unitOfWork.GetRepository<User>();
                var userModel = await repository.GetFirstOrDefaultAsync(predicate:
                    x => x.Account.Equals(model.Account));
                if (userModel!=null)
                {
                    return new ApiResponse{ Message = $"注册账号：{model.Account}已存在,请重新注册" };
                }
                model.CreateTime = System.DateTime.Now;
                model.PassWord = model.PassWord.GetMD5();
                await repository.InsertAsync(model);
                if (unitOfWork.SaveChanges()>0)
                {
                    return new ApiResponse { Status = true, Result = model };
                }
                return new ApiResponse { Message = "注册账户失败" };
            }
            catch (System.Exception ex)
            {

                return new ApiResponse{ Message = ex.Message + "\n注册账户失败" };
            }
        }
    }
}
