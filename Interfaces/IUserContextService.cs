﻿namespace ParkingApi.Interfaces;

public interface IUserContextService
{
    int? GetCurrentUserId();
    string? GetCurrentEmail();
}
