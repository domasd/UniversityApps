//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TOP3_DB.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Treneris
    {
        public Treneris()
        {
            this.Klientas = new HashSet<Klientas>();
        }
    
        public int AsmensKodas { get; set; }
        public string Adresas { get; set; }
        public string GimimoData { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Specialybe { get; set; }
        public string Patirtis { get; set; }
    
        public virtual ICollection<Klientas> Klientas { get; set; }
    }
}