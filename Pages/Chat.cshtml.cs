using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize]
public class ChatModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ChatModel(ApplicationDbContext context)
    {
        _context = context;
    }

    // List of all users (except the logged-in user)
    public List<User> Users { get; set; }

    // Messages between the logged-in user and the selected user
    public List<Message> Messages { get; set; }

    // Selected user to chat with
    [BindProperty(SupportsGet = true)]
    public string SelectedUser { get; set; }

    public async Task OnGetAsync()
    {
        var user = User.Identity.Name;

        // Fetch all users except the logged-in user
        Users = await _context.Users
            .Where(u => u.Username != user)
            .ToListAsync();

        // Fetch messages with the selected user
        if (!string.IsNullOrEmpty(SelectedUser))
        {
            Messages = await _context.Messages
                .Where(m => (m.Sender == user && m.Receiver == SelectedUser) ||
                            (m.Sender == SelectedUser && m.Receiver == user))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }
    }
}
