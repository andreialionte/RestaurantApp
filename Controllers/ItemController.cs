using Microsoft.AspNetCore.Mvc;
using RestaurantApp.Data;
using RestaurantApp.DTOs;
using RestaurantApp.Models;
using RestaurantApp.Repository.IRepository;
using RestaurantApp.Services;

namespace RestaurantApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IUploadService _uploadService;
        private readonly IItemRepository _itemRepository;

        public ItemController(DataContextEf ef, IUploadService uploadService, IItemRepository itemRepository)
        {
            _ef = ef;
            _uploadService = uploadService;
            _itemRepository = itemRepository;
        }

        [HttpGet("GetItems")]
        public IActionResult GetItems()
        {
            IEnumerable<Item> itemsDb = _itemRepository.GetAll();
            return Ok(itemsDb);
        }

        [HttpGet("GetSingleItems")]
        public IActionResult GetSingleItems(int itemId)
        {
            Item? itemDb = _itemRepository.Get(itemId);
            if (itemDb != null)
            {
                throw new Exception($"The item id {itemDb.ItemId} it was not found");
            }
            return Ok(itemDb);
        }

        [HttpPost("AddItems")]
        public async Task<IActionResult> AddItems([FromForm] ItemDto itemDto, IFormFile file)
        {
            string photoUrl = await _uploadService.Upload(file);

            var existingItem = _itemRepository.GetByName(itemDto.ItemName);

            if (existingItem != null)
            {
                throw new Exception($"This item {existingItem.ItemName} is already in use");
            }

            Item newItem = new Item()
            {
                ItemName = itemDto.ItemName,
                Description = itemDto.Description,
                Price = itemDto.Price,
                PhotoUrl = photoUrl
            };

            _itemRepository.Add(newItem);
            _itemRepository.Save();

            return Ok(newItem);
        }

        [HttpPut("UpdateItems")]
        public IActionResult UpdateItems(ItemDto itemDto, int itemId)
        {
            Item? itemDb = _itemRepository.Get(itemId);
            itemDb.ItemName = itemDto.ItemName;
            itemDb.Price = itemDto.Price;
            itemDb.Description = itemDto.Description;

            if (itemDb != null)
            {
                _itemRepository.Update(itemDb);
                _itemRepository.Save();
                throw new Exception($"The item id {itemDb.ItemId} dosent exists");
            }

            return Ok(itemDb);

        }

        [HttpDelete("DeleteItems")]
        public IActionResult DeleteItems(int itemId)
        {
            Item? itemDb = _itemRepository.Get(itemId);
            if (itemDb != null)
            {
                _itemRepository.Remove(itemDb);
                _itemRepository.Save();
            }
            return Ok(itemDb);
        }
    }
}
