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
    
    public partial class AnimalOutcome
    {
        public System.Guid id { get; set; }
        public System.Guid facility_id { get; set; }
        public System.Guid user_id { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> RecordStatus { get; set; }
        public int RecordYear { get; set; }
        public int RecordMonth { get; set; }
        public Nullable<int> AdultC_Adoption { get; set; }
        public Nullable<int> UT5MC_Adoption { get; set; }
        public Nullable<int> AdultF_Adoption { get; set; }
        public Nullable<int> UT5MF_Adoption { get; set; }
        public Nullable<int> AdultC_ReturnedToOwner { get; set; }
        public Nullable<int> UT5MC_ReturnedToOwner { get; set; }
        public Nullable<int> AdultF_ReturnedToOwner { get; set; }
        public Nullable<int> UT5MF_ReturnedToOwner { get; set; }
        public Nullable<int> AdultC_TransferredToAnotherAgency { get; set; }
        public Nullable<int> UT5MC_TransferredToAnotherAgency { get; set; }
        public Nullable<int> AdultF_TransferredToAnotherAgency { get; set; }
        public Nullable<int> UT5MF_TransferredToAnotherAgency { get; set; }
        public Nullable<int> AdultC_ReturnedToField { get; set; }
        public Nullable<int> UT5MC_ReturnedToField { get; set; }
        public Nullable<int> Adultf_ReturnedToField { get; set; }
        public Nullable<int> UT5Mf_ReturnedToField { get; set; }
        public Nullable<int> AdultC_OtherLiveOutcome { get; set; }
        public Nullable<int> UT5MC_OtherLiveOutcome { get; set; }
        public Nullable<int> AdultF_OtherLiveOutcome { get; set; }
        public Nullable<int> UT5MF_OtherLiveOutcome { get; set; }
        public Nullable<int> AdultC_DiedInCare { get; set; }
        public Nullable<int> UT5MC_DiedInCare { get; set; }
        public Nullable<int> AdultF_DiedInCare { get; set; }
        public Nullable<int> UT5MF_DiedInCare { get; set; }
        public Nullable<int> AdultC_LostInCare { get; set; }
        public Nullable<int> UT5MC_LostInCare { get; set; }
        public Nullable<int> AdultF_LostInCare { get; set; }
        public Nullable<int> UT5MF_LostInCare { get; set; }
        public Nullable<int> AdultC_ShelterEuthanasia { get; set; }
        public Nullable<int> UT5MC_ShelterEuthanasia { get; set; }
        public Nullable<int> AdultF_ShelterEuthanasia { get; set; }
        public Nullable<int> UT5MF_ShelterEuthanasia { get; set; }
        public Nullable<int> AdultC_OwnerIntendedEuthanasia { get; set; }
        public Nullable<int> UT5MC_OwnerIntendedEuthanasia { get; set; }
        public Nullable<int> Adultf_OwnerIntendedEuthanasia { get; set; }
        public Nullable<int> UT5Mf_OwnerIntendedEuthanasia { get; set; }
        public Nullable<int> AgeUnknownC_Adoption { get; set; }
        public Nullable<int> AgeUnknownF_Adoption { get; set; }
        public Nullable<int> AgeUnknownC_ReturnedToOwner { get; set; }
        public Nullable<int> AgeUnknownF_ReturnedToOwner { get; set; }
        public Nullable<int> AgeUnknownC_TransferredToAnotherAgency { get; set; }
        public Nullable<int> AgeUnknownF_TransferredToAnotherAgency { get; set; }
        public Nullable<int> AgeUnknownC_ReturnedToField { get; set; }
        public Nullable<int> AgeUnknownF_ReturnedToField { get; set; }
        public Nullable<int> AgeUnknownC_OtherLiveOutcome { get; set; }
        public Nullable<int> AgeUnknownF_OtherLiveOutcome { get; set; }
        public Nullable<int> AgeUnknownC_DiedInCare { get; set; }
        public Nullable<int> AgeUnknownF_DiedInCare { get; set; }
        public Nullable<int> AgeUnknownC_LostInCare { get; set; }
        public Nullable<int> AgeUnknownF_LostInCare { get; set; }
        public Nullable<int> AgeUnknownC_ShelterEuthanasia { get; set; }
        public Nullable<int> AgeUnknownF_ShelterEuthanasia { get; set; }
        public Nullable<int> AgeUnknownC_OwnerIntendedEuthanasia { get; set; }
        public Nullable<int> AgeUnknownF_OwnerIntendedEuthanasia { get; set; }
    
        public virtual Org_Facility Org_Facility { get; set; }
    }
}