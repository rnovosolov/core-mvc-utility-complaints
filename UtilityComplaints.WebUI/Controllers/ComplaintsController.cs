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
using PagedList.EntityFramework;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using UtilityComplaints.WebUI.Models;

namespace UtilityComplaints.WebUI.Controllers
{
    [Authorize]
    public class ComplaintsController : Controller
    {
        //private readonly IDataContext _context;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ComplaintsController(ApplicationDbContext context, UserManager<User> userManager, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _userManager = userManager;
            _dateTimeProvider = dateTimeProvider;

        }


        // GET: Complaints
        public async Task<IActionResult> Index(int? page)
        {
            if (page != null && page < 1)
            {
                page = 1;
            }

            var pageSize = 10;

            var complaints = _context.Complaints.Include(c => c.Author).Include(c => c.Solver);

            return View(await complaints.ToPagedListAsync(page, pageSize));

        }

        [AllowAnonymous]
        public async Task<ActionResult> GetFeatures()
        {
            var complaintFeatures = await _context.Complaints.Select(x => x.Feature).ToListAsync();

            var complaintFeaturesJSON = JsonConvert.SerializeObject(complaintFeatures);

            await Response.WriteAsync(complaintFeaturesJSON);

            return StatusCode(200);
        }




        // GET: Complaints/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.Author)
                .Include(c => c.Solver)
                .FirstOrDefaultAsync(c => c.Id == id);
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
            complaint.Created = _dateTimeProvider.Now();

            return View(complaint);
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,District,Address,Lon,Lat,Description,Created,Status")] Complaint complaint)
        {

            var currentUser = await _userManager.GetUserAsync(User);
            complaint.Author = currentUser;
            complaint.Created = _dateTimeProvider.Now();
            //complaint.Status = Status.Active; 



            if (ModelState.IsValid)
            {
                _context.Add(complaint);
                await _context.SaveChangesAsync(CancellationToken.None);
                return RedirectToAction(nameof(Index));
            }
            return View(complaint);
        }



        //GET 
        //public ActionResult Filter()

        //POST 
        //public ActionResult Filter()

        //GET
        public async Task<IActionResult> Solve(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.Author)
                .Include(c => c.Solver)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (complaint == null)
            {
                return NotFound();
            }

            var currentSolver = await _userManager.GetUserAsync(User);
            if (!_userManager.IsInRoleAsync(currentSolver, complaint.District).Result)
            {
                return RedirectToAction(nameof(Denied));
            }

            SolveComplaintViewModel solveComplaintViewModel = new SolveComplaintViewModel
            {

                District = complaint.District,
                Address = complaint.Address,
                Lat = complaint.Lat,
                Lon = complaint.Lon,
                Description = complaint.Description,
                Created = complaint.Created,
                Status = complaint.Status,


            };

            return View(solveComplaintViewModel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Solve(int id, [Bind("Id,District,Address,Created,Lon,Lat,Description,Status,Solved,Solver,UtilityCommentary")] SolveComplaintViewModel solveComplaintViewModel)
        {
            /*if (id != complaint.Id)
            {
                return NotFound();
            }*/

            var currentSolver = await _userManager.GetUserAsync(User);
            solveComplaintViewModel.Solver = currentSolver;
            solveComplaintViewModel.Solved = _dateTimeProvider.Now();
            solveComplaintViewModel.Status = Status.Solved;

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                        .Include(c => c.Author)
                        .Include(c => c.Solver)
                        .FirstOrDefaultAsync(c => c.Id == id);

                try
                {
                    complaint.Solver = solveComplaintViewModel.Solver;
                    complaint.Solved = solveComplaintViewModel.Solved;
                    complaint.Status = solveComplaintViewModel.Status;
                    complaint.UtilityCommentary = solveComplaintViewModel.UtilityCommentary;


                    _context.Update(complaint);
                    await _context.SaveChangesAsync(CancellationToken.None);
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

            return View(solveComplaintViewModel);
        }


        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Complaints == null)
            {
                return NotFound();
            }

            var currentEditor = await _userManager.GetUserAsync(User);
            var complaint = await _context.Complaints
                .Include(c => c.Author)
                .Include(c => c.Solver)
                .FirstOrDefaultAsync(c => c.Id == id); //await _context.Complaints.FindAsync(id);

            if (complaint == null)
            {
                return NotFound();
            }

            if (complaint.Author != currentEditor)
            {
                return RedirectToAction(nameof(Denied));
            }

            EditComplaintViewModel editComplaintViewModel = new EditComplaintViewModel
            {

                Id = complaint.Id,
                District = complaint.District,
                Address = complaint.Address,
                Lat = complaint.Lat,
                Lon = complaint.Lon,
                Description = complaint.Description,

            };

            return View(editComplaintViewModel);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,District,Address,Created,Lon,Lat,Description")] EditComplaintViewModel editComplaintViewModel)
        {
            if (id != editComplaintViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var complaint = await _context.Complaints
                        .Include(c => c.Author)
                        .Include(c => c.Solver)
                        .FirstOrDefaultAsync(c => c.Id == id);

                try
                {
                    complaint.District = editComplaintViewModel.District;
                    complaint.Address = editComplaintViewModel.Address;
                    complaint.Lat = editComplaintViewModel.Lat;
                    complaint.Lon = editComplaintViewModel.Lon;
                    complaint.Description = editComplaintViewModel.Description;

                    _context.Complaints.Update(complaint);
                    await _context.SaveChangesAsync(CancellationToken.None);
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

            return View(editComplaintViewModel);
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

            await _context.SaveChangesAsync(CancellationToken.None);
            return RedirectToAction(nameof(Index));
        }

        private bool ComplaintExists(int id)
        {
            return (_context.Complaints?.Any(e => e.Id == id)).GetValueOrDefault();
        }



        public ActionResult Denied()
        {
            return View();
        }

    }
}
