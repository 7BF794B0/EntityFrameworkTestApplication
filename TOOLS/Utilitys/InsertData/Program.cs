using System.Data.SqlClient;
using System.IO;

namespace InsertData
{
    class Program
    {
        static void Main()
        {
            var reader = new StreamReader("key");
            var sqlConnection = new SqlConnection(reader.ReadLine());
            reader.Close();

            var login = File.ReadAllLines("login.txt");
            var pass = File.ReadAllLines("pass.txt");

            sqlConnection.Open();
            for (var i = 0; i < 500000; i++)
            {
                var commandInsert = new SqlCommand(@"INSERT INTO Map (Login,Password) VALUES (@login,@password)", sqlConnection);

                commandInsert.Parameters.Add("@login", System.Data.SqlDbType.VarChar).Value = login[i];
                commandInsert.Parameters.Add("@password", System.Data.SqlDbType.VarChar).Value = pass[i];

                commandInsert.ExecuteNonQuery();
            }

            sqlConnection.Close();
        }
    }
}