
namespace ParkingApi.Application.Features.ParkingLots.Handlers;

public class CreatedBatchParkingLotHandler : IRequestHandler<CreateBatchParkingLotCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConverterCsvToList _converterCsvToList;

    public CreatedBatchParkingLotHandler(
        IUnitOfWork unitOfWork,
        IConverterCsvToList converterCsvToList
    )
    {
        _unitOfWork = unitOfWork;
        _converterCsvToList = converterCsvToList;
    }

    public async Task<bool> Handle(CreateBatchParkingLotCommand command, CancellationToken cancellationToken)
    {

        var listParkingsLot = await _converterCsvToList.Converter<ParkingLotDynamo>(command.Csv);

        foreach (ParkingLotDynamo parkingLotDynamo in listParkingsLot)
        {
            parkingLotDynamo.FreeSpaces = parkingLotDynamo.Size;
            await _unitOfWork.ParkingLotRepositoryDynamo.CreateParkingLot(parkingLotDynamo);
        }

        return true;
    }
}
