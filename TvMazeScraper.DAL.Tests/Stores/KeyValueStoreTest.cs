using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using StackExchange.Redis;
using TvMazeScraper.DAL.Helpers;
using TvMazeScraper.DAL.Stores;

namespace TvMazeScraper.DAL.Tests.Stores
{
    public class KeyValueStoreTest
    {
        Mock<IDatabase> _db;
        Mock<ISerializer> _serializer;
        KeyValueStore _sut;

        [SetUp]
        public void Setup()
        {
            _db = new Mock<IDatabase>();

            var dbFactory = new Mock<IDatabaseFactory>();
            dbFactory.Setup(t => t.GetDatabase()).Returns(() => _db.Object);

            _serializer = new Mock<ISerializer>();
            _serializer.Setup(t => t.Serialize(It.IsAny<int>())).Returns<int>(obj => JsonConvert.SerializeObject(obj));
            _serializer.Setup(t => t.Deserialize<int>(It.IsAny<string>())).Returns<string>(JsonConvert.DeserializeObject<int>);

            _sut = new KeyValueStore(dbFactory.Object, _serializer.Object);
        }
    }
}