﻿using System.Threading.Tasks;

namespace TvMazeScraper.Contracts.Stores
{
    public interface IKeyValueStore
    {
        Task SetAsync<T>(string key, T value);
        Task RemoveAsync(string key);
        Task<T> GetAsync<T>(string key);
    }
}