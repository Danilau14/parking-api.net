namespace ParkingApi.Dto.Pagination;
using System.ComponentModel.DataAnnotations;


public class PaginationDto
{
    [Range(1, int.MaxValue, ErrorMessage = "The page must be greater than 0")]
    public int Page { get; set; } = 1;

    [Range(1, int.MaxValue, ErrorMessage = "The limit must be greater than 0")]
    public int limit { get; set; } = 10;
}
