using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data;
using System.Configuration;
using CCARE.Models.MasterData;
using CCARE.Models.Role;
using CCARE.Models.SalesMarketing;

namespace CCARE.Models.General
{
    public partial class CRMDb : DbContext
    {
        public CRMDb()
        {
            Database.SetInitializer<CRMDb>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = 30;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerAddress>().ToTable("CRM.CustomerAddress");
            modelBuilder.Entity<StatusMapper>().ToTable("CRM.StatusMapper");
            modelBuilder.Entity<Request>().ToTable("CRM.Request");
            modelBuilder.Entity<RequestAuditLog>().ToTable("CRM.Request_AuditLog");
            modelBuilder.Entity<Customer>().ToTable("CRM.Customer");
            modelBuilder.Entity<StringMap>().ToTable("CRM.StringMap");
            modelBuilder.Entity<RequestCreationMatrix>().ToTable("CRM.RequestCreationMatrix");
            modelBuilder.Entity<Category>().ToTable("CRM.Category");
            modelBuilder.Entity<Product>().ToTable("CRM.Product");
            modelBuilder.Entity<Branch>().ToTable("CRM.Branch");
            modelBuilder.Entity<WSID>().ToTable("CRM.WsId");
            modelBuilder.Entity<BusinessUnit>().ToTable("CRM.BusinessUnit");
            modelBuilder.Entity<ServiceLevel>().ToTable("CRM.ServiceLevel");
            modelBuilder.Entity<SecurityRole>().ToTable("CRM.SecurityRole");
            modelBuilder.Entity<AccountPayment>().ToTable("CRM.AccountPayment");
            modelBuilder.Entity<OrganizationClosure>().ToTable("CRM.OrganizationClosure");
            modelBuilder.Entity<LetterTemplate>().ToTable("CRM.LetterTemplate");
            modelBuilder.Entity<SystemUser>().ToTable("CRM.SystemUser");
            modelBuilder.Entity<Annotation>().ToTable("CRM.Annotation");
            modelBuilder.Entity<CallWrapUpCategory>().ToTable("CRM.CallWrapUpCategory");
            modelBuilder.Entity<Cause>().ToTable("CRM.Cause");
            modelBuilder.Entity<Queue>().ToTable("CRM.Queue");
            modelBuilder.Entity<QueueUser>().ToTable("CRM.QueueUser");
            modelBuilder.Entity<Mapping>().ToTable("CRM.MappingView");
            modelBuilder.Entity<LetterEntry>().ToTable("CRM.LetterEntry");
            modelBuilder.Entity<NonCustomer>().ToTable("CRM.NonCustomer");
            modelBuilder.Entity<QueueItem>().ToTable("CRM.QueueItem");
            modelBuilder.Entity<Task>().ToTable("CRM.Task");
            modelBuilder.Entity<CallWrapUp>().ToTable("CRM.CallWrapUp");
            modelBuilder.Entity<CallWrapUpCustomer>().ToTable("CRM.CallWrapUpCustomer");
            modelBuilder.Entity<CallWrapUpNonCustomer>().ToTable("CRM.CallWrapUpNonCustomer");
            modelBuilder.Entity<CallType>().ToTable("CRM.CallType");
            modelBuilder.Entity<OrganizationStructure>().ToTable("CRM.OrganizationStructure");
            modelBuilder.Entity<BroadcastMessageDetail>().ToTable("CRM.BroadcastMessageDetail");
            modelBuilder.Entity<Team>().ToTable("CRM.Team");
            modelBuilder.Entity<TeamMember>().ToTable("CRM.TeamMember");
            modelBuilder.Entity<PrivilegeException>().ToTable("CRM.PrivilegeException");
            modelBuilder.Entity<ProductTree>().ToTable("CRM.Product_Tree");
            modelBuilder.Entity<CategoryTree>().ToTable("CRM.Category_Tree");
            modelBuilder.Entity<StatusChange>().ToTable("CRM.StatusChange");
            modelBuilder.Entity<RequestResolution>().ToTable("CRM.RequestResolution");
            modelBuilder.Entity<MasterReport>().ToTable("CRM.MasterReport");
            modelBuilder.Entity<ReportRole>().ToTable("CRM.ReportRole");
            modelBuilder.Entity<ReportFilters>().ToTable("CRM.ReportFilters");
            
            /* Enhanced WorkFlow */
            modelBuilder.Entity<Workflow>().ToTable("CRM.ViewWorkflow");
            modelBuilder.Entity<Request_Workflow>().ToTable("CRM.ViewRequest_Workflow");
            modelBuilder.Entity<Kota>().ToTable("CRM.Kota");
            modelBuilder.Entity<Segmentation>().ToTable("CRM.Segmentation");
            
            /* NEW CRM Dynamic Report */
            modelBuilder.Entity<Entity>().ToTable("rpt.Entity");
            modelBuilder.Entity<EntityColumn>().ToTable("rpt.EntityColumn");
            modelBuilder.Entity<Report>().ToTable("rpt.Report");
            modelBuilder.Entity<ReportView>().ToTable("rpt.ReportView");
            modelBuilder.Entity<ReportFilter>().ToTable("rpt.ReportFilterView");
            modelBuilder.Entity<ReportColumn>().ToTable("rpt.ReportColumnView");
            modelBuilder.Entity<ReportRoleNew>().ToTable("rpt.ReportRole");
            modelBuilder.Entity<ReportRoleView>().ToTable("rpt.ReportRoleView");

            //New Sales and Marketing Module
            modelBuilder.Entity<Campaign>().ToTable("CRM.CampaignView");
            modelBuilder.Entity<CampaignCustomer>().ToTable("CRM.CampaignCustomerView");
            modelBuilder.Entity<CampaignLead>().ToTable("CRM.CampaignLeadView");
            modelBuilder.Entity<CampaignMarketingList>().ToTable("CRM.CampaignMarketingListView");
            modelBuilder.Entity<CampaignType>().ToTable("CRM.CampaignType");
            modelBuilder.Entity<Lead>().ToTable("CRM.LeadView");
            modelBuilder.Entity<MarketingList>().ToTable("CRM.MarketingListView");
            modelBuilder.Entity<MarketingListCustomer>().ToTable("CRM.MarketingListCustomerView");
            modelBuilder.Entity<MarketingListLead>().ToTable("CRM.MarketingListLeadView");
            //modelBuilder.Entity<MarketingListPurpose>().ToTable("CRM.MarketingListPurpose");
            modelBuilder.Entity<CampaignDetail>().ToTable("CRM.CampaignDetail");
            modelBuilder.Entity<MarketingListDetail>().ToTable("CRM.MarketingListDetail");
        }

