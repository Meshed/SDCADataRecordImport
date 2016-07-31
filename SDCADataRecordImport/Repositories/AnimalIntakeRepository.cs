using System;
using System.Collections.Generic;
using System.Linq;
using SDCADataRecordImport.Models;

namespace SDCADataRecordImport.Repositories
{
    public class AnimalIntakeRepository
    {
        public static List<AnimalIntake> GetAllForActiveFacilityAndYearAndMonth(Guid facilityID, int year, int month)
        {
            using (var db = new SDCAV2Entities())
            {
                return db.AnimalIntakes.Where(i => i.facility_id == facilityID &&
                                                   i.RecordYear == year &&
                                                   i.RecordMonth == month).ToList();
            }
        }
        public static void Save(AnimalIntake animalIntake)
        {
            using (var db = new SDCAV2Entities())
            {
                db.AnimalIntakes.Add(animalIntake);
                db.SaveChanges();
            }
        }
    }
}