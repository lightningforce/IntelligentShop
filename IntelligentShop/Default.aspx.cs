using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BSD.Dal;
namespace IntelligentShop
{
    public partial class Default : System.Web.UI.Page
    {
        string _portNo = "4455";
        private bool _isCheck = true;
        private string _textFromBoard = string.Empty;
        private string _productNameA = string.Empty;
        private string _productNameB = string.Empty;
        private string _productNameC = string.Empty;
        private string _productNameD = string.Empty;
        private string _productNameE = string.Empty;
        private int _unitPriceA;
        private int _unitPriceB;
        private int _unitPriceC;
        private int _unitPriceD;
        private int _unitPriceE;
        private int _quantityA;
        private int _quantityB;
        private int _quantityC;
        private int _quantityD;
        private int _quantityE;
        private int _totalPriceA;
        private int _totalPriceB;
        private int _totalPriceC;
        private int _totalPriceD;
        private int _totalPriceE;
        private int _totalCartPrice;
        private int _tempQuantityA;
        private int _tempQuantityB;
        private int _tempQuantityC;
        private int _tempQuantityD;
        private int _tempQuantityE;
        private DataTable _dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                gvProduct.DataSource = getProductInventory();
                gvProduct.DataBind();

                udpListener();
                wordSplitter();
                updateTest();

                _dt = new DataTable();
                _dt.Columns.AddRange(new DataColumn[4] { new DataColumn("productName"), new DataColumn("quantity"), new DataColumn("unitPrice"), new DataColumn("totalPrice") });
                ViewState["Cart"] = _dt;
                gvCart.DataSource = (DataTable)ViewState["Cart"];
                gvCart.DataBind();

