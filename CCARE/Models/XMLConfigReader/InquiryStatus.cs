using System;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Xml.Linq;
using System.Collections.Generic;
using CCARE.Models.General;

namespace CCARE.Models
{
    public class InquiryStatus
    {
        public static XDocument LoadXML()
        {
            XDocument xdoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/App_Data/Configuration.xml"));
            return xdoc;
        }

        public static string getPrevStatusLabel(string val)
        {
            try
            {
                XDocument root = LoadXML();

                var currentstatus = (from a in root.Descendants("InquiryStatus").Descendants("ebanking").Descendants("currentstatus")
                                     where (string)a.Attribute("value") == val
                                     select a.Attribute("label").Value).First();



                return currentstatus;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static string getNewStatusLabel(string val)
        {
            try
            {
                XDocument root = LoadXML();

                var newstatus = (from a in root.Descendants("InquiryStatus").Descendants("ebanking").Descendants("currentstatus").Descendants("newstatus")
                                     where (string)a.Attribute("value") == val
                                     select a.Attribute("label").Value).First();



                return newstatus;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<BaseAttribute> GetTypesName()
        {
            try
            {
                XDocument root = LoadXML();

                var inquirystatus = (from a in root.Descendants("InquiryStatus").Descendants("ebanking")
                                     select new BaseAttribute
                                     {
                                         Text = a.Attribute("label").Value,
                                         Value = a.Attribute("value").Value
                                     }).ToList();

                

                return inquirystatus;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<BaseAttribute> GetPreviousStatus(int parentVal)
        {
            try
            {
                XDocument root = LoadXML();

                var currentstatus = (from a in root.Descendants("InquiryStatus").Descendants("ebanking").Descendants("currentstatus")
                                where (int)a.Parent.Attribute("value") == parentVal
                                select new BaseAttribute
                                {
                                    Text = a.Attribute("label").Value,
                                    Value = a.Attribute("value").Value
                                }).ToList();

                return currentstatus;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<BaseAttribute> GetNewStatus(int ebankingVal, string currentstatusVal)
        {
            try
            {
                XDocument root = LoadXML();

                var currentstatus = (from a in root.Descendants("InquiryStatus").Descendants("ebanking").Descendants("currentstatus")
                                    where (int)a.Parent.Attribute("value") == ebankingVal
                                    select a);

                var newstatus = (from a in currentstatus.Descendants("newstatus")
                                    where (string)a.Parent.Attribute("value") == currentstatusVal
                                    select new BaseAttribute
                                    {
                                        Text = a.Attribute("label").Value,
                                        Value = a.Attribute("value").Value
                                    }).ToList();

                return newstatus;

            }
            catch (Exception e)
            {
                return null;
            }
        }

     }
}