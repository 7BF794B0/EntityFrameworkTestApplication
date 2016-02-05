using System.Collections.Generic;
using System.Transactions;
using EntityFrameworkTestApplication.Models;

namespace EntityFrameworkTestApplication.Model
{
    /// <summary>
    /// Methods the necessary to fill the database.
    /// </summary>
    class FillingData
    {
        /// <summary>
        /// Methods of adding a record in the database (Storage1).
        /// Called from the methods TransactionScopeWithStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="entity">The object to add the type of User.</param>
        /// <param name="count">Counter.</param>
        /// <param name="commitCount">Meaning - "every time commitCount doing SaveChanges()".</param>
        /// <param name="recreateContext">It must be true.</param>
        /// <returns>The context of the interaction with the database.</returns>
        private static Storage1Context AddToStorage1(Storage1Context context, User entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<User>().Add(entity);

            if (count%commitCount != 0)
                return context;

            context.SaveChanges();

            if (!recreateContext)
                return context;

            context.Dispose();
            context = new Storage1Context();
            context.Configuration.AutoDetectChangesEnabled = false;

            return context;
        }

        /// <summary>
        /// Methods of adding a record in the database (Storage2).
        /// Called from the methods TransactionScopeWithStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="entity">The object to add the type of User.</param>
        /// <param name="count">Counter.</param>
        /// <param name="commitCount">Meaning - "every time commitCount doing SaveChanges()".</param>
        /// <param name="recreateContext">It must be true.</param>
        /// <returns>The context of the interaction with the database.</returns>
        private static Storage2Context AddToStorage2(Storage2Context context, User entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<User>().Add(entity);

            if (count%commitCount != 0)
                return context;

            context.SaveChanges();

            if (!recreateContext)
                return context;

            context.Dispose();
            context = new Storage2Context();
            context.Configuration.AutoDetectChangesEnabled = false;

            return context;
        }

        /// <summary>
        /// Methods of adding a record in the database (Storage3).
        /// Called from the methods TransactionScopeWithStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="entity">The object to add the type of User.</param>
        /// <param name="count">Counter.</param>
        /// <param name="commitCount">Meaning - "every time commitCount doing SaveChanges()".</param>
        /// <param name="recreateContext">It must be true.</param>
        /// <returns>The context of the interaction with the database.</returns>
        private static Storage3Context AddToStorage3(Storage3Context context, User entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<User>().Add(entity);

            if (count%commitCount != 0)
                return context;

            context.SaveChanges();

            if (!recreateContext)
                return context;

            context.Dispose();
            context = new Storage3Context();
            context.Configuration.AutoDetectChangesEnabled = false;

            return context;
        }

        /// <summary>
        /// Methods of adding a record in the database (Storage4).
        /// Called from the methods TransactionScopeWithStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="entity">The object to add the type of User.</param>
        /// <param name="count">Counter.</param>
        /// <param name="commitCount">Meaning - "every time commitCount doing SaveChanges()".</param>
        /// <param name="recreateContext">It must be true.</param>
        /// <returns>The context of the interaction with the database.</returns>
        private static Storage4Context AddToStorage4(Storage4Context context, User entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<User>().Add(entity);

            if (count%commitCount != 0)
                return context;

            context.SaveChanges();

            if (!recreateContext)
                return context;

            context.Dispose();
            context = new Storage4Context();
            context.Configuration.AutoDetectChangesEnabled = false;

            return context;
        }

        /// <summary>
        /// Methods of adding a record in the database (Storage5).
        /// Called from the methods TransactionScopeWithStorage.
        /// </summary>
        /// <param name="context">The context of the interaction with the database.</param>
        /// <param name="entity">The object to add the type of User.</param>
        /// <param name="count">Counter.</param>
        /// <param name="commitCount">Meaning - "every time commitCount doing SaveChanges()".</param>
        /// <param name="recreateContext">It must be true.</param>
        /// <returns>The context of the interaction with the database.</returns>
        private static Storage5Context AddToStorage5(Storage5Context context, User entity, int count, int commitCount, bool recreateContext)
        {
            context.Set<User>().Add(entity);

            if (count%commitCount != 0)
                return context;

            context.SaveChanges();

            if (!recreateContext)
                return context;

            context.Dispose();
            context = new Storage5Context();
            context.Configuration.AutoDetectChangesEnabled = false;

            return context;
        }

        /// <summary>
        /// The method for forming transactions with Storage1.
        /// Called from an event handler btnFill_Click.
        /// </summary>
        /// <param name="users">The list of users you want to add.</param>
        public void TransactionScopeWithStorage1(List<User> users)
        {
            using (var scope = new TransactionScope())
            {
                Storage1Context context = null;
                try
                {
                    context = new Storage1Context();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in users)
                    {
                        ++count;
                        context = AddToStorage1(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context?.Dispose();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// The method for forming transactions with Storage2.
        /// Called from an event handler btnFill_Click.
        /// </summary>
        /// <param name="users">The list of users you want to add.</param>
        public void TransactionScopeWithStorage2(List<User> users)
        {
            using (var scope = new TransactionScope())
            {
                Storage2Context context = null;
                try
                {
                    context = new Storage2Context();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in users)
                    {
                        ++count;
                        context = AddToStorage2(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context?.Dispose();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// The method for forming transactions with Storage3.
        /// Called from an event handler btnFill_Click.
        /// </summary>
        /// <param name="users">The list of users you want to add.</param>
        public void TransactionScopeWithStorage3(List<User> users)
        {
            using (var scope = new TransactionScope())
            {
                Storage3Context context = null;
                try
                {
                    context = new Storage3Context();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in users)
                    {
                        ++count;
                        context = AddToStorage3(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context?.Dispose();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// The method for forming transactions with Storage4.
        /// Called from an event handler btnFill_Click.
        /// </summary>
        /// <param name="users">The list of users you want to add.</param>
        public void TransactionScopeWithStorage4(List<User> users)
        {
            using (var scope = new TransactionScope())
            {
                Storage4Context context = null;
                try
                {
                    context = new Storage4Context();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in users)
                    {
                        ++count;
                        context = AddToStorage4(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context?.Dispose();
                }

                scope.Complete();
            }
        }

        /// <summary>
        /// The method for forming transactions with Storage5.
        /// Called from an event handler btnFill_Click.
        /// </summary>
        /// <param name="users">The list of users you want to add.</param>
        public void TransactionScopeWithStorage5(List<User> users)
        {
            using (var scope = new TransactionScope())
            {
                Storage5Context context = null;
                try
                {
                    context = new Storage5Context();
                    context.Configuration.AutoDetectChangesEnabled = false;

                    var count = 0;
                    foreach (var entityToInsert in users)
                    {
                        ++count;
                        context = AddToStorage5(context, entityToInsert, count, 100, true);
                    }

                    context.SaveChanges();
                }
                finally
                {
                    context?.Dispose();
                }

                scope.Complete();
            }
        }
    }
}