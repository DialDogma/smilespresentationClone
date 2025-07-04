using DocumentFormat.OpenXml.Drawing.Charts;
using System;

namespace SmileSClaimPayBack.ViewModels
{
    public class GenerateShortLinkRequestDto
    {
        public string url { get; set; }
        public int projectId { get; set; }
        public DateTime expiredDate { get; set; }
        public int subtypeId { get; set; }
        public bool usePassword { get; set; }
    }

    public class GenerateShortLinkResponseDto
    {
        public GenerateShortLinkResponseData data { get; set; }
        public bool isSuccess { get; set; }
        public DateTime serverDateTime { get; set; }
    }
    public class GenerateShortLinkResponseData
    {
        public string shortUrl { get; set; }
        public string longUrl { get; set; }
        public DateTime expiredDate { get; set; }
        public bool useLogin { get; set; }
    }

}