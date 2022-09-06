using EmployeeProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeProfile.Controllers
{
    public class HRController : Controller
    {
        // GET: HR
        public Employee_PortalEntities db = new Employee_PortalEntities();
        //public ActionResult Index()
        //{
        //    var user = TempData["UserId"] as Login;

        //    var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == user.ID).ToList();
        //    return View(data);

        //}
        
    }
}