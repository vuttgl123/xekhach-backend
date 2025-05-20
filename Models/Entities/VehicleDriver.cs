namespace LuanAnTotNghiep_TuanVu_TuBac.Models.Entities
{
    public class VehicleDriver
    {
        public int Id { get; set; }             
        public int VehicleId { get; set; }        
        public int DriverId { get; set; }         
        public DateTime CreatedAt { get; set; }   
        public DateTime AssignedAt { get; set; }  
        public DateTime? UnassignedAt { get; set; } 
        public int? CreatedByAdminId { get; set; }
        public int? UpdatedByAdminId { get; set; }
        public virtual Driver Driver { get; set; }

        public ICollection<RouteTripSchedule> RouteTripSchedules { get; set; }
    }
}
