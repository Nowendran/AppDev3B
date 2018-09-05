using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Appdev3BAI.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Appdev3BAI.Models
{
    public class AppDevBusiness
    {
        public void UploadPhoto(string containername, HttpPostedFileBase file, string studentnumber)
        {
            var container = GetBlobContainer(containername);

            //Student Number reference
            var fileName = studentnumber ;

            var blockBlob = container.GetBlockBlobReference(fileName);

            blockBlob.UploadFromStream(file.InputStream);
        }
        public void DeletePhoto(string containername, string id)
        {

            CloudBlobContainer container = GetBlobContainer(containername);
            CloudBlockBlob blockblob = container.GetBlockBlobReference(id);
            blockblob.Delete();

        }

        public async Task<List<ViewModelBlobs>> GetPhotos(string containername)
        {
            var container = GetBlobContainer(containername);

            var returnList = new List<ViewModelBlobs>();

            if (container.ListBlobs(null, false).Count() > 0)
            {
                foreach (var item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        var blob = (CloudBlockBlob)item;
                        Uri a = blob.Uri;
                        returnList.Add(new ViewModelBlobs()
                        {
                            Name = blob.Name,
                            URI = blob.Uri.ToString(),
                            details = await detailsPic(a)

                        }
                            );
                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {
                        var pageBlob = (CloudPageBlob)item;
                        Uri a = pageBlob.Uri;
                        returnList.Add(new ViewModelBlobs()
                        {
                            // Information that is on the index page
                            Name = pageBlob.Name,
                            URI = pageBlob.Uri.ToString(),
                            details = await detailsPic(a)
                        }
                        );

                    }
                }
            }
            return returnList;
        }


        private static CloudBlobContainer GetBlobContainer(string containername)
        {

            var storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containername);

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            };

            container.CreateIfNotExists();

            container.SetPermissions(permissions);

            return container;
        }




        public async Task<string> detailsPic(Uri a)
        {
            string contentString = "";
            try
            {
                const string subscriptionKey = "b5a75ee83d4e4b03abdf5e9c12b45e48";
                const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/analyze";
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters ="visualFeatures=Categories,Description,Color";
                string uri = uriBase + "?" + requestParameters;
                HttpResponseMessage response;
                //byte[] byteData = ConvertToBytes(a.OriginalString);
                var webclient = new WebClient();
                byte[] byteData = webclient.DownloadData(a);
                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(uri, content);
                }

                contentString = await response.Content.ReadAsStringAsync();

                contentString = ("\nResponse:\n\n{0}\n" + JToken.Parse(contentString).ToString());
                contentString = Regex.Replace(contentString, @"[^}-{-,-]", "/n");


            }
            catch (Exception e)
            {
                //string contentString = "No details found on this pic";
            }

            return contentString;
        }

      

    }
}