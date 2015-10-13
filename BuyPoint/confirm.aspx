<%@ Page Language="C#" AutoEventWireup="true" CodeFile="confirm.aspx.cs" Inherits="BuyPoint_confirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>君臨天下</title>
<link href="../CSS/style.css" rel="stylesheet" type="text/css" />
<link href="../CSS/intro.css" rel="stylesheet" type="text/css" />
<!--[if lt IE 9]><script src="../JS/IE9.js"></script> <![endif]-->
<script src="../JS/jquery_tools.js"></script>
<script src="../JS/page_header.js"></script>
<script type="text/javascript">
<!--
    function MM_preloadImages() { //v3.0
        var d = document; if (d.images) {
            if (!d.MM_p) d.MM_p = new Array();
            var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
        }
    }
    function MM_swapImgRestore() { //v3.0
        var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
    }

    function MM_findObj(n, d) { //v4.01
        var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
            d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
        }
        if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
        for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
        if (!x && d.getElementById) x = d.getElementById(n); return x;
    }

    function MM_swapImage() { //v3.0
        var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
            if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
    }
    function check() {
        var selected = 0;
        for (var i = 0; i < document.getElementById('rb_Change_Gash').rows.length; i++) {
            var ss = "rb_Change_Gash_" + i;
            if (document.getElementById(ss).checked)
                selected = 1;
        }
        if (document.getElementById('ddl_Server').value == "-1") {
            alert("請選擇伺服器。")
            return false;
        }
        if (document.getElementById('ddl_Character').value == "-1") {
            alert("請選擇角色。")
            return false;
        }
        if (selected == 0) {
            alert("請選擇想要轉換的元寶數。");
            return false;
        }
        return true;
    }
//-->
</script>
</head>
<body>
    <form id="form1" runat="server">
<div class="main">

<!--版頭-->
<div class="header"></div>

<!--LOGO-->
<div class="logo" id="LOGO"></div>

<!--選單-->
<div class="page_menu" id="MENU"></div>

<!--外框開始-->
<!--TOP框-->
<div class="top"></div>
<div class="content">
<table border="0" cellspacing="0" cellpadding="0" class="content_tb">
  <tr>
    <td class="left">

<!--會員專區選單-->    
<div class="menu">
 <img src="../images/main/menu_member.jpg" width="196" height="35" />

<!--帳號相關-->        
 <div class="table2">
  <table border="0" cellspacing="0" cellpadding="0">
  <tr>
    <th>&nbsp;帳號相關&nbsp;</th>
    </tr>
  <tr>
    <td><a href="../Member/account.html">帳號申請</a>&nbsp;</td>
  </tr>
</table>
    </div>

<!--儲值購點相關-->         
    <div class="table2">
  <table border="0" cellspacing="0" cellpadding="0">
  <tr>
    <th>&nbsp;儲值購點相關&nbsp;</th>
    </tr>
  <tr>
    <td style="letter-spacing:0"><a href="http://www.gashplus.com/gashstore/GashStore.aspx" target="_blank">步驟一、購買GASH+點</a>&nbsp;</td>
  </tr>
  <tr>
    <td style="letter-spacing:0"><a href="http://tw.beanfun.com/TW/CheckLogin.aspx?Page=2" target="_blank">步驟二、儲值樂豆點數</a>&nbsp;</td>
  </tr>
  <tr>
    <td style="letter-spacing:0"><a href="agreement.aspx">步驟三、轉換元寶</a>&nbsp;</td>
  </tr>
</table>
    </div>
</div>
<!--//會員專區選單-->    

<!--活動專區-->
<div class="AD">
<table border="0" cellspacing="0" cellpadding="0" align="center">
  <tr>
    <th colspan="2"><a href="http://tw.beanfun.com/GT/NEWS/BulletinsList.aspx?kind=438&toALL=0"  onClick="javascript:dcsMultiTrack('DCSext.tw_GT_index','031');"><img src="../images/main/link_01.jpg" name="link01" width="190" height="54" id="link01" onmouseover="MM_swapImage('link01','','../images/main/link_01_o.jpg',0)" onmouseout="MM_swapImgRestore()" /></a></th>
    </tr>
  <tr>
    <th colspan="2"><a href="https://tw.event.beanfun.com/GT/ExchangeSN/ExchangeSN.aspx" onClick="javascript:dcsMultiTrack('DCSext.tw_GT_index','032');" ><img src="../images/main/link_02.jpg" name="link02" width="190" height="54" id="link02" onmouseover="MM_swapImage('link02','','../images/main/link_02_o.jpg',0)" onmouseout="MM_swapImgRestore()" /></a></th>
    </tr>
