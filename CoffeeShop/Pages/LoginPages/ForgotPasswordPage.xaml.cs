namespace CoffeeShop;

public partial class ForgotPasswordPage : ContentPage
{
	public ForgotPasswordPage()
	{
		InitializeComponent();
	}

    private async void OnContinueClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
        {
            DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        DisplayAlert("Success", "A verification code has been sent to your email.", "OK");
        await Navigation.PushAsync(new VerificationPage("Forgot", email));
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    //private void SendVerificationCode(string email)
    //{
    //    Console.WriteLine($"Verification code sent to {email}");
    //}
}
