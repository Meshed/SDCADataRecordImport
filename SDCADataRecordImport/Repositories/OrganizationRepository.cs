using System.Linq;
using SDCADataRecordImport.Models;

namespace SDCADataRecordImport.Repositories
{
    public class OrganizationRepository
    {
        public static Organization FindByEIN(string ein)
        {
            using (var db = new SDCAV2Entities())
            {
                return db.Organizations.FirstOrDefault(i => i.EIN == ein);
            }
        }
    }
}