using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using SmileSSurvey.Models;

namespace SmileSSurvey.Helper
{
    public class ApiService
    {
        #region Functions

        public class Parameter
        {
            public string Key { get; set; }
            public dynamic Value { get; set; }
        }

        public static T RequestApiProvider<T>(dynamic objRequest, List<Parameter> objParameters, string resourceEndpoint, bool isPost, string apiEndPoint, List<RequestHeaderApiDTO> header = null)
        {
            IRestResponse restResponse;
            dynamic jsonSender = null;

            var request = isPost
                ? new RestRequest(resourceEndpoint, Method.POST)
                : new RestRequest(resourceEndpoint, Method.GET);

            var client = new RestClient(apiEndPoint);

            if(header != null)
            {
                foreach (var item in header)
                {
                    request.AddHeader(item.HeaderName, item.HeaderValue);
                }
            }

            if (objParameters.Count > 0)
                foreach (var item in objParameters)
                    request.AddParameter(item.Key, item.Value);

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