using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Net.Mail;

namespace MSCMSEastSussexControls
{
	/// <summary>
	/// Summary description for WebFormsUtilities.
	/// </summary>
	public class WebFormsUtilities
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="WebFormsUtilities"/> class.
        /// </summary>
		public WebFormsUtilities()
		{
		}

        /// <summary>
        /// Resets controls on a form
        /// </summary>
        /// <param name="frm">The form</param>
		public static void ResetForms(HtmlForm frm)
		{
			string controltype;
			foreach (Control ctrl in frm.Controls)
			{
		
				controltype = ctrl.GetType().ToString();

				switch (controltype)
				{
					case "System.Web.UI.WebControls.TextBox":
						TextBox	txtctrl = ctrl as TextBox;
						txtctrl.Text = "";
						txtctrl.BackColor = Color.White;
						break;

					case "System.Web.UI.WebControls.RadioButtonList":
						RadioButtonList rblctrl = ctrl as RadioButtonList;
						rblctrl.SelectedIndex = -1;
						rblctrl.BackColor = Color.Transparent;
						break;

					case "System.Web.UI.WebControls.RadioButton":
						RadioButton rbtnctrl = ctrl as RadioButton;
						rbtnctrl.Checked = false;
						rbtnctrl.BackColor = Color.Transparent;
						break;


					case "System.Web.UI.WebControls.DropDownList":
						DropDownList ddlctrl = ctrl as DropDownList;
						ddlctrl.SelectedIndex = 0;
						ddlctrl.BackColor = Color.White;
						break;

					case "System.Web.UI.WebControls.CheckBoxList":	
						CheckBoxList cblctrl = ctrl as CheckBoxList;
						cblctrl.SelectedIndex = -1;
						cblctrl.BackColor = Color.Transparent;
						break;

					case "System.Web.UI.WebControls.CheckBox":
						CheckBox cbxctrl = ctrl as CheckBox;
						cbxctrl.Checked = false;
						cbxctrl.BackColor = Color.Transparent;
						break;

					case "System.Web.UI.WebControls.ValidationSummary":
						ValidationSummary valctrl = ctrl as ValidationSummary;
						valctrl.Visible = false;
						break;
				}

			}
		}

        /// <summary>
        /// Changes the background colour of a control which fails its regular expression-based validation
        /// </summary>
        /// <param name="frm">The form</param>
		public static void ValidateReg(HtmlForm frm)
		{
			string controltype;
		//	string[] RegControlsVal = new string[];
			foreach (Control ctrl in frm.Controls)
			{
				controltype = ctrl.GetType().ToString();

				if (controltype.ToString() == "System.Web.UI.WebControls.RegularExpressionValidator")
				{
					RegularExpressionValidator RegVal = ctrl as RegularExpressionValidator;
					
					string conval = RegVal.ControlToValidate.ToString();
					Control ctrlx = RegVal.FindControl(conval);
					TextBox tbx = ctrlx as TextBox;

					if (RegVal.IsValid == false)
					{
						tbx.BackColor = Color.Turquoise;
					}
					
					
				}
			}
												
		}

        /// <summary>
        /// Checks whether a form passed validation
        /// </summary>
        /// <param name="frm">The form</param>
        /// <returns></returns>
        [Obsolete("Use standard validation controls and FormControls.EsccValidationSummary instead.")]
		public static void ValidateForm(HtmlForm frm)
		{
			string controltype;
			foreach (Control ctrl in frm.Controls)
			{
		
				controltype = ctrl.GetType().ToString();

				switch (controltype)
				{
					case "System.Web.UI.WebControls.TextBox":
						TextBox	txtctrl = ctrl as TextBox;
						if ((txtctrl.Text == "")& (txtctrl.ClientID.StartsWith("m"))) // m = mandatory field
						{
							txtctrl.BackColor = Color.Turquoise;
						}
						else
						{
							txtctrl.BackColor = Color.White;
						}
						break;
				
					case "System.Web.UI.WebControls.RadioButtonList":
						RadioButtonList rblctrl = ctrl as RadioButtonList;
						if ((rblctrl.SelectedIndex == -1) & (rblctrl.ClientID.StartsWith("m")))
						{
							rblctrl.BackColor = Color.Turquoise;
						}
						else
						{

							rblctrl.BackColor = Color.Transparent;
						}
						break;

					case "System.Web.UI.WebControls.RadioButton":
						RadioButton rbtnctrl = ctrl as RadioButton;
						if ((rbtnctrl.Checked == false)& (rbtnctrl.ClientID.StartsWith("m")))
						{
							rbtnctrl.BackColor = Color.Turquoise;
						}
						else
						{
							rbtnctrl.BackColor  = Color.Transparent;
						}
						break;
				
					case "System.Web.UI.WebControls.DropDownList":
						DropDownList ddlctrl = ctrl as DropDownList;
						if ((ddlctrl.SelectedIndex == 0) & (ddlctrl.ClientID.StartsWith("m")))
						{
							ddlctrl.BackColor = Color.Turquoise;
						}
						else
						{
							ddlctrl.BackColor = Color.White;
						}
						break;
				
					case "System.Web.UI.WebControls.CheckBoxList":	
						CheckBoxList cblctrl = ctrl as CheckBoxList;
						if ((cblctrl.SelectedIndex == -1) & (cblctrl.ClientID.StartsWith("m")))
						{
							cblctrl.BackColor = Color.Turquoise;
						}
						else
						{
							cblctrl.BackColor = Color.Transparent;
						}
						break;
				
					case "System.Web.UI.WebControls.CheckBox":
						CheckBox cbxctrl = ctrl as CheckBox;
						if ((cbxctrl.Checked == false) & (cbxctrl.ClientID.StartsWith("m")))
						{
							cbxctrl.BackColor = Color.Turquoise;
						}
						else
						{
							cbxctrl.BackColor = Color.Transparent;
						}
						break;
				
				}
			}
		}

