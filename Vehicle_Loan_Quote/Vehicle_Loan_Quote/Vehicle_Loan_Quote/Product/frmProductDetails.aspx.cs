using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Vehicle_Loan_Quote.BLL;

namespace Vehicle_Loan_Quote.Product
{
    public partial class frmProductDetails : System.Web.UI.Page
    {
        #region Variable Declaration

        Exception_Tracking objLog = new Exception_Tracking();
        VehicleDetailsBLL objVehicleDetails = new VehicleDetailsBLL();
        Calculation objCalculation = new Calculation();

        DataSet objDs = new DataSet();

        #endregion

        #region Built In Functions

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    divQuote.Visible = false;
                }

            }
            catch (Exception ex)
            {
                objLog.ErrorLog("Log_" + ex.ToString());
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                objDs = new DataSet();
                objVehicleDetails = new VehicleDetailsBLL();
                objCalculation = new Calculation();

                objVehicleDetails.vehiclePrice = Convert.ToDecimal(txtVehiclePrice.Text);
                objVehicleDetails.depositAmount = Convert.ToDecimal(txtDepositAmount.Text);
                objVehicleDetails.deliveryDate = Convert.ToDateTime(txtDeliveryDate.Text);
                objVehicleDetails.financeOptions = Convert.ToInt32(ddlTerm.SelectedValue);

                objDs = objCalculation.calcLoanQuote(objVehicleDetails);

                if (objDs != null && objDs.Tables.Count > 0)
                {
                    lblTotalLoanAmount.Text = String.Format("{0:C}", objDs.Tables[0].Rows[0]["TotalLoanAmount"]);
                    lblMontlyPayable.Text = String.Format("{0:C}", objDs.Tables[0].Rows[0]["LoanAmount"]);

                    gridLoanDetails.DataSource = objDs.Tables[1];
                    gridLoanDetails.DataBind();
                    divQuote.Visible = true;
                }
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ClearValues();
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
        }

        #endregion

        #region User Defined Functions

        public void ClearValues()
        {
            try
            {
                txtVehiclePrice.Text = "";
                txtDepositAmount.Text = "";
                txtDeliveryDate.Text = "";
                ddlTerm.SelectedIndex = 0;

                gridLoanDetails.DataSource = null;
                gridLoanDetails.DataBind();
                divQuote.Visible = false;
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
        }



        #endregion


    }
}