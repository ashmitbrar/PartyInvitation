using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PA2.Data;
using PA2.Models;
using System.Threading.Tasks;

namespace PA2.Pages.Parties
{ //Handle Part craetion 
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        //Initialize a new instance 
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Party Party { get; set; }
        //handle form submission 
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Parties.Add(Party);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
