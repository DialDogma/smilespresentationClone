using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Hosting;

namespace SmileClaimV2.Models
{
    public class DownloadFiles
    {
        public List<DownLoadFileInformation> GetFilesInfo()
        {
            var lstFiles = new List<DownLoadFileInformation>();
            var dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/MyFile").Normalize());
            var i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                //if ((item.Attributes & FileAttributes.Archive) != 0 && (item.Attributes & FileAttributes.ReadOnly) != 0)
                //{
                lstFiles.Add(new DownLoadFileInformation
                {
                    FileId = i + 1,
                    FileName = item.Name,
                    FilePath = dirInfo.FullName + @"\" + item.Name
                });
                i = i + 1;
                //}
            }
            return lstFiles;
        }
    }
}