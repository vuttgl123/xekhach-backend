namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class RouteTripSchedule
    {
        public int Id { get; set; }
        public int? RouteTripId { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public int? VehicleDriverId { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public DateTime CreatedAt { get; set; }

        public RouteTrip RouteTrip { get; set; }
    }


}
