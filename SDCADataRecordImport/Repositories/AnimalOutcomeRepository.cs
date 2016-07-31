using System;
using System.Collections.Generic;
using System.Linq;
using SDCADataRecordImport.Models;

namespace SDCADataRecordImport.Repositories
{
    public class AnimalOutcomeRepository
    {
        public static List<AnimalOutcome> GetAllForFacilityAndYearAndMonth(Guid facilityID, int year, int month)
        {
            using (var db = new SDCAV2Entities())
            {
                return db.AnimalOutcomes.Where(i => i.facility_id == facilityID &&
                                                    i.RecordYear == year &&
                                                    i.RecordMonth == month).ToList();
            }
        }
        public static void Save(AnimalOutcome animalOutcome)
        {
            using (var db = new SDCAV2Entities())
            {
                db.AnimalOutcomes.Add(animalOutcome);
                db.SaveChanges();
            }
        }
    }
}