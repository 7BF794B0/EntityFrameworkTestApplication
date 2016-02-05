using System.Linq;
using System.Threading;
using EntityFrameworkTestApplication.Models;
using SphinxConnector.SphinxQL;
using StackExchange.Redis;

namespace EntityFrameworkTestApplication.Model
{
    /// <summary>
    /// The necessary methods to determine the status of services.
    /// </summary>
    class Services
    {
        /// <summary>
        /// The flag to check launched Redis.
        /// </summary>
        /// <value>Flag.</value>
        public bool RedisIsStarted { get; private set; }

        /// <summary>
        /// The flag to check launched Sphinx..
        /// </summary>
        /// <value>Flag.</value>
        public bool SphinxIsStarted { get; private set; }

        /// <summary>
        /// Cached Map.
        /// </summary>
        private static void CacheThreadMap()
        {
            using (MapContext connection = new MapContext())
            {
                var entity = connection.Map.ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Cached Storage1.
        /// </summary>
        private static void CacheThreadStorage1()
        {
            using (Storage1Context connection = new Storage1Context())
            {
                var entity = connection.User.ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Cached Storage2.
        /// </summary>
        private static void CacheThreadStorage2()
        {
            using (Storage2Context connection = new Storage2Context())
            {
                var entity = connection.User.ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Cached Storage3.
        /// </summary>
        private static void CacheThreadStorage3()
        {
            using (Storage3Context connection = new Storage3Context())
            {
                var entity = connection.User.ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Cached Storage4.
        /// </summary>
        private static void CacheThreadStorage4()
        {
            using (Storage4Context connection = new Storage4Context())
            {
                var entity = connection.User.ToList().FirstOrDefault();
            }
        }

        /// <summary>
        /// Find out the state services.
        /// </summary>
        public void GetServiceState(Logging logging)
        {
            // Check if Radish running?
            try
            {
                using (var conn = ConnectionMultiplexer.Connect("localhost"))
                {
                    // YES - Redis running.
                    RedisIsStarted = true;
                    conn.Close();
                }
            }
            catch (RedisException ex)
            {
                logging.ProcessingException(ex);
                // NO - Redis is not running.
                RedisIsStarted = false;

                // Cached database initialization form (-2 seconds).
                var cacheMap = new Thread(CacheThreadMap);
                cacheMap.Start();
                var cacheStorage1 = new Thread(CacheThreadStorage1);
                cacheStorage1.Start();
                var cacheStorage2 = new Thread(CacheThreadStorage2);
                cacheStorage2.Start();
                var cacheStorage3 = new Thread(CacheThreadStorage3);
                cacheStorage3.Start();
                var cacheStorage4 = new Thread(CacheThreadStorage4);
                cacheStorage4.Start();
            }

            // Check if running Sphinx?
            try
            {
                var connection = new SphinxQLConnection(@"Data Source=localhost;Port=9306");
                connection.Open();
                // YES - Sphinx running.
                SphinxIsStarted = true;
                connection.Close();
            }
            catch (SphinxQLException ex)
            {
                logging.ProcessingException(ex);
                // NO - Sphinx NOT running.
                SphinxIsStarted = false;
            }
        }
    }
}