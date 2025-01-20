using AppDate.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDate.Application.Abstraction.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}
