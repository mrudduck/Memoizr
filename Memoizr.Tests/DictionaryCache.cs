using System;
using System.Collections.Generic;

namespace Memoizr.Tests
{
    /// <summary>
    /// An <see cref="ICache"/> implementation utilizing a <see cref="T:System.Collections.Generic.Dictionary`2"/> as the backing store.
    /// </summary>
    public class DictionaryCache : ICache
    {
        /// <summary>
        /// Cache backing store.
        /// </summary>
        public readonly Dictionary<string, object> Cache;

        public DictionaryCache()
        {
            Cache = new Dictionary<string, object>();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion Implementation of IDisposable

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

            object obj;

            if ( Cache.TryGetValue( key, out obj ) )
            {
                var cacheValue = ( CacheValue ) obj;

                // If item has expired, remove from cache and return default value.
                if ( cacheValue.CacheExpirationDateTime.HasValue && cacheValue.CacheExpirationDateTime < DateTime.Now )
                {
                    Remove( key );

                    return default( T );
                }

                return ( T ) cacheValue.Value;
            }

            return default( T );
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
            if ( string.IsNullOrWhiteSpace( key ) || ( object ) value == null )
                return false;

            var cacheValue = new CacheValue { Value = value };

            if ( Cache.ContainsKey( key ) )
            {
                Cache[ key ] = cacheValue;

                return true;
            }

            Cache.Add( key, cacheValue );

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

            var cacheValue = new CacheValue { Value = value, CacheExpirationDateTime = DateTime.Now + cacheTime };

            if ( Cache.ContainsKey( key ) )
            {
                Cache[ key ] = cacheValue;

                return true;
            }

            Cache.Add( key, cacheValue );

            return true;
        }

        /// <summary>
        /// Removes an item from cache.
        /// </summary>
        /// <param name="key">Item's key.</param>
        /// <returns>Indicates whether the value was successfully removed from cache.</returns>
        public bool Remove( string key )
        {
            if ( string.IsNullOrWhiteSpace( key ) )
                return false;

            return Cache.Remove( key );
        }

        #endregion Implementation of ICache

        #region Nested Types

        /// <summary>
        /// Cached value wrapper.
        /// </summary>
        public class CacheValue
        {
            /// <summary>
            /// Cached value;
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// Optional cache expiration date and time;
            /// </summary>
            public DateTime? CacheExpirationDateTime { get; set; }
        }
        
        #endregion Nested Types
    }
}