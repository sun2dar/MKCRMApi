using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CCARE.Models.General
{
    public partial class CRMDb
    {
        public KeyValuePair<int, String> AccountPayment_Insert(AccountPayment model, SessionForSP sessionParam)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountPaymentID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.AccountPaymentID) },
                new SqlParameter("@NamaPerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.NamaPerusahaan) },
                new SqlParameter("@JenisPembayaran", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.JenisPembayaran) },
                new SqlParameter("@KodePerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.KodePerusahaan) },
                new SqlParameter("@NoRekPerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.NoRekPerusahaan) },
                new SqlParameter("@CabangKoordinator", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.CabangKoordinator) },
                new SqlParameter("@JenisKerjasama", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.JenisKerjasama) },
                new SqlParameter("@AlurTransaksiATM", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.AlurTransaksiATM) },
                new SqlParameter("@EBanking", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.EBanking) },
                new SqlParameter("@SandiPerusahaanMBCA", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.SandiPerusahaanMBCA) },
                new SqlParameter("@Limit", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Limit) },
                new SqlParameter("@DenominasiVoucher", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.DenominasiVoucher) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@OrganizationID", sessionParam.OrganizationID),
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[AccountPayment_Insert] @AccountPaymentID, @NamaPerusahaan, @JenisPembayaran, @KodePerusahaan, @NoRekPerusahaan, @CabangKoordinator, @JenisKerjasama, @AlurTransaksiATM, @EBanking, @SandiPerusahaanMBCA, @Limit, @DenominasiVoucher, @ModifiedBy, @OrganizationID, @RetVal out, @Message out", param);

            int retVal = (int)param[14].Value;
            string valueRes = param[15].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> AccountPayment_Update(AccountPayment model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountPaymentID", SqlDbType.UniqueIdentifier) { Value = CheckForDbNull(model.AccountPaymentID) },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@NamaPerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.NamaPerusahaan) },
                new SqlParameter("@JenisPembayaran", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.JenisPembayaran) },
                new SqlParameter("@KodePerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.KodePerusahaan) },
                new SqlParameter("@NoRekPerusahaan", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.NoRekPerusahaan) },
                new SqlParameter("@CabangKoordinator", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.CabangKoordinator) },
                new SqlParameter("@JenisKerjasama", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.JenisKerjasama) },
                new SqlParameter("@AlurTransaksiATM", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.AlurTransaksiATM) },
                new SqlParameter("@EBanking", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.EBanking) },
                new SqlParameter("@SandiPerusahaanMBCA", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.SandiPerusahaanMBCA) },
                new SqlParameter("@Limit", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.Limit) },
                new SqlParameter("@DenominasiVoucher", SqlDbType.NVarChar, 200) { Value = CheckForDbNull(model.DenominasiVoucher) },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[AccountPayment_Update] @AccountPaymentID, @ModifiedBy, @NamaPerusahaan, @JenisPembayaran, @KodePerusahaan, @NoRekPerusahaan, @CabangKoordinator, @JenisKerjasama, @AlurTransaksiATM, @EBanking, @SandiPerusahaanMBCA, @Limit, @DenominasiVoucher, @RetVal out, @Message out", param);

            int retVal = (int)param[13].Value;
            string valueRes = param[14].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> AccountPayment_ChangeStatus(AccountPayment model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountPaymentID", SqlDbType.UniqueIdentifier) { Value = model.AccountPaymentID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[AccountPayment_ChangeStatus] @AccountPaymentID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }

        public KeyValuePair<int, String> AccountPayment_Delete(AccountPayment model)
        {
            var param = new SqlParameter[]{
                new SqlParameter("@AccountPaymentID", SqlDbType.UniqueIdentifier) { Value = model.AccountPaymentID },
                new SqlParameter("@ModifiedBy", SqlDbType.UniqueIdentifier) { Value = model.ModifiedBy },
                new SqlParameter("@RetVal", SqlDbType.Int) {Direction = ParameterDirection.Output},
                new SqlParameter("@Message", SqlDbType.NVarChar, 100) {Direction = ParameterDirection.Output}
            };

            int rc = ((IObjectContextAdapter)this).ObjectContext.ExecuteStoreCommand("exec [CRM].[AccountPayment_Delete] @AccountPaymentID, @ModifiedBy, @RetVal out, @Message out", param);

            int retVal = (int)param[2].Value;
            string valueRes = param[3].Value.ToString();

            return new KeyValuePair<int, string>(retVal, valueRes);
        }
    }
}