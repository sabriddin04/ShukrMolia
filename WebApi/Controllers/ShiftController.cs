using Domain.Filters;
using Infrastructure.Services.ShiftService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftController(IShiftService shiftService):ControllerBase
{
         [HttpGet("get-Shifts")]
        public async Task<IActionResult> GetShiftsAsync([FromQuery]ShiftFilter filter)
        {
            var response= await shiftService.GetShiftsAsync(filter);
             return StatusCode((int)response.StatusCode!, response);
        }

        [HttpPost("add-Start-Shifts/{employeeId:int}")]
         public async  Task<IActionResult> StartShiftAsync(int employeeId)
         {
           var response =  await shiftService.StartShiftAsync(employeeId);
           return StatusCode((int)response.StatusCode!, response);
         }

         [HttpPost("add-EndShifts/{employeeId:int}")]
         public async  Task<IActionResult> EndShiftAsync(int employeeId)
         {
           var response =  await shiftService.EndShiftAsync(employeeId);
           return StatusCode((int)response.StatusCode!, response);
         }

       


}
