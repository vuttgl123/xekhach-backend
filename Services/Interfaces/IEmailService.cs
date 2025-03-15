namespace LuanAnTotNghiep_TuanVu_TuBac.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

}
