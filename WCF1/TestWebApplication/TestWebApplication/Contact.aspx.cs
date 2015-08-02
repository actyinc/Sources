using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWebApplication
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GetUser_Click(object sender, EventArgs e)
        {
            var dbCon = new DBConnector();
            if (string.IsNullOrEmpty(Email.Text))
                return;

            var res = dbCon.GetUser(Email.Text);
            DateOfBirth.Text = res.DOB;
            Country.Text = res.Country;
            ZipCode.Text = res.ZipCode;
            DisplayName.Text = res.DisplayName;
            Password.Text = res.Password;
            ProfilePicture.ImageUrl = res.UserProfilePictureBlob;
        }

        protected void GetStories_Click(object sender, EventArgs e)
        {
            var dbCon = new DBConnector();
            if (string.IsNullOrEmpty(Email.Text))
                return;

            var res = dbCon.GetCampaignsForUser(Email.Text);
        }
    }
}