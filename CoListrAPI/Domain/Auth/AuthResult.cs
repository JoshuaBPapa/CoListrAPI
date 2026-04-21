using CoListrAPI.Domain.Entities;

namespace CoListrAPI.Domain.Auth;

public record AuthResult(TokenPair TokenPair, User User);