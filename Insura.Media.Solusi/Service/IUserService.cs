using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Common.Dto;
using Insura.Media.Solusi.Common.Query;

namespace Insura.Media.Solusi.Service
{
    public interface IUserService
    {
        void CreateUser(CreateUserCommand command);
        DataTableDto<UserDto> GetUsersByPage(UserQuery query);
        void UpdateUser(int id, UpdateUserCommand command);
        void DeleteUser(int id);
    }
}
