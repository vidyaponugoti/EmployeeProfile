using EmployeeProfile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeProfile.Controllers
{
    [Authorize]
    public class CHController : Controller
    {
        // GET: CH
        public Employee_PortalEntities db = new Employee_PortalEntities();
        public ActionResult Index()

        {

            var user = TempData["UserId"] as Login;


            var employeeDetails = db.EmployeeDetails.Where(x => x.EmpId == user.ID).FirstOrDefault();

            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == user.ID).ToList();


            int CHtenure = 0;
            decimal CHprofit = 0;
            decimal CH_p_l = 0;
            decimal GM_p_l = 0;


            AddCalculationDetails obj1 = new AddCalculationDetails();
            GetAllEployeedetails model = new GetAllEployeedetails();

            foreach (var item in data)
            {

                GM_p_l += GMcalculation(item.EmpId);
                item.GMprofit_Loss = GMcalculation(item.EmpId);


            }
            if (user.ID == employeeDetails.EmpId)
            {

                DateTime jd1 = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                CHtenure = (DateTime.Parse(DateTime.Now.ToString()) - jd1).Days / 30;
                CHprofit = CHtenure * employeeDetails.Salary.Value;

            }

            CH_p_l = GM_p_l - CHprofit;

            ViewBag.CHprofit = CH_p_l;


            return View(data);

        }
        public decimal GMcalculation(int id)
        {
            var employeeDetails = db.EmployeeDetails.Where(x => x.EmpId == id).FirstOrDefault();
            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == id).ToList();
            int GMtenure = 0;
            decimal GMprofit = 0;
            decimal GM_p_l = 0;
            decimal Manager_p_l = 0;

            AddCalculationDetails obj1 = new AddCalculationDetails();
            if (employeeDetails.EmpId == id)
            {
                foreach (var item in data)
                {

                    Manager_p_l += Managercalculation(item.EmpId);


                }

            }
            if (employeeDetails.EmpId == id)
            {

                DateTime jd1 = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                GMtenure = (DateTime.Parse(DateTime.Now.ToString()) - jd1).Days / 30;
                GMprofit = GMtenure * employeeDetails.Salary.Value;
                GM_p_l = Manager_p_l - GMprofit;

            }
            return GM_p_l;

        }
        public decimal Managercalculation(int id)
        {
            var employeeDetails = db.EmployeeDetails.Where(x => x.EmpId == id).FirstOrDefault();
            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == id).ToList();
            int Managertenure = 0;
            decimal Managerprofit = 0;
            decimal Manager_p_l = 0;
            decimal lead_p_l = 0;

            AddCalculationDetails obj1 = new AddCalculationDetails();
            if (employeeDetails.EmpId == id)
            {
                foreach (var item in data)
                {

                    lead_p_l += leadCalculation(item.EmpId);


                }

            }
            if (employeeDetails.EmpId == id)
            {

                DateTime jd1 = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                Managertenure = (DateTime.Parse(DateTime.Now.ToString()) - jd1).Days / 30;
                Managerprofit = Managertenure * employeeDetails.Salary.Value;
                Manager_p_l = lead_p_l - Managerprofit;

            }

            return Manager_p_l;


        }
        public decimal leadCalculation(int id)
        {
            var employeeDetails = db.EmployeeDetails.Where(x => x.EmpId == id).FirstOrDefault();
            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == id).ToList();
            int leadtenure = 0;
            decimal leadprofit = 0;
            decimal lead_p_l = 0;
            decimal profitdata = 0;

            AddCalculationDetails obj1 = new AddCalculationDetails();
            if (employeeDetails.EmpId == id)
            {

                DateTime jd1 = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                leadtenure = (DateTime.Parse(DateTime.Now.ToString()) - jd1).Days / 30;
                leadprofit = leadtenure * employeeDetails.Salary.Value;


            }
            if (employeeDetails.EmpId == id)
            {
                foreach (var item in data)
                {
                    profitdata += Calculation(item.EID);
                    lead_p_l = profitdata - leadprofit;

                }
            }
            return lead_p_l;


        }
        public decimal Calculation(int Id)
        {
            AsignClienttoEmp obj1 = new AsignClienttoEmp();
            GetAllEployeedetails model = new GetAllEployeedetails();
            List<AsignClienttoEmp> lObj = new List<AsignClienttoEmp>();
            List<Bill> lObj1 = new List<Bill>();
            model.eployeeDetails = db.EmployeeDetails.Where(x => x.EID == Id).FirstOrDefault();
            var clientDetails = db.AsignClienttoEmps.Where(x => x.EID == Id).ToList();
            lObj = clientDetails;
            var billDetails = db.Bills.Where(x => x.EID == Id).ToList();
            lObj1 = billDetails;
            model.clientDetails = lObj;


            List<AddCalculationDetails> obj = new List<AddCalculationDetails>();
            var Bill_details = db.Bills.Where(x => x.EID == Id).FirstOrDefault();
            var employeeDetails = db.EmployeeDetails.Where(x => x.EID == Id).FirstOrDefault();
            var AsignClienttoEmp = db.AsignClienttoEmps.Where(x => x.EID == Id).FirstOrDefault();

            if (clientDetails != null && clientDetails.Count() > 0)
            {
                int btenure = 0;
                int tenure = 0;
                int Ctenure = 0;
                int TotalCtenure = 0;
                decimal paidtillnow = 0;
                decimal ClientBilling = 0;
                decimal Profit_loss = 0;
                foreach (var item in clientDetails)
                {

                    DateTime JD = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                    tenure = (DateTime.Parse(DateTime.Now.ToString()) - JD).Days / 30;
                    paidtillnow = (Convert.ToDecimal(employeeDetails.Salary) * tenure);
                    DateTime p1 = (clientDetails != null ? item.POS.Value : DateTime.Now);
                    DateTime p2 = (clientDetails != null ? item.POE.Value : DateTime.Now);
                    //int Ctenure = ((p2.Year - p1.Year) * 12) + p2.Month - p1.Month;

                    if (employeeDetails.EID == AsignClienttoEmp.EID)
                    {
                        Ctenure = ((p2.Year - p1.Year) * 12) + p2.Month - p1.Month;
                        TotalCtenure = TotalCtenure + Ctenure;

                    }
                    ClientBilling = Convert.ToDecimal(item.ClientBilling);

                }
                btenure = tenure - TotalCtenure;

                decimal bench_exp = Convert.ToDecimal(employeeDetails.Salary);
                decimal bench_expenes = 0;

                if (Bill_details != null)
                {
                    bench_expenes = btenure * (Bill_details.FoodCost.Value + Bill_details.TransportationCost.Value + Bill_details.CubicleCost.Value);
                }
                decimal Clientsal = TotalCtenure * (clientDetails != null ? Convert.ToDecimal(ClientBilling) : 0);
                if (employeeDetails.Salary != null && employeeDetails.Salary > 0 && tenure > 0)
                {
                    decimal Total = paidtillnow;
                }

                Profit_loss = Clientsal - (paidtillnow + bench_expenes);

                return Profit_loss;
            }
            else
            {
                int btenure = 0;
                int tenure = 0;

                DateTime JD = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                tenure = (DateTime.Parse(DateTime.Now.ToString()) - JD).Days / 30;
                decimal paidtillnow = (Convert.ToDecimal(employeeDetails.Salary) * tenure);

                int TotalCtenure = 0;
                btenure = tenure - TotalCtenure;

                decimal bench_exp = Convert.ToDecimal(employeeDetails.Salary);
                decimal bench_expenes = 0;


                if (Bill_details != null)
                {
                    bench_expenes = btenure * (Bill_details.FoodCost.Value + Bill_details.TransportationCost.Value + Bill_details.CubicleCost.Value);
                }
                decimal Profit_loss = (paidtillnow + bench_expenes);

                return Profit_loss;


            }

            model.billDetails = obj;



        }

        public ActionResult GMDetails(int Id)
        {
            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == Id).ToList();
            foreach (var item in data)
            {

                item.Managerprofit_Loss += Managercalculation(item.EmpId);


            }
            return View(data);
        }

        public ActionResult ManagerDetails(int Id)
        {
            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == Id).ToList();
            foreach (var item in data)
            {

                item.Leadprofit += leadCalculation(item.EmpId);


            }
            return View(data);
        }
        public ActionResult LeadDetails(int Id)
        {


            var data = db.EmployeeDetails.Where(x => x.ReportingManagerId == Id).ToList();
            return View(data);

        }


        public ActionResult Details(int Id)
        {
            AsignClienttoEmp obj1 = new AsignClienttoEmp();
            GetAllEployeedetails model = new GetAllEployeedetails();
            List<AsignClienttoEmp> lObj = new List<AsignClienttoEmp>();
            List<Bill> lObj1 = new List<Bill>();
            model.eployeeDetails = db.EmployeeDetails.Where(x => x.EID == Id).FirstOrDefault();
            var clientDetails = db.AsignClienttoEmps.Where(x => x.EID == Id).ToList();
            lObj = clientDetails;
            var billDetails = db.Bills.Where(x => x.EID == Id).ToList();
            lObj1 = billDetails;
            model.clientDetails = lObj;


            List<AddCalculationDetails> obj = new List<AddCalculationDetails>();
            var Bill_details = db.Bills.Where(x => x.EID == Id).FirstOrDefault();
            var employeeDetails = db.EmployeeDetails.Where(x => x.EID == Id).FirstOrDefault();
            var AsignClienttoEmp = db.AsignClienttoEmps.Where(x => x.EID == Id).FirstOrDefault();

            if (clientDetails != null && clientDetails.Count() > 0)
            {
                int btenure = 0;
                int tenure = 0;
                int Ctenure = 0;
                int TotalCtenure = 0;
                decimal paidtillnow = 0;
                decimal ClientBilling = 0;
                foreach (var item in clientDetails)
                {

                    DateTime JD = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                    tenure = (DateTime.Parse(DateTime.Now.ToString()) - JD).Days / 30;
                    paidtillnow = (Convert.ToDecimal(employeeDetails.Salary) * tenure);
                    DateTime p1 = (clientDetails != null ? item.POS.Value : DateTime.Now);
                    DateTime p2 = (clientDetails != null ? item.POE.Value : DateTime.Now);
                    //int Ctenure = ((p2.Year - p1.Year) * 12) + p2.Month - p1.Month;

                    if (employeeDetails.EID == AsignClienttoEmp.EID)
                    {
                        Ctenure = ((p2.Year - p1.Year) * 12) + p2.Month - p1.Month;
                        TotalCtenure = TotalCtenure + Ctenure;

                    }
                    ClientBilling = Convert.ToDecimal(item.ClientBilling);

                }
                btenure = tenure - TotalCtenure;

                decimal bench_exp = Convert.ToDecimal(employeeDetails.Salary);
                decimal bench_expenes = 0;

                if (Bill_details != null)
                {
                    bench_expenes = btenure * (Bill_details.FoodCost.Value + Bill_details.TransportationCost.Value + Bill_details.CubicleCost.Value);
                }
                decimal Clientsal = TotalCtenure * (clientDetails != null ? Convert.ToDecimal(ClientBilling) : 0);
                if (employeeDetails.Salary != null && employeeDetails.Salary > 0 && tenure > 0)
                {
                    decimal Total = paidtillnow;
                }

                decimal Profit_loss = Clientsal - (paidtillnow + bench_expenes);



                obj.Add(new AddCalculationDetails
                {
                    CubicleCost = Bill_details.CubicleCost,
                    FoodCost = Bill_details.FoodCost,
                    TransportationCost = Bill_details.TransportationCost,
                    Salary = Convert.ToDecimal(employeeDetails.Salary),
                    Joiningdate = employeeDetails.Joiningdate,
                    //POE = item.POE,
                    //POS = item.POS,
                    bench_expenes = bench_expenes,
                    Tenure = tenure,
                    BenchTenure = btenure,
                    CTenure = TotalCtenure,
                    //employee_details.Totalexpences = bench_expenes; //employee salary on bench including expenes
                    TotalSalaryTillNow = paidtillnow + bench_expenes,
                    ProfitOrLoss = Profit_loss,


                });
            }
            else
            {
                int btenure = 0;
                int tenure = 0;

                DateTime JD = DateTime.Parse(employeeDetails.Joiningdate.Value.ToString());

                tenure = (DateTime.Parse(DateTime.Now.ToString()) - JD).Days / 30;
                decimal paidtillnow = (Convert.ToDecimal(employeeDetails.Salary) * tenure);

                int TotalCtenure = 0;
                btenure = tenure - TotalCtenure;

                decimal bench_exp = Convert.ToDecimal(employeeDetails.Salary);
                decimal bench_expenes = 0;


                if (Bill_details != null)
                {
                    bench_expenes = btenure * (Bill_details.FoodCost.Value + Bill_details.TransportationCost.Value + Bill_details.CubicleCost.Value);
                }
                decimal Profit_loss = (paidtillnow + bench_expenes);
                obj.Add(new AddCalculationDetails
                {
                    CubicleCost = Bill_details.CubicleCost,
                    FoodCost = Bill_details.FoodCost,
                    TransportationCost = Bill_details.TransportationCost,
                    Salary = Convert.ToDecimal(employeeDetails.Salary),
                    Joiningdate = employeeDetails.Joiningdate,

                    bench_expenes = bench_expenes,
                    Tenure = tenure,
                    BenchTenure = btenure,

                    //employee_details.Totalexpences = bench_expenes; //employee salary on bench including expenes
                    TotalSalaryTillNow = paidtillnow + bench_expenes,
                    ProfitOrLoss = Profit_loss,

                });



            }

            model.billDetails = obj;


            return View(model);


        }


    }
}