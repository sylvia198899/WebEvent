using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

/// <summary>
/// Graph 的摘要描述
/// </summary>

namespace SDCodeCheck
{
    public class Graph
    {
        private int intSaveHeight;
        private int intSaveWidth;

        /// <summary>
        /// 指定上傳後的圖片大小(有要指定的時候才呼叫，不呼叫則不改變圖片大小)
        /// </summary>
        /// <param name="_intSaveHeight">圖片高度</param>
        /// <param name="_intSaveWidth">圖片寬度</param>
        public void SetImgSize(int _intSaveHeight, int _intSaveWidth)
        {
            intSaveHeight = _intSaveHeight;
            intSaveWidth = _intSaveWidth;
        }

        /// <summary>
        /// 圖片上傳函式
        /// </summary>
        /// <param name="FileUpload">FileUpload WebControl</param>
        /// <param name="filename">檔案上傳路徑,不含副檔名</param>
        /// <param name="intImgType">0:jpg ; 1:靜態gif ; 2:動態gif(不可改變圖大小)</param>
        /// <param name="intImgQualityMode">0:default ; 1:High ; 2:Low (只針對intImgType 0,1 有效)</param>
        /// <returns></returns>
        public bool BitMapSave(FileUpload FileUpload, string filename , int intImgType ,int intImgQualityMode)
        {
            System.IO.Stream FileStream;
            string strFilename = string.Empty;
            System.Drawing.Image im;

            try
            {
                //讀取FileUpload檔案
                FileStream = FileUpload.PostedFile.InputStream;
                im = System.Drawing.Image.FromStream(FileStream);//從指定的資料流建立System.Drawing.Image

                //進行縮圖，如果要縮圖則自行定意要縮圖的寬度及高度
                int imHeight;
                int imWidth;
                if (intSaveHeight == 0 && intSaveWidth == 0)
                {
                    imHeight = im.Height;   //取得原圖的高度，要縮圖直接指定縮圖的高
                    imWidth = im.Width;     //取得原圖的寬度，要縮圖直接指定縮圖的寬
                }
                else
                {
                    imHeight = intSaveHeight;
                    imWidth = intSaveWidth;  
                }

                if (intImgType == 0 || intImgType == 1)
                {
                    System.Drawing.Bitmap b = new System.Drawing.Bitmap(imWidth, imHeight);
                    Graphics g = Graphics.FromImage(b);
                    g.Clear(Color.Black);
                    if (intImgQualityMode == 1)
                    {
                        g.InterpolationMode = InterpolationMode.High;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                    }
                    else if (intImgQualityMode == 2)
                    {
                        g.InterpolationMode = InterpolationMode.Low;
                        g.SmoothingMode = SmoothingMode.HighSpeed;
                    }
                    else
                    {
                       g.InterpolationMode = InterpolationMode.Default;
                       g.SmoothingMode = SmoothingMode.Default;
                    }
                                        
                    g.DrawImage(im, new Rectangle(0, 0, imWidth, imHeight), new Rectangle(0, 0, im.Width, im.Height), GraphicsUnit.Pixel);
                    if (intImgType == 0)
                        b.Save(filename + ".jpg", ImageFormat.Jpeg);
                    else
                        b.Save(filename + ".gif", ImageFormat.Gif);

                }
                else
                {                   
                    im.Save(filename + ".gif", ImageFormat.Gif);                                   
                }

                FileStream.Close();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
