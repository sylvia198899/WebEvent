﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;


    // 
    // This source code was auto-generated by wsdl, Version=2.0.50727.42.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ServiceSoap", Namespace = "http://GASHv30FWS/Service/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback Service_GetAllServiceAccountsOperationCompleted;

        private System.Threading.SendOrPostCallback Service_GetAllServiceAccountsStatusOperationCompleted;

        private System.Threading.SendOrPostCallback Service_GetAllServiceAccountsStatusLockOperationCompleted;

        /// <remarks/>
        public Service()
        {
            this.Url = "https://tw.gashws.gamania.com/GASHv30FWS/Service.asmx";
        }

        /// <remarks/>
        public event Service_GetAllServiceAccountsCompletedEventHandler Service_GetAllServiceAccountsCompleted;

        /// <remarks/>
        public event Service_GetAllServiceAccountsStatusCompletedEventHandler Service_GetAllServiceAccountsStatusCompleted;

        /// <remarks/>
        public event Service_GetAllServiceAccountsStatusLockCompletedEventHandler Service_GetAllServiceAccountsStatusLockCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://GASHv30FWS/Service/Service_GetAllServiceAccounts", RequestNamespace = "http://GASHv30FWS/Service/", ResponseNamespace = "http://GASHv30FWS/Service/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Service_GetAllServiceAccounts(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            object[] results = this.Invoke("Service_GetAllServiceAccounts", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginService_GetAllServiceAccounts(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Service_GetAllServiceAccounts", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, callback, asyncState);
        }

        /// <remarks/>
        public System.Data.DataSet EndService_GetAllServiceAccounts(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            this.Service_GetAllServiceAccountsAsync(_strMainAccountID, _strServiceCode, _strServiceRegion, null);
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, object userState)
        {
            if ((this.Service_GetAllServiceAccountsOperationCompleted == null))
            {
                this.Service_GetAllServiceAccountsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnService_GetAllServiceAccountsOperationCompleted);
            }
            this.InvokeAsync("Service_GetAllServiceAccounts", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, this.Service_GetAllServiceAccountsOperationCompleted, userState);
        }

        private void OnService_GetAllServiceAccountsOperationCompleted(object arg)
        {
            if ((this.Service_GetAllServiceAccountsCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Service_GetAllServiceAccountsCompleted(this, new Service_GetAllServiceAccountsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://GASHv30FWS/Service/Service_GetAllServiceAccountsStatus", RequestNamespace = "http://GASHv30FWS/Service/", ResponseNamespace = "http://GASHv30FWS/Service/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Service_GetAllServiceAccountsStatus(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            object[] results = this.Invoke("Service_GetAllServiceAccountsStatus", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginService_GetAllServiceAccountsStatus(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Service_GetAllServiceAccountsStatus", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, callback, asyncState);
        }

        /// <remarks/>
        public System.Data.DataSet EndService_GetAllServiceAccountsStatus(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsStatusAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            this.Service_GetAllServiceAccountsStatusAsync(_strMainAccountID, _strServiceCode, _strServiceRegion, null);
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsStatusAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, object userState)
        {
            if ((this.Service_GetAllServiceAccountsStatusOperationCompleted == null))
            {
                this.Service_GetAllServiceAccountsStatusOperationCompleted = new System.Threading.SendOrPostCallback(this.OnService_GetAllServiceAccountsStatusOperationCompleted);
            }
            this.InvokeAsync("Service_GetAllServiceAccountsStatus", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, this.Service_GetAllServiceAccountsStatusOperationCompleted, userState);
        }

        private void OnService_GetAllServiceAccountsStatusOperationCompleted(object arg)
        {
            if ((this.Service_GetAllServiceAccountsStatusCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Service_GetAllServiceAccountsStatusCompleted(this, new Service_GetAllServiceAccountsStatusCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://GASHv30FWS/Service/Service_GetAllServiceAccountsStatusLock", RequestNamespace = "http://GASHv30FWS/Service/", ResponseNamespace = "http://GASHv30FWS/Service/", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Data.DataSet Service_GetAllServiceAccountsStatusLock(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            object[] results = this.Invoke("Service_GetAllServiceAccountsStatusLock", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion});
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public System.IAsyncResult BeginService_GetAllServiceAccountsStatusLock(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, System.AsyncCallback callback, object asyncState)
        {
            return this.BeginInvoke("Service_GetAllServiceAccountsStatusLock", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, callback, asyncState);
        }

        /// <remarks/>
        public System.Data.DataSet EndService_GetAllServiceAccountsStatusLock(System.IAsyncResult asyncResult)
        {
            object[] results = this.EndInvoke(asyncResult);
            return ((System.Data.DataSet)(results[0]));
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsStatusLockAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion)
        {
            this.Service_GetAllServiceAccountsStatusLockAsync(_strMainAccountID, _strServiceCode, _strServiceRegion, null);
        }

        /// <remarks/>
        public void Service_GetAllServiceAccountsStatusLockAsync(string _strMainAccountID, string _strServiceCode, string _strServiceRegion, object userState)
        {
            if ((this.Service_GetAllServiceAccountsStatusLockOperationCompleted == null))
            {
                this.Service_GetAllServiceAccountsStatusLockOperationCompleted = new System.Threading.SendOrPostCallback(this.OnService_GetAllServiceAccountsStatusLockOperationCompleted);
            }
            this.InvokeAsync("Service_GetAllServiceAccountsStatusLock", new object[] {
                    _strMainAccountID,
                    _strServiceCode,
                    _strServiceRegion}, this.Service_GetAllServiceAccountsStatusLockOperationCompleted, userState);
        }

        private void OnService_GetAllServiceAccountsStatusLockOperationCompleted(object arg)
        {
            if ((this.Service_GetAllServiceAccountsStatusLockCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Service_GetAllServiceAccountsStatusLockCompleted(this, new Service_GetAllServiceAccountsStatusLockCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void Service_GetAllServiceAccountsCompletedEventHandler(object sender, Service_GetAllServiceAccountsCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Service_GetAllServiceAccountsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Service_GetAllServiceAccountsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public System.Data.DataSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void Service_GetAllServiceAccountsStatusCompletedEventHandler(object sender, Service_GetAllServiceAccountsStatusCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Service_GetAllServiceAccountsStatusCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Service_GetAllServiceAccountsStatusCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public System.Data.DataSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    public delegate void Service_GetAllServiceAccountsStatusLockCompletedEventHandler(object sender, Service_GetAllServiceAccountsStatusLockCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "2.0.50727.42")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Service_GetAllServiceAccountsStatusLockCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal Service_GetAllServiceAccountsStatusLockCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState)
            :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public System.Data.DataSet Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((System.Data.DataSet)(this.results[0]));
            }
        }
    }