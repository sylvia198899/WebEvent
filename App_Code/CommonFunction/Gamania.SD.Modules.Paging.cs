using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Gamania.SD.Library;
using System.Web.UI;
 
namespace Gamania.SD.Model
{
    #region
    /// <summary>
    /// 標準四按鈕分頁
    /// 2010.02.01
    /// </summary>
    public class ButtonPaging
    {
        public Literal PageTotalControl;
        public Literal CurrentPageControl;

        public Button NormalPreButton;
        public Button NormalNextButton;
        public Button NormalFirstButton;
        public Button NormalLastButton;

        public LinkButton LinkPreButton;
        public LinkButton LinkNextButton;
        public LinkButton LinkFirstButton;
        public LinkButton LinkLastButton;

        public ImageButton ImagePreButton;
        public ImageButton ImageNextButton;
        public ImageButton ImageFirstButton;
        public ImageButton ImageLastButton;

        public PagedDataSource pds;
        public DataList DataListControl;
        public DataTable BindTable
        {
            get { return (DataTable)HttpContext.Current.Session[this.DataListControl.ID + "Table"]; }
            set { HttpContext.Current.Session[this.DataListControl.ID + "Table"] = (DataTable)value; }
        }

        public bool Visible;
        public int InitType = 0;
        public int ButtonType = 0;
        public int PageRows = 0;

        public int CurrentPageIndex = 0;

        public int PageIndex
        {
            get { return Code.GetInt(HttpContext.Current.Session[this.DataListControl.ID], 1); }
            set { HttpContext.Current.Session[this.DataListControl.ID] = value; }
        }

        /// <summary>
        /// 自定分頁按鈕動作
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="pageRows"></param>
        /// <param name="listControl"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageTotal"></param>
        public ButtonPaging(bool visible, int pageRows, DataList listControl, Literal currentPage, Literal pageTotal)
        {
            this.InitPaging(0, 0, visible, pageRows, listControl, currentPage, pageTotal);
        }

        /// <summary>
        /// 內建分頁按鈕動作
        /// </summary>
        /// <param name="visible"></param>
        /// <param name="pageRows"></param>
        /// <param name="listControl"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageTotal"></param>
        /// <param name="preButton"></param>
        /// <param name="nextButton"></param>
        /// <param name="firstButton"></param>
        /// <param name="lastButton"></param>
        public ButtonPaging(bool visible, int pageRows, DataList listControl, Literal currentPage, Literal pageTotal, LinkButton firstButton, LinkButton preButton, LinkButton nextButton, LinkButton lastButton)
        {         
            this.InitPaging(1, 2, visible, pageRows, listControl, currentPage, pageTotal);
            this.InitLinkButtonClick(firstButton, preButton, nextButton, lastButton);
        }

        /// <summary>
        /// 初始化分頁建構基本資訊
        /// </summary>
        /// <param name="initType"></param>
        /// <param name="buttonType"></param>
        /// <param name="visible"></param>
        /// <param name="pageRows"></param>
        /// <param name="bindControl"></param>
        /// <param name="currentPageControl"></param>
        /// <param name="pageTotalControl"></param>
        public void InitPaging(int initType, int buttonType, bool visible, int pageRows, DataList bindControl, Literal currentPageControl, Literal pageTotalControl)
        {
            this.InitType = initType;
            this.ButtonType = buttonType;
            this.Visible = visible;
            this.PageRows = pageRows;

            this.DataListControl = bindControl;
            this.CurrentPageControl = currentPageControl;
            this.PageTotalControl = pageTotalControl;
        }

        /// <summary>
        /// 擊結資料至控制項,內定分頁按鈕動作時使用
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="pageCate"></param>
        public void BindToListControl(DataTable dataTable, int pageCate)
        {
            if (pageCate == 1) this.PageIndex = 1;
            if (pageCate == 2) this.PageIndex = this.PageIndex - 1;
            if (pageCate == 3) this.PageIndex = this.PageIndex + 1;
            if (pageCate == 4) this.PageIndex = (this.BindTable.Rows.Count % this.PageRows == 0) ? (this.BindTable.Rows.Count / this.PageRows) : (this.BindTable.Rows.Count / this.PageRows) + 1;

            this.BindToListControl(dataTable);
        }

