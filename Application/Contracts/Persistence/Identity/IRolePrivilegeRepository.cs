using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Persistence.Identity
{
    public interface IRolePrivilegeRepository
    {
        Task AddAsync(string roleId, List<int> PageIds);
    }
}
