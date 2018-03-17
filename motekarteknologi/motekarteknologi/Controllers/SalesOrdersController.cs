using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using motekarteknologi.Data;
using motekarteknologi.Models;

namespace motekarteknologi.Controllers
{
    public class SalesOrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult DeleteLine(Guid ID)
        {
            SalesOrderLine salesOrderLine = _context.SalesOrderLine
                .Where(x => x.ID.Equals(ID)).FirstOrDefault();
            string model = Newtonsoft.Json.JsonConvert.SerializeObject(salesOrderLine);
            return PartialView("_ModalDelete", model: model);
        }
        
        [HttpPost, ActionName("DeleteLine")]
        public IActionResult DeleteLineConfirmed(Guid ID)
        {

            var line = _context.SalesOrderLine
                .Where(x => x.ID.Equals(ID)).FirstOrDefault();

            var salesOrderID = line.SalesOrderID;

            _context.SalesOrderLine.Remove(line);

            _context.SaveChanges();

            return RedirectToAction(nameof(Edit), new { ID = salesOrderID });
        }

        [HttpGet]
        public async Task<IActionResult> AddEditLine(Guid? ID)
        {
            SalesOrderLine line = new SalesOrderLine();
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name");
            
            if (ID.HasValue)
            {
                line = await _context.SalesOrderLine
                    .Where(x => x.ID.Equals(ID)).FirstOrDefaultAsync();
            } else
            {
                line.SalesOrderID = (Guid)TempData["SalesOrderID"];
            }

            TempData["SalesOrderID"] = line.SalesOrderID;

            return PartialView("~/Views/SalesOrders/_FormLine.cshtml", line);
        }

        [HttpPost]
        public async Task<IActionResult> AddEditLine(Guid? ID, SalesOrderLine salesOrderLine)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isNew = ID.Equals(Guid.Empty);
                    if (isNew)
                    {
                        salesOrderLine.ID = Guid.NewGuid();
                        _context.Add(salesOrderLine);
                    } else
                    {
                        _context.Update(salesOrderLine);
                    }
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return RedirectToAction(nameof(Edit), new { ID = salesOrderLine.SalesOrderID });
        }

        // GET: SalesOrders
        public async Task<IActionResult> Index()
        {
            return View(await _context.SalesOrder.ToListAsync());
        }

        // GET: SalesOrders/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .SingleOrDefaultAsync(m => m.ID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SalesOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] SalesOrder salesOrder)
        {
            if (ModelState.IsValid)
            {
                salesOrder.ID = Guid.NewGuid();
                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(salesOrder);
        }

        // GET: SalesOrders/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            TempData["SalesOrderID"] = id;

            var salesOrder = await _context.SalesOrder
                .Include(x => x.Lines)
                    .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (salesOrder == null)
            {
                return NotFound();
            }
            return View(salesOrder);
        }

        // POST: SalesOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,Name,Description")] SalesOrder salesOrder)
        {
            if (id != salesOrder.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(salesOrder);
        }

        // GET: SalesOrders/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .SingleOrDefaultAsync(m => m.ID == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var salesOrder = await _context.SalesOrder.SingleOrDefaultAsync(m => m.ID == id);
            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderExists(Guid id)
        {
            return _context.SalesOrder.Any(e => e.ID == id);
        }
    }
}
