namespace LuanAnTotNghiep_TuanVu_TuBac.DTOs
{
    public class CreateRideRequest
    {
        public int UserId { get; set; }
        public int RouteTripScheduleId { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal EstimatedFare { get; set; }
        public bool IsRoundTrip { get; set; }
        public decimal DistanceKm { get; set; }
        public int? ReturnRouteTripScheduleId { get; set; }

        public int TicketCountGo { get; set; }  // Số vé lượt đi
        public int TicketCountReturn { get; set; } // Số vé lượt về (nếu có)
    }
}
