using System.Data;

namespace Solomon_Client.DataBase
{
    public abstract class DBConnectionManager
    {
        public abstract IDbConnection GetConnection();
    }
}
