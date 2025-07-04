using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SmileSClaimPayBack.Models
{
    public static class ResponseResult
    {
        public static ServiceResponse<T> Success<T>(T data, string message = "Success.")
        {
            return new ServiceResponse<T>
            {
                Data = data,
                Message = message
            };
        }

        public static ServiceResponse<T> Success<T>(string message = "Success.")
        {
            return new ServiceResponse<T>
            {
                Message = message
            };
        }

        public static ServiceResponse<T> Failure<T>(string message, int? errorCode = null, object exceptionMessage = null) where T : class
        {
            return new ServiceResponse<T>
            {
                Data = null,
                IsSuccess = false,
                Message = message,
                Code = errorCode,
                ExceptionMessage = exceptionMessage
            };
        }
    }

    public class ServiceResponse<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Include)]
        public T Data { get; set; }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = null;

        public int? Code { get; set; } = null;
        public object ExceptionMessage { get; set; } = null;
        public DateTime ServerDateTime { get; set; } = DateTime.Now;

        public double? TotalAmountRecords { get; set; }
        public double? TotalAmountPages { get; set; }
        public double? CurrentPage { get; set; }
        public double? RecordsPerPage { get; set; }
        public int? PageIndex { get; set; }
    }
}