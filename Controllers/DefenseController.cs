using IronDoneAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronDomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenseController : ControllerBase
    {
        public static Defense _defence = new Defense();

        [HttpPut("missiles")]
        public IActionResult DefindeMissileDefense(Defense defense)
        {
            if (defense == null)
            {
                return NotFound();
            }
            _defence.MissileCount = defense.MissileCount;
            _defence.Types = defense.Types;
            _defence.Status = "Missile Ready";
            return Ok(
                new
                {
                    missilecount = defense.MissileCount,
                    missiletypes = "interceptor",
                    status = _defence.Status
                }
            );
        }

        // GET: api/Defense/missiles/quantity
        [HttpGet("missiles/quantity")]
        public IActionResult GetMissileQuantity()
        {
            var quantity = new { _defence.MissileCount, _defence.Types };
            return Ok(quantity);
        }
    }
}
