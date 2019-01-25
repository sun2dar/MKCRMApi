//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Models
{
	public class ESBKeyValueName
	{
		// KBI = InternetBankingIndividualInquiryStatusResultKeyName
		public const string KBI_ATMCardNo = "cardnumber";
		public const string KBI_BlockStatus = "status";
		public const string KBI_ChangePasswordCounter = "pswdchgflag";
		public const string KBI_CreatedDate = "createddate";
		public const string KBI_Disclaimer = "disclaimer";
		public const string KBI_EmailAddress = "emailaddr";
		public const string KBI_EmailStatus = "emailflag";
		//public const string FMTimeStamp = "FM Time Stamp";
		public const string KBI_Language = "language";
		public const string KBI_LastLoginDate = "lastlogindate";
		public const string KBI_LastUpdateDate = "lastupdatedate";
		public const string KBI_Name = "firstname";
		//public const string PinOffset = "pinOffset";
		public const string KBI_ReferenceNo = "rquid";
		public const string KBI_RegistrationDate = "registerdate";
		//public const string KBI_RegistrationTime = "registerTime";
		public const string KBI_TandemStatus = "status";
		public const string KBI_UpdateBy = "updateofficer";
		//public const string UpdatedByForEmail = "updOfficer";
		public const string KBI_UserId = "custid";
		public const string KBI_UserIdStatus = "inuseflag";
		public const string KBI_WrongPinCounter = "trlcnt";

		// CCConn = CreditCardConnectionOnIBankInquiryStatusResultKeyName
		public const string CCConn_ApplicationName = "kdapl";
		public const string CCConn_CardNo = "no_kartu";
		public const string CCConn_ConnectionDate = "wktkonek";
		public const string CCConn_ConnectionType = "kdnode";
		public const string CCConn_CustomerName = "firstname";
		public const string CCConn_CustomerNo = "no_rekening";
		public const string CCConn_Id1 = "updateofficer";
		public const string CCConn_Id2 = "spvofficer";
		public const string CCConn_KeyId = "keyid";
		//public const string NewStatus = "newStatus";

		// CCAppl = CreditCardAppliedStatusOnIBankResultKeyName
		public const string CCAppl_Address1 = "mfalamat1";
		public const string CCAppl_Address2 = "mfalamat2";
		public const string CCAppl_Address3 = "mfalamat3";
		public const string CCAppl_Address4 = "mfalamat4";
		public const string CCAppl_Address5 = "mfalamat5";
		public const string CCAppl_Address6 = "mfalamat6";
		public const string CCAppl_Address7 = "mfalamat7";
		public const string CCAppl_Address8 = "mfalamat8";
		public const string CCAppl_ApplicationName = "kdapl";
		public const string CCAppl_CardNo = "no_kartu";
		public const string CCAppl_ConnectionDate = "wktkonek";
		public const string CCAppl_ConnectionType = "kdnode";
		public const string CCAppl_CreatedDate = "createddate";
		public const string CCAppl_CustomerName = "firstname";
		public const string CCAppl_CustomerNo = "no_rekening";
		public const string CCAppl_Id1 = "updateofficer";
		public const string CCAppl_Id2 = "spvofficer";
		public const string CCAppl_Reason = "keterangan";
		public const string CCAppl_RegisteredKeyId = "keyid";
		public const string CCAppl_Status = "kd_status";

		// TokInfo = TokenInfoResultKeyName
		//public const string ActivatedBy = "spvOfficer";
		//public const string Branch = "branch";
		//public const string City = "city";
		//public const string LastUpdateDate = "lastUpdateDate";
		//public const string Status = "status";
		//public const string UpdateBy = "updOfficer";

		// TokDet = TokenDetailTblKoneksiBySNTokenResultKeyName
		//public const string ApplicationName = "aplName";
		//public const string CardNo = "cardNo";
		//public const string ConnectedOn = "createdDate";
		//public const string ConnectionType = "connectionType";
		//public const string CustomerName = "custName";
		//public const string KeyId = "keyId";
		//public const string Status = "status";
		//public const string UpdatedDate = "lastUpdateDate";
		//public const string UserId1 = "updOfficer";
		//public const string UserId2 = "spvOfficer";

		// TokDetTbh = TokenDetailTblTbhKoneksiBySNTokenResultKeyName
		//public const string Action = "aplCd";
		//public const string ApplicationName = "aplName";
		//public const string AtmCardNo = "atmNo";
		//public const string ConnectionDate = "connectDate";
		//public const string ConnectionType = "connectionType";
		//public const string CustomerName = "custName";
		//public const string Id1 = "updOfficer";
		//public const string Id2 = "spvOfficer";
		//public const string KeyId = "keyId";
		//public const string RejectionReason = "rejectedReason";
		//public const string RequestDate = "tokenReqDate";
		//public const string Status = "status";

		// MB = MobileBankingInquiryStatusResultKeyName
		public const string MB_ActivationDate = "createddate";
		public const string MB_ActivationFin = "userfin";
		public const string MB_ActivationFinDate = "flagfindate";
		public const string MB_ATMCardNo = "cardnumber";
		public const string MB_BlockStatus = "status";
		public const string MB_ChangePinCounter = "pswdChgFlag";
		public const string MB_CustomerName = "custname";
		public const string MB_Disclaimer = "agree";
		public const string MB_Language = "language";
		public const string MB_LastTransactionDate = "lastlogindate";
		public const string MB_LastUpdateDate = "lastupdate";
		public const string MB_MobileNo = "custid";
		public const string MB_PinActivation = "flagfin";
		public const string MB_UpdateBy = "updateofficer";
		public const string MB_WrongPinCounter = "trlcnt";

		public const string MBT_UserIdStatus = "userIdStatus";

		// SMS BCA = SMSBCAInquiryStatusResultKeyName
		public const string SMSBCA_MobileNo = "custid";
		public const string SMSBCA_CustomerName = "custname";
		public const string SMSBCA_RegistrationDate = "regdate";
		public const string SMSBCA_LastRegistrationDate = "lastregdate";
		public const string SMSBCA_LastTransactionDate = "lasttxndate";
		public const string SMSBCA_Status = "status";
		public const string SMSBCA_CounterCodeAccess = "trlcnt";
		public const string SMSBCA_UpdateOfficer = "updofficer";
		public const string SMSBCA_UpdateDate = "lastupdate";
		public const string SMSBCA_AtmCardNo = "cardnumber";

		// SMS TOP UP = SMSTopUpInquiryStatusResultKeyName
		public const string SMSTopUp_ATMCardNo = "cardnumber";
		public const string SMSTopUp_CardholderName = "custname";
		public const string SMSTopUp_CounterCodeAccess = "trlcnt";
		public const string SMSTopUp_LastRegistrationDate = "lastregdate";
		public const string SMSTopUp_LastTransactionDate = "lasttxndate";
		public const string SMSTopUp_ProviderCode = "paycode";
		public const string SMSTopUp_RegistrationDate = "regdate";
		public const string SMSTopUp_Status = "status";
		public const string SMSTopUp_UpdateDate = "lastupdate";
		public const string SMSTopUp_UpdateOfficer = "updofficer";

		// TokenDetailTblTbhKoneksiBySNTokenResultKeyName
		public const string TokenDetailTblTbhKoneksi_Action = "kdapl";
		public const string TokenDetailTblTbhKoneksi_ApplicationName = "namaapl";
		public const string TokenDetailTblTbhKoneksi_AtmCardNo = "no_kartu";
		public const string TokenDetailTblTbhKoneksi_ConnectionDate = "wktkonek";
		public const string TokenDetailTblTbhKoneksi_ConnectionType = "kdnode";
		public const string TokenDetailTblTbhKoneksi_CustomerName = "firstname";
		public const string TokenDetailTblTbhKoneksi_Id1 = "updateofficer";
		public const string TokenDetailTblTbhKoneksi_Id2 = "spvofficer";
		public const string TokenDetailTblTbhKoneksi_KeyId = "keyid";
		public const string TokenDetailTblTbhKoneksi_RejectionReason = "keterangan";
		public const string TokenDetailTblTbhKoneksi_RequestDate = "tglmohon";
		public const string TokenDetailTblTbhKoneksi_Status = "kd_status";

		// TokenDetailTblKoneksiBySNTokenResultKeyName
		public const string TokenDetailTblKoneksi_ApplicationName = "namaapl";
		public const string TokenDetailTblKoneksi_CardNo = "no_kartu";
		public const string TokenDetailTblKoneksi_ConnectedOn = "createddate";
		public const string TokenDetailTblKoneksi_ConnectionType = "kdnode";
		public const string TokenDetailTblKoneksi_CustomerName = "firstname";
		public const string TokenDetailTblKoneksi_KeyId = "keyid";
		public const string TokenDetailTblKoneksi_Status = "kd_status";
		public const string TokenDetailTblKoneksi_UpdatedDate = "lastupdate";
		public const string TokenDetailTblKoneksi_UserId1 = "updateofficer";
		public const string TokenDetailTblKoneksi_UserId2 = "spvofficer";

		// TokenInfoResultKeyName
		public const string TokenInfo_ActivatedBy = "spvofficer";
		public const string TokenInfo_Branch = "cabtoken";
		public const string TokenInfo_City = "kotatoken";
		public const string TokenInfo_LastUpdateDate = "lastupdatedate";
		public const string TokenInfo_Status = "kd_status";
		public const string TokenInfo_UpdateBy = "updateofficer";

	}
}