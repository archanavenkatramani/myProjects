using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Vehicle_Loan_Quote.BLL;
using System.Configuration;

namespace Vehicle_Loan_Quote.BLL
{
    public class Calculation
    {
        #region Variable Declaration
        VehicleDetailsBLL objVehicleDetails = new VehicleDetailsBLL();
        Exception_Tracking objLog = new Exception_Tracking();

        DataSet objDs = new DataSet();
        DataTable objDt = new DataTable();
        DataTable objCalcDt = new DataTable();
        DataTable objDetailsDt = new DataTable();
        int iMonths = 1;
        int iLoanTerm = 1;
        decimal decLoanAmount;       
        decimal decLoanTotalAmount;
        decimal decAmountDue;
        DateTime dtGetMondayDate = new DateTime();
        DateTime dtDueDate = new DateTime();
        decimal decArrangementFee;
        decimal decCompletionFee;
        
        #endregion

        public DataSet calcLoanQuote(VehicleDetailsBLL objVehicleDetails)
        {
            objDs = new DataSet();
            objCalcDt = new DataTable();
            dtDueDate = new DateTime();
            try
            {
                decLoanTotalAmount = (objVehicleDetails.vehiclePrice - objVehicleDetails.depositAmount) ;
                iLoanTerm = convertYeartoMonths(objVehicleDetails.financeOptions);
                decLoanAmount = Convert.ToDecimal(decLoanTotalAmount / iLoanTerm);
                dtDueDate = getMondayofeverymonth(Convert.ToDateTime(objVehicleDetails.deliveryDate));
                decArrangementFee = Convert.ToDecimal(ConfigurationManager.AppSettings["ArrangementFee"].ToString());
                decCompletionFee = Convert.ToDecimal(ConfigurationManager.AppSettings["CompletionFee"].ToString());
                objDetailsDt = buildDetailsDTColumns();
                objCalcDt = buildColumns();

                DataRow dr = objDetailsDt.NewRow();
                dr["TotalLoanAmount"] = decLoanTotalAmount;
                dr["LoanAmount"] = decLoanAmount;
                objDetailsDt.Rows.Add(dr);
                objDs.Tables.Add(objDetailsDt);

                for (int i=1; i<= iLoanTerm; i++)
                {
                    decAmountDue = Convert.ToDecimal(decLoanTotalAmount - decLoanAmount);
                    if (i == 1)
                    {                        
                        DataRow drow = objCalcDt.NewRow();
                        drow["loanAmount"] = Convert.ToDecimal(decLoanAmount) + decArrangementFee; // The first month add an £88 arrangement fee
                        drow["loanDate"] = dtDueDate;
                        drow["amountDue"] = decAmountDue;
                        objCalcDt.Rows.Add(drow);
                        dtDueDate = getMondayofeverymonth(Convert.ToDateTime(dtDueDate));
                        decLoanTotalAmount = decAmountDue;
                    }
                    else if (i == iLoanTerm)
                    {
                        DataRow drow = objCalcDt.NewRow();
                        drow["loanAmount"] = Convert.ToDecimal(decLoanAmount) + decCompletionFee; //the last a £20 completion fee
                        drow["loanDate"] = dtDueDate;
                        drow["amountDue"] = Convert.ToDecimal(decLoanTotalAmount - decLoanAmount);
                        objCalcDt.Rows.Add(drow);
                    }
                    else
                    {
                        DataRow drow = objCalcDt.NewRow();
                        drow["loanAmount"] = Convert.ToDecimal(decLoanAmount);
                        drow["loanDate"] = dtDueDate;
                        drow["amountDue"] = Convert.ToDecimal(decLoanTotalAmount - decLoanAmount);
                        objCalcDt.Rows.Add(drow);
                        dtDueDate = getMondayofeverymonth(Convert.ToDateTime(dtDueDate));
                        decLoanTotalAmount = decAmountDue;
                    }
                }

                objDs.Tables.Add(objCalcDt);
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
            return objDs;
        }

        public int convertYeartoMonths(int iYear)
        {
            iMonths = 1;
            try
            {
                switch (iYear)
                {
                    case 1:
                        iMonths = 12;
                        break;
                    case 2:
                        iMonths = 24;
                        break;
                    case 3:
                        iMonths = 36;
                        break;
                }
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
            return iMonths;
        }

        public DateTime getMondayofeverymonth(DateTime dtDeliveryDate)
        {
            dtGetMondayDate = new DateTime();
            try
            {
                DateTime dtaddmonths = dtDeliveryDate.AddMonths(1);
                dtGetMondayDate = new DateTime(dtaddmonths.Year, dtaddmonths.Month, 1);
                while (dtGetMondayDate.DayOfWeek != DayOfWeek.Monday)
                {
                    dtGetMondayDate = dtGetMondayDate.AddDays(1);
                }
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
            return dtGetMondayDate;
        }

        public DataTable buildColumns()
        {
            objDt = new DataTable();
            try
            {
                objDt.Columns.Add("loanDate", typeof(DateTime));
                objDt.Columns.Add("loanAmount", typeof(decimal));
                objDt.Columns.Add("amountDue", typeof(decimal));
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
            return objDt;
        }

        public DataTable buildDetailsDTColumns()
        {
            objDt = new DataTable();
            try
            {                
                objDt.Columns.Add("TotalLoanAmount", typeof(decimal));
                objDt.Columns.Add("LoanAmount", typeof(decimal));
            }
            catch (Exception ex)
            {
                objLog.ErrorLog(ex.ToString());
            }
            return objDt;
        }
    }
}