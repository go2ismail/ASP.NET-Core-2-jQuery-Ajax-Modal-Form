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
    public class SalesOrderLinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalesOrderLinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SalesOrderLines
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SalesOrderLine.Include(s => s.Product).Include(s => s.SalesOrder);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: SalesOrderLines/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderLine = await _context.SalesOrderLine
                .Include(s => s.Product)
                .Include(s => s.SalesOrder)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (salesOrderLine == null)
            {
                return NotFound();
            }

            return View(salesOrderLine);
        }

        // GET: SalesOrderLines/Create
        public IActionResult Create()
        {
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name");
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrder, "ID", "Name");
            return View();
        }

        // POST: SalesOrderLines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SalesOrderID,ProductID,Description,Qty")] SalesOrderLine salesOrderLine)
        {
            if (ModelState.IsValid)
            {
                salesOrderLine.ID = Guid.NewGuid();
                _context.Add(salesOrderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", salesOrderLine.ProductID);
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrder, "ID", "Name", salesOrderLine.SalesOrderID);
            return View(salesOrderLine);
        }

        // GET: SalesOrderLines/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderLine = await _context.SalesOrderLine.SingleOrDefaultAsync(m => m.ID == id);
            if (salesOrderLine == null)
            {
                return NotFound();
            }
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", salesOrderLine.ProductID);
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrder, "ID", "Name", salesOrderLine.SalesOrderID);
            return View(salesOrderLine);
        }

        // POST: SalesOrderLines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,SalesOrderID,ProductID,Description,Qty")] SalesOrderLine salesOrderLine)
        {
            if (id != salesOrderLine.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salesOrderLine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderLineExists(salesOrderLine.ID))
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
            ViewData["ProductID"] = new SelectList(_context.Product, "ID", "Name", salesOrderLine.ProductID);
            ViewData["SalesOrderID"] = new SelectList(_context.SalesOrder, "ID", "Name", salesOrderLine.SalesOrderID);
            return View(salesOrderLine);
        }

        // GET: SalesOrderLines/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrderLine = await _context.SalesOrderLine
                .Include(s => s.Product)
                .Include(s => s.SalesOrder)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (salesOrderLine == null)
            {
                return NotFound();
            }

            return View(salesOrderLine);
        }

        // POST: SalesOrderLines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var salesOrderLine = await _context.SalesOrderLine.SingleOrDefaultAsync(m => m.ID == id);
            _context.SalesOrderLine.Remove(salesOrderLine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesOrderLineExists(Guid id)
        {
            return _context.SalesOrderLine.Any(e => e.ID == id);
        }
    }
}
