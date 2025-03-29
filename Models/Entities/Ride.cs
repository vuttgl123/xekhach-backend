using System.Text.Json.Serialization;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Ride
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RouteTripScheduleId { get; set; }

        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal EstimatedFare { get; set; }
        public byte Status { get; set; }
        public bool IsRoundTrip { get; set; }
        public int? ReturnRideId { get; set; }
        public decimal DistanceKm { get; set; }
        public int? VehicleDriverId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public RouteTripSchedule RouteTripSchedule { get; set; }
    }


}
