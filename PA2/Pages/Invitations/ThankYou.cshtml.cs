using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA2.Data;
using PA2.Models;
using Microsoft.EntityFrameworkCore;


namespace PA2.Pages.Invitations
{
    //Displays the thank you page for the invitation
    public class ThankYouModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public Party Party { get; set; }

        public ThankYouModel(ApplicationDbContext context)
        {
            _context = context;
        }
        //get party details using Party ID 
        public async Task OnGetAsync(int id)
        {
            Party = await _context.Parties.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
