using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Interfaces;
using Restaurant.Application.Utils;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;

namespace Restaurant.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public UserService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<User> AddUser(AddUserRequest request)
        {
            var passwordHash = await PasswordHasher.HashPasswordAsync(request.Password);

            if (!Enum.TryParse(request.Role, true, out Role role) || !Enum.IsDefined(typeof(Role), role))
                throw new IncorrectDataEnteredException($"Role '{request.Role}' is invalid.");

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = role
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            return newUser;
        }

        public async Task<User?> GetUserByRequest(SearchUserRequest request)
        {
            var user = await FilterUsers(request).FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("User not found");

            return user;
        }

        public async Task<List<User>> GetUsersByRequest(SearchUserRequest request)
        {
            return await FilterUsers(request).ToListAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<AddUserResponse> RegisterOrLogin(AddUserRequest request)
        {
            var user = await _userRepository.GetByLoginAsync(request.Login);

            if (user == null)
            {
                user = await AddUser(request);
            }
            else
            {
                var validPassword = await PasswordHasher.VerifyPasswordAsync(request.Password, user.PasswordHash);
                if (!validPassword)
                    throw new IncorrectDataEnteredException("Invalid password");
            }

            var jwt = _jwtService.GenerateToken(user.Id, user.Login, user.Role.ToString());

            return new AddUserResponse(jwt, user.Role.ToString());
        }

        public async Task<bool> IsUserExist(Guid userId, CancellationToken ct)
        {
            return await _userRepository.ExistsAsync(userId, ct);
        }

        private IQueryable<User> FilterUsers(SearchUserRequest request)
        {
            IQueryable<User> query = _userRepository.Query();

            if (request.Id.HasValue)
                query = query.Where(u => u.Id == request.Id.Value);

            if (!string.IsNullOrWhiteSpace(request.Login))
                query = query.Where(u => u.Login == request.Login);

            if (!string.IsNullOrWhiteSpace(request.Email))
                query = query.Where(u => u.Email == request.Email);

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                if (Enum.TryParse<Role>(request.Role, out var roleEnum))
                    query = query.Where(u => u.Role == roleEnum);
            }

            return query;
        }
    }
}
