using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Escc.AddressAndPersonalDetails;

namespace Escc.SupportWithConfidence.Controls
{
    public class ProviderMapper 
    {
        private readonly IProviderDataSource _dataSource;
        private readonly IProviderDataRepository _repository;

        public ProviderMapper(IProviderDataSource dataSource)
        {
            _dataSource = dataSource;
        }
        
        public ProviderMapper(IProviderDataRepository repository)
        {
            _repository = repository;
        }

        public void Map(int providerId)
        {
            var data = _dataSource.GetProviderById(providerId, true);
            MapData(data);
           
        }

        public void GetProvider(int providerId)
        {
            var data = _repository.GetProvider(providerId);

            MapData(data);

        }

        public void Map(int pageIndex, int pageSize)
        {
            var data = _repository.GetAllProvidersPaged(pageIndex, pageSize);
            MapData(data);
        }

        public void Map()
        {
            var data = _repository.GetAllProviders();
            MapData(data);
        }


        private void MapData(DataSet data)
        {
            if (data != null)
            {

                foreach (DataRow dbProvider in data.Tables[0].Rows)
                {

                    var provider = new Provider
                        {
                            Id = Convert.ToInt32(dbProvider["Id"]),
                            FlareId = Convert.ToInt32(dbProvider["FlareId"]),
                            ProviderName =
                                dbProvider["ProviderName"] == DBNull.Value
                                    ? string.Empty
                                    : dbProvider["ProviderName"].ToString(),
                            Address = MapAddress(dbProvider),
                            Telephone =
                                dbProvider["TelephoneNumber"] == DBNull.Value
                                    ? string.Empty
                                    : dbProvider["TelephoneNumber"].ToString(),
                            Mobile =
                                dbProvider["MobileNumber"] == DBNull.Value
                                    ? string.Empty
                                    : dbProvider["MobileNumber"].ToString(),
                            Email =
                                dbProvider["EmailAddress"] == DBNull.Value
                                    ? string.Empty
                                    : dbProvider["EmailAddress"].ToString(),
                            Website =
                                dbProvider["WebsiteAddress"] == DBNull.Value
                                    ? string.Empty
                                    : dbProvider["WebsiteAddress"].ToString(),
                            Fax =
                                dbProvider["FaxNumber"] == DBNull.Value ? string.Empty : dbProvider["FaxNumber"].ToString()
                        };

                    if (dbProvider["Easting"].ToString() == "")
                    {
                        provider.Easting = 0;
                    }
                    else
                    {
                        provider.Easting = dbProvider["Easting"] == DBNull.Value ? 0 : Convert.ToInt32(dbProvider["Easting"]);
                    }

                    if (dbProvider["Northing"].ToString() == "")
                    {
                        provider.Northing = 0;
                    }
                    else
                    {
                        provider.Northing = dbProvider["Northing"] == DBNull.Value ? 0 : Convert.ToInt32(dbProvider["Northing"]);
                    }
                    provider.PublishAddress = dbProvider["PublishAddress"] != DBNull.Value && Convert.ToBoolean(dbProvider["PublishAddress"]);
                    provider.Experience = dbProvider["Experience"] == DBNull.Value ? string.Empty : dbProvider["Experience"].ToString().Replace("\r\n", "<br />");
                    provider.Background = dbProvider["Background"] == DBNull.Value ? string.Empty : dbProvider["Background"].ToString().Replace("\r\n", "<br />");
                    provider.Expertise = dbProvider["Expertise"] == DBNull.Value ? string.Empty : dbProvider["Expertise"].ToString().Replace("\r\n", "<br />");
                    provider.Accreditation = dbProvider["Accreditation"] == DBNull.Value ? string.Empty : dbProvider["Accreditation"].ToString().Replace("\r\n", "<br />");
                    provider.Services = dbProvider["Services"] == DBNull.Value ? string.Empty : dbProvider["Services"].ToString().Replace("\r\n", "<br />");
                    provider.Costs = dbProvider["Costs"] == DBNull.Value ? string.Empty : dbProvider["Costs"].ToString().Replace("\r\n", "<br />");
                    provider.Crb = dbProvider["Crb"] == DBNull.Value ? string.Empty : dbProvider["Crb"].ToString();
                    provider.Availability = Availability(dbProvider);

                  
                    
                    provider.ContactName = dbProvider["ContactName"] == DBNull.Value ? string.Empty : dbProvider["ContactName"].ToString().Replace("\r\n", "<br />");
                    provider.Coverage = dbProvider["Coverage"] == DBNull.Value ? string.Empty : dbProvider["Coverage"].ToString().Replace("\r\n", "<br />");
                    provider.Coverage2 = dbProvider["Coverage2"] == DBNull.Value ? string.Empty : dbProvider["Coverage2"].ToString().Replace("\r\n", "<br />");
                    provider.CrbCheckDate = dbProvider["CrbCheckDate"].ToString() == "" ? string.Empty : DateTime.Parse(dbProvider["CrbCheckDate"].ToString()).ToString("MMMM yyyy", CultureInfo.CurrentCulture);
                    provider.BwcMember = dbProvider["BWCFlag"] != DBNull.Value && Convert.ToBoolean(dbProvider["BWCFlag"]);

                    provider.CqcCheckDate = dbProvider["CqcCheckDate"].ToString() == "" ? string.Empty : DateTime.Parse(dbProvider["CqcCheckDate"].ToString()).ToString("MMMM yyyy", CultureInfo.CurrentCulture);

                    //provider.IsDeleted = dbProvider["IsDeleted"] == DBNull.Value ? false : Convert.ToBoolean(dbProvider["IsDeleted"]);
                    //provider.LastModified = dbProvider["LastModified"] == DBNull.Value ? string.Empty : dbProvider["LastModified"].ToString();

                    foreach (DataRow catRow in data.Tables[1].Rows)
                    {

                        if (Convert.ToInt32(catRow["FlareId"]) == provider.FlareId)
                        {
                            provider.CategoryList += "<li><a href=\"/socialcare/athome/approvedproviders/Results.aspx?cat=" + catRow["CategoryId"] + "\">" + catRow["Description"] + "</a></li>";
                        }
                    }


                    if (data.Tables.Count == 3)
                    {
                        provider.TotalResults = Convert.ToInt32(data.Tables[2].Rows[0]["TotalResults"]);
                        _totalResults = provider.TotalResults;
                    }

                    provider.PhotographId = dbProvider["PhotographId"] == DBNull.Value ? 0 : Convert.ToInt32(dbProvider["PhotographId"]);

                    //provider.IsPATrained = Convert.ToBoolean(dbProvider["IsPATrained"]);

                    Providers.Add(provider);

                }
            }

        }


