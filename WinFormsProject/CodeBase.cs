using EllieMae.Encompass.Automation;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.Forms;
using System;

namespace WinFormsProject
{
    
    public class CodeBase : Form
    {
        public Button btnFetchLoan = null;

        public override void CreateControls()
        {
            try
            {
                btnFetchLoan = (Button)FindControl("btnFetchLoan");

                if (btnFetchLoan == null)
                    return;

                btnFetchLoan.Click -= btnFetchLoan_Click;
                btnFetchLoan.Click += btnFetchLoan_Click;

            }
            catch (Exception ex)
            {
                Macro.Alert("Control Init Error: " + ex.Message);
            }

        }
        private void btnFetchLoan_Click(object sender, EventArgs e)
        {
            try
            {
                
                    Loan loan = EncompassApplication.CurrentLoan;
                    if (loan == null)
                    {
                        Macro.Alert("Loan not found.");
                        return;
                    }

                    Form1 frm = new Form1();
                    frm.Show();
            }
            catch (Exception ex)
            {
                Macro.Alert("Open Form Error: " + ex.Message);
            }
        }

    }
}