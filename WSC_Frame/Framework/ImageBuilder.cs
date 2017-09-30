/************************************************************************************************
**********Created by Hedda on Feb-2006                                                  *********
**********Description:                                                                  *********
*************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Web.UI;
using System.Web;
using System.IO;

namespace WSC.Framework
{
    /// <summary>
    /// GDI+图片生成类
    /// Create by Hedda
    /// </summary>
    public sealed class ImageBuilder
    {
        public readonly string BlankImage = "HeaderImages/blank.gif";
        private readonly string m_titleImageText = GlobalDefinition.System_Name().Trim();
        private readonly string m_titleImageTempletePath = "HeaderImages/Header_Title_Template.png";
        private readonly string m_titleImagePath = "HeaderImages/Header_Left_" + GlobalDefinition.System_Name().Trim() + ".bmp";
        private readonly string m_titleFontFamily = "Arial Black";
        private readonly float m_titleFontSize = 40;
        private readonly float m_titleX = 12;
        private readonly float m_titleY = 2;
        private readonly string m_versionImageText = "Version " + GlobalDefinition.CurrentVersion;
        private readonly string m_versionImageTempletePath = "HeaderImages/Header_Version_Template.png";
        private readonly string m_versionImagePath = "HeaderImages/Header_Version_" + GlobalDefinition.CurrentVersion.Trim() + ".bmp";
        private readonly string m_versionFontFamily = "04b_08";
        private readonly float m_versionFontSize = 8;
        private readonly float m_versionX = 1;
        private readonly float m_versionY = 6;
        private string GetPhysicalPath(string VirtualPath)
        {
            Page currentPage = HttpContext.Current.Handler as Page;
            return currentPage.MapPath(VirtualPath).ToString();
        }
        private bool CheckImageExist(string VirtualPath)
        {
            return System.IO.File.Exists(this.GetPhysicalPath(VirtualPath));
        }
        /// <summary>
        /// Drawing
        /// </summary>
        /// <param name="img"></param>
        /// <param name="text"></param>
        /// <param name="fontFamily"></param>
        /// <param name="fontSize"></param>
        /// <param name="fontColor"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool DrawTextOnImage(Image img, string text, string fontFamily, float fontSize, Color fontColor, float x, float y)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Font f = new Font(fontFamily, fontSize))
                {
                    using (SolidBrush sb = new SolidBrush(fontColor))
                    {
                        try
                        {
                            g.DrawString(text, f, sb, x, y);

                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Title图片
        /// </summary>
        /// <returns></returns>
        public string GetSystemTitleImage()
        {
            if (CheckImageExist(m_titleImagePath))
            {
                return m_titleImagePath;
            }
            if (!CheckImageExist(m_titleImageTempletePath))
            {
                return BlankImage;
            }
            using (Image img = Image.FromFile(GetPhysicalPath(m_titleImageTempletePath)))
            {

                if (DrawTextOnImage(img, m_titleImageText, m_titleFontFamily, m_titleFontSize, Color.White, m_titleX, m_titleY))
                {
                    try
                    {

                        img.Save(GetPhysicalPath(m_titleImagePath), System.Drawing.Imaging.ImageFormat.Bmp);

                        return m_titleImagePath;
                    }
                    catch
                    {
                        return BlankImage;
                    }
                }
                else
                {
                    return BlankImage;
                }
            }

        }

        /// <summary>
        /// 版本图片
        /// </summary>
        /// <returns></returns>
        public string GetSystemVersionImage()
        {
            try
            {
                string strFilePath = System.Environment.GetEnvironmentVariable("windir") + "\\Fonts\\";

                if (!Directory.Exists(strFilePath))
                    Directory.CreateDirectory(strFilePath);

                strFilePath += m_versionFontFamily + ".ttf";

                //若不存在字体则复制（需要重启IIS生效）
                if (!System.IO.File.Exists(strFilePath))
                {
                    Byte[] bytFont = Res.MainRes._04b_08;

                    using (FileStream fs = new FileStream(strFilePath, FileMode.CreateNew))
                    {
                        BinaryWriter wr = new BinaryWriter(fs);
                        wr.Write(bytFont);

                        wr.Close();
                        fs.Close();
                    }
                }
            }
            catch { }


            if (CheckImageExist(m_versionImagePath))
            {
                return m_versionImagePath;
            }


            if (!CheckImageExist(m_versionImageTempletePath))
            {
                return BlankImage;
            }

            using (Image img = Image.FromFile(GetPhysicalPath(m_versionImageTempletePath)))
            {

                if (DrawTextOnImage(img, m_versionImageText, m_versionFontFamily, m_versionFontSize, Color.White, m_versionX, m_versionY))
                {
                    try
                    {

                        img.Save(GetPhysicalPath(m_versionImagePath), System.Drawing.Imaging.ImageFormat.Bmp);

                        return m_versionImagePath;
                    }
                    catch
                    {
                        return BlankImage;
                    }
                }
                else
                {
                    return BlankImage;
                }
            }


        }
    }
}
