using LuanAnTotNghiep_TuanVu_TuBac.Models.Entities;

namespace LuanAnTotNghiep_TuanVu_TuBac.Repositories.Interfaces
{
    public interface IRouteTripScheduleRepository
    {
        Task<IEnumerable<RouteTripSchedule>> SearchAsync(DateTime date, string origin, string destination);
        Task<RouteTripSchedule> GetByIdAsync(int id);
        Task DecreaseSeatAsync(int scheduleId);
        Task RefreshSchedulesAsync();
    }


}
