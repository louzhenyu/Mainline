using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Web.SessionState;
using System;


namespace LogView
{

    /// <summary>
    /// Function 的摘要说明
    /// </summary>
    public class Function
    {

        string MyConnString = "";
        public Function()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        #region 批次号码的自动增加
        ///
        /// 单据编号,产生自增编号,如:入参为"DJ000002",将返回"DJ000003"
        ///
        /// 原值
        /// 下一值
        public static string NextNumber(string BaseNumber)
        {
            string NewNumber = "";//新值
            int InNumber = 1;//进位
            int PlaceValue;//位值
            char[] No = BaseNumber.ToCharArray();

            for (int i = BaseNumber.Length - 1; i >= 0; i--)
            {
                if (No[i] == '9' && InNumber == 1)
                {
                    InNumber = 1;
                    NewNumber = "0" + NewNumber;
                }
                else
                    if (InNumber == 1 && No[i] >= '0' && No[i] < '9')
                    {
                        PlaceValue = Int32.Parse(No[i].ToString());
                        PlaceValue = (InNumber + PlaceValue);
                        InNumber = 0;
                        NewNumber = PlaceValue.ToString() + NewNumber;
                    }
                    else
                    {
                        InNumber = 0;
                        NewNumber = No[i] + NewNumber;
                    }
            }
            if (BaseNumber == NewNumber)
                NewNumber = "0000000001";
            return NewNumber;
        }
        #endregion
        #region 判断是否是数字
        public bool IsNum(string Number)
        {
            int outnum;
            if (int.TryParse(Number, out outnum) == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 建立多级目录
        //函数名：CreateDirect
        //参数：
        //string strmappath 是路径
        public void CreateDirect(string strmappath)
        {

            if (System.IO.Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(strmappath)))
            {

            }
            else
            {
                System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(strmappath));
            }
        }
        #endregion

        #region 获取文件夹目录
        public string GetDirectFloder(string PathHead, string FloderType)
        {
            string PathYear = DateTime.Now.Year.ToString();
            string PathMonth = DateTime.Now.Month.ToString();
            string PathDay = DateTime.Now.Day.ToString();
            string imagesfolder = PathHead + "Upload/" + FloderType + "/" + PathYear + "/" + PathMonth + "/" + PathDay + "/";
            CreateDirect(imagesfolder);
            return imagesfolder;
        }
        #endregion

