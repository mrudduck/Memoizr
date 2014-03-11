using System;

namespace Memoizr
{
    /// <summary>
    /// Cache extensions providing helper functions for interacting with an <see cref="ICache"/>.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Gets or adds results to the cache.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="cache">Cache implementation.</param>
        /// <param name="bypassCache">Indicates whether to bypass the cache.</param>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheTime">Time to store the results/value in the cache.</param>
        /// <param name="func">Function to execute.</param>
        /// <returns>Function results which may or may not have been cached depending on the <paramref name="bypassCache"/> value.</returns>
        public static T GetOrAdd<T>( this ICache cache, bool bypassCache, string cacheKey, TimeSpan cacheTime, Func<T> func ) where T : class
        {
            if ( bypassCache )
            {
                var result = func.Invoke();

                return result;
            }

            var cachedResult = GetOrAdd( cache, cacheKey, cacheTime, func );

            return cachedResult;
        }

        /// <summary>
        /// Gets or adds results to the cache.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="cache">Cache implementation.</param>
        /// <param name="cacheKey">Cache key.</param>
        /// <param name="cacheTime">Time to store the results/value in the cache.</param>
        /// <param name="func">Function to execute.</param>
        /// <returns>Cached results of the function.</returns>
        public static T GetOrAdd<T>( this ICache cache, string cacheKey, TimeSpan cacheTime, Func<T> func ) where T : class
        {
            var cachedResults = cache.Get<T>( cacheKey );

            if ( cachedResults != null )
                return cachedResults;

            var results = func.Invoke();

            if ( results != null )
                cache.Add( cacheKey, results, cacheTime );

            return results;
        }
    }
}