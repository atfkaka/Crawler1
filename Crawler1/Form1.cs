using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;

namespace Crawler1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn1_Click(object sender, EventArgs e)
        {//Q
            string url = tbUrl.Text;
            wb1.Navigate(url);
            tbSource.Text = getHtml(url, null);
            ;
        }


        //url是要访问的网站地址，charSet是目标网页的编码，如果传入的是null或者""，那就自动分析网页的编码
        private string getHtml(string url, string charSet)
        {
            //创建WebClient实例myWebClient
            WebClient myWebClient = new WebClient();
            // 需要注意的：
            //有的网页可能下不下来，有种种原因比如需要cookie,编码问题等等
            //这是就要具体问题具体分析比如在头部加入cookie
            // webclient.Headers.Add("Cookie", cookie);
            //这样可能需要一些重载方法。根据需要写就可以了

            //获取或设置用于对向 Internet 资源的请求进行身份验证的网络凭据。
            myWebClient.Credentials = CredentialCache.DefaultCredentials;
            //如果服务器要验证用户名,密码
            //NetworkCredential mycred = new NetworkCredential(struser, strpassword);
            //myWebClient.Credentials = mycred;
            //从资源下载数据并返回字节数组。（加@是因为网址中间有"/"符号）
            byte[] myDataBuffer = myWebClient.DownloadData(url);
            string strWebData = Encoding.Default.GetString(myDataBuffer);

            //获取网页字符编码描述信息
            Match charSetMatch = Regex.Match(strWebData, "<meta([^<]*)charset=([^<]*)\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string webCharSet = charSetMatch.Groups[2].Value;
            if (webCharSet != null || webCharSet != "")
                webCharSet = webCharSet.ToUpper();
            if (webCharSet.IndexOf("UTF-8")>0)
            {
                webCharSet = "UTF-8";
            }
            else if (webCharSet.IndexOf("GB2312") > 0)
            {
                webCharSet = "GB2312";
            }
            else if (webCharSet.IndexOf("GBK") > 0)
            {
                webCharSet = "GBK";
            }
            //            if (charSet != null && charSet != "" && Encoding.GetEncoding(charSet) != Encoding.Default)
            if (webCharSet != null && webCharSet != "" )
            {
                Encoding enc = Encoding.GetEncoding(webCharSet);
                strWebData = enc.GetString(myDataBuffer);
            }
                return strWebData;
        }        

        private void tbUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)//判断回车键  
            {
                this.btn1_Click(sender, e);//触发按钮事件 
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            tbUrl.Focus();
        }
    }
}
