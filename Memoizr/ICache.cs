using System;

namespace Memoizr
{
    /// <summary>
    /// Cache interface.
    /// </summary>
    public interface ICache : IDisposable
    {
        /// <summary>
        /// Gets a value from cache.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <returns>Value of type <typeparamref name="T"/> from cache or null.</returns>
        T Get<T>( string key );

        /// <summary>
        /// Adds a value to cache.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <param name="value">Value to store.</param>
        /// <returns>Indicates whether the value was successfully stored in cache.</returns>
        bool Add<T>( string key, T value );

        /// <summary>
        /// Adds a value to cache for the specified duration.
        /// </summary>
        /// <typeparam name="T">Value's type.</typeparam>
        /// <param name="key">Value's key.</param>
        /// <param name="value">Value to store.</param>
        /// <param name="cacheTime">Duration to store the value in cache.</param>
        /// <returns>Indicates whether the value was successfully stored in cache.</returns>
        bool Add<T>( string key, T value, TimeSpan cacheTime );

        /// <summary>
        /// Removes an item from cache.
        /// </summary>
        /// <param name="key">Item's key.</param>
        /// <returns>Indicates whether the value was successfully removed from cache.</returns>
        bool Remove( string key );
    }
}