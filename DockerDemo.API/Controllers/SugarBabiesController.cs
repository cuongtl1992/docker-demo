using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DockerDemo.API.Infras.Persistence;
using DockerDemo.API.Models;
using DockerDemo.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DockerDemo.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SugarBabiesController : ControllerBase
    {
        private readonly DockerDemoDbContext _dbContext;

        public SugarBabiesController(DockerDemoDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public async Task<GetSugarBabiesResp> GetAsync([FromQuery] GetSugarBabiesReq req)
        {
            var query = _dbContext.SugarBabies.AsQueryable();

            if (!string.IsNullOrEmpty(req.Keyword))
                query = query.Where(s => s.Name.Contains(req.Keyword));

            var total = await query.CountAsync();
            var data = await query.Skip(req.Skip).Take(req.Take).ToListAsync();

            return new GetSugarBabiesResp
            {
                Data = data,
                Total = total
            };
        }

        [HttpGet("{id}")]
        public async Task<SugarBaby> GetSingleById([FromRoute] int id)
        {
            var existBaby = await _dbContext.SugarBabies.FirstOrDefaultAsync(s => s.Id == id);
            if (existBaby == null)
                throw new InvalidDataException($"Sugar baby không tồn tại trong hệ thống");

            return existBaby;
        }
        
        [HttpPost]
        public async Task<SugarBaby> CreateAsync([FromBody] CreateSugarBabyReq req)
        {
            if (await _dbContext.SugarBabies.AnyAsync(s => s.Name.Equals(req.Name)))
                throw new InvalidDataException($"Sugar baby {req.Name} đã tồn tại trong hệ thống");
            
            var baby = new SugarBaby
            {
                Name = req.Name,
                Age = req.Age
            };

            await _dbContext.SugarBabies.AddAsync(baby);
            await _dbContext.SaveChangesAsync();
            
            return baby;
        }

        [HttpPut("{id}")]
        public async Task<SugarBaby> UpdateAsync([FromRoute] int id,[FromBody] UpdateSugarBabyReq req)
        {
            var existBaby = await _dbContext.SugarBabies.FirstOrDefaultAsync(s => s.Id == id);
            if (existBaby == null)
                throw new InvalidDataException($"Sugar baby {req.Name} không tồn tại trong hệ thống");
            
            if (await _dbContext.SugarBabies.AnyAsync(s => s.Name.Equals(req.Name) && req.Id != id))
                throw new InvalidDataException($"Sugar baby {req.Name} đã tồn tại trong hệ thống");

            existBaby.Name = req.Name;
            existBaby.Age = req.Age;

            _dbContext.SugarBabies.Update(existBaby);
            await _dbContext.SaveChangesAsync();
            return existBaby;
        }

        [HttpDelete("{id}")]
        public async Task DeleteAsync([FromRoute] int id)
        {
            var existBaby = await _dbContext.SugarBabies.FirstOrDefaultAsync(s => s.Id == id);
            if (existBaby == null)
                throw new InvalidDataException($"Sugar baby không tồn tại trong hệ thống");

            _dbContext.SugarBabies.Remove(existBaby);
            await _dbContext.SaveChangesAsync();
        }
    }
}