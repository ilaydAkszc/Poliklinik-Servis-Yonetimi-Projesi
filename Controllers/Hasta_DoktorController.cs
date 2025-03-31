using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;
using ProjectPoliklinik_Servis_Final.Models.ViewModel;


namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class Hasta_DoktorController : Controller
    {
        // GET: Hasta_Doktor
        PoliklinikEntities db = new PoliklinikEntities();
        public ActionResult DHastaListesi()
        {
            // Aktif hasta kayıtlarını filtrele
            List<HastaMuayeneViewModel> hastaMuayeneListesi = GetHastaMuayeneListesi()
                .Where(h => h.Aktiflik == true) // Aktif olanları filtrele
                .ToList();

            return View(hastaMuayeneListesi);

        }

        public ActionResult HemsireListesi()
        {
            var hemsireler = db.hemsire.ToList();
            return View(hemsireler);


        }
        public List<HastaMuayeneViewModel> GetHastaMuayeneListesi()
        {
            var tümveri = (from m in db.muayene
                           join h in db.hasta on m.hasta_id equals h.hasta_id
                           join hm in db.hemsire on m.hemsire_id equals hm.hemsire_id into hemsireler
                           from hemsire in hemsireler.DefaultIfEmpty()
                           where h.aktiflik == true // Sadece aktif hastalar
                           select new HastaMuayeneViewModel
                           {
                               muayene_id = m.muayene_id,
                               hasta_tanisi = m.hasta_tanisi,
                               hastakayitno = h.hastakayitno,
                               ad = h.ad,
                               soyad = h.soyad,
                               HemsireAdSoyad = hemsire != null ? hemsire.ad + " " + hemsire.soyad : "N/A", // Eğer hemşire varsa, adını ve soyadını al, yoksa "N/A" yaz
                               oda_id = m.oda_id,
                               yatak_id = m.yatak_id,
                               tedavi = m.tedavi,
                                Aktiflik = h.aktiflik
                           }).ToList();

            return tümveri;
        }

        // GET: Hasta/Edit/5
        public ActionResult DGüncelle(int id)
        {
            var hst = db.hasta.Find(id); // ID ile hastayı bul
            if (hst == null)
            {
                return HttpNotFound();
            }

            // Hasta bilgileri ile birlikte View döndür


            return View(hst);
        }

        // POST: Hasta/Edit/5
        [HttpPost]
        public ActionResult DGüncelle(int id, muayene updatedHasta, string hasta_tanisi, string tedavi)
        {
            try
            {
                // Veritabanından ilgili hastayı bul
                var hst = db.hasta.Find(id);
                if (hst == null)
                {
                    return HttpNotFound();
                }

                // Hasta tanı ve tedavi bilgilerini muayene tablosunda güncelle
                var muayene = db.muayene.FirstOrDefault(m => m.hasta_id == hst.hasta_id);
                if (muayene != null)
                {
                    muayene.hasta_tanisi = hasta_tanisi;
                    muayene.tedavi = tedavi;
                }

                db.SaveChanges();

                return RedirectToAction("DHastaListesi");
            }
            catch
            {

                return View("DGüncelle");
            }
        }
    }
}