        /// <summary>
        /// 擊結資料至控制項,自定分頁按鈕動作時使用
        /// </summary>
        /// <param name="dataTable"></param>
        public void BindToListControl(DataTable dataTable)
        {           
            this.BindTable = dataTable;

            this.pds = new PagedDataSource();
            this.pds.DataSource = this.BindTable.DefaultView;
            this.pds.AllowPaging = true;
            this.pds.PageSize = this.PageRows;

            if (this.PageIndex == pds.PageCount)    this.PageIndex = pds.PageCount;            
            if (this.PageIndex <= 0)    this.PageIndex = 1;

            pds.CurrentPageIndex = this.PageIndex - 1;
   
            this.CurrentPageControl.Text = this.PageIndex.ToString();
            this.PageTotalControl.Text = this.pds.PageCount.ToString();
            
            this.DataListControl.DataSource = this.pds;
            this.DataListControl.DataBind();

            this.SetButtonState();
        }

        /// <summary>
        /// 控制按鈕狀態(隱藏/關閉),內建分頁按鈕動作時使用
        /// </summary>
        public void SetButtonState()
        {
            if (this.ButtonType == 2 && this.InitType == 1)
            {
                this.SetButtonState(this.LinkFirstButton, this.LinkPreButton, this.LinkNextButton, this.LinkLastButton);
            }
        }

        /// <summary>
        /// 控制按鈕狀態(隱藏/關閉),自定分頁按鈕動作時使用
        /// </summary>
        /// <param name="preButton"></param>
        /// <param name="nextButton"></param>
        /// <param name="firstButton"></param>
        /// <param name="lastButton"></param>
        public void SetButtonState(LinkButton firstButton, LinkButton preButton, LinkButton nextButton, LinkButton lastButton)
        {
            if (pds.PageCount < this.PageIndex && this.InitType == 1)
            {
                preButton.Visible = false;
                nextButton.Visible = false;
                firstButton.Visible = false;
                lastButton.Visible = false;
            }

            if (this.Visible)
            {
                preButton.Visible = (this.PageIndex == 1) ? false : true;
                nextButton.Visible = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Visible = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Visible = false;
                if (this.PageIndex == 1) firstButton.Visible = false;
                if (this.PageIndex == pds.PageCount ) lastButton.Visible = false;
            }
            else
            {
                preButton.Enabled = (this.PageIndex == 1) ? false : true;
                nextButton.Enabled = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Enabled = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Enabled = false;
                if (this.PageIndex == 1) firstButton.Enabled = false;
                if (this.PageIndex == pds.PageCount ) lastButton.Enabled = false;
            }
        }

        public void SetButtonState(Button firstButton, Button preButton, Button nextButton, Button lastButton)
        {
            if (pds.PageCount < this.PageIndex && this.InitType == 1)
            {
                preButton.Visible = false;
                nextButton.Visible = false;
                firstButton.Visible = false;
                lastButton.Visible = false;
            }

            if (this.Visible)
            {
                preButton.Visible = (this.PageIndex == 1) ? false : true;
                nextButton.Visible = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Visible = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Visible = false;
                if (this.PageIndex == 1) firstButton.Visible = false;
                if (this.PageIndex == pds.PageSize - 1) lastButton.Visible = false;
            }
            else
            {
                preButton.Enabled = (this.PageIndex == 1) ? false : true;
                nextButton.Enabled = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Enabled = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Enabled = false;
                if (this.PageIndex == 1) firstButton.Enabled = false;
                if (this.PageIndex == pds.PageSize - 1) lastButton.Enabled = false;
            }
        }

        public void SetButtonState(ImageButton firstButton, ImageButton preButton, ImageButton nextButton, ImageButton lastButton)
        {
            if (pds.PageCount < this.PageIndex && this.InitType == 1)
            {
                preButton.Visible = false;
                nextButton.Visible = false;
                firstButton.Visible = false;
                lastButton.Visible = false;
            }

            if (this.Visible)
            {
                preButton.Visible = (this.PageIndex == 1) ? false : true;
                nextButton.Visible = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Visible = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Visible = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Visible = false;
                if (this.PageIndex == 1) firstButton.Visible = false;
                if (this.PageIndex == pds.PageSize - 1) lastButton.Visible = false;
            }
            else
            {
                preButton.Enabled = (this.PageIndex == 1) ? false : true;
                nextButton.Enabled = (this.PageIndex >= pds.PageCount) ? false : true;

                firstButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;
                lastButton.Enabled = (this.BindTable.Rows.Count == 0) ? false : true;

                if (this.BindTable.Rows.Count < this.PageRows) firstButton.Enabled = false;
                if (this.BindTable.Rows.Count < this.PageRows) lastButton.Enabled = false;
                if (this.PageIndex == 1) firstButton.Enabled = false;
                if (this.PageIndex == pds.PageSize - 1) lastButton.Enabled = false;
            }
        }

