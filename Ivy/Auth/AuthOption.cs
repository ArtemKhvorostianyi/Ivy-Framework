﻿using Ivy.Shared;

namespace Ivy.Auth;

public record AuthOption(AuthFlow Flow, string? Name = null, string? Id = null, Icons? Icon = null);

public enum AuthFlow
{
    EmailPassword,
    MagicLink,
    Otp,
    OAuth
}