        /// <summary>
        /// Checks whether a form passed validation
        /// </summary>
        /// <param name="frm">The form</param>
        /// <returns></returns>
        [Obsolete("Use standard validation controls and FormControls.EsccValidationSummary instead.")]
		public static bool CheckForm(HtmlForm frm)
		{
			bool passed;
			int count = 0;
			string controltype;

			foreach (Control ctrl in frm.Controls)
			{
		
				controltype = ctrl.GetType().ToString();

				switch (controltype)
				{
					case "System.Web.UI.WebControls.TextBox":
						TextBox	txtctrl = ctrl as TextBox;
						if (txtctrl.BackColor != Color.White)
						{
							count++;
						}
						
						break;

					case "System.Web.UI.WebControls.RadioButtonList":
						RadioButtonList rblctrl = ctrl as RadioButtonList;
						if (rblctrl.BackColor != Color.Transparent)
						{
							count++;
						};
						break;

					case "System.Web.UI.WebControls.RadioButton":
						RadioButton rbtnctrl = ctrl as RadioButton;
						if (rbtnctrl.BackColor != Color.Transparent)
						{
							count++;
						}
						break;


					case "System.Web.UI.WebControls.DropDownList":
						DropDownList ddlctrl = ctrl as DropDownList;
						if (ddlctrl.BackColor != Color.White)
						{
							count++;
						}
						break;

					case "System.Web.UI.WebControls.CheckBoxList":	
						CheckBoxList cblctrl = ctrl as CheckBoxList;
						if (cblctrl.BackColor != Color.Transparent)
						{
							count++;
						}
						break;

					case "System.Web.UI.WebControls.CheckBox":
						CheckBox cbxctrl = ctrl as CheckBox;
						if (cbxctrl.BackColor != Color.Transparent)
						{
							count++;
						}
						break;

				}


			}
			if (count > 0)
			{
				passed = false;
			}
			else
			{
				passed = true;
			}


			return passed;
		}



        /// <summary>
        /// Emails details of an exception.
        /// </summary>
        /// <param name="From">From email address.</param>
        /// <param name="Body">The body.</param>
		[Obsolete("Use Microsoft Exception Management Application Block or Enterprise Library instead")]
        public static void MailException(string From, string Body)
		{
			HttpContext c = HttpContext.Current;
			//string s = c.Server.MachineName;
			// From = Form name
			// Body = Exception message
			string whatbox;
			whatbox = c.Server.MachineName;
			lock(typeof(WebFormsUtilities))
			{
				MailMessage MyMail = new MailMessage();
				MyMail.To.Add(new MailAddress("webstaff@eastsussex.gov.uk"));
				MyMail.From = new MailAddress(From);
				MyMail.Subject = "Exception raised in webform application"; 
				MyMail.IsBodyHtml = true;
				MyMail.Body = "Machine Name: " + whatbox + "<br /><br />" + Body ;

                SmtpClient smtp = new SmtpClient();
				smtp.Send(MyMail);
			}
		}


