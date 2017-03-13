using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Common;
using System.Text;
using BSD.Dal;
namespace IntelligentShop
{
    public partial class About : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getDropDownProductID();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name=""></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
        private void getDropDownProductID()
        {
            DataTable dt = new DataTable();
            string query = "select productId from Product";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbDataAdapter da = dac.CreateDataAdapter(query);
                da.Fill(dt);
            }
            ddlProductId.DataSource = dt;
            ddlProductId.DataValueField = "productId";
            ddlProductId.DataTextField = "productId";
            ddlProductId.DataBind();
        }
    }
}