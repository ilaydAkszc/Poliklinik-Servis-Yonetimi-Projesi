using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPoliklinik_Servis_Final.Models.Entity;

namespace ProjectPoliklinik_Servis_Final.Models.ViewModel
{
    public class TaburcuViewModel
    {
        public string HastaKayitNo { get; set; }
        public string HastaAd { get; set; }
        public string HastaSoyad { get; set; }
        public string HastaTanisi { get; set; }
        public string Tedavi { get; set; }
        public string DoktorAdSoyad { get; set; }
        public string HemsireAdSoyad { get; set; }
        public int OdaNo { get; set; }
        public int YatakNo { get; set; }
        public DateTime? HastaneCikisTarihi { get; set; }
    }
}