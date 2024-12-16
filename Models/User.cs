using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BryophytaClassifier.Models;

[Index(nameof(Email), IsUnique = true)]
public class User : Entity<int> {
    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "EFCore requirement")]
    private User(string name, string email, string password) {
        Name = name;
        Email = email;
        Password = password;
    }

    public User(UserCreationBodyDto body) {
        Name = body.Name;
        Email = body.Email;
        Password = body.Password;
    }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public bool TryUpdateFromDto(UserUpdateBodyDto userDto) {
        bool changed = false;
        if (!string.IsNullOrWhiteSpace(userDto.Name) && userDto.Name != Name) { Name = userDto.Name; changed = true; }
        if (!string.IsNullOrWhiteSpace(userDto.Email) && userDto.Email != Email) { Email = userDto.Email; changed = true; }
        if (!string.IsNullOrWhiteSpace(userDto.Password) && userDto.Password != Password) { Password = userDto.Password; changed = true; }

        if (changed) LastUpdateTime = DateTimeOffset.UtcNow;

        return changed;
    }

    public override bool IsValid() =>
        !string.IsNullOrWhiteSpace(Name) &&
        !string.IsNullOrWhiteSpace(Email) &&
        !string.IsNullOrWhiteSpace(Password);
}

public record UserCreationBodyDto(string Name, string Email, string Password);

public record UserUpdateBodyDto(int? Id, string? Name, string? Email, string? Password);

public record UserLoginBodyDto(string Email, string Password);

public record UserResponseDto {
    public UserResponseDto(User user) {
        Id = user.Id;
        Name = user.Name;
        Email = user.Email;
        CreationTime = user.CreationTime;
        LastUpdateTime = user.LastUpdateTime;
    }

    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public DateTimeOffset CreationTime { get; }
    public DateTimeOffset LastUpdateTime { get; }
}

public class UserFilter : EntityFilter<int, User> {
    public HashSet<string>? Names { get; set; }

    public HashSet<string>? Email { get; set; }

    protected override bool FilterMatches(User entity) =>
        DefaultContains(Names, entity.Name) &&
        DefaultContains(Email, entity.Email);
}