        protected void InitLinkButtonClick(LinkButton firstButton, LinkButton preButton, LinkButton nextButton, LinkButton lastButton)
        {
            this.LinkNextButton = nextButton;
            this.LinkPreButton = preButton;
            this.LinkFirstButton = firstButton;
            this.LinkLastButton = lastButton;

            this.LinkPreButton.Click += new EventHandler(this.PagingButtonPre_Click);
            this.LinkNextButton.Click += new EventHandler(this.PagingButtonNext_Click);
            this.LinkLastButton.Click += new EventHandler(this.PagingButtonLast_Click);
            this.LinkFirstButton.Click += new EventHandler(this.PagingButtonFirst_Click);
        }

        protected void InitLinkButtonClick(Button firstButton, Button preButton, Button nextButton, Button lastButton)
        {
            this.NormalNextButton = nextButton;
            this.NormalPreButton = preButton;
            this.NormalFirstButton = firstButton;
            this.NormalLastButton = lastButton;

            this.NormalPreButton.Click += new EventHandler(this.PagingButtonPre_Click);
            this.NormalNextButton.Click += new EventHandler(this.PagingButtonNext_Click);
            this.NormalLastButton.Click += new EventHandler(this.PagingButtonLast_Click);
            this.NormalFirstButton.Click += new EventHandler(this.PagingButtonFirst_Click);
        }

        protected void InitLinkButtonClick(ImageButton firstButton, ImageButton preButton, ImageButton nextButton, ImageButton lastButton)
        {
            this.ImageNextButton = nextButton;
            this.ImagePreButton = preButton;
            this.ImageFirstButton = firstButton;
            this.ImageLastButton = lastButton;

            this.ImagePreButton.Click += new ImageClickEventHandler(this.PagingImageButtonPre_Click);
            this.ImageNextButton.Click += new ImageClickEventHandler(this.PagingImageButtonNext_Click);
            this.ImageLastButton.Click += new ImageClickEventHandler(this.PagingImageButtonLast_Click);
            this.ImageFirstButton.Click += new ImageClickEventHandler(this.PagingImageButtonFirst_Click);
        }

        public void PagingButtonFirst_Click(object sender, EventArgs e)
        {
            this.BindToListControl(this.BindTable, 1);
            this.SetButtonState();
        }

        public void PagingButtonPre_Click(object sender, EventArgs e)
        {
            this.BindToListControl(this.BindTable, 2);
            this.SetButtonState();
        }

        public void PagingButtonNext_Click(object sender, EventArgs e)
        {
            this.BindToListControl(this.BindTable, 3);
            this.SetButtonState();
        }

        public void PagingButtonLast_Click(object sender, EventArgs e)
        {
            this.BindToListControl(this.BindTable, 4);
            this.SetButtonState();
        }

        public void PagingImageButtonFirst_Click(object sender, ImageClickEventArgs e)
        {
            this.BindToListControl(this.BindTable, 1);
            this.SetButtonState();
        }

        public void PagingImageButtonPre_Click(object sender, ImageClickEventArgs e)
        {
            this.BindToListControl(this.BindTable, 2);
            this.SetButtonState();
        }

        public void PagingImageButtonNext_Click(object sender, ImageClickEventArgs e)
        {
            this.BindToListControl(this.BindTable, 3);
            this.SetButtonState();
        }

        public void PagingImageButtonLast_Click(object sender, ImageClickEventArgs e)
        {
            this.BindToListControl(this.BindTable, 4);
            this.SetButtonState();
        }
    }
    #endregion

