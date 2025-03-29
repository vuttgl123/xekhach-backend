namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class CreateRideRequest
    {
        public int UserId { get; set; }
        public int RouteTripScheduleId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal EstimatedFare { get; set; }
        public bool IsRoundTrip { get; set; }
        public double DistanceKm { get; set; }
        public int? ReturnRouteTripScheduleId { get; set; }
    }
}
