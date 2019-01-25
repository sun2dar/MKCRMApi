/* Created By Ardi (20151028) */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Configuration;
using CCARE.Models.General;
using CCARE.Models;
using CCARE.Models.Transaction;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Globalization;
using System.Data.Objects;

namespace CCARE.App_Function
{
    public class MergeDocument
    {
        public CRMDb db = new CRMDb();
        public HistDb htdb = new HistDb();
        public String objectTypeCode = ConfigurationManager.AppSettings["typeCodeLetterEntry"];
        public String subject = ConfigurationManager.AppSettings["subjectLetterEntry"];
        public String noteText = ConfigurationManager.AppSettings["noteLetterEntry"];
        public String headerLineNum = ConfigurationManager.AppSettings["lineNum"];
        public String headerTanggal_ID = ConfigurationManager.AppSettings["tanggal_ID"];
        public String headerJam_ID = ConfigurationManager.AppSettings["jam_ID"];
        public String headerTerminal_ID = ConfigurationManager.AppSettings["terminal_ID"];
        public String headerLokasi_ID = ConfigurationManager.AppSettings["lokasi_ID"];
        public String headerNominal_ID = ConfigurationManager.AppSettings["nominal_ID"];
        public String headerNominal_EN = ConfigurationManager.AppSettings["nominal_EN"];
        public String headerTunai_ID = ConfigurationManager.AppSettings["tunai_ID"];
        public String headerKeterangan_ID = ConfigurationManager.AppSettings["keterangan_ID"];
        public String headerKeterangan_EN = ConfigurationManager.AppSettings["keterangan_EN"];
        public String headerLokasiTrx_ID = ConfigurationManager.AppSettings["lokasiTrx_ID"];
        public String headerLokasiTrx_EN = ConfigurationManager.AppSettings["lokasiTrx_EN"];
        public String headerTipeTrx_ID = ConfigurationManager.AppSettings["tipeTrx_ID"];
        public String headerTipeTrx_EN = ConfigurationManager.AppSettings["tipeTrx_EN"];
        public String headerDateTrx_ID = ConfigurationManager.AppSettings["dateTrx_ID"];
        public String headerDateTrx_EN = ConfigurationManager.AppSettings["dateTrx_EN"];
        public String headerDateTimeTrx_ID = ConfigurationManager.AppSettings["dateTimeTrx_ID"];
        public String perihal_ID = ConfigurationManager.AppSettings["perihal_ID"];
        public String perihal_EN = ConfigurationManager.AppSettings["perihal_EN"];
        public String noteForCirrus_ID = ConfigurationManager.AppSettings["noteCirrus_ID"];
        public String noteForCirrus_EN = ConfigurationManager.AppSettings["noteCirrus_EN"];
        public String branchNameHALO_EN = ConfigurationManager.AppSettings["branchNameHALO_EN"];
        public String branchNameHALO_ID = ConfigurationManager.AppSettings["branchNameHALO_ID"];
        public String branchNameSPC_ID = ConfigurationManager.AppSettings["branchNameSPC_ID"];
        public String transaksiDebit_ID = ConfigurationManager.AppSettings["transaksiDebit_ID"];
        public String cirrusReversal = ConfigurationManager.AppSettings["cirrusReversal"];

        public String merchantAccountNoTo = String.Empty;
        public String merchantAccountNameTo = String.Empty;
        public String merchantNameTo = String.Empty;
        public String merchantAddress1To = String.Empty;
        public String merchantAddress2To = String.Empty;
        public String merchantZIPTo = String.Empty;
        public String merchantNumLineTo = String.Empty;

