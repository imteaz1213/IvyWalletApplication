using IvyBackend.Data;
using IvyBackend.Models;
using IvyBackend.Models.DTO;
using IvyBackend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;


namespace IvyBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        IHomeRepository _homeRepository;
        
        public HomeController(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrency()
        {
            var data = await _homeRepository.GetCurrency();
            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAccount()
        {
            var data = await _homeRepository.GetAccount();  
            return Ok(data);
        } 

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCategory()
        {
            var data = await _homeRepository.GetCategory(); 
            return Ok(data);    
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest("Name parameter is required");
            }

            var data = await _homeRepository.FindByName(name);

            return Ok(data);
        }

        [HttpGet("{user_id:int}")]
        public async Task<IActionResult> GetUserCurrency(int user_id)
        {
            var userCurrency = await _homeRepository.GetUserCurrency(user_id);

            return Ok(userCurrency);
        }
    }
} 


