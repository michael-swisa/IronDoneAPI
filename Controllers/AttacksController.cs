using IronDoneAPI.Models;
using IronDoneAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronDoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateAttack([FromBody] Attack Attack)
        {
            Guid newAttackId = Guid.NewGuid();
            Attack.id = newAttackId;
            DBService.Attacks.Add(Attack);
            return Ok(DBService.Attacks.ToArray());
        }
    }
}
