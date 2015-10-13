<%@ Page Language="C#" %>
<%@ Import Namespace="System.Security.Cryptography" %> 
<%@ Import Namespace="System.Threading" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">
 void Page_Load() {   
     byte[] delay = new byte[1];   
      RandomNumberGenerator prng = new RNGCryptoServiceProvider();

      prng.GetBytes(delay);   
      Thread.Sleep((int)delay[0]);   
           
      IDisposable disposable = prng as IDisposable;   
     if (disposable != null) { disposable.Dispose(); }   
    }   
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="robots" content="noindex, nofollow">
    <meta http-equiv="PRAGMA" content="NO-CACHE"/>
    <meta http-equiv="Expires" content="-1"/>
    <meta http-equiv="CACHE-CONTROL" content="NO-CACHE"/>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <meta name="description" content=""/>
    <title>Gamania</title>
</head>
<body>
<p style="text-align: center">
<img src="/UnderConstruction.jpg" alt="An error occurred while processing your request" />
</p>

</body>
</html>
