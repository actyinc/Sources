using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestWebApplication
{
    public class DBConnector
    {
        public bool CreateUserInDB(User user)
        {
            bool retVal = false;
            try
            {
                string url = "http://localhost:62584/ActiDataService.svc/CreateUserProfile";
                //string url = "http://acty.azurewebsites.net/ActiDataService.svc/CreateUserProfile";
                // declare ascii encoding
                ASCIIEncoding encoding = new ASCIIEncoding();
                string strResult = string.Empty;

                string postData = JsonConvert.SerializeObject(user);
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                
                retVal = strResult == "true";
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public bool UpdateUserPreference(User user, bool isAdd)
        {
            bool retVal = false;
            try
            {
                string url = isAdd ? "http://localhost:62584/ActiDataService.svc/AddUserPreference" : "http://localhost:62584/ActiDataService.svc/RemoveUserPreference";
                //string url = isAdd ? "http://acty.azurewebsites.net/ActiDataService.svc/AddUserPreference" : "http://acty.azurewebsites.net/ActiDataService.svc/RemoveUserPreference";
                // declare ascii encoding
                ASCIIEncoding encoding = new ASCIIEncoding();
                string strResult = string.Empty;

                string postData = JsonConvert.SerializeObject(user);
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();

                retVal = strResult == "true";
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public bool CreateStoryInDB(Campaign story)
        {
            bool retVal = false;
            try
            {
                string url = "http://localhost:62584/ActiDataService.svc/CreateCampaign";
                //string url = "http://acty.azurewebsites.net/ActiDataService.svc/CreateCampaign";

                // declare ascii encoding
                ASCIIEncoding encoding = new ASCIIEncoding();
                string strResult = string.Empty;

                string postData = JsonConvert.SerializeObject(story);
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();

                retVal = strResult == "true";
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public bool CheckUserName(string userName)
        {
            bool retVal = false;
            try
            {
                // Restful service URL
                string url = string.Format("http://localhost:62584/ActiDataService.svc/IsUserNameAvailable/userName/{0}", userName);
                //string url = string.Format("http://acty.azurewebsites.net/ActiDataService.svc/IsUserNameAvailable/userName/{0}", userName);
                string strResult = string.Empty;
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "GET";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // assign the final result to text box
                retVal = strResult == "true";
            }
            catch (Exception ex)
            {
                return false;
            }

            return retVal;
        }

        public DBUser GetUser(string userName)
        {
            DBUser retVal = null;
            try
            {
                // Restful service URL
                string url = string.Format("http://localhost:62584/ActiDataService.svc/GetUser/userName/{0}", userName);
                //string url = string.Format("http://acty.azurewebsites.net/ActiDataService.svc/GetUser/userName/{0}", userName);
                string strResult = string.Empty;
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "GET";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // assign the final result to text box
                retVal = JsonConvert.DeserializeObject<DBUser>(strResult);
            }
            catch (Exception ex)
            {
                return null;
            }

            return retVal;
        }

        public IEnumerable<DBStory> GetCampaignsForUser(string userName)
        {
            IEnumerable<DBStory> retVal = null;
            try
            {
                // Restful service URL
                string url = string.Format("http://localhost:62584/ActiDataService.svc/GetCampaignsForUser/userName/{0}", userName);
                //string url = string.Format("http://acty.azurewebsites.net/ActiDataService.svc/GetCampaignsForUser/userName/{0}", userName);
                string strResult = string.Empty;
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "GET";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();
                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                // assign the final result to text box
                retVal = JsonConvert.DeserializeObject<IEnumerable<DBStory>>(strResult);
            }
            catch (Exception ex)
            {
                return null;
            }

            return retVal;
        }

        public bool UpdateUserPicture_Json(UploadProfilePicture profilePic)
        {
            bool retVal = false;
            try
            {
                string url = "http://localhost:62584/ActiDataService.svc/UpdateUserPicture";
                //string url = "http://acty.azurewebsites.net/ActiDataService.svc/UpdateUserPicture";

                // declare ascii encoding
                ASCIIEncoding encoding = new ASCIIEncoding();
                string strResult = string.Empty;

                string postData = JsonConvert.SerializeObject(profilePic);
                // convert xmlstring to byte using ascii encoding
                byte[] data = encoding.GetBytes(postData);
                // declare httpwebrequet wrt url defined above
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(url);
                // set method as post
                webrequest.Method = "POST";
                // set content type
                webrequest.ContentType = "application/x-www-form-urlencoded";
                // set content length
                webrequest.ContentLength = data.Length;
                // get stream data out of webrequest object
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // declare & read response from service
                HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse();

                // set utf8 encoding
                Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
                // read response stream from response object
                StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
                // read string from stream data
                strResult = loResponseStream.ReadToEnd();
                // close the stream object
                loResponseStream.Close();
                // close the response object
                webresponse.Close();
                retVal = strResult == "true";
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public bool UpdateUserPicture_MultiPartFormData(UploadProfilePicture profilePic)
        {
            bool retVal = false;
            try
            {
                string url = "http://localhost:62584/ActiDataService.svc/UpdateUserPicture";
                //string url = "http://acty.azurewebsites.net/ActiDataService.svc/UpdateUserPicture";

                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("UserId", profilePic.UserId);
                postParameters.Add("FileName", profilePic.FileName);
                postParameters.Add("ContentType", profilePic.ContentType);
                postParameters.Add("ContentLength", profilePic.ContentLength);
                postParameters.Add("File", new FormUpload.FileParameter(profilePic.Data, profilePic.FileName, profilePic.ContentType));

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(url, "Activist", postParameters);

                // Process response
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public bool CreateStory_MultiPartFormData(Campaign story)
        {
            bool retVal = false;
            try
            {
                string url = "http://localhost:62584/ActiDataService.svc/CreateCampaign";
                //string url = "http://acty.azurewebsites.net/ActiDataService.svc/CreateCampaign";

                // Generate post objects
                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                postParameters.Add("OwnerId", story.OwnerId);
                postParameters.Add("CreatedDate", story.CreatedDate);
                postParameters.Add("Heading", story.Heading);
                postParameters.Add("Category", story.Category);
                postParameters.Add("Message", story.Message);
                postParameters.Add("IsLocal", story.IsLocal);
                postParameters.Add("ZipCode", story.ZipCode);
                postParameters.Add("Country", story.Country);
                postParameters.Add("Keywords", string.Join(",",story.KeyWords));
                postParameters.Add("FileName", story.StoryVisualResource.FileName);
                postParameters.Add("ContentType", story.StoryVisualResource.ContentType);
                postParameters.Add("ContentLength", story.StoryVisualResource.ContentLength);
                postParameters.Add("File", new FormUpload.FileParameter(story.StoryVisualResource.Data, story.StoryVisualResource.FileName, story.StoryVisualResource.ContentType));

                HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(url, "Activist", postParameters);

                // Process response
                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                string fullResponse = responseReader.ReadToEnd();
                webResponse.Close();
            }
            catch (Exception ex)
            {
                retVal = ex.Message != string.Empty;
            }

            return retVal;
        }

        public static class FormUpload
        {
            private static readonly Encoding encoding = Encoding.UTF8;
            public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
            {
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
                string contentType = "multipart/form-data; boundary=" + formDataBoundary;

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

                return PostForm(postUrl, userAgent, contentType, formData);
            }
            private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
            {
                HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

                if (request == null)
                {
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.CookieContainer = new CookieContainer();
                request.ContentLength = formData.Length;
                
                // You could add authentication here as well if needed:
                // request.PreAuthenticate = true;
                // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                // request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("username" + ":" + "password")));

                // Send the form data to the request.
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }

                return request.GetResponse() as HttpWebResponse;
            }

            private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
            {
                Stream formDataStream = new System.IO.MemoryStream();
                bool needsCLRF = false;

                foreach (var param in postParameters)
                {
                    // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                    // Skip it on the first parameter, add it to subsequent parameters.
                    if (needsCLRF)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsCLRF = true;

                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;

                        // Add just the first part of this param, since we will write the file data directly to the Stream
                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                            boundary,
                            param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.ContentType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                        // Write the file data directly to the Stream, rather than serializing it to a string.
                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();

                return formData;
            }

            public class FileParameter
            {
                public byte[] File { get; set; }
                public string FileName { get; set; }
                public string ContentType { get; set; }
                public FileParameter(byte[] file) : this(file, null) { }
                public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
                public FileParameter(byte[] file, string filename, string contenttype)
                {
                    File = file;
                    FileName = filename;
                    ContentType = contenttype;
                }
            }
        }
    }
}