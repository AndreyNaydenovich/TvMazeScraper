using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TvMazeScraper.Presentation.Domain.Services;
using TvMazeScraper.Presentation.Entities;

namespace TvMazeScraper.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1")]
    [Produces("application/json")]
    public class ShowController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IShowService _showStore;

        public ShowController(IMapper mapper, IShowService showStore)
        {
            _mapper = mapper;
            _showStore = showStore;
        }

        [HttpGet("shows")]
        public async Task<ActionResult<IEnumerable<Show>>> GetAsync(int offset, int limit)
        {
            var rawShowlist = await _showStore.GetAsync(offset, limit);

            var showList = _mapper.Map<List<Show>>(rawShowlist);

            if (showList.Count == 0)
            {
                return new NotFoundResult();
            }

            return showList;
        }

        [HttpGet("show/{id}")]
        public async Task<ActionResult<Show>> GetAsync(int id)
        {
            var rawShow = await _showStore.GetAsync(id);

            if (rawShow == null)
            {
                return new NotFoundResult();
            }

            var show = _mapper.Map<Show>(rawShow);

            return show;
        }
    }
}