
using System.Collections.Generic;

namespace Escc.SupportWithConfidence.ETL
{
    public static class CategoryTransformation
    {
        public static string ProperCase(string category)
        {
            var transformed = category.ToLower();
            return char.ToUpper(transformed[0]) + transformed.Substring(1);


        }



        public static string ReplaceAmpersand(string category)
        {
            return category.Replace("&", "and");
        }

        public static string SubsituteCategory(string category)
        {
            switch (category)
            {
                case "Personal assistant":
                    category = "Personal Assistant";
                    break;

                case "Support planning and brokerage":
                    category = "Help with planning and arranging support";
                    break;
                case "Disabled accessories":
                    category = "Disability equipment";
                    break;
                case "Meal preparation in the home":
                    category = "Preparing meals in the home";
                    break;
                case "Cleaning and household services":
                    category = "Cleaning and laundry";
                    break;
                case "Ironing service":
                    category = "Ironing";
                    break;
                case "Laundry service":
                    category = "Laundry";
                    break;
                case "De-cluttering and organisation service":
                    category = "De-cluttering and organisation";
                    break;
                case "Home maintenance":
                    category = "Home maintenance and gardening";
                    break;
                case "Gardening maintenance":
                    category = "Gardening";
                    break;
                case "Home and property maintenance":
                    category = "Property maintenance";
                    break;
                case "Mobility adaptations":
                    category = "Mobility adaptions - handrails, ramps etc";
                    break;
                case "Garden maintenance (pa service)":
                    category = "Gardening";
                    break;
                case "Pet services (pa service)":
                    category = "Pet services";
                    break;
                case "Transportation services (pa service)":
                    category = "Transport";
                    break;
                case "Secretarial and administrative services":
                    category = "Secretarial and administrative";
                    break;
                case "Live in service":
                    category = "Live-in support";
                    break;
                case "Domiciliary care services":
                    category = "Home care (domiciliary)";
                    break;
                case "Domiciliary care agency":
                    category = "Home care agency";
                    break;
                case "Complimentary and therapeutic services":
                    category = "Complementary and therapeutic services";
                    break;
                case "Specialist support services":
                    category = "Specialist support needs";
                    break;
                case "Dementia specialist / service":
                    category = "Dementia";
                    break;
                case "Reminiscence services":
                    category = "Reminiscence therapies";
                    break;
                case "Language and translation services":
                    category = "Language and translation";
                    break;
                case "Direct payment support services":
                    category = "Direct payment support";
                    break;
                case "Dentistry":
                    category = "Dentist";
                    break;
                case "Social and daytime activities":
                    category = "Social and educational activities";
                    break;
            }
            return category;
        }


        private static readonly List<string> CategoryCodes = new List<string>() { "C02", "D08", "I10" };

        public static bool ExcludeCategory(string categoryCode)
        {
            if (CategoryCodes.Contains(categoryCode))
            {
                return true;
            }

            return false;
        }


    }
}
