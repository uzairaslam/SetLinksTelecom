using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using SetLinksTelecom.Data;
using SetLinksTelecom.DTO;

namespace SetLinksTelecom.GeneralFolder
{
    public static class ExtensionMethods
    {
        private static DataContext _db = new DataContext();
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string ToSubHeadString(this int value)
        {
            return value < 10 && value > 0 ? "0" + value : value.ToString();
        }
        public static string ToAccString(this int value)
        {
            return value > 0 && value < 10
                ? "000" + value
                : (value > 9 && value < 100
                    ? "00" + value
                    : (value > 99 && value < 1000 ? "0" + value : value.ToString()));
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void GetInvalidPersons(this List<DtoPersonExcel> persons)
        {
            Regex cnic = new Regex(@"^[1-4]{1}[0-9]{4}(-)?[0-9]{7}(-)?[0-9]{1}$");
            persons.RemoveAll(p => !string.IsNullOrWhiteSpace(p.Name) && p.Name.Length >= 3 && p.Name.Length <= 30
                                   && p.FatherName.Length <= 30 && cnic.IsMatch(p.CNIC) &&
                                   (p.Gender.Equals("Male") || p.Gender.Equals("Female"))
                                   && !string.IsNullOrWhiteSpace(p.MobilePersonal) && !string.IsNullOrWhiteSpace(p.MobileBusiness)
                                   && p.MobileBusiness != p.MobilePersonal && !ExtensionMethods.PhoneExist(p.MobilePersonal)
                                   && !ExtensionMethods.PhoneExist(p.MobileBusiness) && ExtensionMethods.LineExist(p.BusinessLine)
                                   && ExtensionMethods.LineExist(p.PersonalLine)
            );
        }

        private static bool PhoneExist(string phone)
        {
            return _db.Persons.Any(p => p.MobileBusiness.Substring(p.MobileBusiness.Length - 10).Equals(phone)
                                        || p.MobilePersonal.Substring(p.MobilePersonal.Length - 10).Equals(phone));
        }

        private static bool LineExist(string line)
        {
            return _db.Lines.Any(l => l.Name.Equals(line));
        }
    }
}