        #region 生成缩略图
        //功能：生成缩略图
        //函数名：SaveThumbnailAs
        //参数：
        //file      文件对象
        //path      保存的路径
        //Xwidth    宽度
        //Xheight   高度
        //IsWord    是否有文字水印
        //IsPic     是否有图片水印
        //SavePath  保存路径
        //SaveSmallPath 缩略图路径
        //IsSmall   是否缩
        //返回值： 0 文件错误 1 类型错误 2 上传失败
        //the_Filename 返回文件名
        public int SaveThumbnailAs(HttpPostedFile file, string path, int Xwidth, int Xheight, int IsWord, string Word, int IsPic, string PicPath, string SavePath, string SaveSmallPath, int IsSmall, ref string the_Filename)
        {
            /*
            string PathYear     =DateTime.Now.Year.ToString();
            string PathMonth    =DateTime.Now.Month.ToString();
            string PathDay      =DateTime.Now.Day.ToString();
            string RealPath     ="Upload/BigPic/"+PathYear+"/"+PathMonth+"/"+PathDay+"/";
            string SmallPicPath ="Upload/SmallPic/"+PathYear+"/"+PathMonth+"/"+PathDay+"/";
            //MyFunction.SaveThumbnailAs(File1.PostedFile, RealPath, 100, 100, 1, "HelloWord", 0, "", RealPath, SmallPicPath, 1);
             */

            int ReturnValue = 100;
            HttpPostedFile hpf = file;
            string FileName = file.FileName;//文件名
            string Suffix = Path.GetExtension(FileName).ToLower();//扩展名
            char[] splitChar = { '\\' };
            string[] FilenameArray = hpf.FileName.Split(splitChar);
            string Filename = FilenameArray[FilenameArray.Length - 1].ToLower();
            if (hpf.FileName.Length < 1)
            {
                ReturnValue = 0;
            }
            if (".jpg|.jpeg|.gif|.bmp|.png".IndexOf(Suffix) < 0)
            {
                ReturnValue = 1;
            }
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(DateTime.Now.Year.ToString());
                sb.Append(DateTime.Now.Month.ToString());
                sb.Append(DateTime.Now.Day.ToString());
                sb.Append(DateTime.Now.Hour.ToString());
                sb.Append(DateTime.Now.Minute.ToString());
                sb.Append(DateTime.Now.Second.ToString());
                sb.Append(Suffix);
                Filename = sb.ToString();
                the_Filename = sb.ToString();
                try
                {
                    this.CreateDirect(SavePath);
                    hpf.SaveAs(System.Web.HttpContext.Current.Server.MapPath(SavePath) + Filename);
                }
                catch
                {

                    ReturnValue = 2;
                }

                if (IsSmall == 1)
                {
                    // 生成缩略图
                    //原始图片名称
                    string originalFilename = hpf.FileName;
                    //生成的高质量图片名称
                    this.CreateDirect(SaveSmallPath);
                    string strFile = System.Web.HttpContext.Current.Server.MapPath(SaveSmallPath) + Filename;
                    //从文件取得图片对象
                    System.Drawing.Image image = System.Drawing.Image.FromStream(hpf.InputStream, true);
                    System.Double NewWidth, NewHeight;
                    if (image.Width > image.Height)
                    {
                        NewWidth = Xwidth;
                        NewHeight = image.Height * (NewWidth / image.Width);
                    }
                    else
                    {
                        NewHeight = Xheight;
                        NewWidth = (NewHeight / image.Height) * image.Width;
                    }
                    if (NewWidth > Xwidth)
                    {
                        NewWidth = Xwidth;
                    }
                    if (NewHeight > Xheight)
                    {
                        NewHeight = Xheight;
                    }
                    System.Drawing.Size size = new Size((int)NewWidth, (int)NewHeight); // 图片大小
                    System.Drawing.Image bitmap = new System.Drawing.Bitmap(size.Width, size.Height);   //新建bmp图片
                    System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap);       //新建画板
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;       //设置高质量插值法
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;        //设置高质量,低速度呈现平滑程度
                    graphics.Clear(Color.White);        //清空画布
                    //在指定位置画图
                    graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);
                    if (IsWord == 1)
                    {
                        //文字水印
                        System.Drawing.Graphics textGraphics = System.Drawing.Graphics.FromImage(bitmap);
                        System.Drawing.Font font = new Font("宋体", 10);
                        System.Drawing.Brush brush = new SolidBrush(Color.Black);
                        textGraphics.DrawString(Word, font, brush, 10, 10);
                        textGraphics.Dispose();
                    }
                    if (IsPic == 1)
                    {
                        //图片水印
                        System.Drawing.Image copyImage = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(PicPath));
                        Graphics a = Graphics.FromImage(bitmap);
                        a.DrawImage(copyImage, new Rectangle(bitmap.Width - copyImage.Width, bitmap.Height - copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                        copyImage.Dispose();
                        a.Dispose();
                        copyImage.Dispose();
                    }
                    try
                    {
                        bitmap.Save(strFile, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ReturnValue = 3;
                    }
                    catch
                    {
                        ReturnValue = 2;
                    }
                    graphics.Dispose();
                    image.Dispose();
                    bitmap.Dispose();
                }

            }


            return ReturnValue;



        }



        #endregion

        #region SQL注入
        public string SqlIn(string str)
        {
            try
            {
                str = str.Trim();
                str = str.Replace("'", "''");
                str = str.Replace("&", "&amp;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "''");
                str = str.Replace("*", "");
                str = str.Replace("?", "");
                str = str.Replace("select", "");
                str = str.Replace("insert", "");
                str = str.Replace("delete", "");
                str = str.Replace("update", "");
                str = str.Replace("delete", "");
                str = str.Replace("create", "");
                str = str.Replace("drop", "");
                str = str.Replace("declare", "");
                str = str.Replace("and", "");
                str = str.Replace("%", "");
                str = str.Replace("like", "");
            }
            catch
            {
                str = "";
            }

            return str;
        }
        #endregion

        #region Md5编码
        public string md5(string str, int code)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符）  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            if (code == 32) //32位加密  
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
            return "00000000000000000000000000000000";
        }

        #endregion

