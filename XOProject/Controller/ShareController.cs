using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace XOProject.Controller
{
    [Route("api/Share")]
    public class ShareController : ControllerBase
    {
        public IShareRepository _shareRepository;

        public ShareController(IShareRepository shareRepository)
        {
            _shareRepository = shareRepository;
        }

        [HttpPut("{symbol}")]
        public async Task<IActionResult> UpdateLastPrice([FromRoute]string symbol)
        {
            try
            {
                var share = await _shareRepository.FindLastBySymbolAsync(symbol);
                share.Rate = +10;
                await _shareRepository.UpdateAsync(share);

                return NoContent();
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(500, e.Message);
            }
           
        }

        [HttpGet("{symbol}")]
        public async Task<IActionResult> Get([FromRoute]string symbol)
        {
            var shares = await _shareRepository.FindAsync(x => x.Symbol == symbol);
            return Ok(shares);
        }

        [HttpGet("{symbol}/Latest")]
        public async Task<IActionResult> GetLatestPrice([FromRoute]string symbol)
        {
            var share = await _shareRepository.FindLastBySymbolAsync(symbol);
            return Ok(share?.Rate);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Share value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _shareRepository.InsertAsync(value);

                return Created($"Share/{value.Id}", value);
            }
            catch (Exception e)
            {
                throw new HttpStatusCodeException(500, e.Message);
            }
           
        }
        
    }
}
