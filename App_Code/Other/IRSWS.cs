﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:2.0.50727.3615
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

// 
// 此原始程式碼由 wsdl 版本=2.0.50727.42 自動產生。
// 


/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Web.Services.WebServiceBindingAttribute(Name="IRSWSSoap", Namespace="http://tempuri.org/")]
public partial class IRSWS : System.Web.Services.Protocols.SoapHttpClientProtocol {
    
    private System.Threading.SendOrPostCallback InsErrOperationCompleted;
    
    /// <remarks/>
    public IRSWS() {
        this.Url = "http://10.100.1.96/IRSWebService/IRSWS.asmx";
    }
    
    /// <remarks/>
    public event InsErrCompletedEventHandler InsErrCompleted;
    
    /// <remarks/>
    [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/InsErr", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
    public sInsErr InsErr(int intRSVID, string strURL, string strFuncName, string strFuncID, string strErrorMsg) {
        object[] results = this.Invoke("InsErr", new object[] {
                    intRSVID,
                    strURL,
                    strFuncName,
                    strFuncID,
                    strErrorMsg});
        return ((sInsErr)(results[0]));
    }
    
    /// <remarks/>
    public System.IAsyncResult BeginInsErr(int intRSVID, string strURL, string strFuncName, string strFuncID, string strErrorMsg, System.AsyncCallback callback, object asyncState) {
        return this.BeginInvoke("InsErr", new object[] {
                    intRSVID,
                    strURL,
                    strFuncName,
                    strFuncID,
                    strErrorMsg}, callback, asyncState);
    }
    
    /// <remarks/>
    public sInsErr EndInsErr(System.IAsyncResult asyncResult) {
        object[] results = this.EndInvoke(asyncResult);
        return ((sInsErr)(results[0]));
    }
    
    /// <remarks/>
    public void InsErrAsync(int intRSVID, string strURL, string strFuncName, string strFuncID, string strErrorMsg) {
        this.InsErrAsync(intRSVID, strURL, strFuncName, strFuncID, strErrorMsg, null);
    }
    
    /// <remarks/>
    public void InsErrAsync(int intRSVID, string strURL, string strFuncName, string strFuncID, string strErrorMsg, object userState) {
        if ((this.InsErrOperationCompleted == null)) {
            this.InsErrOperationCompleted = new System.Threading.SendOrPostCallback(this.OnInsErrOperationCompleted);
        }
        this.InvokeAsync("InsErr", new object[] {
                    intRSVID,
                    strURL,
                    strFuncName,
                    strFuncID,
                    strErrorMsg}, this.InsErrOperationCompleted, userState);
    }
    
    private void OnInsErrOperationCompleted(object arg) {
        if ((this.InsErrCompleted != null)) {
            System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
            this.InsErrCompleted(this, new InsErrCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
        }
    }
    
    /// <remarks/>
    public new void CancelAsync(object userState) {
        base.CancelAsync(userState);
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(Namespace="http://tempuri.org/")]
public partial class sInsErr {
    
    private int intResultField;
    
    private string strMsgField;
    
    /// <remarks/>
    public int intResult {
        get {
            return this.intResultField;
        }
        set {
            this.intResultField = value;
        }
    }
    
    /// <remarks/>
    public string strMsg {
        get {
            return this.strMsgField;
        }
        set {
            this.strMsgField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
public delegate void InsErrCompletedEventHandler(object sender, InsErrCompletedEventArgs e);

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
public partial class InsErrCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
    
    private object[] results;
    
    internal InsErrCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
            base(exception, cancelled, userState) {
        this.results = results;
    }
    
    /// <remarks/>
    public sInsErr Result {
        get {
            this.RaiseExceptionIfNecessary();
            return ((sInsErr)(this.results[0]));
        }
    }
}
