using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodCourt.Controllers
{
    public class FoodCourtController : Controller
    {
        // GET: FoodCourt
        #region User
        [HttpGet]
        public ActionResult User()
        {
            return View();
        }

        #endregion

        #region Admin
        FoodCourtEntities foodCourt = new FoodCourtEntities(); 

        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin(Employee emp)
        {

            if (ModelState.IsValid)
            {
                var admin = foodCourt.Employees.FirstOrDefault(check => check.UserName == emp.UserName &&
                                                              check.Password == emp.Password);
                if (admin != null)
                {
                    return RedirectToAction("WelcomeAdmin");
                }
                else
                {
                    TempData["message"] = "Invalid";
                    return View(emp);
                }
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult WelcomeAdmin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddItemTypes()
        {
            ItemMaster m = new ItemMaster();
            var id = foodCourt.ItemMasters     
                .SqlQuery("EXEC SpGetTypeNo").FirstOrDefault();

            if (m != null)
            {
                m.ItemTypeNo = id.ItemTypeNo;
            }
            return View(m);
            
        }

        [HttpPost]
        public ActionResult AddItemTypes(ItemMaster m1)
        {
            if (ModelState.IsValid)
            {
                ItemMaster m2 = new ItemMaster();
                m2.ItemTypeNo = m1.ItemTypeNo;
                m2.ItemType = m1.ItemType;
                foodCourt.ItemMasters.Add(m2);
                foodCourt.SaveChanges();
                if(m2.ItemTypeNo>0)
                {
                    TempData["message"] = "Data Inserted";
                }
                else
                {
                    TempData["message"] = "Invalid Data";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddItemName()
        {
            Item m = new Item();
            var id = foodCourt.Items
                .SqlQuery("EXEC SpGetItemNo").FirstOrDefault();

            if (m != null)
            {
                m.ItemNo = id.ItemNo;
            }
            var itemType = foodCourt.ItemMasters.ToList();
            List<SelectListItem> liItemTypes = new List<SelectListItem>();
            liItemTypes.Add(new SelectListItem() { Text = "Select Type", Value = "0" });
            foreach(var item in itemType)
            {
                liItemTypes.Add(new SelectListItem() { Text = item.ItemType, Value = Convert.ToString(item.ItemTypeNo)});
            }
            ViewData["ItemTypes"] = liItemTypes;
            return View(m);
        }
        [HttpPost]
        public ActionResult AddItemName(Item m1)
        {
            if (ModelState.IsValid)
            {
                Item m2 = new Item();

                m2.ItemNo = m1.ItemNo;
                m2.ItemName = m1.ItemName;
                m2.ItemTypeNo = m1.ItemTypeNo;
                m2.ItemPrice = m1.ItemPrice;
                m2.ItemQuantity = m1.ItemQuantity;

                foodCourt.Items.Add(m2);
                foodCourt.SaveChanges();

                if(m2.ItemNo>0)
                {
                    TempData["message"] = "Data Inserted";
                }
                else
                {
                    TempData["message"] = "Invalid Data";
                }
            }
            return RedirectToAction("AddItemName");
        }
        
        [HttpGet]
        public ActionResult UpdateStock()
        {
            StockTran s = new StockTran();
            var id = foodCourt.StockTrans
                .SqlQuery("EXEC SpGetTransId").FirstOrDefault();

            if (s != null)
            {
                s.TransId = id.TransId;
            }

            var itemType = foodCourt.ItemMasters.ToList();
            List<SelectListItem> liItemTypes = new List<SelectListItem>();
            liItemTypes.Add(new SelectListItem() { Text = "Select Type", Value = "0" });
            foreach (var item in itemType)
            {
                liItemTypes.Add(new SelectListItem() { Text = item.ItemType, Value = Convert.ToString(item.ItemTypeNo) });
            }
            ViewData["ItemTypes"] = liItemTypes;

            return View(s);
        }

        public JsonResult GetItemNames(int id)
        {
            var itemName = foodCourt.Items.Where(x => x.ItemTypeNo == id);
            List<SelectListItem> liItemName = new List<SelectListItem>();
            liItemName.Add(new SelectListItem() { Text = "Select Type", Value = "0" });
            foreach (var item in itemName)
            {
                liItemName.Add(new SelectListItem() { Text = item.ItemName, Value = Convert.ToString(item.ItemNo) });
            }
            return Json(new SelectList(liItemName, "Value", "Text", JsonRequestBehavior.AllowGet));                      
        }
        #endregion
    }
}