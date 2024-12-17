using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using Microsoft.Maui.ApplicationModel.Communication;

public class EmailService
{
    private readonly string _apiKey;
    private string _verificationCode;
    private string _email;
    public EmailService(string email)
    {
        _email = email;
    }

    public async Task SendEmail(string _email, string subject, string message)
    {
        //_verificationCode = await GenerateCode();
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("noreply.grandcaffe@gmail.com");
        mail.To.Add(_email);
        mail.Subject = subject;
        mail.Body = message;

        // Configure the SMTP client to use Gmail
        using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("noreply.grandcaffe@gmail.com", "wemd viez bytp ocwr");
            try
            {
                client.Send(mail);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to senZd email. Error: " + ex.Message);
            }
        }
    }
    public async Task<string> GenerateCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
