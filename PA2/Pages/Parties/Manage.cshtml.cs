using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PA2.Data;
using PA2.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace PA2.Pages.Parties
{//Manage Party details and invitations
    public class ManageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        //configured email accounts
        private const string FromEmail = "ashmitbrar329@gmail.com";
        private const string FromPassword = "nhic ksdv goin pxjg";  // App password for this account

        public ManageModel(ApplicationDbContext context)
        {
            _context = context;
          
        }

        [BindProperty]
        public Party Party { get; set; }

        public int YesCount { get; set; }
        public int NoCount { get; set; }

        // Retrieve the party by ID and calculate the yes/no response counts
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Party = await _context.Parties.Include(p => p.Invitations)
                                          .FirstOrDefaultAsync(p => p.Id == id);

            if (Party == null)
            {
                return NotFound();
            }

            YesCount = Party.Invitations.Count(i => i.Status == InvitationStatus.RespondedYes);
            NoCount = Party.Invitations.Count(i => i.Status == InvitationStatus.RespondedNo);

            return Page();
        }

        // Handle creating new invitation
        public async Task<IActionResult> OnPostCreateInvitationAsync(string guestName, string guestEmail)
        {
            var invitation = new Invitation
            {
                GuestName = guestName,
                GuestEmail = guestEmail,
                PartyId = Party.Id,
                Status = InvitationStatus.InviteNotSent
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Manage", new { id = Party.Id });
        }

        // Sends invitation to guests who ahvent recieved it yet
        public async Task<IActionResult> OnPostSendInvitationsAsync(int id)
        {
            Party = await _context.Parties.Include(p => p.Invitations)
                                          .FirstOrDefaultAsync(p => p.Id == id);

            if (Party == null)
            {
                return NotFound();
            }

            var invitationsToSend = Party.Invitations
                                         .Where(i => i.Status == InvitationStatus.InviteNotSent)
                                         .ToList();

            foreach (var invitation in invitationsToSend)
            { 
                bool emailSent = await SendInvitationEmail(invitation, Party);
                if (emailSent)
                {
                    invitation.Status = InvitationStatus.InviteSent;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Manage", new { id = Party.Id });
        }
        //Send email to guest with party details
        private async Task<bool> SendInvitationEmail(Invitation invitation, Party party)
        {
            try
            {
                var fromAddress = new MailAddress(FromEmail, "Party Organizer");
                var toAddress = new MailAddress(invitation.GuestEmail, invitation.GuestName);
            

                string subject = $"You're invited to {party.Description}!";
                string body = $@"
                    <p>Hello {invitation.GuestName},</p>
                    <p>You have been invited to <b>{party.Description}</b> at <b>{party.Location}</b> on <b>{party.EventDate}</b>.</p>
                    <p>We would love to have you! Let us know if you can make it.</p>
                    <p><a href='https://localhost:7066/Invitations/Respond/{invitation.PartyId}?invitationId={invitation.Id}'>Click here to RSVP</a></p>
                    <p>Best, <br> Party Organizer</p>";
                Console.WriteLine("Attempting to connect to SMTP server...");
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,  
                    EnableSsl = true,  
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(FromEmail, FromPassword),
                    Timeout = 20000


                };
              
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    Console.WriteLine($"Sending email to {invitation.GuestEmail}...");
                    await smtp.SendMailAsync(message);
                    Console.WriteLine($"Email successfully sent to {invitation.GuestEmail}");
                }

                return true; // Email sent successfully
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"SMTP error while sending email to {invitation.GuestEmail}: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email to {invitation.GuestEmail}: {ex.ToString()}");
                return false;
            }
        }
    }
}