using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//using NetTopologySuite.Geometries;
using UtilityComplaints.Core.Entities;
using UtilityComplaints.Core.Interfaces;
using UtilityComplaints.Infrastructure.Data;
using BAMCIS.GeoJSON;
using System.Net;
using static System.Net.Mime.MediaTypeNames;
using System.Linq;
using Newtonsoft.Json;
using NetTopologySuite.Geometries;
using BAMCIS.GeoJSON.Serde;

namespace UtilityComplaints.WebUI.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //private readonly IComplaintService _complaintService;
        private readonly UserManager<User> _userManager;

        public ComplaintsController(ApplicationDbContext context, IComplaintService complaintService, UserManager<User> userManager)
        {
            _context = context;
            //_complaintService = complaintService;
            _userManager = userManager;

        }

        
        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            var complaints = _context.Complaints.Include(c => c.Author).Include(c => c.Solver);

            return View(await complaints.ToListAsync());

        }

        public ActionResult GetFeatures() //move to ComplaintService?
        {
            var complaints = _context.Complaints.Include(c => c.Author).ToList();

            
            var complaintFeatures = from c in complaints //complaint -> DTO -> Feature?
                                    select c.Feature;

            
            var complaintFeaturesJSON = JsonConvert.SerializeObject(complaintFeatures);

            //return JSON to ajax function call
            Response.WriteAsync(complaintFeaturesJSON);

            return StatusCode(200);
        }




        //Authorize
        // GET: Complaints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.Author)
                .Include(c => c.Solver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // GET: Complaints/Create
        public async Task<IActionResult> CreateAsync()
        {
            Complaint complaint = new Complaint();
            var currentUser = await _userManager.GetUserAsync(User);
            complaint.Author = currentUser;
            complaint.Created = DateTime.Now;

            return View(complaint);
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,District,Address,Lon,Lat,Description,Created")] Complaint complaint)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            complaint.Author = currentUser;
            complaint.Created = DateTime.Now;

            /*TODO autofill from leaflet.js marker data
            complaint.Lat = latitude;
            complaint.Lon = longitude;*/


            if (ModelState.IsValid)
            {
                _context.Add(complaint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(complaint);
        }



        //GET 
        //public ActionResult Filter()

        //POST 
        //public ActionResult Filter()




        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }
            
            return View(complaint);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,District,Address,Lon,Lat,Description")/*,Created,Solved,UtilityCommentary")*/] Complaint complaint)
        {
            if (id != complaint.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complaint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplaintExists(complaint.Id))
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

            return View(complaint);
        }

        // GET: Complaints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.Author)
                .Include(c => c.Solver)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Complaints == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Complaints'  is null.");
            }
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplaintExists(int id)
        {
          return (_context.Complaints?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
