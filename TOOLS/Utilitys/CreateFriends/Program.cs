using System;
using System.IO;

namespace CreateFriends
{
    class Program
    {
        static void Main()
        {
            var arrayNumberFriends = new int[500000];

            for (var i = 0; i < 500000; i++) arrayNumberFriends[i] = 0;

            var rnd = new Random();

            var fw1 = new StreamWriter("1.txt");
            var fw1F = new StreamWriter("1f.txt");
            var fw2 = new StreamWriter("2.txt");
            var fw2F = new StreamWriter("2f.txt");
            var fw3 = new StreamWriter("3.txt");
            var fw3F = new StreamWriter("3f.txt");
            var fw4 = new StreamWriter("4.txt");
            var fw4F = new StreamWriter("4f.txt");
            var fw5 = new StreamWriter("5.txt");
            var fw5F = new StreamWriter("5f.txt");

            for (var i = 0; i < 500000; i++)
            {
                if (arrayNumberFriends[i] == 5)
                    continue;
                var shortage = 5 - arrayNumberFriends[i];
                for (var j = 0; j < shortage; j++)
                {
                    var friendId = rnd.Next(1, 500001);
                    if (i == friendId)
                        friendId = rnd.Next(1, 500001);

                    if (i <= 100000)
                    {
                        fw1.WriteLine(i + 1);
                        fw1F.WriteLine(friendId);
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 100000 && i < 200000)
                    {
                        fw2.WriteLine(i + 1);
                        fw2F.WriteLine(friendId);
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 200000 && i < 300000)
                    {
                        fw3.WriteLine(i + 1);
                        fw3F.WriteLine(friendId);
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 300000 && i < 400000)
                    {
                        fw4.WriteLine(i + 1);
                        fw4F.WriteLine(friendId);
                        arrayNumberFriends[i]++;
                    }
                    else if (i > 400000 && i < 500000)
                    {
                        fw5.WriteLine(i + 1);
                        fw5F.WriteLine(friendId);
                        arrayNumberFriends[i]++;
                    }

                    if (friendId > 1 && friendId < 100000)
                    {
                        fw1.WriteLine(friendId);
                        fw1F.WriteLine((i <= 100000) ? i + 1 : i);
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 100000 && friendId < 200000)
                    {
                        fw2.WriteLine(friendId);
                        fw2F.WriteLine((i <= 100000) ? i + 1 : i);
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 200000 && friendId < 300000)
                    {
                        fw3.WriteLine(friendId);
                        fw3F.WriteLine((i <= 100000) ? i + 1 : i);
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 300000 && friendId < 400000)
                    {
                        fw4.WriteLine(friendId);
                        fw4F.WriteLine((i <= 100000) ? i + 1 : i);
                        arrayNumberFriends[friendId]++;
                    }
                    else if (friendId > 400000 && friendId < 500000)
                    {
                        fw5.WriteLine(friendId);
                        fw5F.WriteLine((i <= 100000) ? i + 1 : i);
                        arrayNumberFriends[friendId]++;
                    }
                }
            }
            fw1.Close();
            fw1F.Close();
            fw2.Close();
            fw2F.Close();
            fw3.Close();
            fw3F.Close();
            fw4.Close();
            fw4F.Close();
            fw5.Close();
            fw5F.Close();
        }
    }
}
