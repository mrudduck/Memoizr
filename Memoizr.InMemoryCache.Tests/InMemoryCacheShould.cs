using NUnit.Framework;

namespace Memoizr.InMemoryCache.Tests
{
    [TestFixture]
    public class InMemoryCacheShould
    {
        [Test]
        public void Create_A_New_Instance_Of_MemoryCache()
        {
            // Arrange
            const string cacheName = "SuperHeroesCache";

            // Act
            var cache = new InMemoryCache( cacheName );

            // Assert
            Assert.NotNull( cache.MemoryCache );
        }

        [Test]
        public void Name_The_Instance_Of_MemoryCache()
        {
            // Arrange
            const string cacheName = "SuperHeroesCache";

            // Act
            var cache = new InMemoryCache( cacheName );

            // Assert
            Assert.That( cache.MemoryCache.Name == cacheName );
        }

        [Test]
        public void Return_True_When_A_Value_Is_Successfully_Added()
        {
            // Arrange
            const string cacheName = "SuperHeroesCache";
            var cache = new InMemoryCache( cacheName );
            var superman = new SuperHero { SuperHeroName = "Superman", RealFirstName = "Clark", RealLastName = "Kent" };

            // Act
            var valueAdded = cache.Add( superman.SuperHeroName, superman );

            // Assert
            Assert.True( valueAdded );
        }

        [Test]
        public void Return_False_When_A_Null_Value_Is_Added()
        {
            // Arrange
            const string cacheName = "SuperHeroesCache";
            var cache = new InMemoryCache( cacheName );
            SuperHero superman = null;

            // Act
            var valueAdded = cache.Add( "Superman", superman );

            // Assert
            Assert.False( valueAdded );
        }

        [Test]
        public void Return_False_When_A_Null_CacheKey_Is_Added()
        {
            // Arrange
            const string cacheName = "SuperHeroesCache";
            var cache = new InMemoryCache( cacheName );
            SuperHero superman = null;

            // Act
            var valueAdded = cache.Add( null, superman );

            // Assert
            Assert.False( valueAdded );
        }

        public class SuperHero
        {
            public string SuperHeroName { get; set; }
            public string RealFirstName { get; set; }
            public string RealLastName { get; set; }
        }
    }
}