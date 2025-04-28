namespace ParkingApi.Dto.Pagination;

public class PaginationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "The page must be greater than 0")]
    public int Page { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "The limit must be greater than 0")]
    public int Limit { get; set; } = 10;
}
