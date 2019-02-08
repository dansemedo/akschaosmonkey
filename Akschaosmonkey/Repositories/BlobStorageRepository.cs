using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Akschaosmonkey.Repositories
{
    class BlobStorageRepository
    {

        public async Task<System.IO.Stream> GetKubeConfigFile()
        {
           var connectionstring = Environment.GetEnvironmentVariable("azstorageconnectionstring");
         //var storageConnectionString = "your connection string";

            CloudStorageAccount account = CloudStorageAccount.Parse(connectionstring);
            CloudBlobClient serviceClient = account.CreateCloudBlobClient();

            var container = serviceClient.GetContainerReference("kubeconfig");

            var blobName = container.GetBlockBlobReference("config.txt");

            var blobcontent = await blobName.OpenReadAsync();

            return blobcontent;
        }

    }
}
