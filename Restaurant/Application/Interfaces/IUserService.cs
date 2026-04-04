using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Restaurant.Application.DTOs.Requests;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Domain.Entities;

namespace Restaurant.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> AddUser(AddUserRequest request);
        Task<UserResponse?> GetUserByRequest(SearchUserRequest request);
        Task<List<UserResponse>> GetUsersByRequest(SearchUserRequest request);
        Task DeleteUser(Guid id);
        Task<UserCookiesResponse> Register(AddUserRequest request);
        Task<UserCookiesResponse> Login(LoginUserRequest request);
        Task<bool> IsUserExist(Guid userId, CancellationToken ct);
    }
}
