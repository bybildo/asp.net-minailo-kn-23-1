using Default.Application.DTOs.Requests;
using Default.Application.DTOs.Responses;
using Default.Application.Exceptions;
using Default.Application.Exeptions;
using Default.Infrastructure;
using Default.Models;
using Default.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Default.Application.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;

        public UserService(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<User> AddUser(AddUserRequest request)
        {
            var passwordHash = await Utils.PasswordHasher.HashPasswordAsync(request.Password);
            if (!Enum.TryParse(request.Role, true, out Role role) || !Enum.IsDefined(typeof(Role), role))
            {
                throw new IncorrectDataEnteredException($"Role '{request.Role}' is invalid.");
            }

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Login = request.Login,
                Email = request.Email,
                PasswordHash = passwordHash,
                Role = role
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User?> GetUserByRequest(SearchUserRequest request)
        {
            var user = await FilterUsers(request).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }

        public async Task<List<User>> GetUsersByRequest(SearchUserRequest request)
        {
            return await FilterUsers(request).ToListAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            else throw new NotFoundException("User not found");
        }

        private IQueryable<User> FilterUsers(SearchUserRequest request)
        {
            IQueryable<User> query = _context.Users;

            if (request.Id.HasValue)
                query = query.Where(u => u.Id == request.Id.Value);

            if (!string.IsNullOrWhiteSpace(request.Login))
                query = query.Where(u => u.Login == request.Login);

            if (!string.IsNullOrWhiteSpace(request.Email))
                query = query.Where(u => u.Email == request.Email);

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                if (Enum.TryParse<Role>(request.Role, out var roleEnum))
                {
                    query = query.Where(u => u.Role == roleEnum);
                }
            }

            return query;
        }

        public async Task<AddUserResponse> RegisterOrLogin(AddUserRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login);

            if (user == null)
            {
                var uset = await AddUser(request);
            }
            else
            {
                var validPassword = await Utils.PasswordHasher.VerifyPasswordAsync(user.PasswordHash, request.Password);
                if (!validPassword)
                    throw new IncorrectDataEnteredException("Invalid password");
            }

            var jwt = _jwtService.GenerateToken(user.Id, user.Login, user.Role.ToString());

            return new AddUserResponse(jwt, user.Role.ToString());
        }

        public async Task<bool> IsUserExist(Guid userId, CancellationToken ct)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId, ct);
        }
    }
}