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
        private bool _isCheck;
        private string _textFromBoard = string.Empty;
        private string _productNameA = string.Empty;
        private string _productNameB = string.Empty;
        private string _productNameC = string.Empty;
        private string _productNameD = string.Empty;
        private string _productNameE = string.Empty;
        private int _unitPriceA = 0;
        private int _unitPriceB = 0;
        private int _unitPriceC = 0;
        private int _unitPriceD = 0;
        private int _unitPriceE = 0;
        private int _quantityA = 0;
        private int _quantityB = 0;
        private int _quantityC = 0;
        private int _quantityD = 0;
        private int _quantityE = 0;
        private int _totalPriceA = 0;
        private int _totalPriceB = 0;
        private int _totalPriceC = 0;
        private int _totalPriceD = 0;
        private int _totalPriceE = 0;
        private int _totalCartPrice = 0;
        private DataTable _dt = new DataTable();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                gvProduct.DataSource = getProductInventory();
                gvProduct.DataBind();
                //startThread();
                _dt.Columns.AddRange(new DataColumn[4] { new DataColumn("productName"), new DataColumn("quantity"), new DataColumn("unitPrice"), new DataColumn("totalPrice") });
                gvCart.DataSource = _dt;
                gvCart.DataBind();
            }
        }
        /// <summary>
        /// get product amount in inventory
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// get ip address
        /// </summary>
        /// <returns></returns>
        private string getIp()
        {
            IPHostEntry host = null;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "192.168.0.0";
        }
        /// <summary>
        /// start thread
        /// </summary>
        private void startThread()
        {
            Thread threadUdp = new Thread(new ThreadStart(serverThread));
            _isCheck = true;
            threadUdp.Start();
        }
        /// <summary>
        /// stop thread
        /// </summary>
        private void stopThread()
        {
            _isCheck = false;
        }
        /// <summary>
        /// get string from board
        /// </summary>
        private void serverThread()
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
            int tempQuantityA = 0;
            int tempQuantityB = 0;
            int tempQuantityC = 0;
            int tempQuantityD = 0;
            int tempQuantityE = 0;
            string portNo = "1111";//Port no
            UdpClient udpClient = new UdpClient(Convert.ToInt32(portNo));
            while (_isCheck)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                _textFromBoard = returnData.ToString();
                //this.Invoke(new EventHandler(updatetest));
                word = _textFromBoard.Split(',');
                sensorA = word[0];
                _productNameA = getProductName(sensorA);
                _unitPriceA = getUnitPrice(_productNameA);
                rangeA = int.Parse(word[1]);
                _quantityA = getQuantity(getProductID(_productNameA),rangeA);
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

                if (_quantityA != tempQuantityA || _quantityB != tempQuantityB 
                || _quantityC != tempQuantityC || _quantityD != tempQuantityD 
                || _quantityE != tempQuantityE)
                {
                    if (_quantityA < tempQuantityA)
                    {
                        //คืน
                    }
                    else
                    {
                        _totalPriceA = calculateTotalPrice(tempQuantityA,_unitPriceA);
                        _totalPriceB = calculateTotalPrice(tempQuantityB, _unitPriceB);
                        _totalPriceC = calculateTotalPrice(tempQuantityC, _unitPriceE);
                        _totalPriceD = calculateTotalPrice(tempQuantityD, _unitPriceC);
                        _totalPriceE = calculateTotalPrice(tempQuantityE, _unitPriceE);
                        addToCart(tempQuantityA, tempQuantityB, tempQuantityC, tempQuantityD, tempQuantityE);
                    }
                    tempQuantityA = _quantityA;
                    tempQuantityB = _quantityB;
                    tempQuantityC = _quantityC;
                    tempQuantityD = _quantityD;
                    tempQuantityE = _quantityE; 
                }
            }
            udpClient.Close();
            Thread.CurrentThread.Abort();
        }
        private void addToCart(int tempQuantityA,int tempQuantityB,int tempQuantityC,int tempQuantityD,int tempQuantityE)
        {
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
        private int getQuantity(int productID,int range)
        {
            int quantity = 0;
            DataTable dt = null;
            if (productID == 1)
            {
                dt = getRange(productID);
                if (range < int.Parse(dt.Rows[0]["range1"].ToString()))
                {
                    quantity = 0;
                }
                else if (range == int.Parse(dt.Rows[0]["range1"].ToString()) && range < int.Parse(dt.Rows[0]["range2"].ToString()))
                {
                    quantity = 1;
                }
                else if (range == int.Parse(dt.Rows[0]["range2"].ToString()) && range < int.Parse(dt.Rows[0]["range3"].ToString()))
                {
                    quantity = 2;
                }
                else if (range == int.Parse(dt.Rows[0]["range3"].ToString()) && range < int.Parse(dt.Rows[0]["range4"].ToString()))
                {
                    quantity = 3;
                }
                else if (range == int.Parse(dt.Rows[0]["range4"].ToString()) && range < int.Parse(dt.Rows[0]["range5"].ToString()))
                {
                    quantity = 4;
                }
                else
                {
                    quantity = 5;
                }
                dt.Clear();
            }
            //else if (productID == 2)
            //{
            //    dt = getRange(productID);
            //    if (range < int.Parse(dt.Rows[0]["range1"].ToString()))
            //    {
            //        quantity = 0;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range1"].ToString()) && range < int.Parse(dt.Rows[0]["range2"].ToString()))
            //    {
            //        quantity = 1;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range2"].ToString()) && range < int.Parse(dt.Rows[0]["range3"].ToString()))
            //    {
            //        quantity = 2;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range3"].ToString()) && range < int.Parse(dt.Rows[0]["range4"].ToString()))
            //    {
            //        quantity = 3;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range4"].ToString()) && range < int.Parse(dt.Rows[0]["range5"].ToString()))
            //    {
            //        quantity = 4;
            //    }
            //    else
            //    {
            //        quantity = 5;
            //    }
            //    dt.Clear();
            //}
            //else if (productID == 3)
            //{
            //    dt = getRange(productID);
            //    if (range < int.Parse(dt.Rows[0]["range1"].ToString()))
            //    {
            //        quantity = 0;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range1"].ToString()) && range < int.Parse(dt.Rows[0]["range2"].ToString()))
            //    {
            //        quantity = 1;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range2"].ToString()) && range < int.Parse(dt.Rows[0]["range3"].ToString()))
            //    {
            //        quantity = 2;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range3"].ToString()) && range < int.Parse(dt.Rows[0]["range4"].ToString()))
            //    {
            //        quantity = 3;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range4"].ToString()) && range < int.Parse(dt.Rows[0]["range5"].ToString()))
            //    {
            //        quantity = 4;
            //    }
            //    else
            //    {
            //        quantity = 5;
            //    }
            //    dt.Clear();
            //}
            //else if (productID == 4)
            //{
            //    dt = getRange(productID);
            //    if (range < int.Parse(dt.Rows[0]["range1"].ToString()))
            //    {
            //        quantity = 0;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range1"].ToString()) && range < int.Parse(dt.Rows[0]["range2"].ToString()))
            //    {
            //        quantity = 1;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range2"].ToString()) && range < int.Parse(dt.Rows[0]["range3"].ToString()))
            //    {
            //        quantity = 2;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range3"].ToString()) && range < int.Parse(dt.Rows[0]["range4"].ToString()))
            //    {
            //        quantity = 3;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range4"].ToString()) && range < int.Parse(dt.Rows[0]["range5"].ToString()))
            //    {
            //        quantity = 4;
            //    }
            //    else
            //    {
            //        quantity = 5;
            //    }
            //    dt.Clear();
            //}
            //else
            //{
            //    dt = getRange(5);
            //    if (range < int.Parse(dt.Rows[0]["range1"].ToString()))
            //    {
            //        quantity = 0;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range1"].ToString()) && range < int.Parse(dt.Rows[0]["range2"].ToString()))
            //    {
            //        quantity = 1;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range2"].ToString()) && range < int.Parse(dt.Rows[0]["range3"].ToString()))
            //    {
            //        quantity = 2;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range3"].ToString()) && range < int.Parse(dt.Rows[0]["range4"].ToString()))
            //    {
            //        quantity = 3;
            //    }
            //    else if (range == int.Parse(dt.Rows[0]["range4"].ToString()) && range < int.Parse(dt.Rows[0]["range5"].ToString()))
            //    {
            //        quantity = 4;
            //    }
            //    else
            //    {
            //        quantity = 5;
            //    }
            //    dt.Clear();
            //}
            return quantity;
        }
        private void addRow(string productName,int quanity,int unitPrice,int totalPrice)
        {
            DataRow dr = _dt.NewRow();
            dr["productName"] = productName;
            dr["quantity"] = quanity;
            dr["unitPrice"] = unitPrice;
            dr["totalPrice"] = totalPrice;
            gvCart.DataSource = _dt;
            gvCart.DataBind();     
        }
        private DataTable getRange(int productID)
        {
            DataTable dt = null;
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
    }
}