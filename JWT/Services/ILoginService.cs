using System.Threading.Tasks;
using JWT.Models;

namespace JWT.Services
{
    public interface ILoginService
    {
        Token Login(Login login);
    }
}
