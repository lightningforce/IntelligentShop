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
        private bool isCheck;
        private string textFromBoard = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                gvProduct.DataSource = getProductInventory();
                gvProduct.DataBind();
                //startThread();
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
            isCheck = true;
            threadUdp.Start();            
        }
        /// <summary>
        /// stop thread
        /// </summary>
        private void stopThread()
        {
            isCheck = false;
        }
        /// <summary>
        /// get string from board
        /// </summary>
        private void serverThread()
        {
            
            string portNo = "";//Port no
            UdpClient udpClient = new UdpClient(Convert.ToInt32(portNo));
            while (isCheck)
            {
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                Byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                textFromBoard = returnData.ToString();                
                //this.Invoke(new EventHandler(updatetest));
            }
            udpClient.Close();
            Thread.CurrentThread.Abort();
        }
        //public void updatetest(object sender, EventArgs e)
        //{
        //    richTextBox1.AppendText(settext);
        //    richTextBox1.AppendText("\r");
        //    label3.Text = settext;
        //}
    }
}