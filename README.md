# TourGuideBD API Documentation

## 📖 Overview

TourGuideBD API is a comprehensive RESTful API for exploring tourist destinations across Bangladesh. It provides endpoints for discovering places, managing user profiles, handling reviews, trip planning, and administrative functions.

**Base URL:** `https://your-api-domain.com` *(Replace with your actual base URL)*

**API Version:** v1

---

## 🔐 Authentication

This API uses **Bearer Token** authentication. Include the token in the `Authorization` header:

---

## 📚 Table of Contents

- [Admin Endpoints](#admin-endpoints)
- [Authentication Endpoints](#authentication-endpoints)
- [Location Endpoints](#location-endpoints)
- [Places Endpoints](#places-endpoints)
- [Realtime Endpoints](#realtime-endpoints)
- [Reviews Endpoints](#reviews-endpoints)
- [Trip Planner Endpoints](#trip-planner-endpoints)
- [Users Endpoints](#users-endpoints)
- [Data Models](#data-models)

---

## 👑 Admin Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/admin/users` | Get paginated list of users with optional search |
| PATCH | `/api/v1/admin/users/{userId}/ban` | Ban or unban a user |
| PATCH | `/api/v1/admin/users/{userId}/role` | Assign or remove a role from a user |
| GET | `/api/v1/admin/places/pending` | Get pending places awaiting approval |
| GET | `/api/v1/admin/analytics` | Get analytics dashboard data |
| POST | `/api/v1/admin/broadcast` | Send a broadcast message to users |
| POST | `/api/v1/admin/cache/flush` | Flush cache (optionally by district) |
| GET | `/api/v1/admin/audit-logs` | Get audit logs with filtering |

### Admin Query Parameters

| Endpoint | Parameter | Type | Description |
|----------|-----------|------|-------------|
| `/admin/users` | `search` | string | Search term for users |
| `/admin/users` | `pageNumber` | integer | Page number (default: 1) |
| `/admin/users` | `pageSize` | integer | Items per page (default: 20) |
| `/admin/places/pending` | `pageNumber` | integer | Page number (default: 1) |
| `/admin/places/pending` | `pageSize` | integer | Items per page (default: 10) |
| `/admin/audit-logs` | `entityName` | string | Filter by entity name |
| `/admin/audit-logs` | `pageNumber` | integer | Page number (default: 1) |
| `/admin/audit-logs` | `pageSize` | integer | Items per page (default: 20) |

---

## 🔑 Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/auth/register` | Register a new user |
| POST | `/api/v1/auth/login` | Login and get JWT token |
| POST | `/api/v1/auth/refresh-token` | Refresh authentication token |
| POST | `/api/v1/auth/logout` | Logout user |

---

## 📍 Location Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/locations/divisions` | Get all divisions |
| GET | `/api/v1/locations/divisions/{divisionId}/districts` | Get districts by division ID |
| GET | `/api/v1/locations/districts/{districtId}/upazilas` | Get upazilas by district ID |

---

## 🏛️ Places Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/places/by-district/{districtId}` | Get places by district (paginated) |
| GET | `/api/v1/places/by-category/{category}` | Get places by category |
| GET | `/api/v1/places/{id}` | Get place details by ID |
| PUT | `/api/v1/places/{id}` | Update place information |
| DELETE | `/api/v1/places/{id}` | Delete a place |
| GET | `/api/v1/places/nearby` | Find places near a location |
| GET | `/api/v1/places/search` | Search for places |
| POST | `/api/v1/places` | Create a new place |
| POST | `/api/v1/places/upload-photos` | Upload photos for a place |
| PATCH | `/api/v1/places/{id}/approval` | Approve or reject a place |

### Places Query Parameters

| Endpoint | Parameter | Type | Description |
|----------|-----------|------|-------------|
| `/by-district/{districtId}` | `pageNumber` | integer | Page number (default: 1) |
| `/by-district/{districtId}` | `pageSize` | integer | Items per page (default: 10) |
| `/by-category/{category}` | `districtId` | integer | Filter by district |
| `/by-category/{category}` | `pageNumber` | integer | Page number (default: 1) |
| `/by-category/{category}` | `pageSize` | integer | Items per page (default: 10) |
| `/nearby` | `lat` | double | Latitude |
| `/nearby` | `lng` | double | Longitude |
| `/nearby` | `radiusKm` | integer | Search radius in km (default: 5) |
| `/search` | `q` | string | Search query |
| `/search` | `pageNumber` | integer | Page number (default: 1) |
| `/search` | `pageSize` | integer | Items per page (default: 10) |

### Place Categories

| Value | Category |
|-------|----------|
| 1 | Natural |
| 2 | Historical |
| 3 | Religious |
| 4 | Cultural |
| 5 | Adventure |
| 6 | Others |

---

## 📡 Realtime Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/realtime/crowd-level/{placeId}` | Get crowd level for a place |
| PATCH | `/api/v1/realtime/live-visitor/{placeId}` | Update live visitor count |
| POST | `/api/v1/realtime/weather-alert` | Send a weather alert |

---

## ⭐ Reviews Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/reviews/place/{placeId}` | Get reviews for a place |
| POST | `/api/v1/reviews/photos` | Upload review photos |
| POST | `/api/v1/reviews` | Create a review |
| POST | `/api/v1/reviews/{reviewId}/report` | Report a review |
| GET | `/api/v1/reviews/pending` | Get pending reviews |
| PATCH | `/api/v1/reviews/{reviewId}/approval` | Approve or reject a review |
| GET | `/api/v1/reviews/reports` | Get reported reviews |
| PATCH | `/api/v1/reviews/reports/{reportId}/resolve` | Resolve a review report |

### Reviews Query Parameters

| Endpoint | Parameter | Type | Description |
|----------|-----------|------|-------------|
| `/place/{placeId}` | `pageNumber` | integer | Page number (default: 1) |
| `/place/{placeId}` | `pageSize` | integer | Items per page (default: 10) |
| `/pending` | `pageNumber` | integer | Page number (default: 1) |
| `/pending` | `pageSize` | integer | Items per page (default: 10) |
| `/reports` | `pageNumber` | integer | Page number (default: 1) |
| `/reports` | `pageSize` | integer | Items per page (default: 10) |

---

## 🗺️ Trip Planner Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/trip-planner/route` | Get route between two locations |
| GET | `/api/v1/trip-planner/transport-cost` | Get transport cost options |
| GET | `/api/v1/trip-planner/travel-time` | Get estimated travel time |
| GET | `/api/v1/trip-planner/trip-budget` | Calculate trip budget |
| POST | `/api/v1/trip-planner/itinerary` | Create a trip itinerary |
| GET | `/api/v1/trip-planner/itinerary/{id}` | Get itinerary by ID |
| GET | `/api/v1/trip-planner/smart-calculate` | Smart trip calculation |

### Trip Planner Parameters

| Endpoint | Parameter | Type | Description |
|----------|-----------|------|-------------|
| `/route` | `OriginLat` | double | Origin latitude |
| `/route` | `OriginLng` | double | Origin longitude |
| `/route` | `DestLat` | double | Destination latitude |
| `/route` | `DestLng` | double | Destination longitude |
| `/route` | `Profile` | string | Transport profile |
| `/transport-cost` | `DistanceKm` | double | Distance in kilometers |
| `/travel-time` | `TransportTypeId` | integer | Transport type ID |
| `/travel-time` | `DistanceKm` | double | Distance in kilometers |
| `/travel-time` | `OriginLat` | double | Origin latitude |
| `/travel-time` | `OriginLng` | double | Origin longitude |
| `/travel-time` | `DestLat` | double | Destination latitude |
| `/travel-time` | `DestLng` | double | Destination longitude |
| `/trip-budget` | `PlaceId` | integer | Place ID |
| `/trip-budget` | `TransportTypeId` | integer | Transport type ID |
| `/trip-budget` | `OriginLat` | double | Origin latitude |
| `/trip-budget` | `OriginLng` | double | Origin longitude |
| `/trip-budget` | `FoodCostOverride` | double | Override food cost |
| `/trip-budget` | `PeopleCount` | integer | Number of people |
| `/smart-calculate` | `userLat` | double | User's latitude |
| `/smart-calculate` | `userLng` | double | User's longitude |
| `/smart-calculate` | `destinationPlaceId` | integer | Destination place ID |
| `/smart-calculate` | `people` | integer | Number of people (default: 1) |
| `/smart-calculate` | `days` | integer | Number of days (default: 1) |
| `/smart-calculate` | `foodLevel` | enum | 1=Low, 2=Medium, 3=High |

---

## 👤 Users Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/v1/users/me` | Get current user profile |
| PUT | `/api/v1/users/me` | Update user profile |
| POST | `/api/v1/users/me/avatar` | Upload avatar |
| GET | `/api/v1/users/me/favourites` | Get favourite places |
| POST | `/api/v1/users/me/favourites/{placeId}` | Add place to favourites |
| DELETE | `/api/v1/users/me/favourites/{placeId}` | Remove place from favourites |
| GET | `/api/v1/users/me/visit-history` | Get visit history |
| POST | `/api/v1/users/me/check-in/{placeId}` | Check in at a place |
| GET | `/api/v1/users/me/check-ins` | Get check-in history |
| GET | `/api/v1/users/me/saved-districts` | Get saved districts |
| POST | `/api/v1/users/me/saved-districts/{districtId}` | Save a district |
| DELETE | `/api/v1/users/me/saved-districts/{districtId}` | Unsave a district |

### Users Query Parameters

| Endpoint | Parameter | Type | Description |
|----------|-----------|------|-------------|
| `/visit-history` | `pageNumber` | integer | Page number (default: 1) |
| `/visit-history` | `pageSize` | integer | Items per page (default: 10) |
| `/check-ins` | `pageNumber` | integer | Page number (default: 1) |
| `/check-ins` | `pageSize` | integer | Items per page (default: 10) |

---

## 📊 Data Models

### AnalyticsDashboardDto

| Property | Type | Description |
|----------|------|-------------|
| `totalUsers` | int32 | Total registered users |
| `totalPlaces` | int32 | Total places in system |
| `pendingPlaces` | int32 | Places pending approval |
| `pendingReviews` | int32 | Reviews pending approval |
| `openReports` | int32 | Open review reports |
| `districtStats` | array | Statistics by district |

### PlaceListItemDto

| Property | Type | Description |
|----------|------|-------------|
| `id` | int32 | Place ID |
| `name` | string | Place name (English) |
| `nameBn` | string | Place name (Bangla) |
| `coverPhotoUrl` | string | Cover photo URL |
| `districtName` | string | District name |
| `divisionName` | string | Division name |
| `entryFee` | double | Entry fee (if applicable) |
| `averageRating` | double | Average rating |
| `totalReviews` | int32 | Total number of reviews |
| `categories` | array | Category names |
| `latitude` | double | Latitude |
| `longitude` | double | Longitude |
| `distanceKm` | double | Distance from query point (if applicable) |

### PlaceDetailDto

| Property | Type | Description |
|----------|------|-------------|
| `id` | int32 | Place ID |
| `name` | string | Place name (English) |
| `nameBn` | string | Place name (Bangla) |
| `description` | string | Detailed description |
| `latitude` | double | Latitude |
| `longitude` | double | Longitude |
| `entryFee` | double | Entry fee |
| `bestSeason` | enum | Best season (1=Summer, 2=Monsoon, 3=Autumn, 4=Winter) |
| `divisionName` | string | Division name |
| `districtName` | string | District name |
| `upazilaName` | string | Upazila name |
| `averageRating` | double | Average rating |
| `totalReviews` | int32 | Total reviews count |
| `openingHours` | string | Opening hours |
| `closingHours` | string | Closing hours |
| `photos` | array | Place photos |
| `categories` | array | Categories |
| `tags` | array | Tags |

### ReviewDto

| Property | Type | Description |
|----------|------|-------------|
| `id` | int32 | Review ID |
| `placeId` | int32 | Associated place ID |
| `userId` | string | User ID |
| `userName` | string | User name |
| `userAvatarUrl` | string | User avatar URL |
| `rating` | int32 | Rating (1-5) |
| `commentEn` | string | Comment (English) |
| `commentBn` | string | Comment (Bangla) |
| `photos` | array | Review photos |
| `createdAt` | datetime | Creation timestamp |

### UserProfileDto

| Property | Type | Description |
|----------|------|-------------|
| `id` | string | User ID |
| `fullName` | string | Full name |
| `email` | string | Email address |
| `avatarUrl` | string | Avatar URL |
| `createdAt` | datetime | Account creation date |
| `roles` | array | User roles |

### SmartTripResultDto

| Property | Type | Description |
|----------|------|-------------|
| `fromLocation` | string | Starting location |
| `toLocation` | string | Destination location |
| `fromDistrict` | string | Origin district |
| `toDistrict` | string | Destination district |
| `nearestFromStand` | string | Nearest bus stand (origin) |
| `fromStandLat` | double | Stand latitude (origin) |
| `fromStandLng` | double | Stand longitude (origin) |
| `nearestToStand` | string | Nearest bus stand (destination) |
| `toStandLat` | double | Stand latitude (destination) |
| `toStandLng` | double | Stand longitude (destination) |
| `userToStandKm` | double | Distance to stand |
| `standToDestKm` | double | Distance from stand to destination |
| `totalTravelMinutes` | double | Total travel time in minutes |
| `totalTravelTime` | string | Formatted travel time |
| `budget` | object | Budget breakdown |
| `isDirectRoute` | boolean | Is direct route available |
| `sameDistrict` | boolean | Same district or not |
| `steps` | array | Trip steps |
| `destinationGoogleMapsUrl` | string | Google Maps URL for destination |
| `fromStandGoogleMapsUrl` | string | Google Maps URL for origin stand |
| `toStandGoogleMapsUrl` | string | Google Maps URL for destination stand |
| `placeName` | string | Place name |
| `placeEntryFee` | double | Entry fee |

### BudgetBreakdownDto

| Property | Type | Description |
|----------|------|-------------|
| `userToStandCostPerPerson` | double | Cost to reach stand (per person) |
| `busRouteCostPerPerson` | double | Bus route cost (per person) |
| `standToDestCostPerPerson` | double | Stand to destination cost (per person) |
| `transportTotalPerPerson` | double | Total transport cost (per person) |
| `foodCostPerPerson` | double | Food cost (per person) |
| `totalPerPerson` | double | Total cost (per person) |
| `transportTotalAllPeople` | double | Total transport cost (all people) |
| `foodTotalAllPeople` | double | Total food cost (all people) |
| `grandTotal` | double | Grand total |
| `foodLevelName` | string | Food level name |
| `foodCostPerDayPerPerson` | double | Daily food cost (per person) |
| `days` | int32 | Number of days |
| `people` | int32 | Number of people |

### FoodLevel Enum

| Value | Level |
|-------|-------|
| 1 | Low Budget |
| 2 | Medium Budget |
| 3 | High Budget |

---

## 🔄 Common Response Formats

### Paginated Response

All paginated responses follow this structure:

```json
{
  "items": [...],
  "pageNumber": 1,
  "totalPages": 10,
  "totalCount": 100,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

## 🛠️ Technologies Used

- **Framework:** ASP.NET Core
- **API Style:** RESTful
- **Documentation:** OpenAPI 3.0.4 (Swagger/JSON)
- **Authentication:** JWT Bearer Token
- **Versioning:** URI versioning (v1)
- **Architecture:** Clean Architecture / CQRS 

---

## 👨‍💻 Developer / Contact

- **Developed by:Maruf Hasan
- **Email: marufhasanash@gmail.com
- **Project:** TourGuideBD API

---

**Generated from OpenAPI Specification v3.0.4**  
*API Version: v1*  
*Last Updated: June 2026*



