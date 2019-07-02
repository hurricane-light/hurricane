using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hurricane
{
    public interface IDataProvider
    {
        string GetUserData(string login);
    }
}
