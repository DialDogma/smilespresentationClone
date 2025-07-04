using Google.Apis.Services;
using Google.Apis.Urlshortener.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmileSDuration.Helper
{
    public class ShortURL
    {
        public ShortURL()
        {
        }

        /// <summary>
        /// Convert Full URL To Short URL
        /// </summary>
        /// <param name="fullURL"></param>
        /// <returns></returns>
        public static string GetShortURL(string fullURL)
        {
            string result = "";

            UrlshortenerService service = new UrlshortenerService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyBudrSXnkFZcMFT-190Gy8NdcWSPiZ3fDo",
                ApplicationName = "SiamsmileShortURL",
            });

            var m = new Google.Apis.Urlshortener.v1.Data.Url();
            m.LongUrl = fullURL;
            result = service.Url.Insert(m).Execute().Id;

            return result;
        }
    }
}