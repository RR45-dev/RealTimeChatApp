using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
 // Ensure to include the necessary namespace for your ApplicationDbContext
using Microsoft.EntityFrameworkCore;

public class ChatHub : Hub
{
    private readonly ApplicationDbContext _context;

    public ChatHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SendMessage(string sender, string receiver, string message)
    {
        try
        {
            if (string.IsNullOrEmpty(sender) || string.IsNullOrEmpty(receiver) || string.IsNullOrEmpty(message))
            {
                // Handle invalid parameters
                Console.WriteLine("Invalid parameters passed to SendMessage.");
                return;
            }


            // Save the message to the database
            var newMessage = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            // Add to the database
            _context.Messages.Add(newMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
        }
    }
}
