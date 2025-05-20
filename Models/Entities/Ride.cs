using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class Ride
    {
        public int Id { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int RouteTripScheduleId { get; set; }
        public int? ReturnRideId { get; set; }
        public int? PaymentId { get; set; }
        public int? CreatedByAdminId { get; set; }
        public int? UpdatedByAdminId { get; set; }

        // Ride Details
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal EstimatedFare { get; set; }
        public decimal DistanceKm { get; set; }
        public bool IsRoundTrip { get; set; }
        public byte Status { get; set; }

        // Optional times (nullable)
        public DateTime? PickupTime { get; set; }
        public DateTime? DropoffTime { get; set; }

        // Audit fields
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int TicketCountGo { get; set; }  // Thêm số vé lượt đi
        public int TicketCountReturn { get; set; }

        // Navigation properties
        public RouteTripSchedule RouteTripSchedule { get; set; }
        public Ride ReturnRide { get; set; }

        public Payment Payment { get; set; }
    }
}
