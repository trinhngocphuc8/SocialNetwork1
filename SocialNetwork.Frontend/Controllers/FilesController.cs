using Ionic.Zip;
using SocialNetwork.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileModel = SocialNetwork.Domain.FileModel;

namespace SocialNetwork.Frontend.Controllers
{
    public class FilesController : Controller
    {
        // GET: DownloadFiles
        public ActionResult Download()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/UploadedFiles/"));
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel()
                {
                    FileName = Path.GetFileName(filePath),
                    FilePath = filePath
                });
            }

            return View(files);
        }

        [HttpPost]
        public ActionResult Download(List<FileModel> files)
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                zip.AddDirectoryByName("UploadedFiles");
                foreach (FileModel file in files)
                {
                    if (file.IsSelected)
                    {
                        zip.AddFile(file.FilePath, "UploadedFiles");
                    }
                }
                string zipName = String.Format("FilesZip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    zip.Save(memoryStream);
                    return File(memoryStream.ToArray(), "application/zip", zipName);
                }
            }


        }
    }
}