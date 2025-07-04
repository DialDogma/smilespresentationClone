using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StandardDevelopment.Controllers
{
    public class SortableListController : Controller
    {
        // GET: SortableList
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetListItems()
        {
            var result = GetItems();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<Item> GetItems()
        {
            var result = new List<Item>();

            //add mockup items
            result.Add(new Item() { Id = "1", Name = "Tuna" });
            result.Add(new Item() { Id = "2", Name = "Bacon" });
            result.Add(new Item() { Id = "3", Name = "Salmon" });
            result.Add(new Item() { Id = "4", Name = "Hotdog" });
            result.Add(new Item() { Id = "5", Name = "Sanwich" });
            result.Add(new Item() { Id = "6", Name = "Omlet" });
            result.Add(new Item() { Id = "7", Name = "Salad" });
            result.Add(new Item() { Id = "8", Name = "Ham" });
            result.Add(new Item() { Id = "9", Name = "Milk" });
            result.Add(new Item() { Id = "10", Name = "Cofee" });

            return result;
        }

        public struct Item
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}