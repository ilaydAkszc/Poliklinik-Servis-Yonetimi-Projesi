using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;
using ProjectPoliklinik_Servis_Final.Models.ViewModel;

namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class DoktorHemsireController : Controller
    {
        // GET: DoktorHemsire
        PoliklinikEntities db = new PoliklinikEntities();
        public ActionResult DoktorHemsireListesi()
        {

            var doktorlar = db.doktor.ToList();
            var hemsireler = db.hemsire.ToList();
            var viewModel = new DoktorHemsireViewModel
            {
                doktor = doktorlar,
                hemsire = hemsireler


            };
            return View(viewModel);

        }
    }
}