using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;
namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        PoliklinikEntities db = new PoliklinikEntities();
        public ActionResult Giris()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Giris(string kullaniciadi, string sifre, string role)
        {
            if (role == "Sekreter")
            {
                // Sekreter tablosunda kontrol
                var sekreter = db.sekreter.SingleOrDefault(x => x.kullaniciadi == kullaniciadi && x.sifre == sifre);
                if (sekreter != null)
                {
                    Session["kullaniciID"] = sekreter.kullanici_id;
                    Session["kullaniciAdi"] = sekreter.kullaniciadi;
                    return RedirectToAction("HastaListesi", "Hasta");
                }
                ViewBag.uyari = "Sekreter kullanıcı adı veya şifre hatalı.";
            }
            else if (role == "Doktor")
            {
                // Doktor tablosunda kontrol
                var doktor = db.doktor.SingleOrDefault(x => x.kullaniciAdi == kullaniciadi && x.sifre == sifre);
                if (doktor != null)
                {
                    Session["doktorID"] = doktor.doktor_id;
                    Session["kullaniciAdi"] = doktor.kullaniciAdi;
                    return RedirectToAction("DHastaListesi", "Hasta_Doktor");
                }
                ViewBag.uyari = "Doktor kullanıcı adı veya şifre hatalı.";
            }

            return View();
        }

        public ActionResult SekreterGiris(sekreter Sekreter)
        {
            var login = db.sekreter.Where(x => x.kullaniciadi == Sekreter.kullaniciadi).SingleOrDefault();
            if (login.kullaniciadi == Sekreter.kullaniciadi && login.sifre == Sekreter.sifre)
            {
                Session["kullaniciID"] = login.kullanici_id;
                Session["kullaniciAdi"] = login.kullaniciadi;
                return RedirectToAction("HastaListesi", "Hasta");
            }

            ViewBag.uyari = "Kullanıcı adı veya şifre hatalı";

            return View(Sekreter);
        }
        public ActionResult DoktorGiris(doktor Doktor)
        {
            var login = db.doktor.Where(x => x.kullaniciAdi == Doktor.kullaniciAdi).SingleOrDefault();
            if (login.kullaniciAdi == Doktor.kullaniciAdi && login.sifre == Doktor.sifre)
            {
                Session["doktorID"] = login.doktor_id;
                Session["kullaniciAdi"] = login.kullaniciAdi;
                return RedirectToAction("HastaListesi", "Hasta");
            }
            else
            {
                ViewBag.uyari = "Kullanıcı adı veya şifre hatalı";
            }
            return View(Doktor);
        }
        public ActionResult Cıkıs()
        {
            Session["kullaniciID"] = null;
            Session["kullaniciAdi"] = null;
            Session.Abandon();
            return RedirectToAction("Giris", "Kullanici");
        }
    }
}