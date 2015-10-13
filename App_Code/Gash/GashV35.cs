using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;

/// <summary>
/// GashV35 的摘要描述
/// </summary>
public class GashV35
{
	public GashV35()
	{
		//
		// TODO: 在此加入建構函式的程式碼
		//
	}

    public static string ConvertDisplayName(string _strGashAccount, string _strGameAccount, string _strServiceCode, string _strRegionCode)
    {
        string _return = string.Empty;
        string _strGashVersion = string.Empty;

        try
        {
            
            bfMainAccount.MainAccount bfMain = new bfMainAccount.MainAccount();
            _strGashVersion = bfMain.MainAccount_WhereIsTheUser(_strGashAccount);

            switch (_strGashVersion)
            {
                case "V30_Account":
                    _return = _strGameAccount;
                    break;
                case "V35_Account":
                    bfServiceAccount.ServiceAccount bfService = new bfServiceAccount.ServiceAccount();
                    bfServiceAccount.ServiceAccount_GetIndexDataAll_Result _wsResultIndex;
                    bfServiceAccount.Execute_DataTable_Result _wsResultStatus;
                    _wsResultIndex = bfService.ServiceAccount_GetIndexData_All(_strGameAccount, _strServiceCode, _strRegionCode);

                    if (_wsResultIndex.Result == 1)
                    {
                        int _iServerIndex = Convert.ToInt32(_wsResultIndex.ServerIndex);
                        int _iAccountSN = _wsResultIndex.ServiceAccountSN;
                        _wsResultStatus = bfService.ServiceAccount_GetStatus(_iServerIndex, _strServiceCode, _strRegionCode, _iAccountSN);
                        _return = _wsResultStatus.ResultData.Tables[0].Rows[0]["ServiceAccountDisplayName"].ToString();
                        bfService.Dispose();
                    }
                    break;
            }
            bfMain.Dispose();
        }
        catch//(Exception ex)
        {
            //SDCodeCheck.JSUtil.Alert(ex.Message);
        }
        return _return;
    }
}
