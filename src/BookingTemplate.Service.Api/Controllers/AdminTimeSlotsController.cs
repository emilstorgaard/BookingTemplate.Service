using BookingTemplate.Service.Api.Dtos.Request;
using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingTemplate.Service.Api.Controllers;

[Route("api/admin/timeslots")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AdminTimeSlotsController : ControllerBase
{
    private readonly IAdminTimeSlotService _adminTimeSlotService;

    public AdminTimeSlotsController(IAdminTimeSlotService adminTimeSlotService)
    {
        _adminTimeSlotService = adminTimeSlotService;
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> AddTimeSlot([FromBody] TimeSlotReqDto timeSlotReqDto)
    {
        await _adminTimeSlotService.AddTimeSlot(timeSlotReqDto, GetCurrentUserId());
        return Ok(new { message = "TimeSlot was added successfully" });
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> AddBulk(BulkTimeSlotReqDto bulkTimeSlotReqDto)
    {
        await _adminTimeSlotService.AddBulk(bulkTimeSlotReqDto, GetCurrentUserId());
        return Ok("TimeSlots was added successfully");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _adminTimeSlotService.GetAll();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _adminTimeSlotService.GetTimeSlot(id);
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _adminTimeSlotService.Delete(id);
        return Ok("User was successfully deleted");
    }
}