using MasterMoltyfoam.Data;
using MasterMoltyfoam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PagedList;

namespace MasterMoltyfoam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext db;

        public CustomersController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string txtSearch, int pg = 1)
        {
            List<Customer> customer = await db.Customers.ToListAsync();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                customer = await db.Customers.Where(x => x.Name.Contains(txtSearch)).ToListAsync();
            }
            const int pageSize = 2;
            if (pg < 1)
                pg = 1;
            int recsCount = customer.Count();
            var pager=new Pager(recsCount,pg,pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = customer.Skip(resSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
           // return View(customer);
           return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    var fs = files[0].OpenReadStream();
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                    picture = ms.ToArray();
                    customer.Picture = picture;
                }
                db.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);

        }
        [HttpGet]
        public async Task<IActionResult>Edit(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var customer = await db.Customers.SingleOrDefaultAsync(x => x.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return View(customer);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] picture = null;
                    var fs = files[0].OpenReadStream();
                    var ms = new MemoryStream();
                    fs.CopyTo(ms);
                   picture=ms.ToArray();
                    customer.Picture = picture;

                }
                db.Customers.Update(customer);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var customer = db.Customers.SingleOrDefault(x => x.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return View(customer);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Customer customer)
        {
            db.Customers.Remove(customer);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = db.Customers.SingleOrDefault(x => x.Id == id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return View(customer);
            }
        }
    }
}
