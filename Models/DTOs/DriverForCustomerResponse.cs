namespace LuanAnTotNghiep_TuanVu_TuBac.Models.DTOs
{
    public class DriverForCustomerResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string VehicleType { get; set; }
        public string AvatarUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string LicenseType { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public string OperatingArea { get; set; }
    }
}
