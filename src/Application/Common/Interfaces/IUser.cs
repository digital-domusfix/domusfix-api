namespace DomusFix.Api.Application.Common.Interfaces;


public interface IUser
{
    string? Id { get; }
    string? Email { get; }
    string? UserName { get; }
    string? Role { get; }
}

