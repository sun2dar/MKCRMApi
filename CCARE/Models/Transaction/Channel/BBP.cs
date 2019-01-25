using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Configuration;
using System.Threading;
using System.Globalization;
using CCARE.App_Function;
using BCA.CRM.OSB.Model;

namespace CCARE.Models.Transaction
{
    public partial class ChannelTransaction
    {
        public static BCAByPhoneTransaction BBP(Params request)
        {
            BCAByPhoneTransaction transaction = new BCAByPhoneTransaction();
            ESBData data = EAI.RetrieveESBData(request);
            if (data.Result != null && data.Result.Count() > 0)
            {
                int idx = 0;
                foreach (StringDictionary entry in data.Result)
                {
                    if (idx == 0)
                    {
                        transaction.NextPos = entry["nextPos"];
                        transaction.PrevPos = entry["prevPos"];
                        transaction.CurrPos = entry["currPos"];
                    }
                    else
                    {
                        BBPTRX trx = new BBPTRX();
                        trx.CustomerID = entry["custidoraccnum"];
                        trx.TransactionDate = entry["localdate"];
                        trx.TransactionTime = entry["localtime"];
                        trx.TransactionDesc = entry["txndesc"];
                        trx.ResponseCode = entry["respcde"];
                        transaction.Transactions.Add(trx);
                    }
                    idx++;
                }
            }
            else 
            {
                transaction.Msg = data.Message;
            }
            return transaction;
        }
    }
}