using AutoMapper;
using TourGuideBD.Application.Features.Locations.Queries.GetDistrictsByDivision;
using TourGuideBD.Application.Features.Locations.Queries.GetDivisions;
using TourGuideBD.Application.Features.Locations.Queries.GetUpazilasByDistrict;
using TourGuideBD.Application.Features.Places.Queries.Common;
using TourGuideBD.Application.Features.Places.Queries.GetPlaceDetail;
using TourGuideBD.Application.Features.Reviews.Queries.Common;
using TourGuideBD.Domain.Entities.Location;
using TourGuideBD.Domain.Entities.PlaceDetails;
using TourGuideBD.Domain.Entities.Reviews;

namespace TourGuideBD.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ---------- Locations ----------
        CreateMap<Division, DivisionDto>();
        CreateMap<District, DistrictDto>();
        CreateMap<Upazila, UpazilaDto>();

        // ---------- Places ----------
        CreateMap<Place, PlaceListItemDto>()
            .ForMember(d => d.DistrictName, opt => opt.MapFrom(s => s.District.Name))
            .ForMember(d => d.DivisionName, opt => opt.MapFrom(s => s.Division.Name))
            .ForMember(d => d.CoverPhotoUrl, opt => opt.MapFrom(s =>
                s.Photos.Any(p => p.IsCover)
                    ? s.Photos.First(p => p.IsCover).Url
                    : s.Photos.Select(p => p.Url).FirstOrDefault()))
            .ForMember(d => d.Categories, opt => opt.MapFrom(s =>
                s.CategoryMaps.Select(cm => cm.PlaceCategory.Name)))
            .ForMember(d => d.DistanceKm, opt => opt.Ignore());

        CreateMap<Place, PlaceDetailDto>()
            .ForMember(d => d.DivisionName, opt => opt.MapFrom(s => s.Division.Name))
            .ForMember(d => d.DistrictName, opt => opt.MapFrom(s => s.District.Name))
            .ForMember(d => d.UpazilaName, opt => opt.MapFrom(s => s.Upazila != null ? s.Upazila.Name : null))
            .ForMember(d => d.Photos, opt => opt.MapFrom(s => s.Photos))
            .ForMember(d => d.Tags, opt => opt.MapFrom(s => s.Tags.Select(t => t.Tag)))
            .ForMember(d => d.Categories, opt => opt.MapFrom(s => s.CategoryMaps.Select(cm => cm.PlaceCategory.Name)));

        CreateMap<PlacePhoto, PlacePhotoDto>();

        // ---------- Reviews ----------
        CreateMap<Review, ReviewDto>()
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.UserAvatarUrl, opt => opt.MapFrom(s => s.User.AvatarUrl))
            .ForMember(d => d.Photos, opt => opt.MapFrom(s => s.Photos.Select(p => p.Url)));

        CreateMap<Review, PendingReviewDto>()
            .ForMember(d => d.PlaceName, opt => opt.MapFrom(s => s.Place.Name))
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.Photos, opt => opt.MapFrom(s => s.Photos.Select(p => p.Url)));

        CreateMap<ReviewReport, ReportedReviewDto>()
            .ForMember(d => d.ReportId, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.ReportedByUserName, opt => opt.MapFrom(s => s.ReportedByUser.FullName))
            .ForMember(d => d.Rating, opt => opt.MapFrom(s => s.Review.Rating))
            .ForMember(d => d.CommentEn, opt => opt.MapFrom(s => s.Review.CommentEn))
            .ForMember(d => d.CommentBn, opt => opt.MapFrom(s => s.Review.CommentBn))
            .ForMember(d => d.ReviewedUserName, opt => opt.MapFrom(s => s.Review.User.FullName));
    }
}