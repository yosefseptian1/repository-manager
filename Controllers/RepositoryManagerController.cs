using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepositoryManager.Helpers;

namespace backend_cis_dmc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly RepositoryManagerHelper _repositoryManagerHelper;

        public PersonController(AppDbContext context, IConfiguration config, RepositoryManagerHelper repositoryManagerHelper)
        {
            _context = context;
            _config = config;
            this._repositoryManagerHelper = repositoryManagerHelper;
        }

        [HttpPost("RegisterRepository")]
        public async Task<IActionResult> RegisterRepository(string itemName, string itemContent, short repoType)
        {
            await this._repositoryManagerHelper.Register(itemName, itemContent, repoType);
            return Ok(new
            {
                responseResult = true,
                message = "Add repository successfully!",
                data = 0
            });
        }
        
        [HttpGet("RetrieveRepository/{itemName}")]
        public async Task<IActionResult> RetrieveRepository(string itemName)
        {
            var data = this._repositoryManagerHelper.Retrieve(itemName);
            return Ok(new
            {
                responseResult = true,
                message = "Retrieve repository successfully!",
                data = data
            });
        }
        
        [HttpGet("GetTypeRepository/{itemName}")]
        public async Task<IActionResult> GetTypeRepository(string itemName)
        {
            var data = this._repositoryManagerHelper.GetType(itemName);
            var result = new
            {
                typeReposiry = data
            };
            return Ok(new
            {
                responseResult = true,
                message = "Get type repository successfully!",
                data = result
            });
        }
        
        [HttpDelete("Deregister/{itemName}")]
        public async Task<IActionResult> Deregister(string itemName)
        {
            this._repositoryManagerHelper.Deregister(itemName);
            return Ok(new
            {
                responseResult = true,
                message = "Deregister repository successfully!",
                data = 0
            });
        }
    }
}