    #region
    /// <summary>
    /// Paging 的摘要描述
    /// 2009.10.12
    /// </summary>
    public class Paging
    {
        protected static PagedDataSource pds = new PagedDataSource();
        protected static Dictionary<string, int> dsi;
        public static void Init(int pageRowTotal)
        {
            dsi = HttpContext.Current.Session["__pagedDataSource"] as Dictionary<string, int>;
            if (dsi == null)
            {
                dsi = new Dictionary<string, int>();
                dsi.Add("currentPageIndex", 1);
                dsi.Add("pageRowTotal", pageRowTotal);
                dsi.Add("totalTotal", 0);
            }
            HttpContext.Current.Session["__pagedDataSource"] = dsi;
        }
        protected static int GetSessionByName(string keyName, int defaultValue)
        {
            dsi = HttpContext.Current.Session["__pagedDataSource"] as Dictionary<string, int>;
            return (dsi == null) ? defaultValue : dsi[keyName];
        }
        protected static void SetSessionByName(string keyName, int keyValue)
        {
            dsi = HttpContext.Current.Session["__pagedDataSource"] as Dictionary<string, int>;
            if (dsi != null) dsi[keyName] = keyValue;
            HttpContext.Current.Session["__pagedDataSource"] = dsi;
        }
        public static int CurrentPageIndex
        {
            get { return GetSessionByName("currentPageIndex", 1); }
            set { SetSessionByName("currentPageIndex", value); }
        }
        public static int PageRowTotal
        {
            get { return GetSessionByName("pageRowTotal", 0); }
            set { SetSessionByName("pageRowTotal", value); }
        }
        public static int TotalPage
        {
            get { return GetSessionByName("totalTotal", 0); }
            set { SetSessionByName("totalTotal", value); }
        }
        /// <summary>
        /// 第一頁的文字
        /// </summary>
        private static string firstPageText = "<<";
        /// <summary>
        /// 第一頁的文字
        /// </summary>
        private static string FirstPageText
        {
            set { firstPageText = value; }
        }
        /// <summary>
        /// 最後一頁的文字
        /// </summary>
        private static string lastPageText = ">>";
        /// <summary>
        /// 最後一頁的文字
        /// </summary>
        private static string LastPageText
        {
            set { lastPageText = value; }
        }

        public static int GetPagingData(Repeater bindControl, DataTable dataTable, int currentPageIndex)
        {
            CurrentPageIndex = currentPageIndex;
            pds.DataSource = dataTable.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = PageRowTotal;
            pds.CurrentPageIndex = CurrentPageIndex - 1;
            TotalPage = pds.PageCount;
            bindControl.DataSource = pds;
            bindControl.DataBind();
            return (TotalPage < currentPageIndex) ? -1 : 0;
        }

        public static int GetPagingData(DataList bindControl, DataTable dataTable, int currentPageIndex)
        {
            CurrentPageIndex = currentPageIndex;
            pds.DataSource = dataTable.DefaultView;
            pds.AllowPaging = true;
            pds.PageSize = PageRowTotal;
            pds.CurrentPageIndex = CurrentPageIndex - 1;
            TotalPage = pds.PageCount;
            bindControl.DataSource = pds;
            bindControl.DataBind();
            return (TotalPage < currentPageIndex) ? -1 : 0;
        }

        public static int GetPagingData(Repeater bindControl, DataTable dataTable, int currentPageIndex, Literal currentPageControl, Literal totalPageControl)
        {
            GetPagingData(bindControl, dataTable, currentPageIndex);

            currentPageControl.Text = CurrentPageIndex.ToString();
            totalPageControl.Text = TotalPage.ToString();
            return (TotalPage < currentPageIndex) ? -1 : 0;
        }

        public static int GetPagingData(DataList bindControl, DataTable dataTable, int currentPageIndex, Literal currentPageControl, Literal totalPageControl)
        {
            GetPagingData(bindControl, dataTable, currentPageIndex);

            currentPageControl.Text = CurrentPageIndex.ToString();
            totalPageControl.Text = TotalPage.ToString();
            return (TotalPage < currentPageIndex) ? -1 : 0;
        }

