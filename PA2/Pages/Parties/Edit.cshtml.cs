using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PA2.Data;
using PA2.Models;
using System.Threading.Tasks;

namespace PA2.Pages.Parties
{ //Handles edit requests SS
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Party Party { get; set; }

        // GET request to retrieve the existing party
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Party = await _context.Parties.FindAsync(id);
            if (Party == null)
            {
                return NotFound();
            }
            return Page();
        }

        // POST request to save changes
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Party).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
