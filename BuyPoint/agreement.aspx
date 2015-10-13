<%@ Page Language="C#" AutoEventWireup="true" CodeFile="agreement.aspx.cs" Inherits="BuyPoint_agreement" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>君臨天下</title>
<link href="../CSS/style.css" rel="stylesheet" type="text/css" />
<link href="../CSS/intro.css" rel="stylesheet" type="text/css" />
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

    function close() {
        window.opener = null;
        window.open('', '_self');
        window.close();
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

<p style="font-weight:bold; line-height:20px; letter-spacing:1px">如欲於《君臨天下》遊戲內消費或加入VIP，玩家需先將樂豆點點數兌換成「元寶」，若當樂豆點點數不足，無法兌換「元寶」時，請先至【beanfun!平台】進行儲值動作喔！</p>
<div class="img_step"><img src="../images/step_change.jpg" width="547" height="99" border="0" usemap="#Map">
  <map name="Map" id="Map">
    <area shape="rect" coords="6,5,146,94" href="http://www.gashplus.com/gashstore/GashStore.aspx" target="_blank" title="購買Gash+點">
    <area shape="rect" coords="204,5,344,93" href="http://tw.beanfun.com/TW/CheckLogin.aspx?Page=2" target="_blank" title="儲值樂豆點數">
  </map>
</div>

  <div class="text">
  <h1>&nbsp;樂豆點扣點同意書&nbsp;</h1>
  <p>除了遊戲帳號停權者，凡《君臨天下》玩家皆可以樂豆點點數兌換「元寶」後，至遊戲內消費。兌換前需同意以下樂豆點扣點同意書內容才能進行兌換。</p>
  <p>樂豆點點數與「元寶」的計算方式：台灣樂豆點 1點可購買2個「元寶」。</p>
  <p>為保障您的權益，請於使用本服務前，詳細閱讀同意書中所有內容，當您在線上點選「我同意」後，即表示您已經同意遵守以下條款及相關之法律規定：</p>
  <p class="list_1">(一) 在使用本服務前，請確認您使用本服務兌換《君臨天下》「元寶」所對應的beanfun!會員帳號及《君臨天下》遊戲帳號、密碼是否正確無誤。</p>
  <p class="list_1">(二) 您瞭解樂豆點點數一經兌換成為《君臨天下》「元寶」後，表示您已經確認於《君臨天下》使用「元寶」取得相關服務與虛擬物件，因此一經兌換成功後將視同您已於《君臨天下》消費；存入您遊戲帳號內的「元寶」，本公司將無法轉讓給第三人（包括同一人擁有多組帳號之情形）；亦無法進行兌換「元寶」所扣除之beanfun!會員帳號內點數回復或退費等動作。</p>
  <p class="list_1">(三) 凡使用本服務涉有不法者，應由當事人自負法律責任。當本公司合理懷疑有不法情事發生時，您同意本公司主動或配合相關單位移送您的相關資料供檢警調機關調查處理。</p>
  <p class="list_1">(四) 本同意書未約定事項悉依beanfun!之會員規範及其他服務之相關規範內容解釋之，並以中華民國法令、規章及慣例為處理依據；會員因使用本服務而與本公司間所產生之爭議，以台灣台北板橋地方法院為第一審管轄法院。</p>
  </div>
  
<div class="btn"><asp:ImageButton ID="imgbtAgree" runat="server" 
        ImageUrl="../images/btn/btn_agree.jpg" name="imgbtAgree" width="160" 
        height="47" 
        onmouseover="MM_swapImage('imgbtAgree','','../images/btn/btn_agree_o.jpg',1)" 
        onmouseout="MM_swapImgRestore()" style="margin-right:25px" 
        onclick="imgbtAgree_Click"/>
<a href="http://tw.beanfun.com/GT/index.aspx"><img src="../images/btn/btn_disagree.jpg" name="btn_disagree" width="160" height="47" id="btn_disagree" onmouseover="MM_swapImage('btn_disagree','','../images/btn/btn_disagree_o.jpg',1)" onmouseout="MM_swapImgRestore()" /></a></div>
</div>

<!--底邊-->
<div class="content_end"></div>

</div>
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
    </form>
</body>
</html>
