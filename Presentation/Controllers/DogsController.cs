using DogsWebApi.Application.Interfaces;
using DogsWebApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DogsWebApi.Controllers;

[ApiController]
[Route("api")]
public class DogsController : ControllerBase
{
    private readonly IDogService _dogService;

    public DogsController(IDogService dogService)
    {
        _dogService = dogService;
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        var result = await _dogService.PingAsync();
        return Ok(result);
    }

    [HttpGet("dogs")]
    public async Task<IActionResult> GetDogs([FromQuery] string attribute, [FromQuery] string order,
        [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        try
        {
            var dogs = await _dogService.GetDogsAsync(attribute, order, pageNumber, pageSize);
            return Ok(dogs);
        }
        catch (Exception ex)
        {
            return StatusCode(429, ex.Message);
        }
    }

    [HttpPost("dog")]
    public async Task<IActionResult> CreateDog([FromBody] Dog dog)
    {
        try
        {
            var createdDog = await _dogService.CreateDogAsync(dog);
            return CreatedAtAction(nameof(GetDogs), new { id = createdDog.Id }, createdDog);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}