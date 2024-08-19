using IronDoneAPI.Enums;
using IronDoneAPI.Middelwares.Attack;
using IronDoneAPI.Models;
using IronDoneAPI.Services;
using IronDoneAPI.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronDoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {
        // api/Attacks
        [HttpPost]
        [Produces("application/json")]
        public IActionResult CreateAttack(Attack attack)
        {
            Guid newAttackId = Guid.NewGuid();
            attack.id = newAttackId;
            attack.Status = AttackStatus.PENDING;
            DbService.AttacksList.Add(attack);
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, attack = attack }
            );
        }

        // api/Attacks/{id}/Start
        [HttpPost("{id}/Start")]
        public async Task<IActionResult> StartAttack(Guid id)
        {
            Attack? attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);

            if (attack == null)
            {
                return StatusCode(404, new { success = false, error = "Attack not found." });
            }
            attack.Status = AttackStatus.IN_PROGRESS;
            attack.StartedAt = DateTime.Now;
            Task attackTask = Task.Run(() =>
            {
                Task.Delay(5000);
            });
            if (attack.Status == AttackStatus.COMPLETED)
            {
                return StatusCode(
                    404,
                    new
                    {
                        success = false,
                        error = "Cannot start an attack that has already been completed."
                    }
                );
            }

            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Attack started.", TaskId = attackTask.Id }
            );
        }

        // api/Attacks/{id}/Status
        [HttpGet("{id}/Status")]
        public IActionResult CheckAttackStatus(Guid id)
        {
            Attack? attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);
            if (attack == null)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new { error = "Attack not found." }
                );
            }
            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    id = attack.id,
                    Status = attack.Status,
                    StartedAt = attack.StartedAt
                }
            );
        }

        [HttpPost("{id}/Intercept")]
        public IActionResult InterceptAttack(Guid id)
        {
            Attack? attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);
            if (attack == null)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new { error = "Attack not found." }
                );
            }
            attack.Status = AttackStatus.COMPLETED;
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Attack intercepted.", Status = "Success" }
            );
        }

        // api/Attacks
        [HttpGet]
        public IActionResult GetAllAttacks()
        {
            return StatusCode(
                StatusCodes.Status200OK,
                new { success = true, attacks = DbService.AttacksList.ToArray() }
            );
        }

        [HttpPut("{id}/missiles")]
        public IActionResult DefineAttackMissiles(Guid id, Attack value)
        {
            Attack? attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);
            if (attack == null)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new { error = "Attack not found." }
                );
            }
            attack.MissileTypes = value.MissileTypes;
            attack.MissileCount = value.MissileCount;
            attack.Status = AttackStatus.MISSILES_DEPLOYED;
            return StatusCode(
                StatusCodes.Status200OK,
                new
                {
                    attackId = attack.id,
                    MisseileCount = attack.MissileCount,
                    MissileTypes = attack.MissileTypes,
                    Status = attack.Status
                }
            );
        }

        // api/Attacks/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAttack(Guid id)
        {
            Attack? attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);
            if (attack == null)
            {
                return StatusCode(
                    StatusCodes.Status404NotFound,
                    new { error = "Attack not found." }
                );
            }
            DbService.AttacksList.Remove(attack);
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Attack deleted successfully." }
            );
        }

        // api/Attacks/Origins
        [HttpGet("Origins")]
        public IActionResult GetAttackOrigins()
        {
            var AttackOrigins = DbService
                .AttacksList.Select(att => new { att.id, att.Origin })
                .ToList();
            return Ok(AttackOrigins);
        }

        // api/Attacks/types
        [HttpGet("types")]
        public IActionResult GetAttackTypes()
        {
            var AttackOrigins = DbService
                .AttacksList.Select(att => new { att.id, att.MissileTypes })
                .ToList();
            return Ok(AttackOrigins);
        }
    }
}
