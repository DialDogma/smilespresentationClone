using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace SmileSClaimOnLine.Helper
{
    public class ApiService
    {
        #region Field

        private static string ApiEndPoint = Properties.Settings.Default.Survey_EndPoint;

        #endregion Field

        #region Functions

        public static T RequestApiProvider<T>(dynamic objRequest, string resourceEndpoint, bool isPost)
        {
            IRestResponse restResponse;
            dynamic jsonSender = null;

            var request = isPost
                ? new RestRequest(resourceEndpoint, Method.POST)
                : new RestRequest(resourceEndpoint, Method.GET);

            var client = new RestClient(ApiEndPoint);
            request.RequestFormat = DataFormat.Json;
            jsonSender = isPost ? JsonConvert.SerializeObject(objRequest, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }) : jsonSender;
            request = isPost ? request.AddJsonBody(jsonSender) : request;
            restResponse = isPost ? client.Post(request) : client.Get(request);

            var resultConvert = JsonConvert.DeserializeObject<T>(restResponse.Content);

            return resultConvert;
        }

        #endregion Functions
    }
}