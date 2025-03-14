using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PA2.Data;
using PA2.Models;
using System.Threading.Tasks;

namespace PA2.Pages.Invitations
{ //Hnaldes respons eto party invites 
    public class RespondModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RespondModel(ApplicationDbContext context)
        {
            _context = context;
        }
      
        [BindProperty]
        public Party Party { get; set; }

        [BindProperty]
        public Invitation Invitation { get; set; }


        [BindProperty]
        public string Attending { get; set; }
        //Retrieves part and invitation details on ID 
        public async Task<IActionResult> OnGetAsync(int id, int? invitationId)
        {
            Console.WriteLine($"Current User: {User.Identity.Name}");
            //Get party details with invitations
            Party = await _context.Parties.Include(p => p.Invitations)
                                          .FirstOrDefaultAsync(p => p.Id == id);

            if (Party == null)
            {
                return NotFound();
            }
            // Assign Invitation only if invitationId is provided
            if (invitationId.HasValue)
            {
                Invitation = Party.Invitations.FirstOrDefault(i => i.Id == invitationId.Value);
                if (Invitation == null)
                {
                    return NotFound(); 
                }
            }
            else
            {
                return BadRequest("Missing invitation ID.");
            }

            return Page();
        }
        //handles users respponse to party invite
        public async Task<IActionResult> OnPostAsync(int id, int invitationId)
        {
            
            Console.WriteLine($"Current User: {User.Identity.Name}");
            //Retrieve specific invitation 
            Invitation = await _context.Invitations
                             .FirstOrDefaultAsync(i => i.Id == invitationId && i.PartyId == id);

            if (Invitation == null)
            {
                return NotFound();
            }


            // Update invitation status based on user's response
            Invitation.Status = Attending == "Yes" ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;

            await _context.SaveChangesAsync();

            // Redirecting to ThankYou page after submission
            return RedirectToPage("./ThankYou", new { id = id });
        }
    }
}
