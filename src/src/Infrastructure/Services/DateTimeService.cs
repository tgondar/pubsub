using src.Application.Common.Interfaces;

namespace src.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
