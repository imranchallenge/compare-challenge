using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CompareIt4meChallenge.Models;
using CompareIt4meChallenge.TemperatureConversionService;
using CompareIt4meChallenge.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CompareIt4meChallenge.Controllers
{
    public class HomeController : Controller
    {
        ChallengeDb _db = new ChallengeDb();
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            var model = _db.People.ToList();

            return View(model);
        }
        public ActionResult PersonData()
        {
            var model = _db.People.ToList();

            return PartialView("_PersonRecord", model);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        /// <summary>
        ///  Tempature Service Consume using async
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        public async Task<ViewResult> Contact(string searchTerm = null)
        {
            ViewBag.Message = "Your contact page.";

            TemperatureConversionService.ConvertTemperatureSoapClient service = new ConvertTemperatureSoapClient();

            TemperatureUnit temperature = new TemperatureUnit();
            TemperateVeiw model = new TemperateVeiw();
            double tempinput;

            if (searchTerm != null )
            {
                if (Double.TryParse(searchTerm, out tempinput))
                {
                    var todoItems =
                        await
                            service.ConvertTempAsync(tempinput, TemperatureUnit.degreeFahrenheit,
                                TemperatureUnit.degreeCelsius);

                    model.Temperature = todoItems;
                }
            }

            return View(model);
        }
        /// <summary>
        ///  Challenge 1 Code plus db save
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=challengefile;AccountKey=8QejzHlpvyWXNEg/G3kuPeTfd1BPPbSaIGpAfkbii8OVRMIHLftJcowmaZOofwuWWv4S6dvaplR1baRzDVfnvA==";
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("updloadfiles");
            if (container.CreateIfNotExists())
            {
                // configure container for public access
                var permissions = container.GetPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Container;
                container.SetPermissions(permissions);
            }
            CloudBlockBlob blob = null;

            int count = 0;
            string path = string.Empty;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        blob = container.GetBlockBlobReference(fileName);
                        blob.UploadFromStream(file.InputStream);
                        //file.SaveAs(path);
                        count++;
                    }
                }
            }

            Messages messages = HelperAction.ParseFile(blob.Uri.ToString());

            return Json(new { status = messages.Status, data = messages.Body });
        }
  
        protected override void Dispose(bool disposing)
        {
            if (_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
