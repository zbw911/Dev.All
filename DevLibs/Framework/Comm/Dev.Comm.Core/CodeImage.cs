// ***********************************************************************************
//  Created by zbw911 
//  �����ڣ�2013��06��07�� 14:25
//  
//  �޸��ڣ�2013��09��17�� 11:33
//  �ļ�����Dev.Libs/Dev.Comm.Core/CodeImage.cs
//  
//  ����и��õĽ����������ʼ��� zbw911#gmail.com
// ***********************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using Dev.Comm.Utils;

namespace Dev.Comm
{
    /*
    /// <summary>
    /// �����ˣ��
    /// 2008��12��22�� 16:30
    /// </summary>
    public class CodeImage
    {
        #region ��ø����������

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <returns></returns>
        public static string GetCode(int CodeNumber)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            var random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < CodeNumber; i++)
            {
                number = random.Next();

                code = (char)('0' + (char)(number % 10));

                checkCode += code.ToString();
            }

            return checkCode;
        }

        /// <summary>
        /// �������� ���֣���ĸ
        /// </summary>
        /// <param name="CodeNumber"></param>
        /// <returns></returns>
        public static string GetCodeNumberLetter(int CodeNumber)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            var random = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < CodeNumber; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            return checkCode;
        }

        /// <summary>
        /// ��������ַ���
        /// </summary>
        /// <param name="codeLen">�ַ�������</param>
        /// <param name="zhCharsCount">�����ַ���</param>
        /// <returns></returns>
        public static string CreateVerifyCode(int codeLen, int zhCharsCount)
        {
            var rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            string ChineseChars =
                "��һ���ڲ����к������д�Ϊ�ϸ�������Ҫ��ʱ���������������ڳ��ͷֶԳɻ�������궯ͬ��Ҳ���¹���˵�����������ඨ��ѧ������þ�ʮ��֮���ŵȲ��ȼҵ�������ˮ�����Զ�����С����ʵ�����������ƻ���ʹ���ҵ��ȥ���Ժ�Ӧ�����ϻ�������ЩȻǰ������������������ƽ����ȫ�������ظ��������������ķ�������ԭ��ô���Ȼ�������������˱���ֻû������⽨�¹���ϵ������������������ͨ����ֱ�⵳��չ�������Ա��λ�볣���ܴ�Ʒʽ���輰���ؼ�������ͷ���ʱ���·����ͼɽͳ��֪�Ͻ�����Ʊ����ֽ��ڸ�����ũָ������ǿ�ž�����������ս�Ȼ�����ȡ�ݴ����ϸ�ɫ���ż����α���ٹ������ߺ��ڶ�����ѹ־���������ý���˼����������ʲ������Ȩ��֤���强���ٲ�ת�������д�׽��ٻ������������������ÿĿ�����߻�ʾ��������������뻪��ȷ�ſ�������ڻ�������Ԫ�����´�����Ⱥ��ʯ������н������ɽ��Ҿ���Խ֯װӰ��ͳ������鲼���ݶ�����̷����������ѽ���ǧ��ί�ؼ��������ʡ��ϰ��Լ֧��ʷ���ͱ����������п˺γ���������̫׼��ֵ������ά��ѡ��д���ë�׿�Ч˹Ժ�齭�����������������ɲ�Ƭʼȴר״������ʶ����Բ����ס�����ؾ��ղκ�ϸ������������";
            string EnglishOrNumChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var chs = new char[codeLen];
            int index;
            for (int i = 0; i < zhCharsCount; i++)
            {
                index = rnd.Next(0, codeLen);
                if (chs[index] == '\0')
                    chs[index] = ChineseChars[rnd.Next(0, ChineseChars.Length)];
                else
                    --i;
            }
            for (int i = 0; i < codeLen; i++)
            {
                if (chs[i] == '\0')
                    chs[i] = EnglishOrNumChars[rnd.Next(0, EnglishOrNumChars.Length)];
            }

            return new string(chs, 0, chs.Length);
        }

        #endregion

        #region ���ɸ�����ͼƬ

        public static MemoryStream GetImage(string code)
        {
            return GetImage(code, false);
        }

        /// <summary>
        /// ������֤��ͼƬ
        /// </summary>
        /// <param name="code">�ַ���</param>
        /// <param name="Chaos">�Ƿ����������</param>
        /// <returns></returns>
        public static MemoryStream GetImage(string code, bool Chaos)
        {
            if (code == "")
            {
                return null;
            }
            int Padding = 2; //�߿� Ĭ��4����
            int fSize = 16; //�����С��Ĭ��18����
            int fWidth = fSize + Padding;
            int imageWidth = (code.Length * fWidth) + Padding * 2;
            int imageHeight = fSize + Padding * 3 + Padding * 2;
            var image = new Bitmap(imageWidth, imageHeight);
            Graphics g = Graphics.FromImage(image);

            Color[] Colors =
                {
                    Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown,
                    Color.DarkCyan, Color.Purple
                };
            string[] Fonts = { "Arial", "Batang", "Verdana", "Microsoft Sans Serif", "Shruti" };
            //, "Georgia", "Corbel","Corbel", 
            var rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            try
            {
                //Color border = Color.FromArgb(220, 246, 193);
                g.Clear(Color.White); //������ɫ����ɫ
                //g.Clear(Color.FromArgb(250, 250, 250));//������ɫ����ɫ
                //���������������ɵ����
                //bool Chaos = true;
                if (Chaos)
                {
                    //Pen pen = new Pen(Colors[rnd.Next(Colors.Length - 1)], 0);//�����ɫ�����ɫ����ɫ���̫�򵥡�
                    var pen = new Pen(Color.LightGray, 0); //�����ɫ����ɫ
                    int c = code.Length * 10;
                    for (int i = 0; i < c; i++)
                    {
                        int x = rnd.Next(image.Width);
                        int y = rnd.Next(image.Height);
                        g.DrawRectangle(pen, x, y, 1, 1);
                    }

                    int x1 = rnd.Next(image.Width / 4);
                    int y1 = rnd.Next(image.Height);
                    int x2 = rnd.Next(3 * image.Width / 4, image.Width);
                    int y2 = rnd.Next(image.Height);
                    var pen1 = new Pen(Colors[rnd.Next(Colors.Length - 1)], 1);
                    g.DrawLine(pen1, x1, y1, x2, y2);

                    for (int l = 0; l < 3; l++)
                    {
                        x1 = rnd.Next(image.Width / 4);
                        y1 = rnd.Next(image.Height);
                        x2 = rnd.Next(3 * image.Width / 4, image.Width);
                        y2 = rnd.Next(image.Height);
                        pen1 = new Pen(Color.LightGray, 1);
                        g.DrawLine(pen1, x1, y1, x2, y2);
                    }
                }

                int left = 0, top = 0;
                int n1 = (imageHeight - fSize - Padding * 2);

                Font f;
                Brush b;
                int cindex, findex;

                //����������ɫ����֤���ַ�
                for (int i = 0; i < code.Length; i++)
                {
                    cindex = rnd.Next(Colors.Length - 1);
                    findex = rnd.Next(Fonts.Length - 1);

                    if (code.Substring(i, 1) == "0") //�����0���������� ������ȥ�� o
                    {
                        f = new Font(Fonts[0], fSize, FontStyle.Regular);
                    }
                    else
                    {
                        f = new Font(Fonts[findex], fSize, FontStyle.Bold);
                    }
                    b = new SolidBrush(Colors[cindex]);
                    top = rnd.Next(Convert.ToInt32(n1 * 4 / 5));
                    left = i * fWidth;
                    g.DrawString(code.Substring(i, 1), f, b, left, top);
                }

                //��һ���߿� �߿���ɫΪColor.Gainsboro
                //g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
                g.Dispose();

                //�������Σ�Add By 51aspx.com��
                //image = TwistImage(image, true, 8, 4);
                image = TwistImage(image, true, rnd.Next(0, 3), rnd.Next(0, 6));
                AddBlackBorder(image, 1);
                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Jpeg);
                return ms;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #endregion

        #region ���������˾�Ч��

        /// <summary>
        /// ��������WaveŤ��ͼƬ��Edit By 51aspx.com��
        /// </summary>
        /// <param name="srcBmp">ͼƬ·��</param>
        /// <param name="bXDir">���Ť����ѡ��ΪTrue</param>
        /// <param name="nMultValue">���εķ��ȱ�����Խ��Ť���ĳ̶�Խ�ߣ�һ��Ϊ3</param>
        /// <param name="dPhase">���ε���ʼ��λ��ȡֵ����[0-2*PI)</param>
        /// <returns></returns>
        public static Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            //double PI = 3.1415926535897932384626433832795;
            double PI2 = 6.283185307179586476925286766559;

            var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // ��λͼ�������Ϊ��ɫ
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * j) / dBaseAxisLen : (PI2 * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // ȡ�õ�ǰ�����ɫ
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                        && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion

        #region ��Ӻڱ�

        /// <summary>
        /// ȥ���ڱ�
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="AddPx">Ҫ��ӵĺڱ߿�� ����</param>
        private static void AddBlackBorder(Bitmap Img, int AddPx) //ȥ���ڱ�
        {
            int ImgWidth = Img.Width;
            int ImgHeight = Img.Height;
            Color border = Color.FromArgb(127, 157, 185);
            //Color border = Color.FromArgb(69, 120, 33);
            //Color border = Color.Black;


            for (int w = 0; w < AddPx; w++)
            {
                for (int h = 0; h < ImgHeight; h++)
                {
                    Img.SetPixel(w, h, border);
                }
            }

            for (int w = ImgWidth - AddPx; w < ImgWidth; w++)
            {
                for (int h = 0; h < ImgHeight; h++)
                {
                    Img.SetPixel(w, h, border);
                }
            }

            for (int h = 0; h < AddPx; h++)
            {
                for (int w = 0; w < ImgWidth; w++)
                {
                    Img.SetPixel(w, h, border);
                }
            }

            for (int h = ImgHeight - AddPx; h < ImgHeight; h++)
            {
                for (int w = 0; w < ImgWidth; w++)
                {
                    Img.SetPixel(w, h, border);
                }
            }
        }

        #endregion
    }
    */

