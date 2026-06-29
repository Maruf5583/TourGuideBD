namespace TourGuideBD.Domain.Enums;

public enum BookingStatus
{
    Pending = 0,
    Confirmed = 1,
    Completed = 2,
    CancelledByUser = 3,
    CancelledByGuide = 4,
    Refunded = 5
}