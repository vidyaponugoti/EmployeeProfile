using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EmployeeProfile.Models;



namespace EmployeeProfile.Controllers
{
    [Authorize]
    public class EmployeeDetailsController : Controller
    {
        private Employee_PortalEntities db = new Employee_PortalEntities();



        // GET: EmployeeDetails



        public ActionResult Index()
        {
            var employeeDetails = db.EmployeeDetails.Include(e => e.Department).Include(e => e.Designation).Include(e => e.Employee_Type).Include(e => e.Login).Include(e => e.SubDepartment);
            return View(employeeDetails.ToList());
        }



        // GET: EmployeeDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }



        // GET: EmployeeDetails/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName");
            ViewBag.Employee_Designation_FK = new SelectList(db.Designations, "DesignationId", "DesignationName");
            ViewBag.EmployeeTypeId = new SelectList(db.Employee_Type, "EmployeeTypeid", "EmployeeType");
            ViewBag.ReportingManagerId = new SelectList(db.Logins.Where(x => x.Designation != "CONSULTANT"), "ID", "Name");
            ViewBag.SubDepartmentId = new SelectList(db.SubDepartments, "SDepartmentId", "Name");
            return View();
        }



        // POST: EmployeeDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EID,EmpId,Firstname,Lastname,Email,Phoneno,Worklocation,Employee_Designation_FK,DepartmentId,SubDepartmentId,LancesoftID,EmployeeTypeId,ReportingManagerId,Country,State,City,Street,Pincode,Active_Status,CreatedDate,CreatedBy,Vertical,Gender,Address_status,Working_internal,Joiningdate,Dateofbirth,Salary")] EmployeeDetail employeeDetail)
        {
            if (ModelState.IsValid)
            {
                Login lg = new Login();
                lg.Active_Status = 1;
                lg.CreateBy = employeeDetail.CreatedBy;
                var dd = db.Designations.Where(x => x.DesignationId == employeeDetail.Employee_Designation_FK).FirstOrDefault();

                lg.Designation = dd.DesignationName;
                lg.Email = employeeDetail.Email;
                lg.Mobileno = employeeDetail.Phoneno;
                lg.Name = employeeDetail.Firstname;// + " " + employeeDetail.Lastname;
                lg.Password = employeeDetail.Firstname;
                lg.UserName = employeeDetail.Lastname;
                lg.Empid = employeeDetail.EmpId;
                db.Logins.Add(lg);
                db.SaveChanges();
                employeeDetail.EmpId = lg.ID;



                db.EmployeeDetails.Add(employeeDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }



            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employeeDetail.DepartmentId);
            ViewBag.Employee_Designation_FK = new SelectList(db.Designations, "DesignationId", "DesignationName", employeeDetail.Employee_Designation_FK);
            ViewBag.EmployeeTypeId = new SelectList(db.Employee_Type, "EmployeeTypeid", "EmployeeType", employeeDetail.EmployeeTypeId);
            ViewBag.ReportingManagerId = new SelectList(db.Logins, "ID", "Name", employeeDetail.ReportingManagerId);
            ViewBag.SubDepartmentId = new SelectList(db.SubDepartments, "SDepartmentId", "Name", employeeDetail.SubDepartmentId);
            return View(employeeDetail);
        }



        // GET: EmployeeDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employeeDetail.DepartmentId);
            ViewBag.Employee_Designation_FK = new SelectList(db.Designations, "DesignationId", "DesignationName", employeeDetail.Employee_Designation_FK);
            ViewBag.EmployeeTypeId = new SelectList(db.Employee_Type, "EmployeeTypeid", "EmployeeType", employeeDetail.EmployeeTypeId);
            ViewBag.ReportingManagerId = new SelectList(db.Logins, "ID", "Name", employeeDetail.ReportingManagerId);
            ViewBag.SubDepartmentId = new SelectList(db.SubDepartments, "SDepartmentId", "Name", employeeDetail.SubDepartmentId);
            return View(employeeDetail);
        }



        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeDetail employeeDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentName", employeeDetail.DepartmentId);
            ViewBag.Employee_Designation_FK = new SelectList(db.Designations, "DesignationId", "DesignationName", employeeDetail.Employee_Designation_FK);
            ViewBag.EmployeeTypeId = new SelectList(db.Employee_Type, "EmployeeTypeid", "EmployeeType", employeeDetail.EmployeeTypeId);
            ViewBag.ReportingManagerId = new SelectList(db.Logins, "ID", "Name", employeeDetail.ReportingManagerId);
            ViewBag.SubDepartmentId = new SelectList(db.SubDepartments, "SDepartmentId", "Name", employeeDetail.SubDepartmentId);
            return View(employeeDetail);
        }



        // GET: EmployeeDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            if (employeeDetail == null)
            {
                return HttpNotFound();
            }
            return View(employeeDetail);
        }



        // POST: EmployeeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeDetail employeeDetail = db.EmployeeDetails.Find(id);
            db.EmployeeDetails.Remove(employeeDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}