using InstagramApiSharp.API;
using InstagramApiSharp.API.Builder;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Logger;

namespace InstagramBot
{
    public partial class Form1 : Form
    {
        private static UserSessionData _user;
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            _user = new UserSessionData();

            string userName = textBox1.Text;
            string password = textBox2.Text;

            _user.UserName = userName;
            _user.Password = password;
            apiclass.api = InstaApiBuilder.CreateBuilder()
                .SetUser(_user)
                .SetRequestDelay(RequestDelay.FromSeconds(0, 1))
                .Build();
            var isLog = await apiclass.api.LoginAsync();
            if (isLog.Succeeded)
            {
                MessageBox.Show("Success");
                DeletePhoto();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        public async void UploadPhoto()
        {
            var image = new InstaImage { Uri = @"C:\minion.jpg" };

            var result = await apiclass.api.StoryProcessor.UploadStoryPhotoAsync(image, "someawesomepicture");
            MessageBox.Show(result.Succeeded
                ? $"Story created: {result.Value.Media.Pk}"
                : $"Unable to upload photo story: {result.Info.Message}");
        }
        public async void DeletePhoto()
        {
            var result = await apiclass.api.StoryProcessor.GetUserStoryFeedAsync(apiclass.api.GetLoggedUser().LoggedInUser.Pk);
            foreach (var item in result.Value.Items)
            {
                await apiclass.api.StoryProcessor.DeleteStoryAsync(item.Pk.ToString());

            }
        }
    }
}