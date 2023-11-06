using ev_01.Models;
using ev_01.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace ev_01.Controllers
{
    public class OrdersController : Controller
    {
        ResturantDBcontext db = new ResturantDBcontext();
        public ActionResult Index(string searchCustomerName)
        {
            var orders = db.Orders.Include(o => o.Receipts.Select(r => r.Food)).OrderByDescending(x => x.OrderId);

            if (!string.IsNullOrEmpty(searchCustomerName))
            {
                orders = (IOrderedQueryable<Order>)orders.Where(o => o.CustomerName.Contains(searchCustomerName));
            }
            

            return View(orders.ToList());
        }

        public ActionResult addNewFood(int? id)
        {
            ViewBag.food = new SelectList(db.Foods.ToList(), "FoodId", "FoodName", (id != null) ? id.ToString() : "");
            return PartialView("_addNewFood");
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(OrderVM orderVM, int[] foodID)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    CustomerName = orderVM.CustomerName,
                    OrderDate = orderVM.OrderDate,
                    TableNumber = orderVM.TableNumber,
                    AmountPaid = orderVM.AmountPaid,
                };

                HttpPostedFileBase file = orderVM.PictureFile;

                if (file != null)
                {
                    string filePath = Path.Combine("/Images", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                    file.SaveAs(Server.MapPath(filePath));
                    order.Picture = filePath;
                }

                foreach (var item in foodID)
                {
                    Receipt receipt = new Receipt()
                    {
                        Order = order,
                        OrderId = order.OrderId,
                        FoodId = item
                    };
                    db.Receipts.Add(receipt);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View();
        }

        public ActionResult Edit(int? id)
        {
            Order order = db.Orders.First(o => o.OrderId == id);
            var orderFoods = db.Receipts.Where(r => r.OrderId == id).ToList();

            OrderVM oVM = new OrderVM()
            {
                OrderId = order.OrderId,
                CustomerName = order.CustomerName,
                OrderDate = order.OrderDate,
                TableNumber = order.TableNumber,
                AmountPaid = order.AmountPaid,
                Picture = order.Picture
            };
            if (orderFoods.Count() > 0)
            {
                foreach (var item in orderFoods)
                {
                    oVM.FoodList.Add(item.FoodId);
                }
            }
            return View(oVM);

        }
        [HttpPost]
        public ActionResult Edit(OrderVM orderVM, int[] foodID)
        {
            if (ModelState.IsValid)
            {
                Order order = db.Orders.FirstOrDefault(o => o.OrderId == orderVM.OrderId);

                if (order != null)
                {
                    order.CustomerName = orderVM.CustomerName;
                    order.OrderDate = orderVM.OrderDate;
                    order.TableNumber = orderVM.TableNumber;
                    order.AmountPaid = orderVM.AmountPaid;

                    HttpPostedFileBase file = orderVM.PictureFile;

                    if (file != null)
                    {
                        string filePath = Path.Combine("/Images", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(Server.MapPath(filePath));
                        order.Picture = filePath;
                    }

                    var existFoods = db.Receipts.Where(x => x.OrderId == order.OrderId).ToList();

                    foreach (var food in existFoods)
                    {
                        db.Receipts.Remove(food);
                    }

                    if (foodID != null)
                    {
                        foreach (var item in foodID)
                        {
                            Receipt receipt = new Receipt()
                            {
                                OrderId = order.OrderId,
                                FoodId = item
                            };
                            db.Receipts.Add(receipt);
                        }
                    }
                    db.Entry(order).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return HttpNotFound();
                }

            }
            return View(orderVM);
        }
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {

            Order order = db.Orders.Find(id);

            if (order == null)
            {
                return HttpNotFound();
            }
            db.Entry(order).State = EntityState.Deleted;
            db.SaveChanges();
            return RedirectToAction("Index");

        }


    }


}
