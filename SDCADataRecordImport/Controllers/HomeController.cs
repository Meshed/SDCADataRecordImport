using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualBasic.FileIO;
using SDCADataRecordImport.Models;
using SDCADataRecordImport.Repositories;

namespace SDCADataRecordImport.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            var errorFile = new StringBuilder();

            SetupErrorFile(ref errorFile);

            if (file != null && file.ContentLength > 0)
            {
                using (var parser = new TextFieldParser(file.InputStream))
                {
                    parser.SetDelimiters(new[] {","});
                    //parser.HasFieldsEnclosedInQuotes = false;

                    // Skip over header line
                    parser.ReadLine();

                    // Loop through the file
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();

                        if (fields != null)
                        {
                            // Get organization
                            string ein = fields[0];

                            var organization = OrganizationRepository.FindByEIN(ein);

                            if (organization != null)
                            {
                                // 15 is the cell index that starts the data fields staring with Record Year
                                var organizationFacilities = FacilityRepository.GetAllFacilitiesForOrganization(organization.id);

                                if (organizationFacilities.Count == 0)
                                {
                                    AddErrorRecord(ref errorFile, fields.ToList(), "No facilities found for the organization");
                                }
                                else if (organizationFacilities.Count > 1)
                                {
                                    AddErrorRecord(ref errorFile, fields.ToList(), "More than one facility found for the organization");
                                }
                                else if(organizationFacilities.Count == 1)
                                {
                                    // Look for existing record
                                    int recordYear = int.Parse(fields[15]);
                                    int recordMonth = int.Parse(fields[16]);
                                    Guid facilityID = organizationFacilities.First().id;

                                    if (DataRecordsExist(facilityID, recordYear, recordMonth))
                                    {
                                        AddErrorRecord(ref errorFile, fields.ToList(), "Animal data records already exist for this month and year");
                                    }
                                    else
                                    {
                                        try
                                        {
                                            // Create animal data records
                                            AnimalIntake animalIntake;
                                            AnimalOutcome animalOutcome;
                                            AnimalCount beginningAnimalCount;
                                            AnimalCount endingManualAnimalCount;
                                            AnimalCount endingCalculatedAnimalCount;

                                            CreateAnimalDataRecords(facilityID, recordYear, recordMonth, fields, out animalIntake, out animalOutcome, out beginningAnimalCount, out endingManualAnimalCount, out endingCalculatedAnimalCount);

                                            // Save the data records
                                            try
                                            {
                                                SaveAnimalDataRecords(animalIntake, animalOutcome, beginningAnimalCount, endingManualAnimalCount, endingCalculatedAnimalCount);
                                            }
                                            catch (Exception)
                                            {
                                                AddErrorRecord(ref errorFile, fields.ToList(), "Error saving the data records");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            AddErrorRecord(ref errorFile, fields.ToList(), "Invalid data format");
                                        }

                                    }
                                }
                            }
                            else
                            {
                                AddErrorRecord(ref errorFile, fields.ToList(), "Organization not found");
                            }
                        }
                    }
                }
            }

            return File(new UTF8Encoding().GetBytes(errorFile.ToString()), "text/csv", "import-errors.csv");
        }

        private static void SaveAnimalDataRecords(AnimalIntake animalIntake, AnimalOutcome animalOutcome, AnimalCount beginningAnimalCount, AnimalCount endingManualAnimalCount, AnimalCount endingCalculatedAnimalCount)
        {
            AnimalIntakeRepository.Save(animalIntake);
            AnimalOutcomeRepository.Save(animalOutcome);
            AnimalCountRepository.Save(beginningAnimalCount);
            AnimalCountRepository.Save(endingManualAnimalCount);
            AnimalCountRepository.Save(endingCalculatedAnimalCount);
        }
        private static void CreateAnimalDataRecords(Guid facilityID, int recordYear, int recordMonth, string[] fields, out AnimalIntake animalIntake, out AnimalOutcome animalOutcome, out AnimalCount beginningAnimalCount, out AnimalCount endingManualAnimalCount, out AnimalCount endingCalculatedAnimalCount)
        {
            animalIntake = CreateAnimalIntakeRecord(facilityID, recordYear, recordMonth, fields);
            animalOutcome = CreateAnimalOutcomeRecord(facilityID, recordYear, recordMonth, fields);
            beginningAnimalCount = CreateBeginningAnimalCountRecord(facilityID, recordYear, recordMonth, fields);
            endingManualAnimalCount = CreateEndingManualAnimalCountRecord(facilityID, recordYear, recordMonth, fields);
            endingCalculatedAnimalCount = CreateEndingCalculatedAnimalCountRecord(facilityID, recordYear, recordMonth, fields);
        }
        private static AnimalCount CreateBeginningAnimalCountRecord(Guid facilityID, int recordYear, int recordMonth, string[] fields)
        {
            var animalCount = new AnimalCount
            {
                facility_id = facilityID,
                DateCreated = DateTime.Now,
                RecordYear = recordYear,
                RecordMonth = recordMonth,
                RecordStatus = 1,
                AnimalCountType = fields[101],
                AdultC = int.Parse(fields[102]),
                UT5MC = int.Parse(fields[103]),
                AgeUnknownC = int.Parse(fields[104]),
                AdultF = int.Parse(fields[105]),
                UT5MF = int.Parse(fields[106]),
                AgeUnknownF = int.Parse(fields[107]),
                id = Guid.NewGuid()
            };

            return animalCount;
        }
        private static AnimalCount CreateEndingManualAnimalCountRecord(Guid facilityID, int recordYear, int recordMonth, string[] fields)
        {
            var animalCount = new AnimalCount
            {
                facility_id = facilityID,
                DateCreated = DateTime.Now,
                RecordYear = recordYear,
                RecordMonth = recordMonth,
                RecordStatus = 1,
                AnimalCountType = fields[108],
                AdultC = int.Parse(fields[109]),
                UT5MC = int.Parse(fields[110]),
                AgeUnknownC = int.Parse(fields[111]),
                AdultF = int.Parse(fields[112]),
                UT5MF = int.Parse(fields[113]),
                AgeUnknownF = int.Parse(fields[114]),
                id = Guid.NewGuid()
            };

            return animalCount;
        }
        private static AnimalCount CreateEndingCalculatedAnimalCountRecord(Guid facilityID, int recordYear, int recordMonth, string[] fields)
        {
            var animalCount = new AnimalCount
            {
                facility_id = facilityID,
                DateCreated = DateTime.Now,
                RecordYear = recordYear,
                RecordMonth = recordMonth,
                RecordStatus = 1,
                AnimalCountType = fields[115],
                AdultC = int.Parse(fields[116]),
                UT5MC = int.Parse(fields[117]),
                AgeUnknownC = int.Parse(fields[118]),
                AdultF = int.Parse(fields[119]),
                UT5MF = int.Parse(fields[120]),
                AgeUnknownF = int.Parse(fields[121]),
                id = Guid.NewGuid()
            };

            return animalCount;
        }
        private static AnimalOutcome CreateAnimalOutcomeRecord(Guid facilityID, int recordYear, int recordMonth, string[] fields)
        {
            var animalOutcome = new AnimalOutcome
            {
                facility_id = facilityID,
                DateCreated = DateTime.Now,
                RecordStatus = 1,
                RecordYear = recordYear,
                RecordMonth = recordMonth,
                AdultC_Adoption = int.Parse(fields[47]),
                UT5MC_Adoption = int.Parse(fields[48]),
                AgeUnknownC_Adoption = int.Parse(fields[49]),
                AdultF_Adoption = int.Parse(fields[50]),
                UT5MF_Adoption = int.Parse(fields[51]),
                AgeUnknownF_Adoption = int.Parse(fields[52]),
                AdultC_ReturnedToOwner = int.Parse(fields[53]),
                UT5MC_ReturnedToOwner = int.Parse(fields[54]),
                AgeUnknownC_ReturnedToOwner = int.Parse(fields[55]),
                AdultF_ReturnedToOwner = int.Parse(fields[56]),
                UT5MF_ReturnedToOwner = int.Parse(fields[57]),
                AgeUnknownF_ReturnedToOwner = int.Parse(fields[58]),
                AdultC_TransferredToAnotherAgency = int.Parse(fields[59]),
                UT5MC_TransferredToAnotherAgency = int.Parse(fields[60]),
                AgeUnknownC_TransferredToAnotherAgency = int.Parse(fields[61]),
                AdultF_TransferredToAnotherAgency = int.Parse(fields[62]),
                UT5MF_TransferredToAnotherAgency = int.Parse(fields[63]),
                AgeUnknownF_TransferredToAnotherAgency = int.Parse(fields[64]),
                AdultC_ReturnedToField = int.Parse(fields[65]),
                UT5MC_ReturnedToField = int.Parse(fields[66]),
                AgeUnknownC_ReturnedToField = int.Parse(fields[67]),
                Adultf_ReturnedToField = int.Parse(fields[68]),
                UT5Mf_ReturnedToField = int.Parse(fields[69]),
                AgeUnknownF_ReturnedToField = int.Parse(fields[70]),
                AdultC_OtherLiveOutcome = int.Parse(fields[71]),
                UT5MC_OtherLiveOutcome = int.Parse(fields[72]),
                AgeUnknownC_OtherLiveOutcome = int.Parse(fields[73]),
                AdultF_OtherLiveOutcome = int.Parse(fields[74]),
                UT5MF_OtherLiveOutcome = int.Parse(fields[75]),
                AgeUnknownF_OtherLiveOutcome = int.Parse(fields[76]),
                AdultC_DiedInCare = int.Parse(fields[77]),
                UT5MC_DiedInCare = int.Parse(fields[78]),
                AgeUnknownC_DiedInCare = int.Parse(fields[79]),
                AdultF_DiedInCare = int.Parse(fields[80]),
                UT5MF_DiedInCare = int.Parse(fields[81]),
                AgeUnknownF_DiedInCare = int.Parse(fields[82]),
                AdultC_LostInCare = int.Parse(fields[83]),
                UT5MC_LostInCare = int.Parse(fields[84]),
                AgeUnknownC_LostInCare = int.Parse(fields[85]),
                AdultF_LostInCare = int.Parse(fields[86]),
                UT5MF_LostInCare = int.Parse(fields[87]),
                AgeUnknownF_LostInCare = int.Parse(fields[88]),
                AdultC_ShelterEuthanasia = int.Parse(fields[89]),
                UT5MC_ShelterEuthanasia = int.Parse(fields[90]),
                AgeUnknownC_ShelterEuthanasia = int.Parse(fields[91]),
                AdultF_ShelterEuthanasia = int.Parse(fields[92]),
                UT5MF_ShelterEuthanasia = int.Parse(fields[93]),
                AgeUnknownF_ShelterEuthanasia = int.Parse(fields[94]),
                AdultC_OwnerIntendedEuthanasia = int.Parse(fields[95]),
                UT5MC_OwnerIntendedEuthanasia = int.Parse(fields[96]),
                AgeUnknownC_OwnerIntendedEuthanasia = int.Parse(fields[97]),
                Adultf_OwnerIntendedEuthanasia = int.Parse(fields[98]),
                UT5Mf_OwnerIntendedEuthanasia = int.Parse(fields[99]),
                AgeUnknownF_OwnerIntendedEuthanasia = int.Parse(fields[100]),
                id = Guid.NewGuid()
            };

            return animalOutcome;
        }
        private static AnimalIntake CreateAnimalIntakeRecord(Guid facilityID, int recordYear, int recordMonth, string[] fields)
        {
            var animalIntake = new AnimalIntake
            {
                facility_id = facilityID,
                DateCreated = DateTime.Now,
                RecordStatus = 1,
                RecordYear = recordYear,
                RecordMonth = recordMonth,
                AdultC_StrayAtLarge = int.Parse(fields[17]),
                UT5MC_StrayAtLarge = int.Parse(fields[18]),
                AgeUnknownC_StrayAtLarge = int.Parse(fields[19]),
                AdultF_StrayAtLarge = int.Parse(fields[20]),
                UT5MF_StrayAtLarge = int.Parse(fields[21]),
                AgeUnknownF_StrayAtLarge = int.Parse(fields[22]),
                AdultC_RelinquishedByOwner = int.Parse(fields[23]),
                UT5MC_RelinquishedByOwner = int.Parse(fields[24]),
                AgeUnknownC_RelinquishedByOwner = int.Parse(fields[25]),
                AdultF_RelinquishedByOwner = int.Parse(fields[26]),
                UT5MF_RelinquishedByOwner = int.Parse(fields[27]),
                AgeUnknownF_RelinquishedByOwner = int.Parse(fields[28]),
                AdultC_OwnerRequestedEuthanasia = int.Parse(fields[29]),
                UT5MC_OwnerRequestedEuthanasia = int.Parse(fields[30]),
                AgeUnknownC_OwnerRequestedEuthenasia = int.Parse(fields[31]),
                AdultF_OwnerRequestedEuthanasia = int.Parse(fields[32]),
                UT5MF_OwnerRequestedEuthanasia = int.Parse(fields[33]),
                AgeUnknownF_OwnerRequestedEuthenasia = int.Parse(fields[34]),
                AdultC_TransferredFromAgency = int.Parse(fields[35]),
                UT5MC_TransferredFromAgency = int.Parse(fields[36]),
                AgeUnknownC_TransferredFromAgency = int.Parse(fields[37]),
                Adultf_TransferredFromAgency = int.Parse(fields[38]),
                UT5Mf_TransferredFromAgency = int.Parse(fields[39]),
                AgeUnknownF_TransferredFromAgency = int.Parse(fields[40]),
                AdultC_OtherIntakes = int.Parse(fields[41]),
                UT5MC_OtherIntakes = int.Parse(fields[42]),
                AgeUnknownC_OtherIntakes = int.Parse(fields[43]),
                AdultF_OtherIntakes = int.Parse(fields[44]),
                UT5MF_OtherIntakes = int.Parse(fields[45]),
                AgeUnknownF_OtherIntakes = int.Parse(fields[46]),
                id = Guid.NewGuid()
            };

            return animalIntake;
        }
        private bool DataRecordsExist(Guid organizationID, int recordYear, int recordMonth)
        {
            bool recordsExist = false;

            // Get animal intake record
            var animalIntakeRecord = AnimalIntakeRepository.GetAllForActiveFacilityAndYearAndMonth(organizationID, recordYear, recordMonth);
            // Get animal outcome record
            var animalOutcomeRecord = AnimalOutcomeRepository.GetAllForFacilityAndYearAndMonth(organizationID, recordYear, recordMonth);
            // Get animal count record
            var animalCountRecord = AnimalCountRepository.GetAllForFacilityAndYearAndMonth(organizationID, recordYear, recordMonth);

            if (animalIntakeRecord.Count > 0 ||
                animalOutcomeRecord.Count > 0 ||
                animalCountRecord.Count > 0)
            {
                recordsExist = true;
            }

            return recordsExist;
        }
        private void SetupErrorFile(ref StringBuilder errorFile)
        {
            errorFile.AppendLine(
                "\"Organization EIN\"," +
                "\"Organization Name\"," +
                "\"Organization County\"," +
                "\"Organization Address 1\"," +
                "\"Organization Address 2\"," +
                "\"Organization City\"," +
                "\"Organization State\"," +
                "\"Organization Zip Code\"," +
                "\"Facility Name\"," +
                "\"Facility County\"," +
                "\"Facility Address 1\"," +
                "\"Facility Address 2\"," +
                "\"Facility City\"," +
                "\"Facility State\"," +
                "\"Facility Zip Code\"," +
                "\"Record Year\"," +
                "\"Record Month\"," +
                "\"Stray At Large Canine Adult\"," +
                "\"Stray At Large Canine Up to 5 Months\"," +
                "\"Stray At Large Canine Age Unknown\"," +
                "\"Stray At Large Feline Adult\"," +
                "\"Stray At Large Feline Up to 5 Months\"," +
                "\"Stray At Large Feline Age Unkonwn\"," +
                "\"Relinquished By Owner Canine Adult\"," +
                "\"Relinquished By Owner Canine Up to 5 Months\"," +
                "\"Relinquished By Owner Canine Age Unknown\"," +
                "\"Relinquished By Owner Feline Adult\"," +
                "\"Relinquished By Owner Feline Up to 5 Months\"," +
                "\"Relinquished By Owner Feline Age Unknown\"," +
                "\"Owner-intended Euthanasia Canine Adult\"," +
                "\"Owner-intended Euthanasia Canine Up to 5 Months\"," +
                "\"Owner-intended Euthanasia Canine Age Unknown\"," +
                "\"Owner-intended Euthanasia Feline Adult\"," +
                "\"Owner-intended Euthanasia Feline Up to 5 Months\"," +
                "\"Owner-intended Euthanasia Feline Age Unknown\"," +
                "\"Transferred in From Agency Canine Adult\"," +
                "\"Transferred in From Agency Canine Up to 5 Months\"," +
                "\"Transferred in From Agency Canine Age Unknown\"," +
                "\"Transferred in From Agency Feline Adult\"," +
                "\"Transferred in From Agency Feline Up to 5 Months\"," +
                "\"Transferred in From Agency Feline Age Unknown\"," +
                "\"Other intakes Canine Adult\"," +
                "\"Other intakes Canine Up to 5 Months\"," +
                "\"Other intakes Canine Age Unknown\"," +
                "\"Other intakes Feline Adult\"," +
                "\"Other intakes Feline Up to 5 Months\"," +
                "\"Other intakes Feline Up to 5 Months\"," +
                "\"Adoption Canine Adult\"," +
                "\"Adoption Canine Up to 5 Months\"," +
                "\"Adoption Canine Age Unknown\"," +
                "\"Adoption Feline Adult\"," +
                "\"Adoption Feline Up to 5 Months\"," +
                "\"Adoption Feline Age Unknown\"," +
                "\"Returned To Owner Canine Adult\"," +
                "\"Returned To Owner Canine Up to 5 Months\"," +
                "\"Returned To Owner Canine Age Unknown\"," +
                "\"Returned To Owner Feline Adult\"," +
                "\"Returned To Owner Feline Up to 5 Months\"," +
                "\"Returned To Owner Feline Age Unknown\"," +
                "\"Transfer To Another Agency Canine Adult\"," +
                "\"Transfer To Another Agency Canine Up to 5 Months\"," +
                "\"Transfer To Another Agency Canine Age Unkown\"," +
                "\"Transfer To Another Agency Feline Adult\"," +
                "\"Transfer To Another Agency Feline Up to 5 Months\"," +
                "\"Transfer To Another Agency Feline Age Unknown\"," +
                "\"Returned To Field Canine Adult\"," +
                "\"Returned To Field Canine Up to 5 Months\"," +
                "\"Returned To Field Canine Age Unkown\"," +
                "\"Returned To Field Feline Adult\"," +
                "\"Returned To Field Feline Up to 5 Months\"," +
                "\"Returned To Field Feline Age Unknown\"," +
                "\"Other Live Outcome Canine Adult\"," +
                "\"Other Live Outcome Canine Up to 5 Months\"," +
                "\"Other Live Outcome Canine Age Unknown\"," +
                "\"Other Live Outcome Feline Adult\"," +
                "\"Other Live Outcome Feline Up to 5 Months\"," +
                "\"Other Live outcome Feline Age Unknown\"," +
                "\"Died In Care Canine Adult\"," +
                "\"Died In Care Canine Up to 5 Months\"," +
                "\"Died In Care Canine Age Unknown\"," +
                "\"Died In Care Feline Adult\"," +
                "\"Died In Care Feline Up to 5 Months\"," +
                "\"Died In Care Feline Age Unknown\"," +
                "\"Lost In Care Canine Adult\"," +
                "\"Lost In Care Canine Up to 5 Months\"," +
                "\"Lost In Care Canine Age Unknown\"," +
                "\"Lost In Care Feline Adult\"," +
                "\"Lost In Care Feline Up to 5 Months\"," +
                "\"Lost In Care Feline Age Unknown\"," +
                "\"Shelter Euthanasia Canine Adult\"," +
                "\"Shelter Euthanasia Canine Up to 5 Months\"," +
                "\"Shelter Euthanasia Canine Age Unknown\"," +
                "\"Shelter Euthanasia Feline Adult\"," +
                "\"Shelter Euthanasia Feline Up to 5 Months\"," +
                "\"Shelter Euthanasia Feline Age Unknown\"," +
                "\"Owner-inteded Euthanasia Canine Adult\"," +
                "\"Owner-inteded Euthanasia Canine Up to 5 Months\"," +
                "\"Owner-inteded Euthanasia Canine Age Unknown\"," +
                "\"Owner-inteded Euthanasia Feline Adult\"," +
                "\"Owner-inteded Euthanasia Feline Up to 5 Months\"," +
                "\"Owner-inteded Euthanasia Feline Age Unknown\"," +
                "\"Animal Count Type\"," +
                "\"Beginning Adult Canine\"," +
                "\"Beginning UT5 Months Canine\"," +
                "\"Beginning Age Unknown Canine\"," +
                "\"Beginning Adult Feline\"," +
                "\"Beginning UT5 Months Canine\"," +
                "\"Beginning Age Unknown Feline\"," +
                "\"Animal Count Type\"," +
                "\"Ending Adult Canine\"," +
                "\"Ending UT5 Months Canine\"," +
                "\"Ending Age Unknown Canine\"," +
                "\"Ending Adult Feline\"," +
                "\"Ending UT5 Months Feline\"," +
                "\"Ending Age Unknown Feline\"," +
                "\"Animal Count Type\"," +
                "\"Ending Adult Canine\"," +
                "\"Ending UT5 Months Canine\"," +
                "\"Ending Age Unknown Canine\"," +
                "\"Ending Adult Feline\"," +
                "\"Ending UT5 Months Feline\"," +
                "\"Ending Age Unknown Feline\"" +
                "\"Error\""
                );
        }
        private void AddErrorRecord(ref StringBuilder errorFile, List<string> fields, string error)
        {
            errorFile.AppendLine("\"" + fields[0] + "\",\"" +
                                 fields[1] + "\",\"" +
                                 fields[2] + "\",\"" +
                                 fields[3] + "\",\"" +
                                 fields[4] + "\",\"" +
                                 fields[5] + "\",\"" +
                                 fields[6] + "\",\"" +
                                 fields[7] + "\",\"" +
                                 fields[8] + "\",\"" +
                                 fields[9] + "\",\"" +
                                 fields[10] + "\",\"" +
                                 fields[11] + "\",\"" +
                                 fields[12] + "\",\"" +
                                 fields[13] + "\",\"" +
                                 fields[14] + "\",\"" +
                                 fields[15] + "\",\"" +
                                 fields[16] + "\",\"" +
                                 fields[17] + "\",\"" +
                                 fields[18] + "\",\"" +
                                 fields[19] + "\",\"" +
                                 fields[20] + "\",\"" +
                                 fields[21] + "\",\"" +
                                 fields[22] + "\",\"" +
                                 fields[23] + "\",\"" +
                                 fields[24] + "\",\"" +
                                 fields[25] + "\",\"" +
                                 fields[26] + "\",\"" +
                                 fields[27] + "\",\"" +
                                 fields[28] + "\",\"" +
                                 fields[29] + "\",\"" +
                                 fields[30] + "\",\"" +
                                 fields[21] + "\",\"" +
                                 fields[32] + "\",\"" +
                                 fields[33] + "\",\"" +
                                 fields[34] + "\",\"" +
                                 fields[35] + "\",\"" +
                                 fields[36] + "\",\"" +
                                 fields[37] + "\",\"" +
                                 fields[38] + "\",\"" +
                                 fields[39] + "\",\"" +
                                 fields[40] + "\",\"" +
                                 fields[41] + "\",\"" +
                                 fields[42] + "\",\"" +
                                 fields[43] + "\",\"" +
                                 fields[44] + "\",\"" +
                                 fields[45] + "\",\"" +
                                 fields[46] + "\",\"" +
                                 fields[47] + "\",\"" +
                                 fields[48] + "\",\"" +
                                 fields[49] + "\",\"" +
                                 fields[50] + "\",\"" +
                                 fields[51] + "\",\"" +
                                 fields[52] + "\",\"" +
                                 fields[53] + "\",\"" +
                                 fields[54] + "\",\"" +
                                 fields[55] + "\",\"" +
                                 fields[56] + "\",\"" +
                                 fields[57] + "\",\"" +
                                 fields[58] + "\",\"" +
                                 fields[59] + "\",\"" +
                                 fields[60] + "\",\"" +
                                 fields[61] + "\",\"" +
                                 fields[62] + "\",\"" +
                                 fields[63] + "\",\"" +
                                 fields[64] + "\",\"" +
                                 fields[65] + "\",\"" +
                                 fields[66] + "\",\"" +
                                 fields[67] + "\",\"" +
                                 fields[68] + "\",\"" +
                                 fields[69] + "\",\"" +
                                 fields[70] + "\",\"" +
                                 fields[71] + "\",\"" +
                                 fields[72] + "\",\"" +
                                 fields[73] + "\",\"" +
                                 fields[74] + "\",\"" +
                                 fields[75] + "\",\"" +
                                 fields[76] + "\",\"" +
                                 fields[77] + "\",\"" +
                                 fields[78] + "\",\"" +
                                 fields[79] + "\",\"" +
                                 fields[80] + "\",\"" +
                                 fields[81] + "\",\"" +
                                 fields[82] + "\",\"" +
                                 fields[83] + "\",\"" +
                                 fields[84] + "\",\"" +
                                 fields[85] + "\",\"" +
                                 fields[86] + "\",\"" +
                                 fields[87] + "\",\"" +
                                 fields[88] + "\",\"" +
                                 fields[89] + "\",\"" +
                                 fields[90] + "\",\"" +
                                 fields[91] + "\",\"" +
                                 fields[92] + "\",\"" +
                                 fields[93] + "\",\"" +
                                 fields[94] + "\",\"" +
                                 fields[95] + "\",\"" +
                                 fields[96] + "\",\"" +
                                 fields[97] + "\",\"" +
                                 fields[98] + "\",\"" +
                                 fields[99] + "\",\"" +
                                 fields[100] + "\",\"" +
                                 fields[101] + "\",\"" +
                                 fields[102] + "\",\"" +
                                 fields[103] + "\",\"" +
                                 fields[104] + "\",\"" +
                                 fields[105] + "\",\"" +
                                 fields[106] + "\",\"" +
                                 fields[107] + "\",\"" +
                                 fields[108] + "\",\"" +
                                 fields[109] + "\",\"" +
                                 fields[110] + "\",\"" +
                                 fields[111] + "\",\"" +
                                 fields[112] + "\",\"" +
                                 fields[113] + "\",\"" +
                                 fields[114] + "\",\"" +
                                 fields[115] + "\",\"" +
                                 fields[116] + "\",\"" +
                                 fields[117] + "\",\"" +
                                 fields[118] + "\",\"" +
                                 fields[119] + "\",\"" +
                                 fields[120] + "\",\"" +
                                 fields[121] + "\",\"" +
                                 error + "\""
                );
        }
    }
}