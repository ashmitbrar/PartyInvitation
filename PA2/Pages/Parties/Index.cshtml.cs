using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PA2.Data;
using PA2.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PA2.Pages.Parties
{ //Handle Homepage request 
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // store the list of parties
        public List<Party> Parties { get; set; }

        public async Task OnGetAsync()
        {
            // Load parties including invitations count
            Parties = await _context.Parties
                .Include(p => p.Invitations)
                .ToListAsync();
        }
    }
}
