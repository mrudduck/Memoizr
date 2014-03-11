using System;
using System.Runtime.Caching;

namespace Memoizr.InMemoryCache
{
    /// <summary>
    /// An <see cref="ICache"/> implementation utilizing the <see cref="System.Runtime.Caching.MemoryCache"/> as the backing store.
    /// </summary>
    public class InMemoryCache : ICache
    {
        /// <summary>
        /// The in memory backing store.
        /// </summary>
        public MemoryCache MemoryCache;

        /// <summary>
        /// Initializes with a new instance of a <see cref="System.Runtime.Caching.MemoryCache"/>.
        /// </summary>
        /// <param name="cacheName">Cache name.</param>
        public InMemoryCache( string cacheName )
        {
            MemoryCache = new MemoryCache( cacheName );
        }

        #region Implementation of ICache

        /// <summary>
        /// Gets a value from cache.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <returns>Value of type <typeparamref name="T"/> from cache or null.</returns>
        public T Get<T>( string key )
        {
            if ( string.IsNullOrWhiteSpace( key ) )
                return default( T );

            var obj = MemoryCache.Get( key );

            if ( obj == null )
                return default( T );

            return ( T ) obj;
        }

        /// <summary>
        /// Adds a value to cache.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <param name="value">Value to store.</param>
        /// <returns>Indicates whether the value was successfully stored in cache.</returns>
        public bool Add<T>( string key, T value )
        {
            if ( string.IsNullOrWhiteSpace( key ) || (object) value == null )
                return false;

            MemoryCache.Set( key, value, new CacheItemPolicy() );
            
            return true;
        }

        /// <summary>
        /// Adds a value to cache for the specified duration.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <param name="value">Value to store.</param>
        /// <param name="cacheTime">Duration to store the value in cache.</param>
        /// <returns>Indicates whether the value was successfully stored in cache.</returns>
        public bool Add<T>( string key, T value, TimeSpan cacheTime )
        {
            if ( string.IsNullOrWhiteSpace( key ) || ( object ) value == null )
                return false;

            var cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now + cacheTime };

            MemoryCache.Set( key, value, cacheItemPolicy );

            return true;
        }

        /// <summary>
        /// Removes an item from cache.
        /// </summary>
        /// <param name="key">Item's key.</param>
        /// <returns>Indicates whether the value was successfully removed from cache.</returns>
        public bool Remove( string key )
        {
            var value = MemoryCache.Remove( key );

            return value != null;
        }

        #endregion Implementation of ICache

        #region Implementation of IDisposable

        /// <summary>
        /// Disposes of the <see cref="System.Runtime.Caching.MemoryCache"/> instance.
        /// </summary>
        public void Dispose()
        {
            MemoryCache.Dispose();
        }

        #endregion Implementation of IDisposable
    }
}