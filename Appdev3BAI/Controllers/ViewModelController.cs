using Appdev3BAI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev3BAI.Controllers
{
    public class ViewModelController : Controller
    {
        // GET: ViewModel
        //public List<ViewModelBlobs> GetPhotos(string containername)
        //{
        //    var container = GetBlobContainer(containername);

        //    var returnList = new List<ViewModelBlobs>();

        //    if (container.ListBlobs(null, false).Count() > 0)
        //    {
        //        foreach (var item in container.ListBlobs(null, false))
        //        {
        //            if (item.GetType() == typeof(CloudBlockBlob))
        //            {
        //                var blob = (CloudBlockBlob)item;

        //                returnList.Add(new ViewModelBlobs()
        //                {
        //                    Name = blob.Name,
        //                    URI = blob.Uri.ToString()
        //                }
        //                    );
        //            }
        //            else if (item.GetType() == typeof(CloudPageBlob))
        //            {
        //                var pageBlob = (CloudPageBlob)item;

        //                returnList.Add(new ViewModelBlobs()
        //                {
        //                    // Information that is on the index page
        //                    Name = pageBlob.Name,
        //                    URI = pageBlob.Uri.ToString()
        //                }
        //                );

        //            }
        //        }
        //    }
        //    return returnList;
        //}

    }
}