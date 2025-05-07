namespace ParkingApi.Application.Common.Dtos;

public class UploadCsvDto
{
    [Required]
    [DataType(DataType.Upload)]
    public IFormFile File { get; set; }
}