    /// <summary>
    ///   ������֤�룬 from another project , added by zbw911
    /// </summary>
    public class ValidateCode
    {
        private double _withPerCode = 12.0;
        private double _heightCode = 22.0;

        public ValidateCode()
        {
        }

        public double WithPerCode
        {
            get { return _withPerCode; }
            set { _withPerCode = value; }
        }

        public double HeightCode
        {
            get { return _heightCode; }
            set { _heightCode = value; }
        }

        /// <summary>
        ///   ������֤��
        /// </summary>
        /// <param name="length"> ָ����֤��ĳ��� </param>
        /// <returns> </returns>
        public string CreateValidateCode(int codeLen)
        {
            return CreateValidateCode(codeLen, 0);
        }


        /// <summary>
        ///   ��������ַ���
        /// </summary>
        /// <param name="codeLen"> �ַ����ܳ��� </param>
        /// <param name="zhCharsCount"> �����ַ����� </param>
        /// <returns> </returns>
        public string CreateValidateCode(int codeLen, int zhCharsCount)
        {
            var rnd = new Random(unchecked((int)DateTime.Now.Ticks));
            string ChineseChars =
                "��һ���ڲ����к������д�Ϊ�ϸ�������Ҫ��ʱ���������������ڳ��ͷֶԳɻ�������궯ͬ��Ҳ���¹���˵�����������ඨ��ѧ������þ�ʮ��֮���ŵȲ��ȼҵ�������ˮ�����Զ�����С����ʵ�����������ƻ���ʹ���ҵ��ȥ���Ժ�Ӧ�����ϻ�������ЩȻǰ������������������ƽ����ȫ�������ظ��������������ķ�������ԭ��ô���Ȼ�������������˱���ֻû������⽨�¹���ϵ������������������ͨ����ֱ�⵳��չ�������Ա��λ�볣���ܴ�Ʒʽ���輰���ؼ�������ͷ���ʱ���·����ͼɽͳ��֪�Ͻ�����Ʊ����ֽ��ڸ�����ũָ������ǿ�ž�����������ս�Ȼ�����ȡ�ݴ����ϸ�ɫ���ż����α���ٹ������ߺ��ڶ�����ѹ־���������ý���˼����������ʲ������Ȩ��֤���强���ٲ�ת�������д�׽��ٻ������������������ÿĿ�����߻�ʾ��������������뻪��ȷ�ſ�������ڻ�������Ԫ�����´�����Ⱥ��ʯ������н������ɽ��Ҿ���Խ֯װӰ��ͳ������鲼���ݶ�����̷����������ѽ���ǧ��ί�ؼ��������ʡ��ϰ��Լ֧��ʷ���ͱ����������п˺γ���������̫׼��ֵ������ά��ѡ��д���ë�׿�Ч˹Ժ�齭�����������������ɲ�Ƭʼȴר״������ʶ����Բ����ס�����ؾ��ղκ�ϸ������������";
            string EnglishOrNumChars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var chs = new char[codeLen];
            int index;
            for (int i = 0; i < zhCharsCount; i++)
            {
                index = rnd.Next(0, codeLen);
                if (chs[index] == '\0')
                    chs[index] = ChineseChars[rnd.Next(0, ChineseChars.Length)];
                else
                    --i;
            }
            for (int i = 0; i < codeLen; i++)
            {
                if (chs[i] == '\0')
                    chs[i] = EnglishOrNumChars[rnd.Next(0, EnglishOrNumChars.Length)];
            }

            return new string(chs, 0, chs.Length);
        }


