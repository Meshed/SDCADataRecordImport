using System;
using System.Collections.Generic;
using System.Linq;
using SDCADataRecordImport.Models;

namespace SDCADataRecordImport.Repositories
{
    public class FacilityRepository
    {
        public static List<Org_Facility> GetAllFacilitiesForOrganization(Guid orgID)
        {
            using (var db = new SDCAV2Entities())
            {
                return db.Org_Facility.Where(i => i.Org_id == orgID).ToList();
            }
        }
    }
}