</table>
</div> 
<!--活動專區--> 

</td>
    <td class="right">
    <!--右區塊-->

<div ID="INTRO">
<!--標題-->
<div class="header">
<img src="../images/header/change.jpg" width="726" height="47" />
<h6>您的位置：<a href="../index.aspx" target="_top">首頁</a> > 會員專區 > 兌換元寶</h6>
</div>

<!--內容-->
<div class="content">

  <div class="text">
  <h1>&nbsp;確認兌換元寶&nbsp;</h1>
  
  <div class="status">您目前使用的「beanfun!」會員帳號為<span><asp:Label ID="lbl_Gash_Account" runat="server" Text="" CssClass="aa" ></asp:Label></span>、現有餘額為<span><asp:Label
      ID="lblUserPoint" runat="server" Text="" CssClass="aa"></asp:Label></span>。</div> 
  
  <div class="select2">
  <h2>請於下方選擇您想要兌換的內容：&nbsp;</h2>
    <table border="0" cellspacing="0" cellpadding="0" style="width:100%">
  <tr>
    <th width="16%">1. 伺服器：</th>
    <td width="84%">
        <asp:DropDownList ID="ddl_Server" runat="server" 
            onselectedindexchanged="ddl_Server_SelectedIndexChanged" 
            AutoPostBack="True">
        </asp:DropDownList>
    </td>
  </tr>
  <tr>
    <th>2. 角色名稱：</th>
    <td>
        <asp:DropDownList ID="ddl_Character" runat="server">
        </asp:DropDownList>
        <asp:Label ID="lblCharInfo" runat="server" Text="" CssClass="aa" ForeColor="Red"></asp:Label>
    </td>
  </tr>
  <tr>
    <th colspan="2">3. 使用多少樂豆點數兌換「元寶」：</th>
    </tr>
</table>
  </div>

  <div class="select">
      <asp:Panel ID="Panel1" runat="server" BorderStyle="None" BorderWidth="0">
        <asp:RadioButtonList ID="rb_Change_Gash" runat="server">
        </asp:RadioButtonList>
      </asp:Panel>
  </div> 

  <div class="note">※ 注意事項：
  <ol>
  <li>樂豆點點數兌換為《君臨天下》「元寶」的比值為<asp:Label ID="lbl_change_ratio" runat="server" Text=""></asp:Label>。 </li>  
  <li><asp:Label ID="lblNotice" runat="server" Text=""></asp:Label></li> 
  <li>樂豆點點數一經兌換為《君臨天下》「元寶」，表示您已確認於《君臨天下》使用「元寶」取得相關服務與虛擬物件，因此一經兌換成功後，將視同您已於《君臨天下》消費；存入您《君臨天下》遊戲帳號內的「元寶」，本公司將無法轉讓給第三人（包括同一人擁有多組帳號之情形）；亦無法進行兌換「元寶」所扣除之樂豆點會員帳號內點數回復或退費等動作。</li> 
  </ol>
  </div>

  
  </div>
  
<div class="btn">
    <asp:ImageButton ID="btn_confirm" runat="server" 
        ImageUrl="../images/btn/btn_confirm.jpg" width="160" height="47" 
        style="margin-right:25px" 
        onmouseover="MM_swapImage('btn_confirm','','../images/btn/btn_confirm_o.jpg',1)" 
        onmouseout="MM_swapImgRestore()" OnClientClick="return check()" 
        onclick="btn_confirm_Click"/>
    <asp:ImageButton ID="btn_cancel" runat="server" 
        ImageUrl="../images/btn/btn_cancel.jpg" width="160" height="47" 
        style="margin-right:25px" 
        onmouseover="MM_swapImage('btn_cancel','','../images/btn/btn_cancel_o.jpg',1)" 
        onmouseout="MM_swapImgRestore()" onclick="btn_cancel_Click" />
</div>

<!--底邊-->

</div>
<div class="content_end"></div>

<!--/右區塊-->
    </td>
  </tr>
</table>
</div>

<!--Bottom框-->
<div class="bottom"></div>
</div>

<!--//外框結束-->

<!--copyright-->
<div class="copyright">
<script type="text/javascript" src="../JS/copyright.js"></script>
</div>
<!--//copyright-->
    <asp:HiddenField ID="HFNewGuid" runat="server" />
    </form>
</body>
</html>
