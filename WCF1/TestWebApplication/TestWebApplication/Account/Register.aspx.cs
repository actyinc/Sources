using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using TestWebApplication.Models;
using System.Collections.Generic;

namespace TestWebApplication.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            ////var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ////var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            ////var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
            ////IdentityResult result = manager.Create(user, Password.Text);
            ////if (result.Succeeded)
            ////{
            ////    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            ////    //string code = manager.GenerateEmailConfirmationToken(user.Id);
            ////    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
            ////    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

            ////    signInManager.SignIn( user, isPersistent: false, rememberBrowser: false);
            ////    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            ////}
            ////else 
            ////{
            ////    ErrorMessage.Text = result.Errors.FirstOrDefault();
            ////}
            var dbCon = new DBConnector();
            if (!dbCon.CheckUserName(Email.Text))
                return;

            var user = new User() { DOB = DateOfBirth.Text, Email = Email.Text, Password = Password.Text, DisplayName = DisplayName.Text, Country = Country.Text, ZipCode = ZipCode.Text,
            CreatedDate = DateTime.Now.ToString(), Preferences = new UserPreferences() { InterestedLocales = new List<string>() {Country.Text +"/"+ ZipCode.Text}}};
            
            var res = dbCon.CreateUserInDB(user);
        }

        protected void AddUserPreference_Click(object sender, EventArgs e)
        {
            UpdateUserPreference(sender, e, true);
        }

        protected void RemoveUserPreference_Click(object sender, EventArgs e)
        {
            UpdateUserPreference(sender, e, false);
        }

        private void UpdateUserPreference(object sender, EventArgs e, bool isAdd)
        {
            var dbCon = new DBConnector();
            if (dbCon.CheckUserName(Email.Text))
                return;

            var user = new User()
            {
                DOB = DateOfBirth.Text,
                Email = Email.Text,
                Password = Password.Text,
                DisplayName = DisplayName.Text,
                Country = Country.Text,
                ZipCode = ZipCode.Text,
                CreatedDate = DateTime.Now.ToString(),
                Preferences = new UserPreferences() { InterestedLocales = new List<string>() { Country.Text + "/" + ZipCode.Text } }
            };

            var res = dbCon.UpdateUserPreference(user, isAdd);
        }


        protected void UploadProfilePicture(object sender, EventArgs e)
        {
            if (!ProfilePictureUpload.HasFile)
                return;
            
            HttpPostedFile postedFile = ProfilePictureUpload.PostedFile;
            byte[] fileData = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(fileData, 0, postedFile.ContentLength);
            UploadProfilePicture uploadedFile = new UploadProfilePicture() { UserId = Email.Text, Data = fileData, FileName = postedFile.FileName, ContentLength = postedFile.ContentLength, ContentType = postedFile.ContentType };
            var dbCon = new DBConnector();
            dbCon.UpdateUserPicture_MultiPartFormData(uploadedFile);
            //dbCon.UpdateUserPicture_Json(uploadedFile);
        }
    }
}