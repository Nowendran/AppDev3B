using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Appdev3BAI.Models;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Appdev3BAI.Controllers
{

    public class BlobsController : Controller
    {
        // GET: Blobs




        // GET: Blob
        
        public async System.Threading.Tasks.Task<ActionResult> Index(AppDevBusiness business)
        {
            return View(await business.GetPhotos("images"));
        }

        public ActionResult UploadView()
            {
                return View("Upload");
            }
            public ActionResult Upload(PhotoUpload photo, AppDevBusiness appbusiness, string studentnumber)
            {
                

                if (photo.FileUpload != null && photo.FileUpload.ContentLength > 0)
                {
                appbusiness.UploadPhoto("images", photo.FileUpload, studentnumber);
            }
            return RedirectToAction("Index");
            }


            public ActionResult Delete(string id, AppDevBusiness appdevbusiness)
            {
               
                if (ModelState.IsValid)

                {
                appdevbusiness.DeletePhoto("images", id);
                }
                return RedirectToAction("Index");
            }

        public async System.Threading.Tasks.Task<ActionResult> Details(string containername, string id)
        {
            CloudBlobContainer container = GetBlobContainer("images");
            CloudBlockBlob blockblob = container.GetBlockBlobReference(id);
            ViewModelBlobs vm = new ViewModelBlobs();
            AppDevBusiness appbusiness = new AppDevBusiness();
            vm.Name = blockblob.Name;
            vm.details = blockblob.Uri.ToString();
            Uri a = blockblob.Uri;
            vm.details =await appbusiness.detailsPic(a);
            return View(vm);

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
    }
}
