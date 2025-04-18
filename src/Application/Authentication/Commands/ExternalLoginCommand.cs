using DomusFix.Api.Application.Authentication.Queries;

namespace DomusFix.Api.Application.Authentication.Commands;
// Auth/ExternalLoginCommand.cs
public record ExternalLoginCommand(string Provider, string IdToken) : IRequest<TokenDto>;

