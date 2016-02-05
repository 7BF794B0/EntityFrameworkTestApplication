using System;
using System.IO;
using System.Data.SqlClient;

namespace InsertFriends
{
    class Program
    {
        static void Main()
        {
            using (var sqlConnection = new SqlConnection(@"ololo"))
            {
                var u = File.ReadAllLines("1.txt");
                var uf = File.ReadAllLines("1f.txt");
                
                sqlConnection.Open();

                for (var i = 0; i < u.Length; i++)
                {
                    var commandInsert = new SqlCommand(@"INSERT INTO Friends (User_Id,Friend_Id) VALUES (@userid,@friendid)", sqlConnection);

                    commandInsert.Parameters.Add("@userid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(u[i]);
                    commandInsert.Parameters.Add("@friendid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(uf[i]);
                    
                    commandInsert.ExecuteNonQuery();
                }
                
                sqlConnection.Close();
            }

            using (var sqlConnection = new SqlConnection(@"ololo"))
            {
                var u = File.ReadAllLines("2.txt");
                var uf = File.ReadAllLines("2f.txt");

                sqlConnection.Open();

                for (var i = 0; i < u.Length; i++)
                {
                    var commandInsert = new SqlCommand(@"INSERT INTO Friends (User_Id,Friend_Id) VALUES (@userid,@friendid)", sqlConnection);

                    commandInsert.Parameters.Add("@userid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(u[i]);
                    commandInsert.Parameters.Add("@friendid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(uf[i]);

                    commandInsert.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }

            using (var sqlConnection = new SqlConnection(@"ololo"))
            {
                var u = File.ReadAllLines("3.txt");
                var uf = File.ReadAllLines("3f.txt");

                sqlConnection.Open();

                for (var i = 0; i < u.Length; i++)
                {
                    var commandInsert = new SqlCommand(@"INSERT INTO Friends (User_Id,Friend_Id) VALUES (@userid,@friendid)", sqlConnection);

                    commandInsert.Parameters.Add("@userid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(u[i]);
                    commandInsert.Parameters.Add("@friendid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(uf[i]);

                    commandInsert.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }

            using (var sqlConnection = new SqlConnection(@"ololo"))
            {
                var u = File.ReadAllLines("4.txt");
                var uf = File.ReadAllLines("4f.txt");

                sqlConnection.Open();

                for (var i = 0; i < u.Length; i++)
                {
                    var commandInsert = new SqlCommand(@"INSERT INTO Friends (User_Id,Friend_Id) VALUES (@userid,@friendid)", sqlConnection);

                    commandInsert.Parameters.Add("@userid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(u[i]);
                    commandInsert.Parameters.Add("@friendid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(uf[i]);

                    commandInsert.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }

            using (var sqlConnection = new SqlConnection(@"ololo"))
            {
                var u = File.ReadAllLines("5.txt");
                var uf = File.ReadAllLines("5f.txt");

                sqlConnection.Open();

                for (var i = 0; i < u.Length; i++)
                {
                    var commandInsert = new SqlCommand(@"INSERT INTO Friends (User_Id,Friend_Id) VALUES (@userid,@friendid)", sqlConnection);

                    commandInsert.Parameters.Add("@userid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(u[i]);
                    commandInsert.Parameters.Add("@friendid", System.Data.SqlDbType.Int).Value = Convert.ToInt32(uf[i]);

                    commandInsert.ExecuteNonQuery();
                }

                sqlConnection.Close();
            }
        }
    }
}
