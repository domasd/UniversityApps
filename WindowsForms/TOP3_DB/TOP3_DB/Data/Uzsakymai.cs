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
    
    public partial class Uzsakymai
    {
        public int Id { get; set; }
        public int KlientoId { get; set; }
        public int AbonementoId { get; set; }
        public Nullable<System.DateTime> PirkimoData { get; set; }
    
        public virtual Abonementas Abonementas { get; set; }
        public virtual Klientas Klientas { get; set; }
    }
}
