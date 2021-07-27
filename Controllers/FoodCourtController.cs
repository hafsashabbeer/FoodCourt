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
        FoodCourtEntities foodCourt = new FoodCourtEntities();
        #region User

        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(Employee e1)
        {
            var password = foodCourt.Employees.FirstOrDefault(p => p.UserName.Equals(e1.UserName) && p.Hint.Equals(e1.Hint)).Password;
            TempData["Password"] = "Your Password is " + password;
            return RedirectToAction("ForgetPassword");
        }
        [HttpGet]
        public new ActionResult User()
        {
            return View();
        }
        [HttpPost]
        public new ActionResult User(Employee e1)
        {
            
            if (ModelState.IsValid)
            {
                var user = foodCourt.Employees.FirstOrDefault(check => check.UserName == e1.UserName &&
                                                              check.Password == e1.Password && check.IsAdmin == 1).EmpName;
                Session["id"] = foodCourt.Employees.FirstOrDefault(check => check.UserName == e1.UserName && check.Password == e1.Password).EId;

                if (user != null )
                {
                    TempData["Name"] = user;
                    return RedirectToAction("WelcomeUser");
                }
                else
                {
                    TempData["message"] = "Invalid User... Please check your Username or Password";
                    return View(e1);
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult WelcomeUser()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewStock()
        {
            var itemType = foodCourt.ItemMasters.ToList();
            List<SelectListItem> liItemTypes = new List<SelectListItem>();
            liItemTypes.Add(new SelectListItem() { Text = "Select Type", Value = "0" });
            foreach (var item in itemType)
            {
                liItemTypes.Add(new SelectListItem() { Text = item.ItemType, Value = Convert.ToString(item.ItemTypeNo) });
            }
            ViewData["ItemTypes"] = liItemTypes;
            return View();
        }

        public JsonResult GetItemName(int id)
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

        [HttpPost]
        public ActionResult ViewStock(Item i1)
        {
            var quantity = foodCourt.Items.FirstOrDefault(check => check.ItemNo.Equals(i1.ItemNo) && check.ItemTypeNo.Equals(i1.ItemTypeNo)).ItemQuantity;
            var price = foodCourt.Items.FirstOrDefault(check => check.ItemNo.Equals(i1.ItemNo) && check.ItemTypeNo.Equals(i1.ItemTypeNo)).ItemPrice;
            
            TempData["Quantity"] = "Available Quantity " + quantity;
            TempData["Price"] = "Price " + price / quantity;
            
            return View();
        }
        
        [HttpGet]
        public ActionResult MakeSales()
        {
            var itemType = foodCourt.ItemMasters.ToList();
            List<SelectListItem> liItemTypes = new List<SelectListItem>();
            liItemTypes.Add(new SelectListItem() { Text = "Select Type", Value = "0" });
            foreach (var item in itemType)
            {
                liItemTypes.Add(new SelectListItem() { Text = item.ItemType, Value = Convert.ToString(item.ItemTypeNo) });
            }
            ViewData["ItemTypes"] = liItemTypes;
            return View();
        }
        public JsonResult DisplayItemName(int id)
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

        [HttpGet]
        public ActionResult Price(int id)
        {
            var itemPrice = foodCourt.Items.FirstOrDefault(x => x.ItemNo == id).ItemPrice;
            var itemQuantity = foodCourt.Items.FirstOrDefault(x => x.ItemNo == id).ItemQuantity;
            var result = new { Value = itemPrice / itemQuantity };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TotalAmount(int price, int quantity)
        {
            var totalAmount = new {Value = quantity * price };
            return Json(totalAmount, JsonRequestBehavior.AllowGet);
        }

        

        [HttpPost]
        public ActionResult MakeSales(Models.BillValues b1, string continueBtn)
        {
            
            if (ModelState.IsValid)
            {
                if (Session["BMId"] == null)
                {
                    BillMaster b2 = new BillMaster();
                    if (Session["id"] != null)
                    {

                        
                        b2.TotalAmount = 0;
                        b2.BDate = DateTime.Today;
                        b2.TotalSales = 0;
                        b2.EId = (int)Session["id"];
                        
                        foodCourt.BillMasters.Add(b2);
                        foodCourt.SaveChanges();

                        Session["BMId"] = b2.BMId;
                        Session["Date"] = b2.BDate;
                        

                    }
                   
                }

                BillTran b3 = new BillTran();

                if (Session["BMId"] != null)
                {
                    b3.ItemNo = b1.ItemNo;
                    b3.Quantity = b1.Quantity;
                    b3.Amount = b1.Amount;
                    b3.BMId = (int)Session["BMId"];

                    var bmid = (int)Session["BMId"];
                    var itemUpdate = foodCourt.BillMasters.Where(id => id.BMId.Equals(bmid));
                    
                    foreach(var item in itemUpdate)
                    {
                        item.TotalAmount = item.TotalAmount + b1.Amount;
                        item.TotalSales = item.TotalSales + 1;
                        Session["TotalAmt"] = "Total Amount is " + item.TotalAmount;
                    }

                    foodCourt.BillTrans.Add(b3);
                    
                    foodCourt.SaveChanges();
                }


                return RedirectToAction("MakeSales");
            }
            else
            {
                return View();
            }
            
        }
        [ChildActionOnly]
        public ActionResult SalesValues()
        {
            
            if (Session["BMId"] != null)
            {
                int id = (int)Session["BMId"];
                var data = foodCourt.BillTrans.Where(item => item.BMId == id).ToList();
                return PartialView(data);
            }
            else
            {
                return PartialView();
            }
            
        }
        
        [HttpGet]
        public ActionResult GenerateBill()
        {
            if (Session["BMId"] != null)
            {
                List<Models.FinalBill> f1 = new List<Models.FinalBill>();

                int id = (int)Session["BMId"];
                int count = 0;
                var data = foodCourt.BillTrans.Where(item => item.BMId == id).ToList();
                foreach (var item in data)
                {
                    count++;
                    f1.Add(new Models.FinalBill() { Number = count, ItemName = item.Item.ItemName, Price = item.Amount / item.Quantity, Quantity = item.Quantity, Amount = item.Amount });
                }
                return View(f1);
            }
            else
            {
                return View();
            }

        }

        #endregion

        #region Admin 

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

        [HttpPost]
        public ActionResult UpdateStock(StockTran s1)
        {
            if(ModelState.IsValid)
            {
                StockTran s2 = new StockTran();
                
                s2.TransId = s1.TransId;
                s2.ItemNo = s1.ItemNo;
                s2.StockQuantity = s1.StockQuantity;
                s2.StockAmount = s1.StockAmount;
                s2.Date = DateTime.Today;

                var itemNo = foodCourt.Items.Where(i => i.ItemNo.Equals(s1.ItemNo));

                foreach (var item in itemNo)
                {
                    item.ItemQuantity = s1.StockQuantity;
                    item.ItemPrice = s1.StockAmount;
                }

                foodCourt.StockTrans.Add(s2);
                foodCourt.SaveChanges();


                if (s2.TransId > 0)
                {
                    TempData["message"] = "Data Inserted";
                }
                else
                {
                    TempData["message"] = "Invalid Data";
                }
            }
               return RedirectToAction("UpdateStock");
        }
        
        [HttpGet]
        public ActionResult ViewSales()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewDateSales()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult ViewDateSales(BillTran b1)
        {
            Session["Date"] = b1.BillMaster.BDate;
            return View();
        }

        [ChildActionOnly]
        public ActionResult DateSales()
        {
            if (Session["Date"] != null)
            {
                List<Models.SalesList> s1 = new List<Models.SalesList>();
                
                DateTime salesDate = (DateTime)Session["Date"];
                var salesList = foodCourt.BillTrans.Where(date => date.BillMaster.BDate.Equals(salesDate)).ToList();

                foreach (var item in salesList)
                {

                    s1.Add(new Models.SalesList()
                    {
                        BillId = item.BMId,
                        ItemType = item.Item.ItemMaster.ItemType,
                        ItemName = item.Item.ItemName,
                        Amount = item.Amount,
                        Date = salesDate,
                        EmpName = item.BillMaster.Employee.EmpName
                        
                    });
                    
                }
                Session["TotAmt"] = foodCourt.BillMasters.Where(date => date.BDate.Equals(salesDate)).Sum(a => a.TotalAmount);
                return PartialView(s1);

            }
            else
            {
                return PartialView();
            }
        }

        [HttpGet]
        public ActionResult DateToDateSales()
        {
            return View();

        }
        [HttpPost]
        public ActionResult DateToDateSales(Models.DateToDateSalesList d1)
        {
            Session["FromDate"] = d1.FromDate;
            Session["TillDate"] = d1.TillDate;
            return View();
        }
        [ChildActionOnly]
        public ActionResult DateToDateList()
        {
            if (Session["FromDate"] != null && Session["TillDate"] != null)
            {
                List<Models.DateToDateSalesList> d1 = new List<Models.DateToDateSalesList>();

                DateTime fromDate = (DateTime)Session["FromDate"];
                DateTime tillDate = (DateTime)Session["TillDate"];
                var salesList = foodCourt.BillTrans.Where(date => date.BillMaster.BDate >= fromDate && date.BillMaster.BDate <= tillDate).ToList();

                foreach (var item in salesList)
                {

                    d1.Add(new Models.DateToDateSalesList()
                    {
                        BillId = item.BMId,
                        ItemType = item.Item.ItemMaster.ItemType,
                        ItemName = item.Item.ItemName,
                        Amount = item.Amount,
                        Date = item.BillMaster.BDate,
                        EmpName = item.BillMaster.Employee.EmpName

                    }) ;

                }
                Session["TotAmt"] = foodCourt.BillMasters.Where(date => date.BDate >= fromDate && date.BDate <= tillDate).Sum(a => a.TotalAmount);
                return PartialView(d1);

            }
            else
            {
                return PartialView();
            }
        }

        [HttpGet]
        public ActionResult AddEmployees()
        {
            Employee e = new Employee();
            var id = foodCourt.Employees
                .SqlQuery("EXEC SpGetEmpId").FirstOrDefault();

            if (e!= null)
            {
                e.EId = id.EId;
            }
            return View(e);
        }

        [HttpPost]
        public ActionResult AddEmployees(Employee e1)
        {
            if (ModelState.IsValid)
            {
                Employee e2 = new Employee();

                e2.EId = e1.EId;
                e2.EmpName = e1.EmpName;
                e2.UserName = e1.UserName;
                e2.Password = e1.Password;
                e2.PhoneNumber = e1.PhoneNumber;
                e2.Address = e1.Address;
                e2.Hint = e1.Hint;
                e2.IsAdmin = 1;

                foodCourt.Employees.Add(e2);
                foodCourt.SaveChanges();

                if (e2.EId > 0)
                {
                    TempData["message"] = "Data Inserted";
                }
                else
                {
                    TempData["message"] = "Invalid";
                }
                return RedirectToAction("AddEmployees");
            }
            return View(e1);
        }

        
        #endregion
    }
}