using System.Threading.Tasks;
namespace MomitKiller.Api.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(string subject, string message);
    }
}
