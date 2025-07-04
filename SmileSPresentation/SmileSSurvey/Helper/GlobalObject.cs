using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;

namespace SmileSSurvey.Helper
{
    public static class GlobalObject
    {
        public static JsonSerializerSettings carmelSetting()
        {
            return new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        }
    }
}