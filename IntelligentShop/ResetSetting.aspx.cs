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
using System.Net.Sockets;
using System.Net;

namespace IntelligentShop
{
    public partial class ResetSetting : System.Web.UI.Page
    {
        private string _portNo = "1111";
        private string[] _device;
        private string[] _range;
        private string _textFromBoard;

        protected void Page_Load(object sender, EventArgs e)
        {

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
        }
    }
}