        public String[] arrMonthEng = new String[] { "January", "February", "March", "April", "May", "June", "July", "Agustus", "September", "October", "November", "December" };
        public String[] arrMonthInd = new String[] { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
        public String[] arrNumInd = new String[] { "nol", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh" };

        public const String constDelimeter = ",";
        public const String constSpace = " ";
        public const String constDash = "-";
        public const String constSlash = "/";
        public const String constError = "ERROR";

        public CultureInfo provider = CultureInfo.CurrentCulture;
        public String formatDateTime = "dd-MM-yyyy HH:mm:ss";
        public String formatDate = "dd-MM-yyyy";

        public String FormatDateTime(DateTime date, Boolean toEnglish)
        {
            String result = null;
            try
            {
                String monthName = (toEnglish) ? arrMonthEng[date.Month - 1] : arrMonthInd[date.Month - 1];
                result = date.Day.ToString().PadLeft(2, '0') + constSpace + monthName + constSpace + date.Year.ToString();
            }
            catch (Exception e)
            {
                result = constError;
            }

            return result;
        }

        public DateTime? ConvertToDateTime(String strDate)
        {
            DateTime? result = null;

            try
            {
                if (!String.IsNullOrEmpty(strDate))
                {
                    strDate = strDate.Trim();
                    String[] strDatePart = strDate.Split(constSpace.ToCharArray());

                    if (strDatePart.Length.Equals(2))
                    {
                        result = DateTime.ParseExact(strDate, formatDateTime, provider);
                    }
                    else if (strDatePart.Length.Equals(1))
                    {
                        result = DateTime.ParseExact(strDate, formatDate, provider);
                    }
                    else throw new Exception();
                }
                else throw new Exception();
            }
            catch (Exception e)
            {
                result = null;
            }

            return result;
        }

        public JSONResponse Merge(Guid letterEntryID)
        {
            JSONResponse result = new JSONResponse();
            result.Value = false;

            string letterNoFrom = "|;;|";
            string letterDateFrom = "|``|";
            string description1From = "|,,|";
            string description2From = "|..|";
            string description3From = "|++|";
            string description4From = "|--|";
            string branchNameFrom = "|::|";
            string yearFrom = "|$year$|";
            string letterNoteFrom = "|$NOTES$|";
            string attachmentB4From = "|~*~|";
            string attachmentB4To = " ";
            string csAttachment = "|~ATTACHMENT~|";

            string transactionDateTimeFrom = "|$request.waktuTransTxt$|";
            string transactionDateFrom = "|$request.waktuTransTxt_date$|";
            string transactionTimeFrom = "|$request.waktuTransTxt_time$|";
            string WSIDKeyFrom = "|$request.wsid_key$|";
            string transactionAmountFrom = "|$request.nilaiTransaksi$|";
            string pKeyFrom = "|$request.pkey$|";
            string requestDateCreatedFrom = "|$request.datecreated$|";
            string address1From = "|$address.address1$|";
            string address2From = "|$address.address2$|";
            string address3From = "|$address.address3$|";
            string cityFrom = "|$address.city$|";
            string zipCodeFrom = "|$address.zipcode$|";
            string customerFullNameFrom = "|$customer.fullname$|";
            string cardNoFrom = "|$customer.noKartu$|";
            string accountNoFrom = "|$customer.noRekening$|";
            string contactMethodFrom = "|$contact.method$|";
            string productDescFrom = "|$product.descriptiontree$|";
            string yearNumFrom = "|$YearNum$|";

            string merchantAccountNoFrom = "|,merchantaccno,|";
            string merchantAccountNameFrom = "|,merchantaccnm,|";
            string merchantNameFrom = "|,merchant,|";
            string merchantAddress1From = "|,merchantaddr1,|";
            string merchantAddress2From = "|,merchantaddr2,|";
            string merchantZIPFrom = "|,merchantzip,|";
            string merchantNumLineFrom = "|,num,|";
            string fullpath = null;

            try
            {
                LetterEntry aLetterEntry = db.letterEntry.Where(x => x.LetterEntryID == letterEntryID).FirstOrDefault();
                LetterTemplate aLetterTemplate = db.letterTemplate.Where(x => x.LetterTemplateId == aLetterEntry.TemplateID).FirstOrDefault();
                Annotation aAnnotation = db.annotation.Where(x => x.ObjectID == aLetterTemplate.LetterTemplateId).FirstOrDefault();
                Request aRequest = db.request.Where(x => x.RequestID == aLetterEntry.RequestID).FirstOrDefault();
                WSID aWSID = db.wsid.Where(x => x.WSIDID == aRequest.WsIdID).FirstOrDefault();

                int letterType = aLetterTemplate.Type.Value;
                int language = aLetterTemplate.Language.Value;
                bool isAttachMaestro = aLetterEntry.AttchMaestro.HasValue ? aLetterEntry.AttchMaestro.Value : false;
                bool isAttachATM = aLetterEntry.AttchATM.HasValue ? aLetterEntry.AttchATM.Value : false;
                bool isAttachATMSwitching = aLetterEntry.AttchATMSwitching.HasValue ? aLetterEntry.AttchATMSwitching.Value : false;
                bool isAttachDebit = aLetterEntry.AttchDebit.HasValue ? aLetterEntry.AttchDebit.Value : false;
                bool isAttachCirrus = aLetterEntry.AttchCirrus.HasValue ? aLetterEntry.AttchCirrus.Value : false;

                if (aAnnotation == null) throw new Exception(Resources.LetterEntry.Alert_TemplateNoAttachment);
                else
                {
                    string path = System.Configuration.ConfigurationManager.AppSettings["pathLetterTemplate"];
                    string ext = System.IO.Path.GetExtension(aAnnotation.FileName);
                    string filename = aLetterTemplate.Name + constSpace + (!string.IsNullOrEmpty(aLetterEntry.TicketNumber) ? aLetterEntry.TicketNumber : string.Empty) + constSpace + (!string.IsNullOrEmpty(aLetterEntry.LetterNo) ? aLetterEntry.LetterNo : string.Empty) + constSpace + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    filename = filename.Replace(constSlash, constSpace);
                    fullpath = System.IO.Path.Combine(path, filename + ext);

                    if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);

                    System.IO.FileStream oFileStream = new System.IO.FileStream(fullpath, System.IO.FileMode.OpenOrCreate);
                    byte[] fileContent = Convert.FromBase64String(aAnnotation.DocumentBody);
                    oFileStream.Write(fileContent, 0, fileContent.Length);
                    oFileStream.Close();

                    /*Jika letter date tidak null, maka di format sesuai dengan language di letter template. Sebaliknya letter date diisi dengan tanggal sekarang dan di format sesuai language di letter template.*/
                    string letterDateTo = aLetterEntry.LetterDate.HasValue ? (language.Equals((int)LanguageType.ID) ? this.FormatDateTime(aLetterEntry.LetterDate.Value, false) : this.FormatDateTime(aLetterEntry.LetterDate.Value, true)) : (language.Equals((int)LanguageType.ID) ? this.FormatDateTime(DateTime.Now, false) : this.FormatDateTime(DateTime.Now, true));

                    string letterNoTo = string.IsNullOrEmpty(aLetterEntry.LetterNo) ? string.Empty : aLetterEntry.LetterNo;
                    string description1To = string.IsNullOrEmpty(aLetterEntry.Description1) ? string.Empty : aLetterEntry.Description1;
                    string description2To = string.IsNullOrEmpty(aLetterEntry.Description2) ? string.Empty : aLetterEntry.Description2;
                    string description3To = string.IsNullOrEmpty(aLetterEntry.Description3) ? string.Empty : aLetterEntry.Description3;
                    string description4To = string.IsNullOrEmpty(aLetterEntry.Description4) ? string.Empty : aLetterEntry.Description4;
                    string branchNameTo = string.IsNullOrEmpty(aLetterEntry.CC_Name) ? string.Empty : aLetterEntry.CC_Name;
                    string letterNoteTo = string.Empty;


                    /*Jika letter date tidak null, ambil empat digit tahun. Sebaliknya gunakan tahun sekarang.*/
                    string yearTo = aLetterEntry.LetterDate.HasValue ? aLetterEntry.LetterDate.Value.Year.ToString() : DateTime.Now.Year.ToString();

                    /*Ambil dua digit tahun dari variable yearTo diatas.*/
                    string yearNumTo = yearTo.Substring(2, 2);

                    /*Jika created date di Request tidak null, maka di format sesuai dengan language di letter template. Sebaliknya, kosongkan.*/
                    string requestDateCreatedTo = aRequest.CreatedOn.HasValue ? (language.Equals((int)LanguageType.ID) ? this.FormatDateTime(aRequest.CreatedOn.Value, false) : this.FormatDateTime(aRequest.CreatedOn.Value, true)) : string.Empty;

                    /*Jika Transaction Time di Request tidak null, maka convert ke dalam bentuk DateTime.*/
                    DateTime? dtTransactionTime = (!string.IsNullOrEmpty(aRequest.TransactionTime)) ? this.ConvertToDateTime(aRequest.TransactionTime) : null;
                    string transactionDateTo = dtTransactionTime.HasValue ? (language.Equals((int)LanguageType.ID) ? this.FormatDateTime(dtTransactionTime.Value, false) : this.FormatDateTime(dtTransactionTime.Value, true)) : string.Empty;
                    string transactionTimeTo = dtTransactionTime.HasValue ? dtTransactionTime.Value.ToString("HH:mm:ss") : string.Empty;
                    string transactionDateTimeTo = constError;
                    if (!string.IsNullOrEmpty(transactionDateTo) && !string.IsNullOrEmpty(transactionTimeTo))
                    {
                        transactionDateTimeTo = string.Format("{0} Pukul {1}", transactionDateTo, transactionTimeTo);
                    }
                    else if (string.IsNullOrEmpty(transactionDateTo))
                    {
                        transactionDateTo = constError;
                    }
                    else if (string.IsNullOrEmpty(transactionTimeTo))
                    {
                        transactionTimeTo = constError;
                    }

                    string WSIDKeyTo = aWSID != null ? (string.IsNullOrEmpty(aWSID.Name) ? string.Empty : aWSID.Name) : string.Empty;
                    /* Request Bu Reno SPC, 28 Oktober 2016. Remove ',00' */
                    //string transactionAmountTo = string.IsNullOrEmpty(aRequest.TransactionAmount_txt) ? string.Empty : aRequest.TransactionAmount_txt.Trim();
                    string transactionAmountTo = string.IsNullOrEmpty(aRequest.TransactionAmount_txt) ? "0" : aRequest.TransactionAmount_txt.Trim().Split(',')[0].ToString();
                    string pKeyTo = string.IsNullOrEmpty(aLetterEntry.TicketNumber) ? string.Empty : aLetterEntry.TicketNumber.Trim();
                    string address1To = string.IsNullOrEmpty(aLetterEntry.Address1) ? string.Empty : aLetterEntry.Address1.Trim();
                    string address2To = string.IsNullOrEmpty(aLetterEntry.Address2) ? string.Empty : aLetterEntry.Address2.Trim();
                    string address3To = string.IsNullOrEmpty(aLetterEntry.Address3) ? string.Empty : aLetterEntry.Address3.Trim();
                    string cityTo = string.IsNullOrEmpty(aLetterEntry.City) ? string.Empty : aLetterEntry.City.Trim();
                    string zipCodeTo = string.IsNullOrEmpty(aLetterEntry.ZIPPostalCode) ? string.Empty : aLetterEntry.ZIPPostalCode.Trim();
                    string strCityZip = cityTo + constSpace + zipCodeTo;
                    if (string.IsNullOrEmpty(address1To))
                    {
                        if (string.IsNullOrEmpty(address2To))
                        {
                            address1To = address3To;
                            address2To = strCityZip;
                            address3To = string.Empty;
                            cityTo = string.Empty;
                            zipCodeTo = string.Empty;
                        }
                        else if (string.IsNullOrEmpty(address3To))
                        {
                            address1To = address2To;
                            address2To = strCityZip;
                            address3To = string.Empty;
                            cityTo = string.Empty;
                            zipCodeTo = string.Empty;
                        }
                        else
                        {
                            address1To = address2To;
                            address2To = address3To;
                            address3To = strCityZip;
                            cityTo = string.Empty;
                            zipCodeTo = string.Empty;
                        }
                    }
                    else if (string.IsNullOrEmpty(address2To))
                    {
                        if (string.IsNullOrEmpty(address3To))
                        {
                            address2To = strCityZip;
                            address3To = string.Empty;
                            cityTo = string.Empty;
                            zipCodeTo = string.Empty;
                        }
                        else
                        {
                            address2To = address3To;
                            address3To = strCityZip;
                            cityTo = string.Empty;
                            zipCodeTo = string.Empty;
                        }
                    }
                    else if (string.IsNullOrEmpty(address3To))
                    {
                        address3To = strCityZip;
                        cityTo = string.Empty;
                        zipCodeTo = string.Empty;
                    }

                    string customerFullNameTo = string.IsNullOrEmpty(aLetterEntry.FullName) ? string.Empty : aLetterEntry.FullName;
                    string cardNoTo = string.IsNullOrEmpty(aLetterEntry.CardNo) ? string.Empty : aLetterEntry.CardNo;
                    string accountNoTo = string.IsNullOrEmpty(aLetterEntry.AccountNo) ? string.Empty : aLetterEntry.AccountNo;
                    if (letterType.Equals((int)LetterType.SPC) && isAttachDebit && !string.IsNullOrEmpty(aLetterEntry.AccountNo))
                    {
                        string tmp = aLetterEntry.AccountNo.Substring(0, 3);
                        int len = aLetterEntry.AccountNo.Length;
                        string tmp1 = aLetterEntry.AccountNo.Substring(len - 1, 1);
                        string tmp2 = aLetterEntry.AccountNo.Substring(3, len - 4);
                        accountNoTo = tmp + constDash + tmp2 + constDash + tmp1;
                    }

                    string contactMethodTo = string.IsNullOrEmpty(aRequest.ContactMethodLabel) ? string.Empty : aRequest.ContactMethodLabel;
                    string productDescTo = string.IsNullOrEmpty(aRequest.ProductName) ? string.Empty : aRequest.ProductName;
                    if (!string.IsNullOrEmpty(branchNameTo))
                    {
                        if (letterType.Equals((int)LetterType.HALO))
                        {
                            branchNameTo = (language.Equals((int)LanguageType.EN)) ? string.Format(branchNameHALO_EN, branchNameTo) : string.Format(branchNameHALO_ID, branchNameTo);
                        }
                        else
                        {
                            branchNameTo = string.Format(branchNameSPC_ID, branchNameTo);
                        }
                    }

                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Open(fullpath, true))
                    {
                        var body = wordDocument.MainDocumentPart.Document.Body;
                        var paras = body.Elements<Paragraph>();
                        List<Paragraph> pList = paras.ToList();
                        RunProperties rPr = null;
                        int index = 0;

                        foreach (Paragraph paragraph in pList)
                        {
                            string contentParagraph = paragraph.InnerText;
                            if (!string.IsNullOrEmpty(contentParagraph))
                            {
                                /*Get proper RunProperties*/
                                if (rPr == null)
                                {
                                    var tempRun = paragraph.Elements<Run>().ToList().Where(x => x.RunProperties != null).Select(x => x.RunProperties).FirstOrDefault();
                                    if (tempRun != null) rPr = new RunProperties(tempRun.OuterXml);
                                }

                                if (contentParagraph.Contains("|"))
                                {
                                    if (isAttachDebit) {
                                        GetDebitMerchant(letterType, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value);
                                    }

                                    if (contentParagraph.Trim().ToUpper().Equals(csAttachment.Trim().ToUpper()))
                                    {
                                        if (isAttachATM | isAttachATMSwitching | isAttachCirrus | isAttachDebit | isAttachMaestro)
                                        {
                                            int currentPosition = index;
                                            paragraph.RemoveAllChildren<Run>();

                                            if (letterType.Equals((int)LetterType.HALO))
                                            {
                                                Run run = new Run();
                                                run.AppendChild<RunProperties>(rPr);
                                                run.AppendChild<Text>(new Text(language.Equals((int)LanguageType.EN) ? perihal_EN : perihal_ID));
                                                paragraph.AppendChild<Run>(run);

                                                currentPosition += 2;
                                            }

                                            if (isAttachATM)
                                            {
                                                Table tblATM = CreateAttachment(rPr, letterType, AttachmentType.ATM, language, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value, transactionAmountTo);

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Header */
                                                    body.InsertAt(this.CreateTableTitle("ATM", rPr.OuterXml), currentPosition);
                                                    currentPosition += 1;
                                                }

                                                /* Create Table Content */
                                                body.InsertAt(new Paragraph(new Run(tblATM)), currentPosition);
                                                currentPosition += 1;
                                            }

                                            if (isAttachDebit)
                                            {
                                                Table tblDebit = CreateAttachment(rPr, letterType, AttachmentType.Debit, language, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value, transactionAmountTo);

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Header */
                                                    body.InsertAt(this.CreateTableTitle("Debit", rPr.OuterXml), currentPosition);
                                                    currentPosition += 1;
                                                }

                                                /* Create Table Content */
                                                body.InsertAt(new Paragraph(new Run(tblDebit)), currentPosition);
                                                currentPosition += 1;
                                            }

                                            if (isAttachCirrus)
                                            {
                                                Table tblCirrus = CreateAttachment(rPr, letterType, AttachmentType.Cirrus, language, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value, transactionAmountTo);

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Header */
                                                    body.InsertAt(this.CreateTableTitle("Cirrus", rPr.OuterXml), currentPosition);
                                                    currentPosition += 1;
                                                }

                                                /* Create Table Content */
                                                body.InsertAt(new Paragraph(new Run(tblCirrus)), currentPosition);
                                                currentPosition += 1;

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Footer */
                                                    body.InsertAt(new Paragraph(new Run(new Text(language.Equals((int)LanguageType.EN) ? noteForCirrus_EN : noteForCirrus_ID))), currentPosition);
                                                    currentPosition += 1;
                                                }
                                            }

                                            if (isAttachMaestro)
                                            {
                                                Table tblMaestro = CreateAttachment(rPr, letterType, AttachmentType.Maestro, language, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value, transactionAmountTo);

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Header */
                                                    body.InsertAt(this.CreateTableTitle("Maestro", rPr.OuterXml), currentPosition);
                                                    currentPosition += 1;
                                                }

                                                /* Create Table Content */
                                                body.InsertAt(new Paragraph(new Run(tblMaestro)), currentPosition);
                                                currentPosition += 1;

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Footer */
                                                    body.InsertAt(new Paragraph(new Run(new Text(language.Equals((int)LanguageType.EN) ? noteForCirrus_EN : noteForCirrus_ID))), currentPosition);
                                                    currentPosition += 1;
                                                }
                                            }

                                            if (isAttachATMSwitching)
                                            {
                                                Table tblATMSwitching = CreateAttachment(rPr, letterType, AttachmentType.ATMSwitching, language, cardNoTo, System.IO.Path.GetFileNameWithoutExtension(fullpath), dtTransactionTime, aLetterEntry.AttchDuration.Value, transactionAmountTo);

                                                if (letterType.Equals((int)LetterType.HALO))
                                                {
                                                    /* Create Table Header */
                                                    body.InsertAt(this.CreateTableTitle("ATM Switching", rPr.OuterXml), currentPosition);
                                                    currentPosition += 1;
                                                }

                                                /* Create Table Content */
                                                body.InsertAt(new Paragraph(new Run(tblATMSwitching)), currentPosition);
                                            }
                                        }
                                        else
                                        {
                                            paragraph.Remove();
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        string contentText = string.Empty;
                                        var runElements = paragraph.Elements<Run>();
                                        List<Run> rList = runElements.ToList();
                                        foreach (Run run in rList)
                                        {
                                            var textElements = run.Elements<Text>();
                                            List<Text> tList = textElements.ToList();
                                            foreach (Text text in tList)
                                            {
                                                if (!string.IsNullOrEmpty(text.Text) && text.Text.Trim().Contains("|"))
                                                {
                                                    contentText += text.Text;
                                                    int charCount = contentText.Count(x => x == '|');
                                                    if (charCount % 2 == 0)
                                                    {
                                                        contentText = contentText.Replace(letterNoFrom, letterNoTo);
                                                        contentText = contentText.Replace(letterDateFrom, letterDateTo);
                                                        contentText = contentText.Replace(yearNumFrom, yearNumTo);
                                                        contentText = contentText.Replace(description1From, description1To);
                                                        contentText = contentText.Replace(description2From, description2To);
                                                        contentText = contentText.Replace(description3From, description3To);
                                                        contentText = contentText.Replace(description4From, description4To);
                                                        contentText = contentText.Replace(yearFrom, yearTo);
                                                        contentText = contentText.Replace(branchNameFrom, branchNameTo);
                                                        contentText = contentText.Replace(requestDateCreatedFrom, requestDateCreatedTo);
                                                        contentText = contentText.Replace(letterNoteFrom, letterNoteTo);
                                                        contentText = contentText.Replace(attachmentB4From, attachmentB4To);
                                                        contentText = contentText.Replace(transactionDateTimeFrom, transactionDateTimeTo);
                                                        contentText = contentText.Replace(transactionDateFrom, transactionDateTo);
                                                        contentText = contentText.Replace(transactionTimeFrom, transactionTimeTo);
                                                        contentText = contentText.Replace(WSIDKeyFrom, WSIDKeyTo);
                                                        contentText = contentText.Replace(transactionAmountFrom, transactionAmountTo);
                                                        contentText = contentText.Replace(pKeyFrom, pKeyTo);
                                                        contentText = contentText.Replace(yearFrom, yearTo);
                                                        contentText = contentText.Replace(address1From, address1To);
                                                        contentText = contentText.Replace(address2From, address2To);
                                                        contentText = contentText.Replace(address3From, address3To);
                                                        contentText = contentText.Replace(cityFrom, cityTo);
                                                        contentText = contentText.Replace(zipCodeFrom, zipCodeTo);
                                                        contentText = contentText.Replace(customerFullNameFrom, customerFullNameTo);
                                                        contentText = contentText.Replace(cardNoFrom, cardNoTo);
                                                        contentText = contentText.Replace(accountNoFrom, accountNoTo);
                                                        contentText = contentText.Replace(contactMethodFrom, contactMethodTo);
                                                        contentText = contentText.Replace(productDescFrom, productDescTo);
                                                        contentText = contentText.Replace(merchantAccountNameFrom, merchantAccountNameTo);
                                                        contentText = contentText.Replace(merchantAccountNoFrom, merchantAccountNoTo);
                                                        contentText = contentText.Replace(merchantAddress1From, merchantAddress1To);
                                                        contentText = contentText.Replace(merchantAddress2From, merchantAddress2To);
                                                        contentText = contentText.Replace(merchantNameFrom, merchantNameTo);
                                                        contentText = contentText.Replace(merchantNumLineFrom, merchantNumLineTo);
                                                        contentText = contentText.Replace(merchantZIPFrom, merchantZIPTo);

                                                        text.Text = contentText;
                                                        contentText = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        text.Remove();
                                                    }
                                                }
                                                else if (contentText.Contains("|"))
                                                {
                                                    contentText += text.Text;
                                                    text.Remove();
                                                }
                                                else
                                                {
                                                    contentText = string.Empty;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            index++;
                        }
                    }

                    Annotation oAnnotation = db.annotation.Where(x => x.ObjectID == aLetterEntry.LetterEntryID).FirstOrDefault();
                    bool isCreate = oAnnotation == null ? true : false;

                    if (isCreate)
                    {
                        oAnnotation = new Annotation();
                        oAnnotation.AnnotationID = Guid.NewGuid();
                    }

                    // Open a file and read the contents into a byte array.
                    System.IO.FileStream aFileStream = new System.IO.FileStream(fullpath, System.IO.FileMode.OpenOrCreate);
                    byte[] byteData = new byte[aFileStream.Length];
                    aFileStream.Read(byteData, 0, byteData.Length);
                    aFileStream.Close();

                    // Encode the data using base64.
                    string encodedData = System.Convert.ToBase64String(byteData);

                    // Create Annotation and save contents of document
                    oAnnotation.ObjectID = aLetterEntry.LetterEntryID;
                    oAnnotation.ObjectTypeCode = int.Parse(objectTypeCode);
                    oAnnotation.MimeType = aAnnotation.MimeType;
                    oAnnotation.IsDocument = true;
                    oAnnotation.Subject = subject;
                    oAnnotation.DocumentBody = encodedData;
                    oAnnotation.FileSize = byteData.Length;
                    oAnnotation.FileName = System.IO.Path.GetFileName(fullpath);
                    oAnnotation.BusinessUnitID = aLetterEntry.BusinessUnitID;
                    oAnnotation.ModifiedByID = aLetterEntry.ModifiedBy;
                    oAnnotation.NoteText = noteText;

                    result = (isCreate) ? SubmitFile(oAnnotation, ActionType.Create) : SubmitFile(oAnnotation, ActionType.Edit);
                }
            }
            catch (Exception e)
            {
                result.Value = false;
                result.Response = e.Message;
            }
            finally
            {
                if (!string.IsNullOrEmpty(fullpath) && System.IO.File.Exists(fullpath)) System.IO.File.Delete(fullpath);
            }

            return result;

        }

        public JSONResponse CreateHeader(RunProperties runProp, int letterType, int languageType, bool isAttachDebit)
        {
            JSONResponse result = new JSONResponse();
            Table aTable = new Table();

            //============= Create Table Properties
            TableBorders aTblBorders = new TableBorders()
            {
                TopBorder = new TopBorder() { Val = BorderValues.Single, Size = 8 },
                BottomBorder = new BottomBorder() { Val = BorderValues.Single, Size = 8 },
                LeftBorder = new LeftBorder() { Val = BorderValues.Single, Size = 8 },
                RightBorder = new RightBorder() { Val = BorderValues.Single, Size = 8 },
                InsideHorizontalBorder = new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 8 },
                InsideVerticalBorder = new InsideVerticalBorder() { Val = BorderValues.Single, Size = 8 }
            };
            TableProperties aTblProp = new TableProperties()
            {
                TableBorders = aTblBorders,
                TableIndentation = new TableIndentation() { Width = 108 }
            };
            aTable.AppendChild<TableProperties>(aTblProp);

            /* Configure Columns Header */
            List<string> columnHeaders = new List<string>();
            List<string> columnWidth;
            if (letterType.Equals((int)LetterType.SPC) && isAttachDebit) //============ SPC & DEBIT
            {
                columnHeaders.Add(headerTanggal_ID);
                columnHeaders.Add(headerJam_ID);
                columnHeaders.Add(headerTerminal_ID);
                columnHeaders.Add(headerLokasi_ID);
                columnHeaders.Add(headerNominal_ID);
                columnHeaders.Add(headerTunai_ID);
                columnHeaders.Add(headerKeterangan_ID);

                columnWidth = new List<string>() { "1000", "700", "1000", "2300", "1150", "1150", "1000" };
            }
            else //========== OTHERS
            {
                columnHeaders.Add(headerLineNum);
                columnHeaders.Add(letterType.Equals((int)LetterType.SPC) ? headerDateTimeTrx_ID : (languageType.Equals((int)LanguageType.EN) ? headerDateTrx_EN : headerDateTrx_ID));
                columnHeaders.Add(languageType.Equals(2) ? headerLokasiTrx_EN : headerLokasiTrx_ID);
                columnHeaders.Add(languageType.Equals(2) ? headerTipeTrx_EN : headerTipeTrx_ID);
                columnHeaders.Add(languageType.Equals(2) ? headerNominal_EN : headerNominal_ID);
                columnHeaders.Add(languageType.Equals(2) ? headerKeterangan_EN : headerKeterangan_ID);

                columnWidth = new List<string>() { "400", "1400", "2400", "1700", "1400", "1000" };
            }

            /*Create Header Attachment*/
            TableRow aTblRow = new TableRow();
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                TableCell aTblCell = this.CreateTableCell(columnHeaders.ElementAt(i), true, runProp.OuterXml, columnWidth.ElementAt(i));
                aTblRow.AppendChild<TableCell>(aTblCell);
            }
            aTable.Append(aTblRow);

