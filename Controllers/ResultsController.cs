using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using edusync.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace edusync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly EduSyncDbContext _context;
        private readonly EventHubProducer _eventHubProducer;

        public ResultsController(EduSyncDbContext context, EventHubProducer eventHubProducer)
        {
            _context = context;
            _eventHubProducer = eventHubProducer;
        }
        // GET: api/Results/average/{userId}
        [HttpGet("average/{userId}")]
        public async Task<ActionResult<double>> GetAverageScore(Guid userId)
        {
            var results = await _context.Results
                .Where(r => r.UserId == userId)
                .ToListAsync();

            if (!results.Any())
            {
                return NotFound("No results found for the user.");
            }

            double averageScore = results.Average(r => r.Score);
            return Ok(averageScore);
        }

        [Authorize]
        [HttpGet("average")]
        public async Task<ActionResult<double>> GetAverageScore()
        {
            try
            {
                // Get UserId from JWT claims
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("userId") ?? User.FindFirst("sub");
                if (userIdClaim == null)
                {
                    return Unauthorized("UserId not found in token.");
                }

                var userId = Guid.Parse(userIdClaim.Value);

                // Query average score for that user
                var averageScore = await _context.Results
                    .Where(r => r.UserId == userId)
                    .AverageAsync(r => (double?)r.Score) ?? 0.0;

                return Ok(averageScore);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            return await _context.Results.ToListAsync();
        }

        // GET: api/Results/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Results/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(Guid id, Result result)
        {
            if (id != result.ResultId)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<Result>> PostResult(ResultDto dto)
        {
            try
            {
                // 1. Send to Event Hub
                var eventPayload = JsonSerializer.Serialize(dto);
                await _eventHubProducer.SendAsync(eventPayload); // This sends the data

                // 2. Save result to DB (after event is sent)
                var result = new Result
                {
                    ResultId = Guid.NewGuid(),
                    UserId = Guid.Parse(dto.UserId),  // Assuming UserId is in string format but needs to be parsed
                    AssessmentId = Guid.Parse(dto.AssessmentId),  // Use AssessmentId instead of QuizId
                    Score = dto.Score,
                    AttemptDate = dto.SubmittedAt  // Map the AttemptDate correctly
                };

                _context.Results.Add(result);
                await _context.SaveChangesAsync();

                // 3. Return created result
                return CreatedAtAction("GetResult", new { id = result.ResultId }, result);
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., failed Event Hub send or DB save error)
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // DELETE: api/Results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultExists(Guid id)
        {
            return _context.Results.Any(e => e.ResultId == id);
        }

        // DTO classes for result submission
        public class AnswerDto
        {
            public int QuestionId { get; set; }
            public int SelectedOption { get; set; }
        }

        public class ResultDto
        {
            public string UserId { get; set; }      // UserId is a string in the DTO
            public string AssessmentId { get; set; } // AssessmentId is now in the DTO
            public DateTime SubmittedAt { get; set; }
            public int Score { get; set; }
            public List<AnswerDto> Answers { get; set; }
        }
    }
}
