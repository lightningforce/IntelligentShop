using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BSD.Dal;
namespace IntelligentShop
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvProduct.DataSource = getProductInventory();
                gvProduct.DataBind();
            }
        }
        private DataTable getProductInventory()
        {
            DataTable dt = new DataTable();
            string query = "select productName,amount from vproductAmount";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbDataAdapter da = dac.CreateDataAdapter(query);
                da.Fill(dt);
            }
            return dt;
        }
        
    }
}