using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_MVC_Core.Model;
using Project_MVC_Core.ViewModels;
using X.PagedList;
using static NuGet.Packaging.PackagingConstants;

namespace Project_MVC_Core.Controllers
{
    public class FoodItemsController : Controller
    {
        private readonly FoodDbContext db;
        private readonly IWebHostEnvironment env;
        public FoodItemsController(FoodDbContext db, IWebHostEnvironment env)
        {
            this.db = db;
            this.env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.FoodItems.Include(x => x.Orders).OrderBy(x => x.FoodItemId).ToListAsync());
        }
        public IActionResult Create()
        {
            var model = new FoodInputModel();
            model.Orders.Add(new Order());
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(FoodInputModel model, string act = "")
        {
            if (act == "add")
            {
                model.Orders.Add(new Order());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.RawValue = null;
                }

            }
            if (act.StartsWith("remove"))
            {

                int index = int.Parse(act.Substring(act.IndexOf('_') + 1));
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.RawValue = null;
                }
                model.Orders.RemoveAt(index);
            }
            if (act == "insert")
            {
                var fooditem = new FoodItem
                {
                    FoodItemName = model.FoodItemName,
                    Price = model.Price,
                    ExpireDate = model.ExpireDate
                };
                string ext = Path.GetExtension(model.Picture.FileName);
                string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
                FileStream fs = new FileStream(savePath, FileMode.Create);
                await model.Picture.CopyToAsync(fs);
                fs.Close();
                fooditem.Picture = fileName;
                foreach (var s in model.Orders)
                {
                    fooditem.Orders.Add(s);
                }
                db.FoodItems.Add(fooditem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var p = await db.FoodItems.Include(x => x.Orders).FirstOrDefaultAsync(x => x.FoodItemId == id);
            if (p == null)
            {
                return NotFound();
            }
            var model = new FoodEditModel
            {
                FoodItemId = id,
                FoodItemName = p.FoodItemName,
                Price = p.Price,
                ExpireDate = p.ExpireDate,
                Orders = p.Orders.ToList(),
            };
            ViewBag.CurrentPic = p.Picture;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(FoodEditModel model, string act = "")
        {
            var p = await db.FoodItems.Include(x => x.Orders).FirstOrDefaultAsync(x => x.FoodItemId == model.FoodItemId);
            if (p == null) { return NotFound(); }
            if (act == "add")
            {
                model.Orders.Add(new Order());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.RawValue = null;
                }

            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.RawValue = null;
                }
                model.Orders.RemoveAt(index);
            }
            if (act == "update")
            {
                p.FoodItemName = model.FoodItemName;
                p.Price = model.Price;
                p.ExpireDate = model.ExpireDate;
                if (model.Picture != null)
                {
                    string ext = Path.GetExtension(model.Picture.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(this.env.WebRootPath, "Pictures", fileName);
                    FileStream fs = new FileStream(savePath, FileMode.Create);
                    await model.Picture.CopyToAsync(fs);
                    fs.Close();
                    p.Picture = fileName;
                }
                db.Orders.RemoveRange(p.Orders.ToList());
                p.Orders.Clear();
                foreach (var s in model.Orders)
                {
                    db.Orders.Add(new Order { OrderDate = s.OrderDate, FoodItemId = p.FoodItemId, Quantity = s.Quantity });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CurrentPic = p.Picture;
            return View(model);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var fooditem = new FoodItem { FoodItemId = id };
            db.Entry(fooditem).State = EntityState.Deleted;
            db.SaveChanges();
            return Json(new { success = true, msg = "Data deleted" });
        }
    }
}

