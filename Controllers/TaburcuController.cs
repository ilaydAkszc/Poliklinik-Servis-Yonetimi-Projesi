using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectPoliklinik_Servis_Final.Models.Entity;
using ProjectPoliklinik_Servis_Final.Models.ViewModel;


namespace ProjectPoliklinik_Servis_Final.Controllers
{
    public class TaburcuController : Controller
    {
        // GET: Taburcu
        PoliklinikEntities db = new PoliklinikEntities();
        public ActionResult TaburcuListesi()
        {
            var taburcuListesi = (from h in db.hasta
                                  join m in db.muayene on h.hasta_id equals m.hasta_id
                                  join d in db.doktor on m.doktor_id equals d.doktor_id
                                  join hs in db.hemsire on m.hemsire_id equals hs.hemsire_id
                                  join o in db.oda on m.oda_id equals o.oda_id
                                  join y in db.yatak on m.yatak_id equals y.yatak_id
                                  where h.aktiflik == false
                                  select new TaburcuViewModel
                                  {
                                      HastaKayitNo = h.hastakayitno,
                                      HastaAd = h.ad,
                                      HastaSoyad = h.soyad,
                                      HastaTanisi = m.hasta_tanisi,
                                      Tedavi = m.tedavi,
                                      DoktorAdSoyad = d.ad + " " + d.soyad,
                                      HemsireAdSoyad = hs.ad + " " + hs.soyad,
                                      OdaNo = o.oda_id,
                                      YatakNo = y.yatak_id,
                                      HastaneCikisTarihi = m.hastanecikistarihi
                                  }).ToList();

            return View(taburcuListesi);
        }
          
    }
}