            result.Value = aTable;
            result.Data = columnWidth.ToArray();

            return result;
        }

        public Paragraph CreateTableTitle(string content, string outerXML)
        {
            RunProperties rPr = new RunProperties(outerXML);
            Run run = new Run();
            run.AppendChild<RunProperties>(rPr);
            run.AppendChild<Text>(new Text(content));
            Paragraph paragraph = new Paragraph();
            paragraph.AppendChild<ParagraphProperties>(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }));
            paragraph.AppendChild<Run>(run);

            return paragraph;
        }

        public Table CreateAttachment(RunProperties runProp,
                                        int letterType,
                                        AttachmentType attachType,
                                        int languageType,
                                        string cardNo,
                                        string docName,
                                        DateTime? trxDate,
                                        int attachDuration,
                                        string requestAmount)
        {
            Table aTable = null;

            try
            {
                cardNo = cardNo.Replace(constDash, string.Empty);

                /*Create Header Attachment*/
                bool isAttachDebit = attachType.Equals(AttachmentType.Debit) ? true : false;
                JSONResponse header = this.CreateHeader(runProp, letterType, languageType, isAttachDebit);
                aTable = (Table)header.Value;
                List<string> columnWidth = ((string[])header.Data).ToList();

                /*Create Body Attachment*/
                int line = 0;
                switch (attachType)
                {
                    case AttachmentType.ATM:
                        {
                            //IEnumerable<ATMTransaction> tempList = htdb.LetterEntry_GetATMHistory(trxDate, cardNo, attachDuration, letterType);

                            List<ATMTransaction> atmList = htdb.LetterEntry_GetATMHistory(trxDate, cardNo, attachDuration, letterType).Cast<ATMTransaction>().ToList();
                            foreach (ATMTransaction aATM in atmList)
                            {
                                line++;
                                TableRow aTblRow = new TableRow();
                                TableCell aTblCell = this.CreateTableCell(line.ToString(), false, runProp.OuterXml, columnWidth.ElementAt(0));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTblCell = this.CreateTableCell(aATM.DateTime.HasValue ? aATM.DateTime.Value.ToString(letterType.Equals((int)LetterType.SPC) ? "dd/MM/yy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*Cari WSID yang field Location nya sama dengan field Terminal di ATMTransaction*/
                                var aWSID = db.wsid.Where(x => x.Location == aATM.Terminal).FirstOrDefault();

                                /*Ambil field Lokasi dari WSID yang ditemukan, sebaliknya kosongkan*/
                                string strBCALoc = aWSID == null ? string.Empty : (string.IsNullOrEmpty(aWSID.Lok) ? string.Empty : aWSID.Lok);

                                /*Ambil field Name dari WSID yang ditemukan, sebaliknya kosongkan*/
                                string strBCAName = aWSID == null ? string.Empty : (string.IsNullOrEmpty(aWSID.Name) ? string.Empty : aWSID.Name);

                                /*Jika field Location di ATMTransaction kosong atau null, maka:
                                 * 1. Jika field Lokasi dari WSID sama dengan "BCA", hasilnya adalah penggabungan antara field Lokasi dan Name.
                                 * 2. Selain itu, hasilnya adalah field Name dari WSID */
                                string location = string.IsNullOrEmpty(aATM.Location) ? string.Empty : (strBCALoc.Trim().ToUpper().Equals("BCA") ? strBCALoc.Trim().ToUpper() + constSpace + strBCAName.Trim() : strBCAName.Trim());

                                aTblCell = this.CreateTableCell(location, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Aturan:
                                 * 1. Jika transaksi berupa TRANSFER, maka TransactionDescription ditambahkan dengan field ToAccount. Contoh: "TRANSFER 9876543210".
                                 * 2. Jika transaksi berupa PEMBAYARAN atau PEMBELIAN, maka cek dulu ke field ProductName.
                                 * 2a.  Jika field ProductName kosong atau null, maka TransactionDescription langsung ditambahkan field ToAccount.
                                 * 2b.  Sebaliknya, ambil ProductName yang dipisahkan dengan separator "/".
                                 *      Contoh: "KARTU KREDIT/DEUTSCHE BANK ( GE )" menjadi "DEUTSCHE BANK ( GE )"
                                 *      Kemudian TransactionDescription ditambahkan dengan ProductName dan ToAccount.
                                 */
                                string trxDesc = string.IsNullOrEmpty(aATM.TransactionDescription) ? string.Empty : aATM.TransactionDescription;
                                // 2016-02-18 (william minta tambah "SETORAN TUNAI")
                                if (trxDesc.ToUpper().Contains("TRANSFER") || trxDesc.ToUpper().Contains("SETORAN TUNAI"))
                                {
                                    trxDesc = trxDesc + constSpace + aATM.ToAccount;
                                }
                                else if (trxDesc.ToUpper().Contains("PEMBAYARAN") | trxDesc.ToUpper().Contains("PEMBELIAN"))
                                {
                                    string productName = string.Empty;
                                    if (!string.IsNullOrEmpty(aATM.Company))
                                    {
                                        AccountPayment accountPayment = db.accountPayment.Where(x => x.KodePerusahaan.Equals(aATM.Company)).FirstOrDefault();
                                        if (accountPayment != null)
                                        {
                                            productName = accountPayment.JenisPembayaran;
                                        }
                                    }
                                    trxDesc = string.IsNullOrEmpty(productName) ? (trxDesc + constSpace + aATM.ToAccount) : (trxDesc + constSpace + productName.Substring(aATM.ProductName.IndexOf(constSlash) + 1) + constSpace + aATM.ToAccount);
                                }

                                aTblCell = this.CreateTableCell(trxDesc, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTblCell = this.CreateTableCell(aATM.Amount.Substring(0, aATM.Amount.IndexOf(constDelimeter)), false, runProp.OuterXml, columnWidth.ElementAt(4));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Aturan:
                                 * Jika ResponseDescription tidak kosong, maka hasilnya adalah gabungan dari Response + ", " + ResponseDescription
                                 * Selain itu hasilnya adalah Response
                                 */
                                aTblCell = this.CreateTableCell(string.IsNullOrEmpty(aATM.ResponseDescription) ? aATM.Response : aATM.Response + constDelimeter + constSpace + aATM.ResponseDescription, false, runProp.OuterXml, columnWidth.ElementAt(5));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTable.Append(aTblRow);
                            }
                        }
                        break;

                    case AttachmentType.ATMSwitching:
                        {
                            List<ATMSwitchingTransaction> atmsList = htdb.LetterEntry_GetATMSwitchingHistory(trxDate, cardNo, attachDuration).Cast<ATMSwitchingTransaction>().ToList();
                            foreach (ATMSwitchingTransaction aSwitching in atmsList)
                            {
                                line++;
                                TableRow aTblRow = new TableRow();
                                TableCell aTblCell = this.CreateTableCell(line.ToString(), false, runProp.OuterXml, columnWidth.ElementAt(0));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Jika LetterType nya SPC maka formatnya "dd/MM/yy HH:mm:ss"
                                 * Jika LetterType nya HALO maka formatnya "dd-MM-yyyy HH:mm:ss"
                                 */
                                aTblCell = this.CreateTableCell(aSwitching.TransactionDate.HasValue ? aSwitching.TransactionDate.Value.ToString(letterType.Equals((int)LetterType.SPC) ? "dd/MM/yy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                aTblCell = this.CreateTableCell(aSwitching.Location, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTblCell = this.CreateTableCell(aSwitching.TransactionCode, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Selalu ambil AmountIDR
                                 * Ambil nominal tanpa koma. Contoh: "100.000,00" maka nominal yang diambil adalah "100.000" 
                                 */
                                aTblCell = this.CreateTableCell(aSwitching.AmountIDR, false, runProp.OuterXml, columnWidth.ElementAt(4));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Aturan:
                                 * 1. Jika field Reversal sama dengan "R", maka hasilnya adalah label "Reversal"
                                 * 2. Jika tidak, cek apakah field Remark null atau tidak. Jika ya, maka hasilnya adalah gabungan dari field Response dan Remark
                                 * 3. Selain itu, hasilnya adalah field Response
                                 */
                                string response = !string.IsNullOrEmpty(aSwitching.Reversal) ? aSwitching.Reversal.Trim() : (string.IsNullOrEmpty(aSwitching.Response) ? string.Empty : (string.IsNullOrEmpty(aSwitching.Reason) ? aSwitching.Response : aSwitching.Response + constDelimeter + constSpace + aSwitching.Reason));
                                aTblCell = this.CreateTableCell(response, false, runProp.OuterXml, columnWidth.ElementAt(5));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTable.Append(aTblRow);
                            }
                        }
                        break;

                    case AttachmentType.Cirrus:
                        {
                            List<CirrusTransaction> cirrusList = htdb.LetterEntry_GetCirrusHistory(trxDate, cardNo, attachDuration).Cast<CirrusTransaction>().ToList();
                            foreach (CirrusTransaction aCirrus in cirrusList)
                            {
                                line++;
                                TableRow aTblRow = new TableRow();
                                TableCell aTblCell = this.CreateTableCell(line.ToString(), false, runProp.OuterXml, columnWidth.ElementAt(0));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Jika LetterType nya SPC maka formatnya "dd/MM/yy HH:mm:ss"
                                 * Jika LetterType nya HALO maka formatnya "dd-MM-yyyy HH:mm:ss"
                                 */
                                aTblCell = this.CreateTableCell(aCirrus.TransactionDate.HasValue ? aCirrus.TransactionDate.Value.ToString(letterType.Equals((int)LetterType.SPC) ? "dd/MM/yy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                aTblCell = this.CreateTableCell(aCirrus.Location, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTblCell = this.CreateTableCell(aCirrus.Transaction, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Selalu ambil AmountIDR
                                 * Ambil nominal tanpa koma. Contoh: "100.000,00" maka nominal yang diambil adalah "100.000" 
                                 */
                                aTblCell = this.CreateTableCell(aCirrus.AmountIDR, false, runProp.OuterXml, columnWidth.ElementAt(4));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Aturan:
                                 * 1. Jika field Reversal sama dengan "R", maka hasilnya adalah label "Reversal"
                                 * 2. Jika tidak, cek apakah field Remark null atau tidak. Jika ya, maka hasilnya adalah gabungan dari field Response dan Remark
                                 * 3. Selain itu, hasilnya adalah field Response
                                 */
                                string response = !string.IsNullOrEmpty(aCirrus.Reversal) ? aCirrus.Reversal.Trim() : (string.IsNullOrEmpty(aCirrus.Response) ? string.Empty : (string.IsNullOrEmpty(aCirrus.Remark) ? aCirrus.Response : aCirrus.Response + constDelimeter + constSpace + aCirrus.Remark));
                                aTblCell = this.CreateTableCell(response, false, runProp.OuterXml, columnWidth.ElementAt(5));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTable.Append(aTblRow);
                            }
                        }
                        break;

                    case AttachmentType.Maestro:
                        {
                            List<CirrusTransaction> cirrusList = htdb.LetterEntry_GetCirrusHistory(trxDate, cardNo, attachDuration).Cast<CirrusTransaction>().ToList();
                            foreach (CirrusTransaction aCirrus in cirrusList)
                            {
                                line++;
                                TableRow aTblRow = new TableRow();
                                TableCell aTblCell = this.CreateTableCell(line.ToString(), false, runProp.OuterXml, columnWidth.ElementAt(0));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Jika LetterType nya SPC maka formatnya "dd/MM/yy HH:mm:ss"
                                 * Jika LetterType nya HALO maka formatnya "dd-MM-yyyy HH:mm:ss"
                                 */
                                aTblCell = this.CreateTableCell(aCirrus.TransactionDate.HasValue ? aCirrus.TransactionDate.Value.ToString(letterType.Equals((int)LetterType.SPC) ? "dd/MM/yy HH:mm:ss" : "dd-MM-yyyy HH:mm:ss") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                aTblCell = this.CreateTableCell(aCirrus.Location, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTblCell = this.CreateTableCell(aCirrus.Transaction, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Selalu ambil AmountIDR
                                 * Ambil nominal tanpa koma. Contoh: "100.000,00" maka nominal yang diambil adalah "100.000" 
                                 */
                                aTblCell = this.CreateTableCell(aCirrus.AmountIDR, false, runProp.OuterXml, columnWidth.ElementAt(4));
                                aTblRow.AppendChild<TableCell>(aTblCell);

                                /*
                                 * Aturan:
                                 * 1. Jika field Reversal sama dengan "R", maka hasilnya adalah label "Reversal"
                                 * 2. Jika tidak, cek apakah field Remark null atau tidak. Jika ya, maka hasilnya adalah gabungan dari field Response dan Remark
                                 * 3. Selain itu, hasilnya adalah field Response
                                 */
                                string response = !string.IsNullOrEmpty(aCirrus.Reversal) ? aCirrus.Reversal.Trim() : (string.IsNullOrEmpty(aCirrus.Response) ? string.Empty : (string.IsNullOrEmpty(aCirrus.Remark) ? aCirrus.Response : aCirrus.Response + constDelimeter + constSpace + aCirrus.Remark));
                                aTblCell = this.CreateTableCell(response, false, runProp.OuterXml, columnWidth.ElementAt(5));
                                aTblRow.AppendChild<TableCell>(aTblCell);
                                aTable.Append(aTblRow);
                            }
                        }
                        break;

                    case AttachmentType.Debit:
                        {
                            List<DebitTransaction> debitList = htdb.LetterEntry_GetDebitHistory(trxDate, cardNo, attachDuration, letterType, docName).Cast<DebitTransaction>().ToList();
                            
                            int count = debitList.Count + 1;
                            

                            /*Ambil data TerminalId dari hasil query*/
                            string terminalID = debitList.Select(x => x.TerminalId).FirstOrDefault();

                            /*Gunakan TerminalId tersebut untuk mendapatkan informasi Merchant*/
                            Params param = new Params() { Parameter = new Dictionary<string, string>() };
                            param.RequestTransType = "GetMerchantInfoByTerminalId";
                            param.Parameter.Add("merchantId", terminalID);
                            Merchant aMerchant = MerchantInquiry.Search(param);
                            merchantNumLineTo = (count > 10 || count < 0) ? Convert.ToString(count - 1) : string.Format("{0} ({1})", count - 1, arrNumInd[count - 1]);

                            /* Temuan SPC, 01 11 2016, Wrong Field, SWAP! */ 
                            // merchantAccountNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Name) ? string.Empty : aMerchant.Name.Trim());
                            // merchantNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.MerchantName) ? string.Empty : aMerchant.MerchantName.Trim());
                            merchantAccountNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.MerchantName) ? string.Empty : aMerchant.MerchantName.Trim());
                            merchantNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Name) ? string.Empty : aMerchant.Name.Trim());

                            /* Temuan SPC, 01 11 2016, User Number */ 
                            // merchantAccountNoTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Account) ? string.Empty : aMerchant.Account.Trim());
                            merchantAccountNoTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.UserNumber) ? string.Empty : aMerchant.UserNumber.Trim());

                            /* Temuan SPC, 01 11 2016, Adress Null */ 
                            merchantAddress1To = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Address1) ? string.Empty : aMerchant.Address1.Trim());
                            merchantAddress2To = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Address2) ? string.Empty : aMerchant.Address2.Trim());
                            // merchantAddress1To = aMerchant == null ? string.Empty : aMerchant.Address1.Trim() + " " + aMerchant.Address2.Trim();
                            // merchantAddress2To = aMerchant == null ? string.Empty : aMerchant.Address3.Trim() + " " + aMerchant.Address4.Trim();
                            
                            merchantZIPTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.ZipCode) ? string.Empty : aMerchant.ZipCode.Trim());


                            foreach (DebitTransaction aDebit in debitList)
                            {
                                line++;
                                TableRow aTblRow = new TableRow();
                                if (letterType.Equals((int)LetterType.SPC)) /*==SPC==*/
                                {
                                    TableCell aTblCell = this.CreateTableCell(aDebit.DateTime.HasValue ? aDebit.DateTime.Value.ToString("dd/MM/yy") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(0));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(aDebit.DateTime.HasValue ? aDebit.DateTime.Value.ToString("HH:mm") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(aDebit.TerminalId, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(merchantNameTo, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                    aTblRow.AppendChild<TableCell>(aTblCell);

                                    /* Temuan SPC, 31 10 2016 Amount on Debit Wiothout ,00 */
                                    // aTblCell = this.CreateTableCell(aDebit.Amount.Substring(0, aDebit.Amount.IndexOf(constDelimeter)), false, runProp.OuterXml, columnWidth.ElementAt(4));
                                    aTblCell = this.CreateTableCell(aDebit.Amount, false, runProp.OuterXml, columnWidth.ElementAt(4));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    
                                    aTblCell = this.CreateTableCell(aDebit.Cash.Substring(0, aDebit.Cash.IndexOf(constDelimeter)), false, runProp.OuterXml, columnWidth.ElementAt(5));
                                    aTblRow.AppendChild<TableCell>(aTblCell);

                                    /*
                                     * Selalu "BERHASIL"
                                     */
                                    aTblCell = this.CreateTableCell("BERHASIL", false, runProp.OuterXml, columnWidth.ElementAt(6));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                }
                                else /*==HALO==*/
                                {
                                    TableCell aTblCell = this.CreateTableCell(line.ToString(), false, runProp.OuterXml, columnWidth.ElementAt(0));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(aDebit.DateTime.HasValue ? aDebit.DateTime.Value.ToString("dd-MM-yyyy HH:mm:ss") : string.Empty, false, runProp.OuterXml, columnWidth.ElementAt(1));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(merchantNameTo, false, runProp.OuterXml, columnWidth.ElementAt(2));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                    aTblCell = this.CreateTableCell(transaksiDebit_ID, false, runProp.OuterXml, columnWidth.ElementAt(3));
                                    aTblRow.AppendChild<TableCell>(aTblCell);

                                    // aTblCell = this.CreateTableCell(aDebit.Amount.Substring(0, aDebit.Amount.IndexOf(constDelimeter)), false, runProp.OuterXml, columnWidth.ElementAt(4));
                                    aTblCell = this.CreateTableCell(aDebit.Amount, false, runProp.OuterXml, columnWidth.ElementAt(4));
                                    
                                    aTblRow.AppendChild<TableCell>(aTblCell);

                                    /* Jika field Remark tidak null, maka hasilnya field Response ditambahkan dengan field Remark.
                                     * Selain itu, hasilnya adalah field Response
                                     */
                                    string response = string.IsNullOrEmpty(aDebit.Response) ? string.Empty : (string.IsNullOrEmpty(aDebit.Remark) ? aDebit.Response : aDebit.Response + constDelimeter + constSpace + aDebit.Remark);
                                    aTblCell = this.CreateTableCell(response, false, runProp.OuterXml, columnWidth.ElementAt(5));
                                    aTblRow.AppendChild<TableCell>(aTblCell);
                                }
                                aTable.Append(aTblRow);
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
                log.Info(e.StackTrace);
            }

            return aTable;
        }

        public TableCell CreateTableCell(string strValue, bool isHeader, string outerXML, string columnWidth)
        {
            TableCell aTblCell = new TableCell();
            aTblCell.AppendChild<TableCellProperties>(new TableCellProperties(new TableCellWidth() { Width = columnWidth, Type = TableWidthUnitValues.Dxa }, new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center }));
            RunProperties rPr = new RunProperties(outerXML);
            rPr.Append(new Bold() { Val = isHeader });
            Run run = new Run();
            run.AppendChild<RunProperties>(rPr);
            run.AppendChild<Text>(new Text(strValue));
            Paragraph paragraph = new Paragraph();
            paragraph.AppendChild<ParagraphProperties>(new ParagraphProperties(new Justification() { Val = JustificationValues.Center }));
            paragraph.AppendChild<Run>(run);
            aTblCell.AppendChild<Paragraph>(paragraph);

            return aTblCell;
        }

        public JSONResponse SubmitFile(Annotation model, ActionType actionType)
        {
            JSONResponse result = new JSONResponse();

            try
            {
                KeyValuePair<int, string> results = new KeyValuePair<int, string>(1, string.Empty);
                switch (actionType)
                {
                    case ActionType.Create:
                        results = db.Annotation_Insert(model);
                        break;
                    case ActionType.Edit:
                        results = db.Annotation_Update(model);
                        break;
                    default:
                        break;
                }

                if (results.Key == 0 || results.Key == 16 || results.Key == 6)
                {
                    result.Value = true;
                    result.Response = Resources.LetterTemplate.Alert_Success;
                }
                else throw new Exception(results.Value.ToString());
            }
            catch (Exception e)
            {
                result.Value = false;
                result.Response = e.Message;
            }

            return result;
        }

        private void GetDebitMerchant(int letterType, string cardNo, string docName, DateTime? trxDate, int attachDuration)
        {
            List<DebitTransaction> debitList = htdb.LetterEntry_GetDebitHistory(trxDate, cardNo, attachDuration, letterType, docName).Cast<DebitTransaction>().ToList();
            string terminalID = debitList.Select(x => x.TerminalId).FirstOrDefault();
            Params param = new Params() { Parameter = new Dictionary<string, string>() };
            param.Parameter.Add("merchantId", terminalID);
            Merchant aMerchant = MerchantInquiry.Search(param);
            merchantAccountNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.MerchantName) ? string.Empty : aMerchant.MerchantName.Trim());
            merchantNameTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Name) ? string.Empty : aMerchant.Name.Trim());
            merchantAccountNoTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.UserNumber) ? string.Empty : aMerchant.UserNumber.Trim());
            merchantAddress1To = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Address1) ? string.Empty : aMerchant.Address1.Trim());
            merchantAddress2To = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.Address2) ? string.Empty : aMerchant.Address2.Trim());
            merchantZIPTo = aMerchant == null ? string.Empty : (string.IsNullOrEmpty(aMerchant.ZipCode) ? string.Empty : aMerchant.ZipCode.Trim());
        }
    }
}