        private string Availability(DataRow dbProvider)
        {
            string a1 = dbProvider["Availability1"] == DBNull.Value ? string.Empty : dbProvider["Availability1"].ToString();
            string a2 = dbProvider["Availability2"] == DBNull.Value ? string.Empty : dbProvider["Availability2"].ToString();
            string a3 = dbProvider["Availability3"] == DBNull.Value ? string.Empty : dbProvider["Availability3"].ToString();
            return a1 + " " +  a2 + " " + a3;
        }

        private BS7666Address MapAddress(DataRow dbProvider)
        {
            
            var address = new BS7666Address
                {
                    Paon = dbProvider["Address1"] == DBNull.Value ? string.Empty : dbProvider["Address1"].ToString(),
                    StreetName = dbProvider["Address2"] == DBNull.Value ? string.Empty : dbProvider["Address2"].ToString(),
                    Town = dbProvider["Address3"] == DBNull.Value ? string.Empty : dbProvider["Address3"].ToString(),
                    Locality = dbProvider["Address4"] == DBNull.Value ? string.Empty : dbProvider["Address4"].ToString(),
                    Postcode = dbProvider["Postcode"] == DBNull.Value ? string.Empty : dbProvider["Postcode"].ToString()
                };

            if (dbProvider["Easting"].ToString() == "")
            {
                address.GridEasting = 0;
            }
            else
            {
                address.GridEasting = dbProvider["Easting"] == DBNull.Value ? 0 : Convert.ToInt32(dbProvider["Easting"]);
            }

            if (dbProvider["Northing"].ToString() == "")
            {
                address.GridNorthing = 0;
            }
            else
            {
                address.GridNorthing = dbProvider["Northing"] == DBNull.Value ? 0 : Convert.ToInt32(dbProvider["Northing"]);
            }
            return address;

        }

        private Int32 _totalResults;

        public Int32 TotalResults
        {
            get { return _totalResults; }
            set { _totalResults = value; }
        }


        private IList<Provider> _provider = new List<Provider>();
        public IList<Provider> Providers
        {
            get { return _provider; }
            set { _provider = value; }
        }

    }
}