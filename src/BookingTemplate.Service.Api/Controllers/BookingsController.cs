using BookingTemplate.Service.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingTemplate.Service.Api.Controllers;

[ApiController]
[Route("api")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    private int GetCurrentUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("timeslots/available")]
    public async Task<IActionResult> GetAvailable()
    {
        var result = await _bookingService.GetAvailable();
        return Ok(result);
    }

    [HttpPost("timeslots/{id:int}/book")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Book(int id)
    {
        await _bookingService.Book(id, GetCurrentUserId());
        return Ok("TimeSlot was successfully booked");
    }

    [HttpGet("bookings")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetBookings()
    {
        var result = await _bookingService.GetBookinsByUserId(GetCurrentUserId());
        return Ok(result);
    }

    [HttpDelete("bookings/{id:int}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        await _bookingService.CancelBooking(id, GetCurrentUserId());
        return Ok("Booking was successfully cancelled");
    }
}