using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjectPoliklinik_Servis_Final.Models.Entity;

namespace ProjectPoliklinik_Servis_Final.Models.ViewModel
{
    public class DoktorHemsireViewModel
    {
        public IEnumerable<doktor> doktor { get; set; }
        public IEnumerable<hemsire> hemsire { get; set; }
    }
}