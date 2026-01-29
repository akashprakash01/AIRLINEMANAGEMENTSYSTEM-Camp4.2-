using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIRLINEMANAGEMENTSYSTEM.Repository
{
    public interface ILoginRepository
    {
        // returnValue: 1 = success, 0 = failure
        Task<(bool isSuccess, int administratorId)> LoginAsync(string username, string password);
    }

}
