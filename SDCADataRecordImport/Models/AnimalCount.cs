//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDCADataRecordImport.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AnimalCount
    {
        public System.Guid id { get; set; }
        public System.Guid facility_id { get; set; }
        public System.Guid user_id { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public int RecordYear { get; set; }
        public int RecordMonth { get; set; }
        public Nullable<int> AdultC { get; set; }
        public Nullable<int> AdultF { get; set; }
        public Nullable<int> UT5MC { get; set; }
        public Nullable<int> UT5MF { get; set; }
        public Nullable<int> AgeUnknownC { get; set; }
        public Nullable<int> AgeUnknownF { get; set; }
        public string AnimalCountType { get; set; }
    
        public virtual Org_Facility Org_Facility { get; set; }
    }
}
