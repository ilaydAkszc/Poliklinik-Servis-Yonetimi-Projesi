using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPoliklinik_Servis_Final.Models.Entity;
namespace ProjectPoliklinik_Servis_Final.Models.ViewModel
{
    public class HastaMuayeneViewModel
    {
        public int hasta_id { get; set; }
        public int muayene_id { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string hastakayitno { get; set; }
        public string hasta_tanisi { get; set; }
        public string tedavi { get; set; }
        public string DoktorAdSoyad { get; set; }
        public string HemsireAdSoyad { get; set; }
        public int? oda_id { get; set; }
        public int? yatak_id { get; set; }
        public DateTime hastanegiristarihi { get; set; }
        public string adres { get; set; }

        public string tel { get; set; }
        public DateTime dob { get; set; }
        public bool Aktiflik { get; set; }
    
    }
}