﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages.Popups.Sale
{
    public partial class AllPotn_TnC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String selectedRFQ = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
                BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRFQ);


                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL])
                {
                    if (BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL.Equals(Request.QueryString.GetValues("createMode")[0]))
                    {
                        Button_TnC.Enabled = true; TextBox_TnC.Enabled = true;
                    }
                    else
                    {
                        Label_TnC_Stat.Visible = true;
                        Label_TnC_Stat.Text = "You can't edit this Potential T&C as this was not created by your orginzation";
                        Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    Label_TnC_Stat.Visible = true;
                    Label_TnC_Stat.Text = "You dont have edit access to Potential records";
                    Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
                }

                String tnCText = rfqObj.getTermsandConds();

                TextBox_TnC.Text = (!tnCText.Equals("") ? tnCText : TextBox_TnC.Text);
            }
        }

        protected void Button_TnC_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> tagetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
            tagetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_T_AND_C, TextBox_TnC.Text);

            try
            {
                BackEndObjects.RFQDetails.updateRFQDetailsDB(tagetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_TnC_Stat.Visible = true;
                Label_TnC_Stat.Text = "T&C details updated successfully";
                Label_TnC_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_TnC_Stat.Visible = true;
                Label_TnC_Stat.Text = "T&C details update Failed";
                Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
            }

        }
    }
}