        /// <summary>
        ///   ������֤���ͼƬ
        /// </summary>
        /// <param name="validateCode"> ��֤�� </param>
        public byte[] CreateValidateGraphic(string validateCode)
        {
            //Ӧ���ǵ����ĵĿ�ȣ�added by zbw911
            var len = StringUtil.GetGBStrLen(validateCode);
            var image = new Bitmap((int)Math.Ceiling(len * _withPerCode), (int)_heightCode);
            Graphics g = Graphics.FromImage(image);
            try
            {
                //�������������
                var random = new Random();
                //���ͼƬ����ɫ
                g.Clear(Color.White);
                //��ͼƬ�ĸ�����
                for (int i = 0; i < 25; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                var font = new Font("Arial", (float)_withPerCode, (FontStyle.Bold | FontStyle.Italic));
                var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
                                                    Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(validateCode, font, brush, 3, 2);
                //��ͼƬ��ǰ�����ŵ�
                for (int i = 0; i < 100; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //��ͼƬ�ı߿���
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);

                TwistImage(image, true, random.Next(0, 3), random.Next(0, 6));
                //����ͼƬ����
                var stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                //���ͼƬ��
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        #region ���������˾�Ч��

        /// <summary>
        ///   ��������WaveŤ��ͼƬ��Edit By 51aspx.com��
        /// </summary>
        /// <param name="srcBmp"> ͼƬ·�� </param>
        /// <param name="bXDir"> ���Ť����ѡ��ΪTrue </param>
        /// <param name="nMultValue"> ���εķ��ȱ�����Խ��Ť���ĳ̶�Խ�ߣ�һ��Ϊ3 </param>
        /// <param name="dPhase"> ���ε���ʼ��λ��ȡֵ����[0-2*PI) </param>
        /// <returns> </returns>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            //double PI = 3.1415926535897932384626433832795;
            double PI2 = 6.283185307179586476925286766559;

            var destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // ��λͼ�������Ϊ��ɫ
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? destBmp.Height : destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * j) / dBaseAxisLen : (PI2 * i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // ȡ�õ�ǰ�����ɫ
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                        && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }

        #endregion

        /// <summary>
        ///   �õ���֤��ͼƬ�ĳ���
        /// </summary>
        /// <param name="validateNumLength"> ��֤��ĳ��� </param>
        /// <returns> </returns>
        public static int GetImageWidth(int validateNumLength)
        {

            return (int)(validateNumLength * 12.0);
        }

        /// <summary>
        ///   �õ���֤��ĸ߶�
        /// </summary>
        /// <returns> </returns>
        public static double GetImageHeight()
        {
            return 22.5;
        }
    }
}