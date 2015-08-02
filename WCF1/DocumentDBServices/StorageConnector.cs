using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.IO;

namespace DocumentDBDataService
{
    public static class StorageConnector
    {
        private static CloudStorageAccount _storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);

        public static CloudBlobClient ActiBlobClient = _storageAccount.CreateCloudBlobClient();

        public static CloudBlobContainer GetUsersContainer()
        {
            return GetContainer("users");
        }

        public static CloudBlobContainer GetStoriesContainer()
        {
            return GetContainer("stories");
        }

        private static CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobContainer container = ActiBlobClient.GetContainerReference(containerName);

            if (!container.Exists())
                container.Create(BlobContainerPublicAccessType.Blob);

            return container;
        }

        public static async Task UploadUserData(string uniqueBlobName, string contentType, Stream data)
        {
            var usersContainer = GetUsersContainer();
            var blob = usersContainer.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(data);
        }

        public static async Task DeleteUserProfilePicture(string uniqueBlobName)
        {
            var usersContainer = GetUsersContainer();
            var blob = usersContainer.GetBlockBlobReference(uniqueBlobName);
            await blob.DeleteIfExistsAsync();
        }

        public static async Task UploadStory(string uniqueBlobName, string contentType, Stream data)
        {
            var usersContainer = GetStoriesContainer();
            var blob = usersContainer.GetBlockBlobReference(uniqueBlobName);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(data);
        }
    }
}