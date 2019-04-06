using System.Data;
using System.Data.SqlClient;

namespace SuperDigital.DigitalAccount.Data.Repositories.DB
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork Create(string connectionString)
        {
            string connString = connectionString;

            IDbConnection connection = new SqlConnection(connString);

            connection.Open();

            return new UnitOfWork(connection, true);
        }
    }
}
