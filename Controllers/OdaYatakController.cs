using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;

namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class OdaYatakController : Controller
    {
        // GET: OdaYatak
        PoliklinikEntities db = new PoliklinikEntities();
        public ActionResult OdaYatakListesi()
        {

            var yataklar = db.yatak.ToList();
            return View(yataklar);
        }

        public void GuncelleOdaVeYatakDurumu(int yatakId)
        {
            // Yatak tablosunda seçilen yatak bulunuyor
            var yatak = db.yatak.FirstOrDefault(y => y.yatak_id == yatakId);

            if (yatak != null)
            {
                // Yatağın doluluk durumunu güncelle
                yatak.doluluk = true;
                db.SaveChanges();

                // İlgili odadaki yatakların durumunu kontrol et
                var odaId = yatak.oda_id;
                var odadakiYataklar = db.yatak.Where(y => y.oda_id == odaId).ToList();

                // Eğer odadaki tüm yataklar doluysa odanın doluluk durumunu güncelle
                if (odadakiYataklar.All(y => y.doluluk == true))
                {
                    var oda = db.oda.FirstOrDefault(o => o.oda_id == odaId);
                    if (oda != null)
                    {
                        oda.doluluk = true;
                        db.SaveChanges();
                    }
                }
            }

        }
        public  void SilinenOdaVeYatakDurumu(int yatakId, int odaId)
        {
            // Yatak tablosundaki doluluk durumunu güncelle
            var ytk = db.yatak.FirstOrDefault(y => y.yatak_id == yatakId);
            if (ytk != null)
            {
                ytk.doluluk = false; // Yatak boş oldu
                db.SaveChanges();
            }

            // Oda tablosundaki doluluk durumunu güncelle
            var odadakiYataklar = db.yatak.Where(y => y.oda_id == odaId).ToList();
            var o = db.oda.FirstOrDefault(a => a.oda_id == odaId);
            if (o != null)
            {
                // Eğer odadaki tüm yataklar boşsa, odanın doluluk durumunu false yapıyoruz
                if (odadakiYataklar.All(y => y.doluluk == true))
                {
                    o.doluluk = true; // Oda boş oldu
                   
                }
                else
                {
                    o.doluluk = false;
                }
                db.SaveChanges();
            }
        }

        public JsonResult GetBosYataklarByOdaId(int odaId)
        {
            // Oda ID'sine göre boş yatakları filtrele
            var bosYataklar = db.yatak
                                .Where(y => y.oda_id == odaId && y.doluluk == false)
                                .Select(y => new
                                {
                                    yatakId = y.yatak_id,
                                    yatakAdi = "Yatak " + y.yatak_id
                                })
                                .ToList();

            return Json(bosYataklar, JsonRequestBehavior.AllowGet);
        }


    }
}