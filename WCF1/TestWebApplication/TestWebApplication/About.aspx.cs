using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestWebApplication
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CreateStory_Click(object sender, EventArgs e)
        {
            var dbCon = new DBConnector();
            if (dbCon.CheckUserName(OwnerId.Text) || !MediaUpload.HasFile)
                return;

            HttpPostedFile postedFile = MediaUpload.PostedFile;
            byte[] fileData = new byte[postedFile.ContentLength];
            postedFile.InputStream.Read(fileData, 0, postedFile.ContentLength);

            var story = new Campaign()
            {
                Country = Country.Text,
                ZipCode = ZipCode.Text,
                CreatedDate = CreatedDate.Text,
                OwnerId = OwnerId.Text,
                Heading = Heading.Text,
                Category = Category.SelectedItem.Value,
                IsLocal = IsLocal.SelectedItem.Value == "true",
                KeyWords = Keywords.Text.Split(',').Select(kw => kw.Trim()).ToArray(),
                Message = Message.Text,
                StoryVisualResource = new CampaignMedia() { ContentLength = postedFile.ContentLength, ContentType = postedFile.ContentType, Data = fileData, FileName = postedFile.FileName, UserId = OwnerId.Text }
            };

            var res = dbCon.CreateStory_MultiPartFormData(story);
        }
    }
}