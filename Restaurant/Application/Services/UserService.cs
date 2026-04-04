using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Exceptions;
using Restaurant.Application.Interfaces;
using Restaurant.Application.Utils;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using System;

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

        public async Task<UserResponse> AddUser(AddUserRequest request)
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

            return newUser == null ? null : new UserResponse(newUser);
        }

        public async Task<UserResponse?> GetUserByRequest(SearchUserRequest request)
        {
            var user = await FilterUsers(request).FirstOrDefaultAsync();

            if (user == null)
                throw new NotFoundException("User not found");

            return user == null ? null : new UserResponse(user);
        }

        public async Task<List<UserResponse>> GetUsersByRequest(SearchUserRequest request)
        {
            var users = await FilterUsers(request).ToListAsync();
            return users == null ? null : users.Select(u => new UserResponse(u)).ToList();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<UserCookiesResponse> Login(LoginUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                throw new NotFoundException("User not found");

            var validPassword = await PasswordHasher.VerifyPasswordAsync(request.Password, user.PasswordHash);
            if (!validPassword)
                throw new IncorrectDataEnteredException("Invalid password");

            var jwt = _jwtService.GenerateToken(user.Id, user.Login, user.Role.ToString());

            return new UserCookiesResponse(jwt);
        }

        public async Task<UserCookiesResponse> Register(AddUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user != null)
                throw new IncorrectDataEnteredException("User with this email already exists");

            var userResponse = await AddUser(request);

            user = new User
            {
                Id = userResponse.Id,
                Login = userResponse.Login,
                Role = userResponse.Role
            };

            var jwt = _jwtService.GenerateToken(user.Id, user.Login, user.Role.ToString());

            return new UserCookiesResponse(jwt);
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
