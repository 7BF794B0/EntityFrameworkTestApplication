using System.IO;
using System.Data.SqlClient;

namespace GetData
{
    class Program
    {
        static void Main()
        {
            var sqLcnn = new SqlConnection(@"ololo");

            var sw1 = new StreamWriter("Login.txt");
            sqLcnn.Open();
            for (var i = 1; i < 500001; i++)
            {
                var commandSelect = new SqlCommand($"SELECT Login FROM Map WHERE User_Id={i}", sqLcnn);
                sw1.WriteLine(commandSelect.ExecuteScalar());
            }
            sw1.Close();

            var sw2 = new StreamWriter("Password.txt");
            for (var i = 1; i < 500001; i++)
            {
                var commandSelect = new SqlCommand($"SELECT Password FROM Map WHERE User_Id={i}", sqLcnn);
                sw2.WriteLine(commandSelect.ExecuteScalar());
            }
            sw2.Close();

            sqLcnn.Close();
        }
    }
}
