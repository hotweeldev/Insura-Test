using AutoMapper;
using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Common.Dto;
using Insura.Media.Solusi.Common.Query;
using Insura.Media.Solusi.Exceptions;
using Insura.Media.Solusi.Models;
using Insura.Media.Solusi.Repository;

namespace Insura.Media.Solusi.Service.Impl
{
    public class UserServiceImpl : IUserService
    {
        private readonly DataContext db;
        private readonly IMapper mapper;

        public UserServiceImpl(DataContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public void CreateUser(CreateUserCommand command)
        {
            var isNikUsed = db.Users.Where(x => x.NIK == command.NIK);
            if (isNikUsed.Any())
            {
                throw new BadRequestException("Nik has been used!!");
            }
            db.Users.Add(mapper.Map<Users>(command));
            db.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            db.Users.Remove(user);
            db.SaveChanges();
        }

        public DataTableDto<UserDto> GetUsersByPage(UserQuery query)
        {
            var users = db.Users.ToList();
            if (!string.IsNullOrEmpty(query.Name))
            {
                users = users.Where(x => x.Name.ToLower().Contains(query.Name.ToLower())).ToList();
            }

            DataTableDto<UserDto> response = new DataTableDto<UserDto>()
            {
                TotalData = users.Count,
                Size = query.Size,
                Page = query.Page,
                TotalPages = (int)Math.Ceiling(users.Count / (double)query.Size),
                Data = mapper.Map<IList<UserDto>>(users).Skip((query.Page - 1) * query.Size).Take(query.Size).ToList()
            };

            return response;
        }

        public void UpdateUser(int id, UpdateUserCommand command)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User not found!");
            }

            if (!string.IsNullOrEmpty(command.Name))
            {
                user.Name = command.Name;
            }

            if (!string.IsNullOrEmpty(command.Address))
            {
                user.Address = command.Address;
            }

            user.Status = command.Status ?? user.Status;
            db.Users.Update(user);
            db.SaveChanges();
        }
    }
}