        /// <summary>
        /// 設定分頁按鈕
        /// </summary>
        /// <param name="preButton"></param>
        /// <param name="nextButton"></param>
        public static void SetButtonLink(WebControl preButton, WebControl nextButton)
        {
            if (preButton != null) preButton.Visible = (CurrentPageIndex == 1) ? false : true;
            if (nextButton != null) nextButton.Visible = (CurrentPageIndex >= TotalPage) ? false : true;
        }
        /// <summary>
        /// 初始設置下拉分頁選項
        /// </summary>
        /// <param name="dropdownList"></param>
        public static void InitListLink(DropDownList dropdownList)
        {
            if (dropdownList.Items.Count == 0)
            {
                for (int i = 0; i < TotalPage; i++)
                {
                    dropdownList.Items.Add(new ListItem((i + 1).ToString(), (i + 1).ToString()));
                }
            }
        }
        /// <summary>
        /// 初始設定分頁連結
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageEvent"></param>
        /// <param name="totalPage"></param>
        public static void InitHyperLink(EventHandler pageEvent, int totalPage, PlaceHolder placeHolder)
        {
            PlaceHolder ph = new PlaceHolder();
            ph.ID = "PagingPlaceHolder";

            int sideTotal = 0;
            int pageLinkTotal = 0;

            sideTotal = 5;

            if (totalPage > 10)
                pageLinkTotal = ((sideTotal * 2 + 1) > totalPage) ? totalPage + 1 : (sideTotal * 2 + 1);
            else
                pageLinkTotal = totalPage;

            // 第一頁的按鈕
            LinkButton lbFirst = new LinkButton();
            lbFirst.ID = "_LinkButtonFirst";
            lbFirst.Text = "<<";
            lbFirst.ToolTip = "1";
            lbFirst.CommandName = "*";
            lbFirst.CommandArgument = "1";
            lbFirst.Click += new EventHandler(pageEvent);
            lbFirst.Style.Add("text-decoration", "none");

            Literal lFirst = new Literal();
            lFirst.Text = "&nbsp;";

            ph.Controls.Add(lFirst);
            ph.Controls.Add(lbFirst); 

            // 中間頁的按鈕
            for (int i = 1; i <= pageLinkTotal; i++)
            {
                LinkButton lb = new LinkButton();
                lb.ID = "_LinkButton" + i.ToString();
                lb.Text = string.Format("{0}", i.ToString());
                lb.ToolTip = string.Format("{0}", i.ToString());
                lb.CommandArgument = string.Format("{0}", i.ToString());
                lb.Click += new EventHandler(pageEvent);

                if (i == 1) lb.Style.Add("text-decoration", "none");

                Literal l = new Literal();
                l.Text = "&nbsp;";

                ph.Controls.Add(lb);
                if (i != (sideTotal * 2 + 1)) ph.Controls.Add(l);
            }

            // 最後一頁的按鈕
            LinkButton lbLast = new LinkButton();
            lbLast.ID = "_LinkButtonLast";
            lbLast.Text = ">>";
            lbLast.ToolTip = totalPage.ToString();
            lbLast.CommandName = "*";
            lbLast.CommandArgument = totalPage.ToString();
            lbLast.Click += new EventHandler(pageEvent);
            lbLast.Style.Add("text-decoration", "none");

            Literal lLast = new Literal();
            lLast.Text = "&nbsp;";

            ph.Controls.Add(lbLast);
 
            placeHolder.Controls.Add(ph);
        }
        /// <summary>
        /// 設置分頁連結
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="selectPage"></param>
        /// <param name="totalPage"></param>
        public static void SetHyperLink(PlaceHolder placeHolder, LinkButton pagingButton)
        {
            // 取得目前選擇的頁次與值
            int nowPageValue = (pagingButton.CommandName == "*" ) ? int.Parse(pagingButton.ToolTip) : int.Parse(pagingButton.Text);
            int nowPageIndex = int.Parse(pagingButton.ToolTip);

            // 取得之前選擇的頁次與值
            int lastPageValue = 0;
            int lastPageIndex = 0;

            for (int i = 1; i <= ((pds.PageCount > 10) ? 11 : pds.PageCount); i++)
            {
                LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + i.ToString());
                if (lb != null)
                {
                    // 初始化
                    lb.Visible = true;
                    if (lb.Style.Count > 0 && lb.Style["text-decoration"].ToString() == "none")
                    {
                        lastPageValue = (pagingButton.CommandName == "*" ) ? int.Parse(pagingButton.ToolTip) : int.Parse(lb.Text);
                        lastPageIndex = int.Parse(lb.ToolTip);
                    }
                }
            }

            // 新增位移的差距值            
            int offset = (nowPageValue - lastPageValue);

            // 新的起始值是否小於0
            bool isLow = false;


