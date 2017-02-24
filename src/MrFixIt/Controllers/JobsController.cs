using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrFixIt.Models;
using Microsoft.EntityFrameworkCore;

namespace MrFixIt.Controllers
{
    public class JobsController : Controller
    {
        private MrFixItContext db = new MrFixItContext();

        public IActionResult Index()
        {
            return View(db.Jobs.Include(i => i.Worker).ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            db.Jobs.Add(job);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Claim(int id)
        {
            var thisItem = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisItem);
        }

        [HttpPost]
        public IActionResult ClaimJob(Job job)
        {
            job.Worker = db.Workers.FirstOrDefault(i => i.UserName == User.Identity.Name);
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }
        public IActionResult Status(int id)
        {
            var thisJob = db.Jobs.FirstOrDefault(items => items.JobId == id);
            return View(thisJob);
        }
        [HttpPost]
        public IActionResult PendingJob(Job job)
        {
            job.Pending = true;
            job.Completed = false;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }
        public IActionResult CompleteJob(Job job)
        {
            job.Pending = false;
            job.Completed = true;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();
            return Json(job);
        }


    }
}
