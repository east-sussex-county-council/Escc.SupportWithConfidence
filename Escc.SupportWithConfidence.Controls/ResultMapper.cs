using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using EsccWebTeam.Gdsc;

namespace Escc.SupportWithConfidence.Controls
{
    public class ResultMapper : IMapper
    {
        public void Map(DataSet data, QueryParameter queryparameters)
        {

            if (data != null)
            {


                foreach (DataRow resultRow in data.Tables[0].Rows)
                {
                    var result = new Result
                        {
                            Id = Convert.ToInt32(resultRow["FlareId"]),
                            Name = resultRow["Name"].ToString(),
                            Address = GetAddress(resultRow),
                            PublishAddress = Convert.ToBoolean(resultRow["PublishAddress"]),
                            BasedIn = resultRow["Based in"] == null ? string.Empty : resultRow["Based in"].ToString(),
                            Telephone = resultRow["Telephone"] == null ? string.Empty : resultRow["Telephone"].ToString(),
                            Mobile = resultRow["Mobile"] == null ? string.Empty : resultRow["Mobile"].ToString(),
                            Email = resultRow["Email"] == null ? string.Empty : resultRow["Email"].ToString(),
                            Distance = Convert.ToInt16(resultRow["Distance from me"])
                        };
                    string coverage1 = resultRow["Coverage"] == null ? string.Empty : resultRow["Coverage"].ToString();
                    string coverage2 = resultRow["Coverage2"] == null ? string.Empty : resultRow["Coverage2"].ToString();
                    if (coverage1.Length > 0 & coverage2.Length == 0)
                    {
                        result.Coverage = coverage1;
                    }
                    else if (coverage1.Length > 0 & coverage2.Length > 0)
                    {
                        result.Coverage = coverage1 + " " + coverage2;
                    }
                    else
                    {
                        result.Coverage = coverage2;
                    }

                    try
                    {
                        result.CurrentPage = HttpContext.Current.Request.QueryString["page"] == null ? 1 : Convert.ToInt32(HttpContext.Current.Request.QueryString["page"]);

                    }
                    catch (FormatException)
                    {
                        result.CurrentPage = 1;
                    }
                    catch (OverflowException)
                    {
                        result.CurrentPage = 1;
                    }

                    result.QueryString = queryparameters.ToString();
                    result.ShowDistance = queryparameters.Easting > 0;


                    foreach (DataRow catRow in data.Tables[1].Rows)
                    {

                        if (Convert.ToInt32(catRow["FlareId"]) == result.Id)
                        {
                            result.CategoryList += "<li><a href=\"results.aspx?cat=" + catRow["CategoryId"] + "\">" + catRow["Description"] + "</a></li>";
                        }
                    }

                    result.TotalResults = Convert.ToInt32(data.Tables[2].Rows[0]["TotalResults"]);
                    TotalResults = result.TotalResults;

                    if (data.Tables.Count == 4)
                    {
                        CategoryHeading = data.Tables[3].Rows[0]["Description"].ToString();
                    }




                    _collection.Add(result);

                }
            }
        }

        private string GetAddress(DataRow dbAddress)
        {
            var address = new BS7666Address
                {
                    Paon = dbAddress["Paon"] == null ? string.Empty : dbAddress["Paon"].ToString(),
                    StreetName = dbAddress["StreetName"] == null ? string.Empty : dbAddress["StreetName"].ToString(),
                    Town = dbAddress["Town"] == null ? string.Empty : dbAddress["Town"].ToString(),
                    AdministrativeArea =
                        dbAddress["AdministrativeArea"] == null ? string.Empty : dbAddress["AdministrativeArea"].ToString(),
                    Postcode = dbAddress["Postcode"] == null ? string.Empty : dbAddress["Postcode"].ToString()
                };


            return address.GetSimpleAddress().ToString();

        }

        #region IMapper Members

        public int TotalResults { get; set; }

        public string CategoryHeading { get; set; }


        private IList<IResult> _collection = new List<IResult>();

        public ResultMapper()
        {
            CategoryHeading = string.Empty;
        }

        public IList<IResult> Collection
        {
            get { return _collection; }
            set { _collection = value; }
        }



        #endregion


    }
}
