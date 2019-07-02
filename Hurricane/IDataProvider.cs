using System.Collections.Generic;

namespace Hurricane
{
    public interface IDataProvider
    {
        HashSet<string> GetAllUsers();
        string GetUserData(string login);
    }
}
