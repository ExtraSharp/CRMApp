namespace DataAccessLibrary
{
    public class SqliteDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            using IDbConnection connection = new SQLiteConnection(connectionString);
            var rows = connection.Query<T>(sqlStatement, parameters).ToList();
            return rows;
        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using IDbConnection connection = new SQLiteConnection(connectionString);
            connection.Execute(sqlStatement, parameters);
        }
    }
}
