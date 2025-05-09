﻿namespace ParkingApi.Dto.Error;

public class ErrorResponse
{
    public bool Success { get; set; } = false;
    public required string Message { get; set; }
    public required string ErrorCode { get; set; }
}
