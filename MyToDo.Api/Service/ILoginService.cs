using MyToDo.Shared.Contact;
using MyToDo.Shared.Dtos;
using System.Threading.Tasks;

namespace MyToDo.Api.Service
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync (string account, string password);
        Task<ApiResponse> Register(UserDto user);
    }
}
