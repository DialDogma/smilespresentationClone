using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;
using SmileSDataCenterV2.Models;
using RoutePrefixAttribute = System.Web.Http.RoutePrefixAttribute;
using SmileSDataCenterV2.DTOs;


namespace SmileSDataCenterV2.API
{
    [RoutePrefix("api/Product")]
    public class ProductController : ApiController
    {
        private readonly DataCenterV1Entities _context;
        public ProductController() => _context = new DataCenterV1Entities();


        [HttpGet]
        [Route("GetAllProductGroups")]
      public IHttpActionResult GetAllProductGroups()
        {
            try
            {
                var result = _context.usp_API_ProductGroup_Select().ToList();
                return Json(result);
            }
            catch (Exception e)
            {

                return Json("Select API ProductGroup Error: " + e.Message);
            }
        }
        
        [HttpGet]
        [Route("GetAllProductPackages")]
        public IHttpActionResult GetAllProductPackages(int productPackageDisplayId, DateTime? startCoverDate = null,DateTime? birthDate = null,int? genderId = null)
        {
            try
            {
                var result = _context.usp_API_ProductPackage_Select(productPackageDisplayId, null,startCoverDate,birthDate,genderId).ToList();
                return Json(result);
            }
            catch (Exception e)
            {

                return Json("Select API ProductALLPackage Error: " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetProductPackage")]
        public IHttpActionResult GetProductPackage(int productPackageId)
        {
            try
            {
                var rsPackage = _context.usp_API_ProductPackage_Select(null, productPackageId, null, null, null).Single();
                var rs = from x in _context.usp_API_ProductPackageXProduct_Select(productPackageId) select new ProductReturnDTO() { ProductId = x.ProductId.Value, ProductName = x.ProductName,
                                                                                    Premium = (float)x.Premium.Value,ProductTypeId = x.ProductTypeId.Value
                                                                                  ,ProductTypeName = x.ProductTypeName,ProductGroupId = x.ProductGroupId.Value,
                                                                                   ProductGroupName = x.ProductGroupName };
                var rsPeriodType = from x in _context.usp_API_ProductPackageXPayPeriodType_Select(productPackageId) select new PeriodTypeReturnDTO() { PeriodTypeId = x.PeriodTypeId.Value,
                                                                                    PeriodTypeName = x.PeriodTypeName, TotalPremium = (float)x.TotalPremium.Value };
                var result = new ProductPackageReturnDTO();
                result.ProductPackageId = rsPackage.ProductPackageId.Value;
                result.ProductPackageName = rsPackage.ProductPackageName;
                result.Products = rs.ToList();
                result.PeriodTypes = rsPeriodType.ToList();
                return Json(result);
            }
            catch (Exception e)
            {

                return Json("Select API ProductPakage: " + e.Message);
            }
        }
        [HttpGet]
        [Route("GetAllProductPackageDisplay")]
        public IHttpActionResult GetAllProductPackageDisplay()
        {
            try
            {
                var result = _context.usp_API_ProductPackageDisplay_Select().ToList();
                return Json(result);
            }
            catch (Exception e)
            {

                return Json("Select API ProductPackageDisplay error: " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllProductByProductGroupIds")]
        public IHttpActionResult GetAllProductByProductGroupIds(string productGroupList)
        {
            try
            {
                var result = _context.usp_API_ProductByProductGroupList_Select(productGroupList).ToList();
                return Json(result);
            }
            catch(Exception e)
            {
                return Json("Select API ProductByProductGroupList error: " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllProductType")]
        public IHttpActionResult GetAllProductType(int? productGroupId)
        {
            try
            {
                var result = _context.usp_API_ProductType_Select(productGroupId).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_ProductType_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetAllProductByProductTypeIds")]
        public IHttpActionResult GetAllProductByProductTypeIds(string productTypeListIds)
        {
            try
            {
                var result = _context.usp_API_ProductByProductTypeList_Select(productTypeListIds).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_ProductByProductTypeList_Select fail. Message : " + e.Message);
            }
        }

        [HttpGet]
        [Route("GetProductByPeriodTypeId")]
        public IHttpActionResult GetProductByPeriodTypeId(int? productPackageId, int? periodTypeId )
        {
            try
            {
                var result = _context.usp_API_ProductByPeriodType_Select(productPackageId, periodTypeId).ToList();

                return Json(result);
            }
            catch (Exception e)
            {
                return Json("Select from usp_API_ProductByProductTypeList_Select fail. Message : " + e.Message);
            }
        }
    }
}