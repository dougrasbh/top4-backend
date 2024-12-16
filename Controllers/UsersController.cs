using BryophytaClassifier.Database;
using BryophytaClassifier.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BryophytaClassifier.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext dbContext) : ControllerBase {
    [HttpGet("{id}")]  // TODO maybe: Add caching, sorting, pagination, etc.
    public async Task<ActionResult<UserResponseDto>> Get(int id) {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();
        return new UserResponseDto(user);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<UserResponseDto>> Login(UserLoginBodyDto loginData) {
        var user = await dbContext.Users.FirstOrDefaultAsync(u =>
            u.Email == loginData.Email &&
            u.Password == loginData.Password
        );

        if (user == null) return NotFound();

        return new UserResponseDto(user);
    }

    [HttpPost("Create")]
    public async Task<ActionResult<UserResponseDto>> Create(UserCreationBodyDto newUser) {
        var existentUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == newUser.Email);
        if (existentUser != null) return Conflict();

        var user = new User(newUser);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        var userResponse = new UserResponseDto(user);
        return CreatedAtAction(nameof(Get), new { id = userResponse.Id }, userResponse);
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update(UserUpdateBodyDto userUpdateBody) {
        User? existingUser = null;
        if (userUpdateBody.Id != default) {
            existingUser = await dbContext.Users.FindAsync(userUpdateBody.Id);
        } else if (!string.IsNullOrWhiteSpace(userUpdateBody.Email)) {
            existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == userUpdateBody.Email);
        }

        if (existingUser == null) return NotFound();

        if (!existingUser.TryUpdateFromDto(userUpdateBody)) return Ok();

        var entry = dbContext.Entry(existingUser);

        entry.State = EntityState.Modified;

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id) {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null) return NotFound();

        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
