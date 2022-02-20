using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ware.Data;

namespace Ware.Controllers
{
    [ApiController]
    [Route("api / warehouses")]
    public class WarehousesController : ControllerBase
    {


        private WarInterface _dbService;

        public WarehousesController(WarInterface service)
        {
            _dbService = service;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllOWarehouses(Prod Id)
        {

            return  Ok(await _dbService.addToWareAsymc(Id));
        }


    }
}
