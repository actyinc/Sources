using System;
using System.IO;
using System.Text;
using Acty;
using Acty.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace DocumentDBServiceTest
{
    public class Helper
    {
        public static byte[] ConvertFileToByteArray(string path, long length)
        {
            try
            {
                byte[] buffer = null;
                var fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                long tb = length;
                buffer = br.ReadBytes((int)length);
                fs.Close();
                fs.Dispose();
                br.Close();
                return buffer;
            }
            catch
            {
                return null;
            }
        }

        public static async void DeleteUserIfExists(User user, UsersController usersController)
        {
            await  usersController.DeleteUserTest666(user.Email);
        }

        public static void CheckUserExists(User user, UsersController usersController)
        {
            var dbUser = usersController.GetUser(user.Email);
            Assert.AreEqual(user.Country, dbUser.Country);
            Assert.AreEqual(user.DisplayName, dbUser.DisplayName);
            Assert.AreEqual(user.DOB, dbUser.DOB);
            Assert.AreEqual(user.Email, dbUser.Email);
            Assert.AreEqual(user.Password, dbUser.Password);
            Assert.AreEqual(user.ZipCode, dbUser.ZipCode);
            //Assert.AreEqual(user.Preferences.InterestedLocales.Count, dbUser.Preferences.InterestedLocales.Count);
        }

        public static Stream ConvertObjectToStream(object obj)
        {
            try
            {
                var jsonObj = JsonConvert.SerializeObject(obj);
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonObj));
                ////System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                ////bf.Serialize(stream, jsonObj);
                return stream;
            }
            catch (Exception ex)
            {
                Assert.IsFalse(true, ex.Message);
            }

            return null;
        }
    }
}
