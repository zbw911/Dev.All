using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dev.Demo.Web.Controllers
{
    using Dev.Demo.Application.MainBoundedContext.TestModule;

    public class fasdfasdfasdfasdfController : Controller
    {

         private ITestService TestService;



         public fasdfasdfasdfasdfController(ITestService TestService)
        {

            this.TestService = TestService;
        }
        //
        // GET: /fasdfasdfasdfasdf/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /fasdfasdfasdfasdf/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /fasdfasdfasdfasdf/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /fasdfasdfasdfasdf/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /fasdfasdfasdfasdf/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /fasdfasdfasdfasdf/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /fasdfasdfasdfasdf/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /fasdfasdfasdfasdf/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