		/// <summary>
		/// Date method used by blue car badge form only
		/// </summary>
		/// <param name="ddlDays"></param>
		/// <param name="ddlMonths"></param>
		/// <param name="ddlYears"></param>
        public static void PopulateDate(DropDownList ddlDays, DropDownList ddlMonths, DropDownList ddlYears)
		{
			
			DataTable dtDays = new DataTable();
			DataColumn dcday = new DataColumn("days");
			dtDays.Columns.Add("days");
			
			
			object[] DayRowVals = new object[1];
			
           DayRowVals[0] = "-Select-"; //increased ddlDate css to 25%
			dtDays.Rows.Add(DayRowVals);
			for (int i = 1; i < 32; i++)
			{
				
                DayRowVals[0] = i;
				
				dtDays.Rows.Add(DayRowVals);

			}
			ddlDays.DataSource = dtDays;
			ddlDays.DataTextField="days";
			ddlDays.DataValueField="days";
			ddlDays.DataBind();
			ddlDays.SelectedIndex = 0;



			/////////////////////////////////
			
			DataTable dtMonths = new DataTable();
			DataColumn dcMonths = new DataColumn("months");
			DataColumn dcIndexMonths = new DataColumn("monthindex");
			dtMonths.Columns.Add(dcMonths);
			dtMonths.Columns.Add(dcIndexMonths);
			object[] MonthRowVals = new Object[1];
			string[] Months = GetMonths();
			
			MonthRowVals[0] = "-Select-";
			dtMonths.Rows.Add(MonthRowVals);
			
			for(int i = 0; i < 12; i++)
			{
				DataRow row = dtMonths.NewRow();
				row["months"] = Months[i];
				int currentmonth = i;
				row["monthindex"] = currentmonth + 1;
				dtMonths.Rows.Add(row);
			}

			
				
			
			ddlMonths.DataSource = dtMonths;
			ddlMonths.DataTextField="months" ;
			ddlMonths.DataValueField="monthindex";
			ddlMonths.DataBind();
			ddlMonths.SelectedIndex = 0;

			/////////////////////////////////
			
			DataTable dtYears = new DataTable();
			DataColumn dcYears = new DataColumn("years");
			dtYears.Columns.Add(dcYears);
			object[] YearRowVals = new Object[1];
			int yearNow = DateTime.Now.Year;
			int startYear = yearNow;
			int endYear = yearNow - 100;
			YearRowVals[0] = "-Select-";
			dtYears.Rows.Add(YearRowVals);

			for(int i = startYear; i > endYear; i--)
			{
				int y = i;
				YearRowVals[0] = y;
				dtYears.Rows.Add(YearRowVals);
			}
			ddlYears.DataSource = dtYears;
			ddlYears.DataTextField="years" ;
			ddlYears.DataValueField="years";
			ddlYears.DataBind();
			ddlYears.SelectedIndex = 0;
	
		}
		
        /// <summary>
        /// Used by local and family history form only, can be deleted when that is moved into eforms
        /// </summary>
        /// <param name="ddlDays"></param>
        /// <param name="ddlMonths"></param>
        /// <param name="ddlYears"></param>
        public static void PopulateEventDate(DropDownList ddlDays, DropDownList ddlMonths, DropDownList ddlYears)
		{
			
			DataTable dtDays = new DataTable();
			DataColumn dcday = new DataColumn("days");
			dtDays.Columns.Add("days");
			
			
			object[] DayRowVals = new object[1];
			
			DayRowVals[0] = "-Select-"; //increased ddlDate css to 25%
			dtDays.Rows.Add(DayRowVals);
			for (int i = 1; i < 32; i++)
			{
				
				DayRowVals[0] = i;
				
				dtDays.Rows.Add(DayRowVals);

			}
			ddlDays.DataSource = dtDays;
			ddlDays.DataTextField="days";
			ddlDays.DataValueField="days";
			ddlDays.DataBind();
			ddlDays.SelectedIndex = 0;



			/////////////////////////////////
			
			DataTable dtMonths = new DataTable();
			DataColumn dcMonths = new DataColumn("months");
			DataColumn dcIndexMonths = new DataColumn("monthindex");
			dtMonths.Columns.Add(dcMonths);
			dtMonths.Columns.Add(dcIndexMonths);
			object[] MonthRowVals = new Object[1];
			string[] Months = GetMonths();
			
			MonthRowVals[0] = "-Select-";
			dtMonths.Rows.Add(MonthRowVals);
			
			for(int i = 0; i < 12; i++)
			{
				DataRow row = dtMonths.NewRow();
				row["months"] = Months[i];
				int currentmonth = i;
				row["monthindex"] = currentmonth + 1;
				dtMonths.Rows.Add(row);
			}

			
				
			
			ddlMonths.DataSource = dtMonths;
			ddlMonths.DataTextField="months" ;
			ddlMonths.DataValueField="monthindex";
			ddlMonths.DataBind();
			ddlMonths.SelectedIndex = 0;

			/////////////////////////////////
			
			DataTable dtYears = new DataTable();
			DataColumn dcYears = new DataColumn("years");
			dtYears.Columns.Add(dcYears);
			object[] YearRowVals = new Object[1];
			int yearNow = DateTime.Now.Year;
			int startYear = yearNow;
			int endYear = yearNow + 2;
			YearRowVals[0] = "-Select-";
			dtYears.Rows.Add(YearRowVals);

			for(int i = startYear; i < endYear; i++)
			{
				int y = i;
				YearRowVals[0] = y;
				dtYears.Rows.Add(YearRowVals);
			}
			ddlYears.DataSource = dtYears;
			ddlYears.DataTextField="years" ;
			ddlYears.DataValueField="years";
			ddlYears.DataBind();
			ddlYears.SelectedIndex = 0;
	
		}

		private static string[] GetMonths()
		{
			string[] Months = new String[12];
			Months[0] = "January";
			Months[1] = "February";
			Months[2] = "March";
			Months[3] = "April";
			Months[4] = "May";
			Months[5] = "June";
			Months[6] = "July";
			Months[7] = "August";
			Months[8] = "September";
			Months[9] = "October";
			Months[10] = "November";
			Months[11] = "December";
			return Months;
		}

	}
				
}

		


