using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TvMazeScraper.Presentation.Domain;
using TvMazeScraper.Presentation.Entities;

namespace TvMazeScraper.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/shows")]
    [Produces("application/json")]
    public class ShowController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ISortedShowStore _showStore;

        public ShowController(IMapper mapper, ISortedShowStore showStore)
        {
            _mapper = mapper;
            _showStore = showStore;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Show>>> GetAsync(int offset, int limit)
        {
            var rawShowlist = await _showStore.GetAsync(offset, limit);

            if (rawShowlist.Count == 0)
            {
                return new NotFoundResult();
            }

            var showList = _mapper.Map<List<Show>>(rawShowlist);

            return showList;
        }

        //[HttpGet("show/{id}")]
        //public async Task<ActionResult<Show>> GetAsync(int id)
        //{
        //    var rawShow = await _showStore.GetAsync(id);
        //    if (rawShow == null)
        //    {
        //        return new NotFoundResult();
        //    }

        //    var show = _mapper.Map<Show>(rawShow);

        //    return show;
        //}
    }
}