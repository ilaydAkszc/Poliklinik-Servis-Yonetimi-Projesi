//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectPoliklinik_Servis_Final.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class hasta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public hasta()
        {
            this.muayene = new HashSet<muayene>();
        }
    
        public string hastakayitno { get; set; }
        public string ad { get; set; }
        public string soyad { get; set; }
        public string tel { get; set; }
        public string adres { get; set; }
        public Nullable<System.DateTime> hastanegiristarihi { get; set; }
        public System.DateTime dob { get; set; }
        public int hasta_id { get; set; }
        public bool aktiflik { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<muayene> muayene { get; set; }
    }
}