        public DbSet<CustomerAddress> customeradress { get; set; }
        public DbSet<StatusMapper> statusmapper { get; set; }
        public DbSet<Request> request { get; set; }
        public DbSet<RequestAuditLog> requestauditlog { get; set; }
        public DbSet<StringMap> pickList { get; set; }
        public DbSet<RequestCreationMatrix> requestCreationMatrix { get; set; }
        public DbSet<Customer> customer { get; set; }
        public DbSet<Category> category { get; set; }
        public DbSet<Product> product { get; set; }
        public DbSet<Branch> branch { get; set; }
        public DbSet<WSID> wsid { get; set; }
        public DbSet<BusinessUnit> businessunit { get; set; }
        public DbSet<ServiceLevel> serviceLevel { get; set; }
        public DbSet<AccountPayment> accountPayment { get; set; }
        public DbSet<OrganizationClosure> organizationClosure { get; set; }
        public DbSet<LetterTemplate> letterTemplate { get; set; }
        public DbSet<SystemUser> systemUser { get; set; }
        public DbSet<Annotation> annotation { get; set; }
        public DbSet<CallWrapUpCategory> callWrapUpCategory { get; set; }
        public DbSet<Cause> cause { get; set; }
        public DbSet<Queue> queue { get; set; }
        public DbSet<QueueUser> queueuser { get; set; }
        public DbSet<LetterEntry> letterEntry { get; set; }
        public DbSet<NonCustomer> noncustomer { get; set; }
        public DbSet<Mapping> mapping { get; set; }
        public DbSet<SecurityRole> securityRole { get; set; }
        public DbSet<QueueItem> queueitem { get; set; }
        public DbSet<Task> task { get; set; }
        public DbSet<CallWrapUp> callwrapup { get; set; }
        public DbSet<CallWrapUpCustomer> callwrapupcustomer { get; set; }
        public DbSet<CallWrapUpNonCustomer> callwrapupnoncustomer { get; set; }
        public DbSet<CallType> calltype { get; set; }
        public DbSet<StatusMapperNewCode> statusMapperNewCode { get; set; }
        public DbSet<PrivilegeException> privilegeException { get; set; }
        public DbSet<BroadcastMessageDetail> broadcastMessageDetail { get; set; }
        public DbSet<Team> team { get; set; }
        public DbSet<TeamMember> teamMember { get; set; }
        public DbSet<OrganizationStructure> organizationStructure { get; set; }
        public DbSet<ProductTree> producttree { get; set; }
        public DbSet<CategoryTree> categorytree { get; set; }
        public DbSet<StatusChange> statuschange { get; set;} 
        public DbSet<RequestResolution> requestresolution { get; set; }
        public DbSet<MasterReport> masterReport { get; set; }
        public DbSet<ReportRole> reportRole { get; set; }
        public DbSet<ReportFilters> reportFilters { get; set; }

        /* Enhanced WorkFlow */
        public DbSet<Workflow> Workflow { get; set; }
        public DbSet<Request_Workflow> Request_Workflow { get; set; }
        public DbSet<Kota> Kota { get; set; }
        public DbSet<Segmentation> Segmentation { get; set; }
        
        /* NEW CRM Dynamic Report */
        public DbSet<Entity> entity { get; set; }
        public DbSet<EntityColumn> entityColumn { get; set; }
        public DbSet<Report> report { get; set; }
        public DbSet<ReportView> reportView { get; set; }
        public DbSet<ReportFilter> reportFilter { get; set; }
        public DbSet<ReportColumn> reportColumn { get; set; }
        public DbSet<ReportRoleNew> reportRoleNew { get; set; }
        public DbSet<ReportRoleView> reportRoleView { get; set; }

        //New Sales and Marketing Module
        public DbSet<Campaign> campaign { get; set; }
        public DbSet<CampaignCustomer> campaignCustomer { get; set; }
        public DbSet<CampaignLead> campaignLead { get; set; }
        public DbSet<CampaignMarketingList> campaignMarketingList { get; set; }
        public DbSet<CampaignType> campaignType { get; set; }
        public DbSet<Lead> lead { get; set; }
        public DbSet<MarketingList> marketingList { get; set; }
        public DbSet<MarketingListCustomer> marketingListCustomer { get; set; }
        public DbSet<MarketingListLead> marketingListLead { get; set; }
        //public DbSet<MarketingListPurpose> marketingListPurpose { get; set; }
        public DbSet<CampaignDetail> campaignDetail { get; set; }
        public DbSet<MarketingListDetail> marketingListDetail { get; set; }

        public static object CheckForDbNull(object value)
        {
            if (value == null)
            {
                return DBNull.Value;
            }
            return value;
        }
    }
}