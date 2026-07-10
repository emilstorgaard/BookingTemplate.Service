using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Dtos.Response;

namespace BookingTemplate.Service.Api.Services.Interfaces;

public interface IUserService
{
    Task<List<UserRespDto>> GetAll();
    Task<UserRespDto> GetUser(int userId);
    Task AddUser(UserReqDto userReqDto);
    Task Delete(int userId);
}