            if (pagingButton.CommandName == "*" && pagingButton.ID == "_LinkButtonFirst")
            {
                for (int i = 1; i <= ((TotalPage >= 11) ? 11 : TotalPage); i++)
                {
                    LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + i.ToString());
                    lb.Text = i.ToString();
                    lb.CommandArgument = lb.Text;

                    lb.Style.Add("text-decoration", (i==1) ? "none" : "underline;");
                    if (int.Parse(lb.Text) > TotalPage) lb.Visible = false;
                }
            }
            else if (pagingButton.CommandName == "*" && pagingButton.ID == "_LinkButtonLast")
            {
                for (int i = 1; i <= ((TotalPage >= 11) ? 11 : TotalPage); i++)
                {
                    LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + i.ToString());
                    lb.Text = (TotalPage - (( TotalPage < 6 ) ? TotalPage : 6 ) + i).ToString();
                    lb.CommandArgument = lb.Text;

                    lb.Style.Add("text-decoration", (int.Parse(lb.Text) == TotalPage) ? "none" : "underline;");
                    if (int.Parse(lb.Text) > TotalPage) lb.Visible = false;
                }
            }
            else
            {

                // 如果是增值
                if (offset > 0 && pagingButton.CommandName != "*")
                {
                    if (lastPageValue == 1)
                    {
                        // 首次切換頁次
                        for (int i = 1; i <= ((TotalPage >= 11) ? 11 : TotalPage); i++)
                        {
                            LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + i.ToString());

                            if (lastPageIndex < 6 && nowPageValue > 6)
                                lb.Text = (int.Parse(lb.Text) + (nowPageValue - 6)).ToString();
                            else
                                lb.Text = i.ToString();

                            lb.CommandArgument = lb.Text;
                            lb.Style.Add("text-decoration", (int.Parse(lb.Text) == nowPageValue) ? "none" : "underline;");

                            if (int.Parse(lb.Text) > TotalPage) lb.Visible = false;
                        }
                    }
                    else
                    {
                        // 非首次切換頁次
                        for (int i = 1; i <= ((TotalPage >= 11) ? 11 : TotalPage); i++)
                        {
                            LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + i.ToString());

                            if (lastPageIndex == 6)
                                lb.Text = (int.Parse(lb.Text) + offset).ToString();
                            else if (lastPageIndex < 6 && nowPageValue > 6)
                                lb.Text = (int.Parse(lb.Text) + nowPageValue - 6).ToString();
                            else if (lastPageIndex < 6 && nowPageValue <= 6)
                                lb.Text = i.ToString();
                            else
                                lb.Text = (int.Parse(lb.Text) + offset - lastPageIndex).ToString();

                            lb.CommandArgument = lb.Text;
                            lb.Style.Add("text-decoration", (int.Parse(lb.Text) == nowPageValue) ? "none" : "underline;");

                            if (int.Parse(lb.Text) > TotalPage) lb.Visible = false;
                        }
                    }
                }
                else
                {
                    // 檢查新的起始值是否會小於0
                    for (int j = 1; j <= ((TotalPage >= 11) ? 11 : TotalPage); j++)
                    {
                        if (!isLow)
                        {
                            LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + j.ToString());
                            if ((int.Parse(lb.Text) + offset) <= 0) isLow = true;
                        }
                    }

                    if (isLow)
                    {
                        // 如果起始值小於0,則從1開始
                        for (int k = 1; k <= ((TotalPage >= 11) ? 11 : TotalPage); k++)
                        {
                            LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + k.ToString());
                            lb.Text = k.ToString();
                            lb.CommandArgument = lb.Text;
                            lb.Style.Add("text-decoration", (int.Parse(lb.Text) == nowPageValue) ? "none" : "underline;");
                        }
                    }
                    else
                    {
                        // 如果起始值大於0,則減offset值
                        for (int l = 1; l <= ((TotalPage >= 11) ? 11 : TotalPage); l++)
                        {
                            LinkButton lb = (LinkButton)placeHolder.FindControl("_LinkButton" + l.ToString());
                            lb.Text = (int.Parse(lb.Text) + offset).ToString();
                            lb.CommandArgument = lb.Text;
                            lb.Visible = (int.Parse(lb.Text) <= TotalPage) ? true : false;
                            lb.Style.Add("text-decoration", (int.Parse(lb.Text) == nowPageValue) ? "none" : "underline;");
                        }
                    }
                }
            }
        }
        public static void ClearPagingSession()
        {
            HttpContext.Current.Session.Remove("currentPageIndex");
            HttpContext.Current.Session.Remove("pageRowTotal");
            HttpContext.Current.Session.Remove("totalTotal");
        }
    }
    #endregion
}
