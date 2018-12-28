using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TvMazeScraper.Contracts.Entities;
using TvMazeScraper.Integration.Domain.Configurations;
using TvMazeScraper.Integration.Domain.Entities;
using TvMazeScraper.Integration.Domain.Extensions;

namespace TvMazeScraper.Integration.Domain.Api
{
    public class TvMazeApiClient : ITvMazeApiClient
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _client;
        private readonly ApiClientConfiguration _config;

        public TvMazeApiClient(HttpClient client, IMapper mapper, ApiClientConfiguration config)
        {
            _client = client;
            _mapper = mapper;
            _config = config;
        }

        public async Task<(bool IsRateLimitExceed, Dictionary<int, int> Data)> GetUpdateListAsync(CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(_config.UpdateEndpoint, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<Dictionary<int, int>>(cancellationToken).ConfigureAwait(false);
                return (false, data);
            }

            if (response.IsRateLimitExceed())
            {
                return (true, null);
            }

            response.EnsureSuccessStatusCode();
            return (false, null);
        }

        public async Task<(bool IsRateLimitExceed, IShow Data)> GetShowInfoAsync(int id, CancellationToken cancellationToken)
        {
            var response = await _client.GetAsync(string.Format(_config.ShowEndpoint, id), cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<TvMazeShow>(cancellationToken);
                return (false, _mapper.Map<Show>(data));
            }

            if (response.IsRateLimitExceed())
            {
                return (true, null);
            }

            response.EnsureSuccessStatusCode();
            return (false, null);
        }
    }
}