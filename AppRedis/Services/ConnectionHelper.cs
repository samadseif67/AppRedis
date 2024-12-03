using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

namespace AppRedis.Services
{
    public class ConnectionHelper
    {
        private IConfiguration configuration;
         ConnectionHelper(IConfiguration _configuration)
        {
            configuration= _configuration;
            ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() => {
                return ConnectionMultiplexer.Connect(configuration.GetSection("RedisURL").ToString());
            });
        }
        private static Lazy<ConnectionMultiplexer> lazyConnection;
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
