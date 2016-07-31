using System;
using System.Collections.Generic;
using System.Linq;
using SDCADataRecordImport.Models;

namespace SDCADataRecordImport.Repositories
{
    public class AnimalCountRepository
    {
        public static List<AnimalCount> GetAllForFacilityAndYearAndMonth(Guid facilityID, int year, int month)
        {
            using (var db = new SDCAV2Entities())
            {
                return db.AnimalCounts.Where(i => i.facility_id == facilityID &&
                                                  i.RecordYear == year &&
                                                  i.RecordMonth == month).ToList();
            }
        }
        public static void Save(AnimalCount animalCount)
        {
            using (var db = new SDCAV2Entities())
            {
                db.AnimalCounts.Add(animalCount);
                db.SaveChanges();
            }
        }
    }
}