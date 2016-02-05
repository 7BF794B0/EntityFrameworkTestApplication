using System.Collections.Generic;
using System.Linq;
using EntityFrameworkTestApplication.Models;

namespace EntityFrameworkTestApplication.Model
{
    /// <summary>
    /// The methods needed to fill your friends list.
    /// </summary>
    class Friend
    {
        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer.
        /// For Storage1.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="listOfFriends">Friend list.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage1(Storage1Context context, Friends f, List<int> listOfFriends)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            listOfFriends.Add(elementToAdd[0].Id);
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer.
        /// For Storage2.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="listOfFriends">Friend list.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage2(Storage2Context context, Friends f, List<int> listOfFriends)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            listOfFriends.Add(elementToAdd[0].Id);
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer.
        /// For Storage3.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="listOfFriends">Friend list.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage3(Storage3Context context, Friends f, List<int> listOfFriends)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            listOfFriends.Add(elementToAdd[0].Id);
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer.
        /// For Storage4.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="listOfFriends">Friend list.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage4(Storage4Context context, Friends f, List<int> listOfFriends)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            listOfFriends.Add(elementToAdd[0].Id);
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer.
        /// For Storage5.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="listOfFriends">Friend list.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage5(Storage5Context context, Friends f, List<int> listOfFriends)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            listOfFriends.Add(elementToAdd[0].Id);
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer (modal).
        /// For Storage1.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="frm">Instance a modal window.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage1(Storage1Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer (modal).
        /// For Storage2.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="frm">Instance a modal window.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage2(Storage2Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer (modal).
        /// For Storage3.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="frm">Instance a modal window.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage3(Storage3Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer (modal).
        /// For Storage4.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="frm">Instance a modal window.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage4(Storage4Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }

        /// <summary>
        /// The method of forming a string name = Last Name + First Name, which will be added to the list.
        /// Called by method Connect To Server ConnectToServer (modal).
        /// For Storage5.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="f">The object to be added such as Friends.</param>
        /// <param name="frm">Instance a modal window.</param>
        /// <returns>The format string "{Last Name} + {First Name}".</returns>
        public string FindFriendsStorage5(Storage5Context context, Friends f, InfoWindow frm)
        {
            var queryFriend = context.User.Where(x => x.Id == f.Friend_Id);
            var elementToAdd = queryFriend.ToList();
            frm.ListOfFriendsParam = elementToAdd[0].Id;
            return (elementToAdd[0].Last_Name + ' ' + elementToAdd[0].First_Name);
        }
    }
}