        #region 判断是否是数字
        public bool isNumber(string s)
        {
            int Flag = 0;
            char[] str = s.ToCharArray();
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsNumber(str[i]))
                {
                    Flag++;
                }
                else
                {
                    Flag = -1;
                    break;
                }
            }
            if (Flag > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region 弹出对话框
        //Msg 弹出的消息
        //Flag 是否返回上一页  0 不返回 1 返回前一页 2 返回到ReUrl
        //ReUrl 要跳转到的地址
        public void ShowMessage(string Msg, int Flag, string ReUrl)
        {
            switch (Flag)
            {
                case 0:
                    System.Web.HttpContext.Current.Response.Write("<script>alert('" + Msg + "');</script>");
                    System.Web.HttpContext.Current.Response.End();
                    break;
                case 1:
                    System.Web.HttpContext.Current.Response.Write("<script>alert('" + Msg + "');history.back(-1)</script>");
                    System.Web.HttpContext.Current.Response.End();
                    break;
                case 2:
                    System.Web.HttpContext.Current.Response.Write("<script>alert('" + Msg + "');window.location='" + ReUrl + "'</script>");
                    System.Web.HttpContext.Current.Response.End();
                    break;
            }
        }
        #endregion

        #region 获取页面内容getPage()
        public string getPage(string url)
        {
            string strResult = "";

            try
            {
                // Create a 'WebRequest' object with the specified url. 
                WebRequest myWebRequest = WebRequest.Create(url);
                myWebRequest.Timeout = 10000;
                // Send the 'WebRequest' and wait for response.
                WebResponse myWebResponse = myWebRequest.GetResponse();
                // Obtain a 'Stream' object associated with the response object.
                Stream ReceiveStream = myWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("GB2312");

                // Pipe the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];

                // Read 256 charcters at a time.    
                int count = readStream.Read(read, 0, 256);

                while (count > 0)
                {
                    // Dump the 256 characters on a string and display the string onto the console.
                    String str = new String(read, 0, count);
                    strResult = strResult + str;
                    count = readStream.Read(read, 0, 256);
                }
                // Release the resources of stream object.
                readStream.Close();

                // Release the resources of response object.
                myWebResponse.Close();

            }
            catch (Exception e)
            {
                strResult = strResult + e.ToString();
            }

            return strResult;
        }

        #endregion

        #region 数字随机数
        /**/
        /// <summary>
        /// 数字随机数
        /// </summary>
        /// <param name="n">生成长度</param>
        /// <returns></returns>
        public static string RandNum(int n)
        {
            char[] arrChar = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < n; i++)
            {
                num.Append(arrChar[rnd.Next(0, 9)].ToString());

            }

            return num.ToString();
        }
        #endregion

        #region 数字和字母随机数
        /**/
        /// <summary>
        /// 数字和字母随机数
        /// </summary>
        /// <param name="n">生成长度</param>
        /// <returns></returns>
        public static string RandCode(int n)
        {
            char[] arrChar = new char[]{
           'a','b','d','c','e','f','g','h','i','j','k','l','m','n','p','r','q','s','t','u','v','w','z','y','x',
           '0','1','2','3','4','5','6','7','8','9',
           'A','B','C','D','E','F','G','H','I','J','K','L','M','N','Q','P','R','T','S','V','U','W','X','Y','Z'
          };

            StringBuilder num = new StringBuilder();

            Random rnd = new Random(DateTime.Now.Millisecond);
            for (int i = 0; i < n; i++)
            {
                System.Threading.Thread.Sleep(2);
                num.Append(arrChar[rnd.Next(0, arrChar.Length)].ToString());

            }

            return num.ToString();
        }
        #endregion

        #region  随机生成
        /// <summary> 
        /// 获取随机字符串 
        /// </summary> 
        /// <param name="strLength">字符串长度</param> 
        /// <param name="Seed">随机函数种子值</param> 
        /// <returns>指定长度的随机字符串</returns> 
        public static string RndString(int strLength, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();

            //因1与l不容易分清楚，所以去除 
            string strChar = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,v,w,x,y,z"
             + ",A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,X,Y,Z";

            string[] aryChar = strChar.Split(chrSep, strChar.Length);

            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }

            //生成随机字符串 
            for (int i = 0; i < strLength; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
                System.Threading.Thread.Sleep(1);
            }

            return strRandom;
        }
        #endregion
        #region  随机生成数字
        /// <summary> 
        /// 获取随机字符串 
        /// </summary> 
        /// <param name="strLength">字符串长度</param> 
        /// <param name="Seed">随机函数种子值</param> 
        /// <returns>指定长度的随机字符串</returns> 
        public static string RndString1(int strLength, params int[] Seed)
        {
            string strSep = ",";
            char[] chrSep = strSep.ToCharArray();

            //因1与l不容易分清楚，所以去除 
            string strChar = "1,2,3,4,5,6,7,8,9,0";
            string[] aryChar = strChar.Split(chrSep, strChar.Length);
            string strRandom = string.Empty;
            Random Rnd;
            if (Seed != null && Seed.Length > 0)
            {
                Rnd = new Random(Seed[0]);
            }
            else
            {
                Rnd = new Random();
            }

            //生成随机字符串 
            for (int i = 0; i < strLength; i++)
            {
                strRandom += aryChar[Rnd.Next(aryChar.Length)];
                System.Threading.Thread.Sleep(1);
            }

            return strRandom;
        }
        #endregion
        #region 日期随机函数
        /**/
        /// <summary>
        /// 日期随机函数
        /// </summary>
        /// <param name="ra">长度</param>
        /// <returns></returns>
        public static string DateRndName(System.Random ra)
        {
            DateTime d = DateTime.Now;
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            s += y + m + dd + h + mm + ss;
            s += ra.Next(100, 999).ToString();
            return s;
        }
        #endregion

        #region 格式化日期
        /**/
        /// <summary>
        /// 日期随机函数
        /// </summary>
        /// <param name="ra">长度</param>
        /// <returns></returns>
        public static string DateRndName(string OldDateTime, int DisplayType)
        {
            DateTime d = DateTime.Parse(OldDateTime);
            string s = null, y, m, dd, h, mm, ss;
            y = d.Year.ToString();
            m = d.Month.ToString();
            if (m.Length < 2) m = "0" + m;
            dd = d.Day.ToString();
            if (dd.Length < 2) dd = "0" + dd;
            h = d.Hour.ToString();
            if (h.Length < 2) h = "0" + h;
            mm = d.Minute.ToString();
            if (mm.Length < 2) mm = "0" + mm;
            ss = d.Second.ToString();
            if (ss.Length < 2) ss = "0" + ss;
            switch (DisplayType)
            {
                case 1:
                    s += y + "-" + m + "-" + dd + " " + h + ":" + mm + ":" + ss;
                    break;
                case 2:
                    s += y + "年" + m + "月" + dd + "日 " + h + "时" + mm + "分" + ss + "秒";
                    break;
                case 3:
                    s += y + "年" + m + "月" + dd + "日 ";
                    break;
                case 4:
                    s += y + "-" + m + "-" + dd + " ";
                    break;
                case 5:
                    s += m + "-" + dd + " ";
                    break;
                case 6:
                    s += y + "-" + m;
                    break;
                case 7:
                    s += y.ToString().Substring(2, 2) + "/" + m + "/" + dd + " ";
                    break;
                case 8:
                    s += m + "-" + dd;
                    break;
                case 9:
                    s += y + "年" + m + "月" + dd + "日 " + h + ":" + mm;
                    break;
            }

            return s;
        }
        #endregion

        #region 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /**/
        /// <summary>
        /// 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <returns></returns>
        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1) m = 13;
            if (m == 2) m = 14;
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7 + 1;
            string weekstr = "";
            switch (week)
            {
                case 1: weekstr = "星期一"; break;
                case 2: weekstr = "星期二"; break;
                case 3: weekstr = "星期三"; break;
                case 4: weekstr = "星期四"; break;
                case 5: weekstr = "星期五"; break;
                case 6: weekstr = "星期六"; break;
                case 7: weekstr = "星期日"; break;
            }

            return weekstr;
        }
        #endregion

        #region 自动截取字符串长度
        public string GetStrLen(string stringToSub, int length)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + "...";
            else
                return sb.ToString();
        }

        /// <summary>
        /// 自动截取字符串长度,如果超出自动添加CutStr字符
        /// </summary>
        /// <param name="stringToSub">要截取的字串</param>
        /// <param name="length">截取的长度</param>
        /// <param name="CutStr">超出后添加 的字符</param>
        /// <returns></returns>
        public string GetStrLen(string stringToSub, int length, string CutStr)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = stringToSub.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > length)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + CutStr;
            else
                return sb.ToString();
        }
        #endregion

        #region 生成验证码
        //调用实例:MyFunction.CreateCheckCodeImage(MyFunction.GenerateCheckCode());
        public string GenerateCheckCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            System.Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            HttpContext.Current.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode.ToUpper()));

            return checkCode;
        }

        public void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random random = new Random();

                //清空图片背景色
                g.Clear(Color.White);

                //画图片的背景噪音线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);

                //画图片的前景噪音点
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);

                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                //画图片的边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ContentType = "image/Gif";
                HttpContext.Current.Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion

        #region 数据库系列
        #region 获取数据库链接字符串
        //函数名：ConnStr()
        public string ConnStr()
        {

            string ConnString = "Provider=Microsoft.Jet.OleDb.4.0;";
            ConnString += @"Data Source=" + System.Web.HttpContext.Current.Server.MapPath("/Lanter/DataBase/tab_news.MDB");
            return ConnString;

        }
        #endregion

        #region 关闭对象
        public void CloseSqlObject(SqlConnection MyConn, SqlCommand MyComm, SqlDataReader MyReader)
        {
            MyReader.Close();
            MyReader.Dispose();
            MyComm.Dispose();
            MyConn.Close();
            MyConn.Dispose();
        }
        public void CloseOleObject(OleDbConnection MyConn, OleDbCommand MyComm, OleDbDataReader MyReader)
        {
            MyReader.Close();
            MyReader.Dispose();
            MyComm.Dispose();
            MyConn.Close();
            MyConn.Dispose();
        }
        #endregion

        #region 绑定DataList对像
        public void DataBind_DataList(DataList DL, string Sql, string TableName)
        {
            string RealString = ConnStr();
            OleDbConnection MyConn = new OleDbConnection(RealString);
            OleDbDataAdapter MyDr = new OleDbDataAdapter(Sql, MyConn);
            DataSet Ds = new DataSet();
            MyDr.Fill(Ds, TableName);
            DL.DataSource = Ds.Tables[0].DefaultView;
            DL.DataBind();
        }

        #endregion

        #region 执行Sql语句
        public string CommandExec(string SqlStr, int IsSqlorOle)
        {
            Function MyFunction = new Function();
            string RealString = MyFunction.ConnStr();
            // int RowCount = 0;
            string ReturnValue = "";
            if (IsSqlorOle == 1)
            {
                SqlConnection MyConn = new SqlConnection(RealString);
                SqlCommand MyComm = new SqlCommand(SqlStr, MyConn);
                MyConn.Open();
                SqlDataReader MyReader = MyComm.ExecuteReader();
                while (MyReader.Read())
                {
                    ReturnValue = MyReader["Id"].ToString();
                }
                MyReader.Close();
                MyReader.Dispose();
                MyComm.Dispose();
                MyConn.Close();
                MyConn.Dispose();


            }
            else
            {
                OleDbConnection MyConn = new OleDbConnection(RealString);
                OleDbCommand MyComm = new OleDbCommand(SqlStr, MyConn);
                MyConn.Open();
                OleDbDataReader MyReader = MyComm.ExecuteReader();
                while (MyReader.Read())
                {
                    ReturnValue = MyReader["Id"].ToString();
                }
                MyReader.Close();
                MyReader.Dispose();
                MyComm.Dispose();
                MyConn.Close();
                MyConn.Dispose();

            }
            return ReturnValue;


        }

        #endregion

        #region 返回DataSet
        public DataSet ReturnDataSet(string MySql)
        {
            #region 返回DataSet
            MyConnString = ConnStr();
            SqlConnection MyConn = new SqlConnection(MyConnString);
            SqlDataAdapter MyDataAd = new SqlDataAdapter(MySql, MyConn);
            DataSet Da = new DataSet();
            MyDataAd.Fill(Da);
            MyDataAd.Dispose();
            MyConn.Close();
            return Da;
            #endregion
        }
        #endregion

        #region 返回Sql语句
        public string ReturnSql(int PageSize, string TableName, string FieldName, int CurrentPage, string DescOrAsc, string WhereSql)
        {
            if (CurrentPage == 1)
            {
                string ReturnSql = "Select top " + PageSize + " * From " + TableName + "  Where 1=1 " + WhereSql + " order by " + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
            else
            {
                string ReturnSql = "Select Top " + PageSize + " * from " + TableName + "  where " + FieldName + " not in(select top " + (PageSize * (CurrentPage - 1)) + " " + FieldName + " from " + TableName + " order by " + FieldName + " " + DescOrAsc + ") " + WhereSql + " order by " + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageSize">页数</param>
        /// <param name="Fields">字段列</param>
        /// <param name="TableName">表名</param>
        /// <param name="FieldName">后面表的字段</param>
        /// <param name="CurrentPage">当前页</param>
        /// <param name="DescOrAsc">排序方式</param>
        /// <param name="WhereSql">条件</param>
        /// <returns></returns>
        public string ReturnSql(int PageSize, string Fields, string TableName, string FieldName, int CurrentPage, string DescOrAsc, string WhereSql)
        {
            if (CurrentPage == 1)
            {
                string ReturnSql = "Select top " + PageSize + " " + Fields + " From " + TableName + "  Where 1=1 " + WhereSql + " order by " + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
            else
            {
                string ReturnSql = "Select Top " + PageSize + " " + Fields + " from " + TableName + "  where " + FieldName + " not in(select top " + (PageSize * (CurrentPage - 1)) + " " + FieldName + " from " + TableName + " order by " + FieldName + " " + DescOrAsc + ") " + WhereSql + " order by " + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
        }
        public string ReturnSql(int PageSize, string Fields, string TableName, string FieldName, int CurrentPage, string DescOrAsc, string WhereSql, string Orderstr)
        {
            if (CurrentPage == 1)
            {
                string ReturnSql = "Select top " + PageSize + " " + Fields + " From " + TableName + "  Where 1=1 " + WhereSql + " order by " + Orderstr + "," + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
            else
            {
                string ReturnSql = "Select Top " + PageSize + " " + Fields + " from " + TableName + "  where " + FieldName + " not in(select top " + (PageSize * (CurrentPage - 1)) + " " + FieldName + " from " + TableName + " order by " + Orderstr + "," + FieldName + " " + DescOrAsc + ") " + WhereSql + " order by " + Orderstr + "," + FieldName + " " + DescOrAsc;
                return ReturnSql;
            }
        }

        #endregion

        #region 执行Sql语句
        public void Exec_Sql(string SqlString)
        {
            #region 执行sql语句
            //Sqlstring 要执行的Sql语句
            MyConnString = ConnStr();
            SqlConnection MyConn = new SqlConnection(MyConnString);
            SqlCommand MyCommand = new SqlCommand(SqlString, MyConn);
            MyConn.Open();
            MyCommand.ExecuteNonQuery();
            MyConn.Close();
            MyCommand.Dispose();
            MyConn.Dispose();
            #endregion

        }
        #endregion

        #region 得到记录个数
        public int Re_RecordCount(string SqlString)
        {
            #region  返回sql的记录个数
            //Sqlstring 要执行的Sql语句
            MyConnString = ConnStr();
            SqlConnection MyConn = new SqlConnection(MyConnString);
            SqlCommand MyCommand = new SqlCommand(SqlString, MyConn);
            MyConn.Open();
            int RecordCount = (int)MyCommand.ExecuteScalar();//返回记录个数
            MyConn.Close();
            MyCommand.Dispose();
            MyConn.Dispose();
            return RecordCount;
            #endregion
        }
        #endregion

        #region 实例
        public void User_Vote_Add_Return(int User_Vote_id)
        {
            #region 执行带返回值的存储过程
            MyConnString = ConnStr();
            SqlConnection MyConn = new SqlConnection(MyConnString);
            SqlCommand MyCommand = new SqlCommand("User_Vote_Update", MyConn);
            MyCommand.CommandType = CommandType.StoredProcedure;
            MyCommand.Parameters.Add("@User_Vote_Id", SqlDbType.Int, 4);
            MyCommand.Parameters["@User_Vote_id"].Value = User_Vote_id.ToString();
            MyCommand.Parameters.Add("@ReturnValue", SqlDbType.Int, 4);
            MyCommand.Parameters["@ReturnValue"].Direction = ParameterDirection.ReturnValue;
            MyConn.Open();
            MyCommand.ExecuteNonQuery();
            int T = (int)MyCommand.Parameters["@ReturnValue"].Value;
            HttpContext.Current.Response.Write(T.ToString());
            MyConn.Close();
            #endregion

        }
        public void User_Vote_Add_OutPut(int User_Vote_id)
        {
            #region 执行带输出值的存储过程
            //User_Vote_Id  投票的ID
            MyConnString = ConnStr();
            SqlConnection MyConn = new SqlConnection(MyConnString);
            SqlCommand MyCommand = new SqlCommand("User_Vote_Update_output", MyConn);
            MyCommand.CommandType = CommandType.StoredProcedure;
            //添加参数一
            SqlParameter UserVoteId = new SqlParameter("@User_Vote_ID", SqlDbType.Int, 4);
            UserVoteId.Value = User_Vote_id.ToString();
            MyCommand.Parameters.Add(UserVoteId);
            //添加参数二
            SqlParameter ReturnValue = new SqlParameter("@ReturnValue", SqlDbType.VarChar, 100);
            ReturnValue.Direction = ParameterDirection.Output;
            MyCommand.Parameters.Add(ReturnValue);
            //执行
            MyConn.Open();
            MyCommand.ExecuteNonQuery();
            String T = ReturnValue.Value.ToString();
            HttpContext.Current.Response.Write(T.ToString());
            MyConn.Close();

            #endregion

        }

        #endregion

        #endregion

        #region 文件操作
        #region 读取文件
        public string ReadFile(string FilePath)
        {
            string path = FilePath;
            System.IO.StreamReader reader = new System.IO.StreamReader(path, System.Text.Encoding.Default);
            string FileContent = reader.ReadToEnd();
            reader.Close();
            return FileContent;
        }
        #endregion


        #endregion

        #region 自定义分页函数
        //分页计算函数
        //AllCount 纪录总数
        //ThisPage 当前页码
        //PageSize 每页显示的数量
        //PageLink 自定义连接符
        public string GetPages(long AllCount, long ThisPage, long PageSize, string PageLink)
        {
            string sHTML = "";
            string sLeftPageHTML = "";
            string sRightPageHTML = "";
            if (PageLink == "") { PageLink = "?"; }
            //开始计算
            long iPageCount = 0;
            if (AllCount % PageSize == 0)
            {
                iPageCount = AllCount / PageSize;
            }
            else
            {
                iPageCount = (AllCount / PageSize) + 1;
            }
            for (int i = 3; i >= 1; i--)
            {
                if (ThisPage - i >= 1)
                {
                    sLeftPageHTML += "<a href=\"" + PageLink + "page=" + (ThisPage - i) + "\">[" + (ThisPage - i) + "]</a> ";
                }
            }
            for (int j = 1; j <= 3; j++)
            {
                if (ThisPage + j <= iPageCount)
                {
                    sRightPageHTML += " <a href=\"" + PageLink + "page=" + (ThisPage + j) + "\">[" + (ThisPage + j) + "]</a>";
                }
            }
            long iFPage = ThisPage - 1;
            if (iFPage < 1) { iFPage = 1; }
            long iNPage = ThisPage + 1;
            if (iNPage > iPageCount) { iNPage = iPageCount; }
            //计算结束
            //sHTML = "第 " + ThisPage.ToString() + "页  共" + iPageCount.ToString() + "页 共" + AllCount.ToString() + "条记录 <a href=\"" + PageLink + "page=1\">首页</a> <a href=\"" + PageLink + "page=" + iFPage.ToString() + "\">上一页</a> " + sLeftPageHTML + "<b>[" + ThisPage.ToString() + "]</b>" + sRightPageHTML + " <a href=\"" + PageLink + "page=" + iNPage.ToString() + "\">下一页</a> <a href=\"" + PageLink + "page=" + iPageCount.ToString() + "\">尾页</a> 转到 <input type=\"text\" style=\"font-family:'宋体'; font-size:12px ; border:solid 1px #666666;\" size=\"5\" onkeydown=\"javascript:if(event.keyCode==13 || event.which==13){if(this.value<=" + iPageCount.ToString() + "){window.location='" + PageLink + "page='+this.value;return false;}else{alert(this.value);}}\" /> 页";
            sHTML = "第 " + ThisPage.ToString() + "页  共" + iPageCount.ToString() + "页 共" + AllCount.ToString() + "条记录 <a href=\"" + PageLink + "page=1\">首页</a> <a href=\"" + PageLink + "page=" + iFPage.ToString() + "\">上一页</a> " + sLeftPageHTML + "<b>[" + ThisPage.ToString() + "]</b>" + sRightPageHTML + " <a href=\"" + PageLink + "page=" + iNPage.ToString() + "\">下一页</a> <a href=\"" + PageLink + "page=" + iPageCount.ToString() + "\">尾页</a> 转到 <input type=\"text\" style=\"font-family:'宋体'; font-size:12px ; border:solid 1px #666666;\" size=\"5\" onkeydown=\"javascript:if(event.keyCode==13 || event.which==13){if(this.value<=" + iPageCount.ToString() + "){window.location='" + PageLink + "page='+this.value;return false;}else{alert('超过实际页面数！');}}\" /> 页";
            return sHTML;
        }
        #endregion

        #region 自定义分页函数
        //分页计算函数
        //AllCount 纪录总数
        //ThisPage 当前页码
        //PageSize 每页显示的数量
        //PageLink 自定义连接符
        public string GetPages( long ThisPage, long PageSize, string PageLink,int ThisPageCount)
        {
            string sHTML = "";
            string sLeftPageHTML = "";
            string sRightPageHTML = "";
            if (PageLink == "") { PageLink = "?"; }
            long iFPage = ThisPage - 1;
            if (iFPage < 1) { iFPage = 1; }
            long iNPage = ThisPage + 1;
            sHTML = "<a href=\"" + PageLink + "page=" + iFPage.ToString() + "\">上一页</a> " + sLeftPageHTML + "<b>[" + ThisPage.ToString() + "]</b>" + sRightPageHTML + " <a href=\"" + PageLink + "page=" + iNPage.ToString() + "\">下一页</a> ";
            if (ThisPageCount == 0) 
            { 
                iNPage = ThisPage;
                sHTML = "<a href=\"" + PageLink + "page=" + iFPage.ToString() + "\">上一页</a> " + sLeftPageHTML + "<b>[" + ThisPage.ToString() + "]</b>" + sRightPageHTML + "下一页";
            }
            
            //计算结束
           
           
            return sHTML;
        }
        #endregion

        #region 获得客户端IP

        /// 获得客户端IP

        public string GetClientIP()
        {
            string ip;
            string[] temp;
            bool isErr = false;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"] == null)
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            else
                ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_ForWARDED_For"].ToString();
            if (ip.Length > 15)
                isErr = true;
            else
            {
                temp = ip.Split('.');
                if (temp.Length == 4)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].Length > 3) isErr = true;
                    }
                }
                else
                    isErr = true;
            }
            if (isErr)
                return "1.1.1.1";
            else
                return ip;
        }


        #endregion

        #region 返回页面执行时间
        public void ReturnPageRunTime(DateTime StartTime, DateTime EndTime, Label LbName)
        {
            TimeSpan TempTime = (EndTime - StartTime);
            LbName.Text = "页面执行时间：" + TempTime.TotalMilliseconds.ToString() + " 毫秒";
        }

        #endregion

        #region 全角半角转换

        /// <summary> 
        /// 转全角的函数(SBC case) 
        /// </summary> 
        /// <param name="input">任意字符串</param> 
        /// <returns>全角字符串</returns> 
        ///<remarks> 
        ///全角空格为12288，半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</remarks>         
        public static string ToSBC(string input)
        {
            //半角转全角： 
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }



        /// <summary> 
        /// 转半角的函数(DBC case) 
        /// </summary> 
        /// <param name="input">任意字符串</param> 
        /// <returns>半角字符串</returns> 
        ///<remarks> 
        ///全角空格为12288，半角空格为32 
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248 
        ///</remarks> 
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        /// <summary>
        /// 把一个字符串类型转化为可能的整型
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static int StringToInt32(string S)
        {
            S = S.Trim();
            if (S.Trim() == "") return (0);
            if (S.IndexOf("-") >= 0)
            {   //有负号，但不是开头，则转换为0
                if (S.StartsWith("-") == false) return (0);
                if (S.StartsWith("--")) return (0);
            }
            for (int i = 0; i < S.Length; i++)
            {
                switch (S.Substring(i, 1))
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        break;
                    case "-":
                        if (S.Length == 1) return (0);
                        break;
                    default:
                        if (i == 0)
                            return (0);
                        else
                            try
                            {
                                return (Convert.ToInt32(S.Substring(0, i)));
                            }
                            catch
                            {
                                return 0;
                            }
                        break;
                }
            }
            try
            {
                return (Convert.ToInt32(S));
            }
            catch
            {
                return 0;
            }
        }

        #region  日期时间
        /// <summary>
        /// 是否为合法日期类型。"YYYYMMDD"
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public bool IsDate(string S)
        {
            string T;
            int Num;

            S = S.Trim();
            Num = StringToInt32(S);
            T = Num.ToString();

            if ((T.Length != 8) || (S.Length != 8)) return (false);

            Num = Convert.ToInt32(S.Substring(0, 4));
            if (Num < 0 || Num > 3000) return (false);

            Num = Convert.ToInt32(S.Substring(4, 2));
            if (Num < 0 || Num > 12) return (false);

            Num = Convert.ToInt32(S.Substring(6, 2));
            switch (S.Substring(4, 2))
            {
                case "02":
                    if (Num < 0 || Num > 29) return (false);
                    if (Num == 29 && DateTime.IsLeapYear(Convert.ToInt32(S.Substring(0, 4))) == false) return (false);
                    break;
                case "04":
                case "06":
                case "09":
                case "11":
                    if (Num < 0 || Num > 30) return (false);
                    break;
                default:
                    if (Num < 0 || Num > 31) return (false);
                    break;
            }
            return (true);
        }

        public bool IsDateTime(string strDateTime)
        {
            bool boolreturn;
            try
            {
                DateTime dt = DateTime.Parse(strDateTime);
                boolreturn = true;
            }
            catch
            {
                boolreturn = false;
            }
            return boolreturn;
        }

        /// <summary>
        /// 返回时间戳
        /// </summary>
        /// <returns></returns>
        public string TimeStamp()
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dtNow = DateTime.Parse(DateTime.Now.ToString());
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        /// <summary>
        /// 返回两时间间时间秒数
        /// </summary>
        /// <returns></returns>
        public string TimeStamp(DateTime startTime, DateTime endTime)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(startTime);
            DateTime dtNow = endTime;
            TimeSpan toNow = dtNow.Subtract(dtStart);
            string timeStamp = toNow.Ticks.ToString();
            timeStamp = timeStamp.Substring(0, timeStamp.Length - 7);
            return timeStamp;
        }

        #endregion

        /// <summary>
        /// 将html代码显示在网页上
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string OutputText(string inputString)
        {
            string retVal = System.Web.HttpUtility.HtmlDecode(inputString);
            retVal = retVal.Replace("<br>", "");
            retVal = retVal.Replace("&amp", "&;");
            retVal = retVal.Replace("&quot;", "\"");
            retVal = retVal.Replace("&lt;", "<");
            retVal = retVal.Replace("&gt;", ">");
            retVal = retVal.Replace("&nbsp;", " ");
            retVal = retVal.Replace("&nbsp;&nbsp;", "  ");
            return retVal;
        }

    }

}
