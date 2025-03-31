using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;
using ProjectPoliklinik_Servis_Final.Models.ViewModel;
using ProjectPoliklinik_Servis_Final.Controllers;

namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class HastaController : Controller
    {
        PoliklinikEntities db = new PoliklinikEntities();
        // GET: Hasta
        public ActionResult HastaListesi()
        {
            var aktifHastalar = db.hasta
        .Where(h => h.aktiflik)
        .ToList();

            return View(aktifHastalar);
        }

        // GET: Hasta/Details/5
        public ActionResult Detay(int? hasta_id)
        {


            if (!hasta_id.HasValue)
            {
                return HttpNotFound("Hasta ID bulunamadı."); // Hata sayfası döndür
            }
            var hasta = db.hasta.FirstOrDefault(h => h.hasta_id == hasta_id.Value);
            if (hasta == null)
            {
                return HttpNotFound("Hasta bulunamadı.");// Hasta bulunamazsa hata sayfası döndürün
            }

            var tümveri = (from m in db.muayene
                           join h in db.hasta on m.hasta_id equals h.hasta_id
                           join d in db.doktor on m.doktor_id equals d.doktor_id into doktorlar
                           from doktor in doktorlar.DefaultIfEmpty()
                           join hm in db.hemsire on m.hemsire_id equals hm.hemsire_id into hemsireler
                           from hemsire in hemsireler.DefaultIfEmpty()
                           where h.hasta_id == hasta_id.Value // Hasta ID'sine göre filtreleme
                           select new HastaMuayeneViewModel
                           {
                               muayene_id = m.muayene_id,
                               hasta_tanisi = m.hasta_tanisi,
                               hastakayitno = h.hastakayitno,
                               ad = h.ad,
                               soyad = h.soyad,
                               DoktorAdSoyad = doktor != null ? doktor.ad + " " + doktor.soyad : "N/A",
                               HemsireAdSoyad = hemsire != null ? hemsire.ad + " " + hemsire.soyad : "N/A",
                               oda_id = m.oda_id,
                               yatak_id = m.yatak_id,
                               tedavi = m.tedavi

                           }).FirstOrDefault();
            return View(tümveri);
        }

        // GET: Hasta/Create
        public ActionResult HastaEkle()
        {
            // Müsait doktor ve hemşireleri getir
            ViewBag.MüsaitDoktorlar = db.doktor
                                         .Where(d => d.musaitlik_d == true)
                                         .Select(d => new SelectListItem
                                         {
                                             Value = d.doktor_id.ToString(),
                                             Text = d.ad + " " + d.soyad
                                         })
                                         .ToList();

            ViewBag.MüsaitHemşireler = db.hemsire
                                          .Where(h => h.musaitlik_d == true)
                                          .Select(h => new SelectListItem
                                          {
                                              Value = h.hemsire_id.ToString(),
                                              Text = h.ad + " " + h.soyad
                                          })
                                          .ToList();

            // Müsait odaları getir
            ViewBag.MüsaitOdalar = db.oda
                                      .Where(o => o.doluluk == false)
                                      .Select(o => new SelectListItem
                                      {
                                          Value = o.oda_id.ToString(),
                                          Text = "Oda " + o.oda_id
                                      })
                                      .ToList();

            return View();
        }
        // POST: Hasta/Create
        [HttpPost]
        public ActionResult HastaEkle(hasta h, int hemsire_id, int oda_id, int yatak_id, string hasta_tanisi, string tedavi)
        {
            try
            {
                // Hasta giriş tarihini o günün tarihi olarak ata
                h.hastanegiristarihi = DateTime.Now;

                // Yeni hasta eklenirken aktiflik durumunu true yap
                h.aktiflik = true;
                

                // Hasta tablosuna veri ekle
                db.hasta.Add(h);
                db.SaveChanges();

               


                // Oda ve yatak durumlarını güncellemek için OdaYatakController'dan çağır
                var odaYatakController = new OdaYatakController();
                odaYatakController.GuncelleOdaVeYatakDurumu(yatak_id);

                // Muayene tablosuna veri ekle
                var muayene = new muayene
                {
                    hasta_tanisi = hasta_tanisi,
                    tedavi = tedavi,
                    hastakayitno = h.hastakayitno,
                    doktor_id = (int)(db.doktor.FirstOrDefault(d => d.musaitlik_d == true)?.doktor_id), // Müsait doktor seçimi
                    hemsire_id = hemsire_id,
                    oda_id = oda_id,
                    yatak_id = yatak_id,
                    hasta_id = h.hasta_id
                };
                db.muayene.Add(muayene);
                db.SaveChanges(); // Muayene kaydını kaydet

                // Başarıyla işlemi tamamladıktan sonra hasta listesine yönlendir
                return RedirectToAction("HastaListesi"); // Hasta listesi veya ana sayfa yönlendirmesi
            }
            catch (Exception ex)
            {
                // Hata mesajını ayrıntılı şekilde yazdır
         
                ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
                return View(h);  // Formu tekrar gönder
            }

        }

        // GET: Hasta/Edit/5
        public ActionResult Güncelle(int? id)

        {
            if (!id.HasValue)
            {
                return HttpNotFound("Hasta ID bulunamadı."); // Hata sayfası döndür
            }
          
            var hasta = db.hasta.FirstOrDefault(h => h.hasta_id == id.Value);
            if (hasta == null)
            {
                return HttpNotFound("Hasta bulunamadı.");// Hasta bulunamazsa hata sayfası döndürün
            }
            // Hasta bilgilerini ve ilgili muayene detaylarını al
            var hastaMuayene = (from m in db.muayene
                                join h in db.hasta on m.hasta_id equals h.hasta_id
                                join d in db.doktor on m.doktor_id equals d.doktor_id into doktorlar
                                from doktor in doktorlar.DefaultIfEmpty()
                                join hm in db.hemsire on m.hemsire_id equals hm.hemsire_id into hemsireler
                                from hemsire in hemsireler.DefaultIfEmpty()
                                where h.hasta_id == id.Value // Hasta ID'sine göre filtreleme
                                select new HastaMuayeneViewModel
                                {
                                    muayene_id = m.muayene_id,
                                    hasta_tanisi = m.hasta_tanisi,
                                    hastakayitno = h.hastakayitno,
                                    ad = h.ad,
                                    soyad = h.soyad,
                                    DoktorAdSoyad = doktor != null ? doktor.ad + " " + doktor.soyad : "N/A",
                                    HemsireAdSoyad = hemsire != null ? hemsire.ad + " " + hemsire.soyad : "N/A",
                                    oda_id = m.oda_id,
                                    yatak_id = m.yatak_id,
                                    tedavi = m.tedavi

                                }).FirstOrDefault();

            if (hastaMuayene == null)
            {
                return HttpNotFound("Hasta bulunamadı.");
            }

            return View(hastaMuayene); // Güncelleme ekranına verileri gönder


        }

        // POST: Hasta/Edit/5
        [HttpPost]
        public ActionResult Güncelle(int id, hasta updatedHasta, string hasta_tanisi, string tedavi, int oda_id, int yatak_id, int doktor_id, int hemsire_id)
        {
            try
            {
                // İlgili hastayı veritabanında bul
                var hasta = db.hasta.Find(id);
                if (hasta == null)
                {
                    return HttpNotFound("Hasta bulunamadı.");
                }

                // Hasta bilgilerini güncelle
                hasta.ad = updatedHasta.ad;
                hasta.soyad = updatedHasta.soyad;
                hasta.tel = updatedHasta.tel;
                hasta.adres = updatedHasta.adres;
                hasta.hastanegiristarihi = updatedHasta.hastanegiristarihi;
                hasta.dob = updatedHasta.dob;

                // İlgili muayene kaydını bul ve güncelle
                var muayeneb = db.muayene.FirstOrDefault(m => m.hasta_id == hasta.hasta_id);
                if (muayeneb != null)
                {
                    muayeneb.hasta_tanisi = hasta_tanisi;
                    muayeneb.tedavi = tedavi;
                    muayeneb.oda_id = oda_id;
                    muayeneb.yatak_id = yatak_id;
                    muayeneb.doktor_id = doktor_id;
                    muayeneb.hemsire_id = hemsire_id;
                }

                // Yatak bilgilerini güncelle
                var yatak = db.yatak.Find(yatak_id);
                if (yatak != null)
                {
                    yatak.doluluk = true; // Yatak dolu olarak işaretlendi
                }

                // İlgili odadaki tüm yatakların doluluk durumunu kontrol et
                var oda = db.oda.Find(oda_id);
                if (oda != null)
                {
                    // İlgili odadaki tüm yatakları getir
                    var yataklar = db.yatak.Where(y => y.oda_id == oda.oda_id).ToList();

                    // Eğer odadaki tüm yataklar doluysa, oda dolu olarak işaretlenir
                    if (yataklar.All(y => y.doluluk))
                    {
                        oda.doluluk = true; // Oda dolu
                    }
                    else
                    {
                        oda.doluluk = false; // Oda boş
                    }
                }

                db.SaveChanges(); // Tüm değişiklikleri kaydet
                return RedirectToAction("HastaListesi");

            }
            catch

            {

                ViewBag.ErrorMessage = "Hasta güncellenemedi";
                return View("Güncelle","Hasta");

            }
        }

   
       
            // POST: Hasta/Delete/5
         [HttpPost]
        public ActionResult Sil(int id)
        {
            try
            {
                if (id <= 0)
                {
                    ModelState.AddModelError("", "Geçersiz hasta ID.");
                    return RedirectToAction("HastaListesi", "Hasta");
                }
                // Hasta kaydını bul
                var hastab = db.hasta.FirstOrDefault(h => h.hasta_id == id);
                if (hastab == null)
                {
                    return RedirectToAction("HastaListesi","Hasta");
                }

                // Hasta kaydına bağlı muayene kaydını bul
                var muayeneb = db.muayene.FirstOrDefault(m => m.hasta_id == id);
                if (muayeneb == null)
                {
                    return RedirectToAction("HastaListesi","Hasta");
                }

                // Aktiflik durumunu güncelle
                hastab.aktiflik = false;

                // Muayene tablosundaki hastane çıkış tarihini güncelle
                muayeneb.hastanecikistarihi = DateTime.Now;

                // Oda ve yatak doluluk durumlarını güncelle
                var odaId =(int) muayeneb.oda_id;
                var yatakId =(int) muayeneb.yatak_id;


                OdaYatakController odaYatakController = new OdaYatakController();
                odaYatakController.SilinenOdaVeYatakDurumu(yatakId, odaId);


                db.SaveChanges();

                return RedirectToAction("HastaListesi","Hasta");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Hata oluştu: " + ex.Message);
                return RedirectToAction("HastaListesi");
            }
        }

        public JsonResult GelenHastaSayisi()
        {
            // Güncel tarihi alalım
            DateTime today = DateTime.Today;

            // Tarih karşılaştırmasını sadece tarih kısmına göre yap
            var hastaSayisi = db.hasta.Count(h => DbFunctions.TruncateTime(h.hastanegiristarihi) == today);

            // Sonucu JSON olarak döndür
            return Json(hastaSayisi, JsonRequestBehavior.AllowGet);
        }
    }
}
