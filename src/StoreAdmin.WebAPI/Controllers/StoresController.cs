using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreAdmin.Core.BusinessInterfaces;
using StoreAdmin.Core.Models;

namespace StoreAdmin.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoresController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpGet]
        public IActionResult GetAllStores()
        {
            var stores = _storeService.GetAllStores();
            return Ok(stores);
        }

        [HttpGet("{id}")]
        public IActionResult GetStoreById(int id)
        {
            var store = _storeService.GetStoreById(id);
            if (store == null)
            {
                return NotFound();
            }
            return Ok(store);
        }

        [HttpPost]
        public IActionResult CreateStore(Store store)
        {
            var newStore = _storeService.CreateStore(store);
            return CreatedAtAction(nameof(GetStoreById), new { id = newStore.Id }, newStore);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStore(int id, Store updatedStore)
        {
            var store = _storeService.GetStoreById(id);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            _storeService.UpdateStore(id, updatedStore);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStore(int id)
        {
            var store = _storeService.GetStoreById(id);

            if (store == null)
            {
                return NotFound("Store not found.");
            }

            _storeService.DeleteStore(id);

            return NoContent();
        }
    }
}



