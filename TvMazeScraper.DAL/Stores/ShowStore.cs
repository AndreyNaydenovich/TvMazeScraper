using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Contracts.Stores;
using TvMazeScraper.DAL.Entities;
using TvMazeScraper.DAL.Helpers;

namespace TvMazeScraper.DAL.Stores
{
    public class ShowStore : IShowStore
    {
        public readonly string NewShowIds = nameof(NewShowIds);
        public readonly string ShowIds = nameof(ShowIds);
        public readonly string ShowList = nameof(ShowList);
        public readonly string ShowIdsOrdered = nameof(ShowIdsOrdered);
        public readonly string UpdateDate = nameof(UpdateDate);

        private readonly IDatabase _db;
        private readonly ISerializer _serializer;

        public ShowStore(IDatabaseFactory databaseFactory, ISerializer serializer)
        {
            _db = databaseFactory.GetDatabase();
            _serializer = serializer;
        }

        public async Task<IShow> GetAsync(int id)
        {
            var redisValue = await _db.HashGetAsync(ShowList, id).ConfigureAwait(false);

            if (redisValue.HasValue)
            {
                return _serializer.Deserialize<Show>(redisValue);
            }

            return null;
        }

        public async Task<IEnumerable<IShow>> GetAsync(int offset, int count)
        {
            if (count < 1)
            {
                throw new System.ArgumentOutOfRangeException(nameof(count), $@"{nameof(count)} must be > 0");
            }

            var showIds = await _db.SortedSetRangeByRankAsync(ShowIdsOrdered, offset, offset + count - 1).ConfigureAwait(false);
            var redisValues = await _db.HashGetAsync(ShowList, showIds).ConfigureAwait(false);

            var shows = new List<IShow>(redisValues.Length);

            foreach (var serializedShow in redisValues)
            {
                shows.Add(_serializer.Deserialize<Show>(serializedShow));
            }

            return shows;
        }

        public Task SetAsync(IShow show)
        {
            var tran = _db.CreateTransaction();

#pragma warning disable CS4014 // we have not to await here due to library design
            tran.SetAddAsync(ShowIds, show.Id);
            tran.SortedSetAddAsync(ShowIdsOrdered, show.Id, (double)show.Id);
            tran.HashSetAsync(ShowList, show.Id, _serializer.Serialize(show));
#pragma warning restore CS4014 // we have not to await here due to library design

            return tran.ExecuteAsync(CommandFlags.DemandMaster);
        }

        public async Task RemoveNotMatchedAsync(IEnumerable<int> ids)
        {
            await _db.KeyDeleteAsync(NewShowIds, CommandFlags.DemandMaster).ConfigureAwait(false);
            await _db.SetAddAsync(NewShowIds, ids.Select(t => (RedisValue)t).ToArray(), CommandFlags.DemandMaster).ConfigureAwait(false);
            var showIdsToRemove = await _db.SetCombineAsync(SetOperation.Difference, ShowIds, NewShowIds, CommandFlags.DemandMaster)
                .ConfigureAwait(false);

            var tran = _db.CreateTransaction();

            foreach (var id in showIdsToRemove)
            {
#pragma warning disable CS4014 // we have not to await here due to library design
                tran.SetRemoveAsync(ShowIds, id);
                tran.SortedSetRemoveAsync(ShowIdsOrdered, id);
                tran.HashDeleteAsync(ShowList, id);
#pragma warning restore CS4014 // we have not to await here due to library design
            }

            await tran.ExecuteAsync(CommandFlags.DemandMaster).ConfigureAwait(false);
        }
    }
}