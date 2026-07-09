using Microsoft.EntityFrameworkCore;
using TourGuideBD.Domain.Entities.Audit;
using TourGuideBD.Domain.Entities.Guide;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Entities.Realtime;
using TourGuideBD.Domain.Entities.Reviews;
using TourGuideBD.Domain.Entities.Trip;
using TourGuideBD.Domain.Entities.Users;

namespace TourGuideBD.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }

    DbSet<Division> Divisions { get; }
    DbSet<District> Districts { get; }
    DbSet<Upazila> Upazilas { get; }
    DbSet<Place> Places { get; }
    DbSet<PlacePhoto> PlacePhotos { get; }
    DbSet<PlaceCategory> PlaceCategories { get; }
    DbSet<PlaceCategoryMap> PlaceCategoryMaps { get; }
    DbSet<PlaceTag> PlaceTags { get; }

    DbSet<Review> Reviews { get; }
    DbSet<ReviewPhoto> ReviewPhotos { get; }
    DbSet<ReviewReport> ReviewReports { get; }

    DbSet<TransportType> TransportTypes { get; }
    DbSet<TransportRate> TransportRates { get; }
    DbSet<Itinerary> Itineraries { get; }
    DbSet<ItineraryStop> ItineraryStops { get; }

    DbSet<FavouritePlace> FavouritePlaces { get; }
    DbSet<VisitHistory> VisitHistories { get; }
    DbSet<CheckIn> CheckIns { get; }
    DbSet<SavedDistrict> SavedDistricts { get; }

    DbSet<LiveVisitor> LiveVisitors { get; }
    DbSet<WeatherAlert> WeatherAlerts { get; }
    DbSet<Broadcast> Broadcasts { get; }

    DbSet<AuditLog> AuditLogs { get; }

    DbSet<BusStand> BusStands { get; }
    DbSet<DistrictBusRoute> DistrictBusRoutes { get; }

    DbSet<GuideApplication> GuideApplications { get; }
    DbSet<GuideProfile> GuideProfiles { get; }
    DbSet<TourPackage> TourPackages { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<GuideAvailability> GuideAvailabilities { get; }
    DbSet<GuideReview> GuideReviews { get; }
    DbSet<GuideLiveLocation> GuideLiveLocations { get; }
    DbSet<GuideRevenue> GuideRevenues { get; }

    DbSet<GuidePaymentMethod> GuidePaymentMethods { get; }
    DbSet<WithdrawalRequest> WithdrawalRequests { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}