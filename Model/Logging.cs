using System;
using System.IO;
using StackExchange.Redis;
using SphinxConnector.SphinxQL;

namespace EntityFrameworkTestApplication.Model
{
    /// <summary>
    /// The necessary methods for processing different Exception.
    /// </summary>
    class Logging
    {
        /// <summary>
        /// Processing method of RedisConnectionException.
        /// </summary>
        /// <param name="ex">Instance RedisConnectionException.</param>
        public void ProcessingException(RedisConnectionException ex)
        {
            var swRedisExceptionLog = new StreamWriter("RedisExceptionLog.txt", true);
            swRedisExceptionLog.WriteLine("#########################################################################################");
            swRedisExceptionLog.WriteLine($"RedisException DateTime: {DateTime.Now}\n");
            swRedisExceptionLog.WriteLine($"RedisException Data: {ex.Data}\n");
            swRedisExceptionLog.WriteLine($"RedisException HelpLink: {ex.HelpLink}\n");
            swRedisExceptionLog.WriteLine($"RedisException HResult: {ex.HResult}\n");
            swRedisExceptionLog.WriteLine($"RedisException InnerException: {ex.InnerException}\n");
            swRedisExceptionLog.WriteLine($"RedisException Message: {ex.Message}\n");
            swRedisExceptionLog.WriteLine($"RedisException Source: {ex.Source}\n");
            swRedisExceptionLog.WriteLine($"RedisException StackTrace: {ex.StackTrace}\n");
            swRedisExceptionLog.WriteLine($"RedisException TargetSite: {ex.TargetSite}\n");
            swRedisExceptionLog.Close();
        }

        /// <summary>
        /// Processing method of RedisException.
        /// </summary>
        /// <param name="ex">Instance RedisException.</param>
        public void ProcessingException(RedisException ex)
        {
            var swRedisExceptionLog = new StreamWriter("RedisExceptionLog.txt", true);
            swRedisExceptionLog.WriteLine("#########################################################################################");
            swRedisExceptionLog.WriteLine($"RedisException DateTime: {DateTime.Now}\n");
            swRedisExceptionLog.WriteLine($"RedisException Data: {ex.Data}\n");
            swRedisExceptionLog.WriteLine($"RedisException HelpLink: {ex.HelpLink}\n");
            swRedisExceptionLog.WriteLine($"RedisException HResult: {ex.HResult}\n");
            swRedisExceptionLog.WriteLine($"RedisException InnerException: {ex.InnerException}\n");
            swRedisExceptionLog.WriteLine($"RedisException Message: {ex.Message}\n");
            swRedisExceptionLog.WriteLine($"RedisException Source: {ex.Source}\n");
            swRedisExceptionLog.WriteLine($"RedisException StackTrace: {ex.StackTrace}\n");
            swRedisExceptionLog.WriteLine($"RedisException TargetSite: {ex.TargetSite}\n");
            swRedisExceptionLog.Close();
        }

        /// <summary>
        /// Processing method of ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="ex">Instance ArgumentOutOfRangeException.</param>
        public void ProcessingException(ArgumentOutOfRangeException ex)
        {
            var swConnectedExceptionLog = new StreamWriter("ConnectedExceptionLog.txt", true);
            swConnectedExceptionLog.WriteLine("#########################################################################################");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException DateTime: {DateTime.Now}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException ActualValue: {ex.ActualValue}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Data: {ex.Data}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException HelpLink: {ex.HelpLink}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException HResult: {ex.HResult}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException InnerException: {ex.InnerException}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Message: {ex.Message}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException ParamName: {ex.ParamName}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Source: {ex.Source}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException StackTrace: {ex.StackTrace}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException TargetSite: {ex.TargetSite}\n");
            swConnectedExceptionLog.Close();
        }

        /// <summary>
        /// Processing method of SphinxQLException.
        /// </summary>
        /// <param name="ex">Instance SphinxQLException.</param>
        public void ProcessingException(SphinxQLException ex)
        {
            var swConnectedExceptionLog = new StreamWriter("SphinxExceptionLog.txt", true);
            swConnectedExceptionLog.WriteLine("#########################################################################################");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException DateTime: {DateTime.Now}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Data: {ex.Data}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException HelpLink: {ex.HelpLink}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException HResult: {ex.HResult}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException InnerException: {ex.InnerException}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Message: {ex.Message}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Code: {ex.Code}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException Source: {ex.Source}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException StackTrace: {ex.StackTrace}\n");
            swConnectedExceptionLog.WriteLine($"ArgumentOutOfRangeException TargetSite: {ex.TargetSite}\n");
            swConnectedExceptionLog.Close();
        }
    }
}