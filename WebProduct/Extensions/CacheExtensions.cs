using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.SystemTextJson;

namespace Api_Store.Extensions
    {
    public static class CacheExtensions
        {

        public static IHostApplicationBuilder AddCache(this IHostApplicationBuilder builder)
            {
          

            //   builder.Services.AddScoped<ICacheRegistry, CacheRegistry>();
            var cacheBuider = builder.Services.AddFusionCache()
             .WithOptions(options =>
             {
                 //DistributedCacheCircuitBreakerDuration : to temporarily disable the distributed cache in case of hard errors
                 //so that, if the distributed cache is having issues, it will have less requests to handle and maybe it will be
                 //able to get back on its feet.
                 options.DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2);


                 // CUSTOM LOG LEVELS
                 options.FailSafeActivationLogLevel = LogLevel.Debug;
                 options.SerializationErrorsLogLevel = LogLevel.Warning;
                 options.DistributedCacheSyntheticTimeoutsLogLevel = LogLevel.Debug;
                 options.DistributedCacheErrorsLogLevel = LogLevel.Error;
                 options.FactorySyntheticTimeoutsLogLevel = LogLevel.Debug;
                 options.FactoryErrorsLogLevel = LogLevel.Error;
                 options.FactorySyntheticTimeoutsLogLevel = LogLevel.Debug;
                
             })
             .WithDefaultEntryOptions(new FusionCacheEntryOptions
                 {
                 Duration = TimeSpan.FromMinutes(1),
                 IsFailSafeEnabled = true,
                 FailSafeMaxDuration = TimeSpan.FromHours(1),
                 FailSafeThrottleDuration = TimeSpan.FromSeconds(30),

                 FactorySoftTimeout = TimeSpan.FromSeconds(2),
                 FactoryHardTimeout = TimeSpan.FromSeconds(10),

                 DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1),
                 DistributedCacheHardTimeout = TimeSpan.FromSeconds(2),
                 AllowBackgroundDistributedCacheOperations = true,
                 JitterMaxDuration = TimeSpan.FromSeconds(2)
                 })
             .WithSerializer(
                 new FusionCacheSystemTextJsonSerializer()
             );
            return builder;
            }


        }
    }
