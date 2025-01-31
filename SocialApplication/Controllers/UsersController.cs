using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApplication.Entities;

namespace SocialApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController: ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsrs()
        {
            var users = await _context.Users.ToListAsync();
            if(users==null)
            {
                return NotFound();
            }
            return Ok(users);

        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user!=null)
            {
                return Ok(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(AppUser user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok();    

        }

    }
}