                displayCart();
                _totalCartPrice = getTotalCartPrice();
                lblTotal.Text = _totalCartPrice.ToString();
            }
        }
        /// <summary>
        /// get product amount in inventory
        /// </summary>
        /// <returns></returns>

        private void udpListener()
        {
            UdpClient udpClient = new UdpClient(Convert.ToInt32(_portNo));
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);
            _textFromBoard = returnData.ToString();
            udpClient.Close();
        }

        private void wordSplitter()
        {
            string[] word;
            string sensorA = string.Empty;
            string sensorB = string.Empty;
            string sensorC = string.Empty;
            string sensorD = string.Empty;
            string sensorE = string.Empty;
            int rangeA = 0;
            int rangeB = 0;
            int rangeC = 0;
            int rangeD = 0;
            int rangeE = 0;
            word = _textFromBoard.Split(',');
            sensorA = word[0];
            _productNameA = getProductName(sensorA);
            _unitPriceA = getUnitPrice(_productNameA);
            rangeA = int.Parse(word[1]);
            _quantityA = getQuantity(getProductID(_productNameA), rangeA);
            sensorB = word[2];
            _productNameB = getProductName(sensorB);
            _unitPriceB = getUnitPrice(_productNameB);
            rangeB = int.Parse(word[3]);
            _quantityB = getQuantity(getProductID(_productNameB), rangeB);
            sensorC = word[4];
            _productNameC = getProductName(sensorC);
            _unitPriceC = getUnitPrice(_productNameC);
            rangeC = int.Parse(word[5]);
            _quantityC = getQuantity(getProductID(_productNameC), rangeC);
            sensorD = word[6];
            _productNameD = getProductName(sensorD);
            _unitPriceD = getUnitPrice(_productNameD);
            rangeD = int.Parse(word[7]);
            _quantityD = getQuantity(getProductID(_productNameD), rangeD);
            sensorE = word[8];
            _productNameE = getProductName(sensorE);
            _unitPriceE = getUnitPrice(_productNameE);
            rangeE = int.Parse(word[9]);
            _quantityE = getQuantity(getProductID(_productNameE), rangeE);
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
        /// <summary>
        /// get product that remove from shelf
        /// </summary>
        private void checkProductAmount()
        {
            //wait for condition
        }

        private void updateTest()
        {

            DataTable dt = getTempQuantity();
            _tempQuantityA = int.Parse(dt.Rows[0]["tempQuantityA"].ToString());
            _tempQuantityB = int.Parse(dt.Rows[0]["tempQuantityB"].ToString());
            _tempQuantityC = int.Parse(dt.Rows[0]["tempQuantityC"].ToString());
            _tempQuantityD = int.Parse(dt.Rows[0]["tempQuantityD"].ToString());
            _tempQuantityE = int.Parse(dt.Rows[0]["tempQuantityE"].ToString());
            _totalPriceA = calculateTotalPrice(_quantityA, _unitPriceA);
            _totalPriceB = calculateTotalPrice(_quantityB, _unitPriceB);
            _totalPriceC = calculateTotalPrice(_quantityC, _unitPriceC);
            _totalPriceD = calculateTotalPrice(_quantityD, _unitPriceD);
            _totalPriceE = calculateTotalPrice(_quantityE, _unitPriceE);

            if (_quantityA != _tempQuantityA)
            {
                if (hasInCart(_productNameA))
                {
                    if (_quantityA == 0)
                    {
                        deleteCart(_productNameA);
                    }
                    else
                    {
                        updateCart(_productNameA, _quantityA,_totalPriceA);
                    }
                }
                else
                {
                    insertToCart(_productNameA, _quantityA, _unitPriceA, _totalPriceA);

                }
            }
            if (_quantityB != _tempQuantityB)
            {
                if (hasInCart(_productNameB))
                {
                    if (_quantityB == 0)
                    {
                        deleteCart(_productNameB);
                    }
                    else
                    {
                        updateCart(_productNameB, _quantityB,_totalPriceB);
                    }
                }
                else
                {
                    insertToCart(_productNameB, _quantityB, _unitPriceB, _totalPriceB);

                }
            }
            if (_quantityC != _tempQuantityC)
            {
                if (hasInCart(_productNameC))
                {
                    if (_quantityC == 0)
                    {
                        deleteCart(_productNameC);
                    }
                    else
                    {
                        updateCart(_productNameC, _quantityC,_totalPriceC);
                    }
                }
                else
                {
                    insertToCart(_productNameC, _quantityC, _unitPriceC,_totalPriceC);

                }
            }
            if (_quantityD != _tempQuantityD)
            {
                if (hasInCart(_productNameD))
                {
                    if (_quantityD == 0)
                    {
                        deleteCart(_productNameD);
                    }
                    else
                    {
                        updateCart(_productNameD, _quantityD,_totalPriceD);
                    }
                }
                else
                {
                    insertToCart(_productNameD, _quantityD, _unitPriceD, _totalPriceD);

                }
            }
            if (_quantityE != _tempQuantityE)
            {
                if (hasInCart(_productNameE))
                {
                    if (_quantityE == 0)
                    {
                        deleteCart(_productNameE);
                    }
                    else
                    {
                        updateCart(_productNameE, _quantityE,_totalPriceE);
                    }
                }
                else
                {
                    insertToCart(_productNameE, _quantityE, _unitPriceE, _totalPriceE);

                }
            }
            updateTempQuantity(_quantityA, _quantityB, _quantityC, _quantityD, _quantityE);
        }
        public void updateCart(string productName, int quantity,int totalPrice)
        {
            string query = "update Cart set quantity = @quantity,totalPrice = @totalPrice where productName = @productName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@quantity", quantity));
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                cmd.Parameters.Add(dac.CreateParameter("@totalPrice", totalPrice));
                cmd.ExecuteNonQuery();
            }
        }
        public void deleteCart(string productName)
        {
            string query = "delete from Cart where productName = @productName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                cmd.ExecuteNonQuery();
            }
        }
        private bool hasInCart(string productName)
        {
            bool has = false;
            DataTable dt = new DataTable();
            string query = "select productName from Cart where productName = @productName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                DbDataAdapter da = dac.CreateDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count > 0)
            {
                has = true;
            }
            else
            {
                has = false;
            }
            return has;
        }
        private void addToCart(int QuantityA, int QuantityB, int QuantityC, int QuantityD, int QuantityE)
        {
            if (QuantityA != _tempQuantityA)
            {
                insertToCart(_productNameA, QuantityA, _unitPriceA, _totalPriceA);
            }
            if (QuantityB != _tempQuantityB)
            {
                insertToCart(_productNameB, QuantityB, _unitPriceB, _totalPriceB);
            }
            if (QuantityC != _tempQuantityC)
            {
                insertToCart(_productNameC, QuantityC, _unitPriceC, _totalPriceC);
            }
            if (QuantityD != _tempQuantityD)
            {
                insertToCart(_productNameD, QuantityD, _unitPriceD, _totalPriceD);
            }
            if (QuantityE != _tempQuantityE)
            {
                insertToCart(_productNameE, QuantityE, _unitPriceE, _totalPriceE);
            }
            //if (quantity != 0)
            //{
            //    addRow(productName,quantity,unitPrice,totalPrice);

            //    _totalCartPrice = _totalCartPrice + totalPrice;
            //    lblTotal.Text = _totalCartPrice.ToString();
            //}

        }
        /// <summary>
        /// get quntity
        /// </summary>
        /// <param name="productID"></param>
        /// <returns>quantity</returns>
        private int getQuantity(int productID, int range)
        {
            int quantity = 0;
            DataTable dt = null;
            if (productID == 1)
            {
                dt = getRange(productID);
                if (range <= int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range > int.Parse(dt.Rows[0]["range1"].ToString()) && range <= int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range > int.Parse(dt.Rows[0]["range2"].ToString()) && range <= int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range > int.Parse(dt.Rows[0]["range3"].ToString()) && range <= int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range > int.Parse(dt.Rows[0]["range4"].ToString()) && range <= int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            else if (productID == 2)
            {
                dt = getRange(productID);
                if (range <= int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range > int.Parse(dt.Rows[0]["range1"].ToString()) && range <= int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range > int.Parse(dt.Rows[0]["range2"].ToString()) && range <= int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range > int.Parse(dt.Rows[0]["range3"].ToString()) && range <= int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range > int.Parse(dt.Rows[0]["range4"].ToString()) && range <= int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            else if (productID == 3)
            {
                dt = getRange(productID);
                if (range <= int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range > int.Parse(dt.Rows[0]["range1"].ToString()) && range <= int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range > int.Parse(dt.Rows[0]["range2"].ToString()) && range <= int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range > int.Parse(dt.Rows[0]["range3"].ToString()) && range <= int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range > int.Parse(dt.Rows[0]["range4"].ToString()) && range <= int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            else if (productID == 4)
            {
                dt = getRange(productID);
                if (range <= int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range > int.Parse(dt.Rows[0]["range1"].ToString()) && range <= int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range > int.Parse(dt.Rows[0]["range2"].ToString()) && range <= int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range > int.Parse(dt.Rows[0]["range3"].ToString()) && range <= int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range > int.Parse(dt.Rows[0]["range4"].ToString()) && range <= int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            else
            {
                dt = getRange(5);
                if (range <= int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range > int.Parse(dt.Rows[0]["range1"].ToString()) && range <= int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range > int.Parse(dt.Rows[0]["range2"].ToString()) && range <= int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range > int.Parse(dt.Rows[0]["range3"].ToString()) && range <= int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range > int.Parse(dt.Rows[0]["range4"].ToString()) && range <= int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            return quantity;
        }
        private void insertToCart(string productName, int quantity, int unitPrice, int totalPrice)
        {
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand("usp_InsertToCart");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                cmd.Parameters.Add(dac.CreateParameter("@quantity", quantity));
                cmd.Parameters.Add(dac.CreateParameter("@unitPrice", unitPrice));
                cmd.Parameters.Add(dac.CreateParameter("@totalPrice", totalPrice));
                cmd.ExecuteNonQuery();
            }
        }
        private void addRow(string productName, int quanity, int unitPrice, int totalPrice)
        {
            DataTable dt = (DataTable)ViewState["Cart"];
            DataRow dr = dt.NewRow();
            dr["productName"] = productName;
            dr["quantity"] = quanity;
            dr["unitPrice"] = unitPrice;
            dr["totalPrice"] = totalPrice;
            dt.Rows.Add(dr);
            gvCart.DataSource = dt;
            gvCart.DataBind();
        }
        private DataTable getRange(int productID)
        {
            DataTable dt = new DataTable();
            string query = "select range1,range2,range3,range4,range5 from Range where productID = @productID";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@productID", productID));
                DbDataAdapter da = dac.CreateDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// get productname based on sensor name
        /// </summary>
        /// <param name="sensorName"></param>
        /// <returns></returns>
        private string getProductName(string sensorName)
        {
            string productName = string.Empty;
            string query = "select productName from Product where sensorName = @sensorName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@sensorName", sensorName));
                productName = cmd.ExecuteScalar().ToString();
            }
            return productName;
        }
        /// <summary>
        /// get unit price of each product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        private int getUnitPrice(string productName)
        {
            int unitPrice = 0;
            string query = "select unitPrice from Product where productName = @productName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                unitPrice = int.Parse(cmd.ExecuteScalar().ToString());
            }
            return unitPrice;
        }
        /// <summary>
        /// get product id from database
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        private int getProductID(string productName)
        {
            int pid = 0;
            string query = "select productId from Product where productName = @productName";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(dac.CreateParameter("@productName", productName));
                pid = int.Parse(cmd.ExecuteScalar().ToString());
            }
            return pid;
        }
        /// <summary>
        /// calculate total price of each product
        /// </summary>
        /// <param name="quantity"></param>
        /// <param name="unitPrice"></param>
        /// <returns></returns>
        private int calculateTotalPrice(int quantity, int unitPrice)
        {
            int totalPrice = quantity * unitPrice;
            return totalPrice;
        }
        private void displayCart()
        {
            DataTable dt = new DataTable();
            string query = "select productName,quantity,unitPrice,totalPrice from Cart";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbDataAdapter da = dac.CreateDataAdapter(query);
                da.Fill(dt);
            }
            gvCart.DataSource = dt;
            gvCart.DataBind();
        }
        private void updateTempQuantity(int QuantityA, int QuantityB, int QuantityC, int QuantityD, int QuantityE)
        {
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand("usp_updateTempQuantity");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(dac.CreateParameter("@tempQuantityA", QuantityA));
                cmd.Parameters.Add(dac.CreateParameter("@tempQuantityB", QuantityB));
                cmd.Parameters.Add(dac.CreateParameter("@tempQuantityC", QuantityC));
                cmd.Parameters.Add(dac.CreateParameter("@tempQuantityD", QuantityD));
                cmd.Parameters.Add(dac.CreateParameter("@tempQuantityE", QuantityE));
                cmd.ExecuteNonQuery();
            }
        }
        private DataTable getTempQuantity()
        {
            DataTable dt = new DataTable();
            string query = "select tempQuantityA,tempQuantityB,tempQuantityC,tempQuantityD,tempQuantityE from TempQuantity";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                DbDataAdapter da = dac.CreateDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        private int getTotalCartPrice()
        {
            int totalCartPrice = 0;
            DataTable dt = new DataTable();
            string query = "select sum(totalPrice) as totalCartPrice from Cart having sum(totalPrice) is not null";
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand(query);
                cmd.CommandType = CommandType.Text;
                DbDataAdapter da = dac.CreateDataAdapter(cmd);
                da.Fill(dt);
            }
            if (dt.Rows.Count != 0)
            {
                totalCartPrice = int.Parse(dt.Rows[0]["totalCartPrice"].ToString());
            }
            else
            {
                totalCartPrice = 0;
            }
            return totalCartPrice;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            using (DataAccess dac = new DataAccess())
            {
                dac.Open(Provider.MSSQL);
                DbCommand cmd = dac.CreateCommand("usp_Reset");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
            }
        }
    }
}