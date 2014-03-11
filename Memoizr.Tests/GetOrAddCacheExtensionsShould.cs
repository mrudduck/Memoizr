using System;
using NUnit.Framework;

namespace Memoizr.Tests
{
    [TestFixture]
    public class GetOrAddCacheExtensionsShould
    {
        [Test]
        public void Call_The_Function_When_A_Value_Does_Not_Exist_In_Cache()
        {
            // Arrange
            ICache cache = new DictionaryCache();

            const int customerId = 1;

            bool functionCalled = false;

            // Act
            var customer = cache.GetOrAdd( customerId.ToString(), TimeSpan.FromSeconds( 30 ), () =>
                {
                    functionCalled = true;

                    return GetCustomer( customerId );
                } );

            // Assert
            Assert.That( customer.CustomerId == customerId );
            Assert.True( functionCalled  );
        }

        [Test]
        public void Not_Call_The_Function_When_A_Value_Exists_In_Cache()
        {
            // Arrange
            ICache cache = new DictionaryCache();

            const int customerId = 1;

            bool functionCalled = false;

            cache.GetOrAdd( customerId.ToString(), TimeSpan.FromSeconds( 30 ), () => GetCustomer( customerId ) );

            // Act
            cache.GetOrAdd( customerId.ToString(), TimeSpan.FromSeconds( 30 ), () =>
                {
                    functionCalled = true;

                    return GetCustomer( customerId );
                } );

            // Assert
            Assert.False( functionCalled );
        }

        [Test]
        public void Call_The_Function_When_Instructed_To_BypassCache()
        {
            // Arrange
            ICache cache = new DictionaryCache();
            const bool bypassCache = true;
            bool functionCalled = false;
            
            // Act
            cache.GetOrAdd( bypassCache, "SomeKey", TimeSpan.FromSeconds( 30 ), () =>
            {
                functionCalled = true;

                return GetCustomer( 1 );
            } );

            // Assert
            Assert.True( functionCalled );
        }

        public Customer GetCustomer( int id )
        {
            return new Customer { CustomerId = id, FirstName = "Clark", LastName = "Kent" };
        }

        public class Customer
        {
            public int CustomerId;
            public string FirstName;
            public string LastName;
        }
    }
}