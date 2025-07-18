﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSSurvey.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SurveyEntities : DbContext
    {
        public SurveyEntities()
            : base("name=SurveyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CheckPoint> CheckPoint { get; set; }
        public virtual DbSet<ReportResult> ReportResult { get; set; }
        public virtual DbSet<ReportResultDetail> ReportResultDetail { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<SmileSSurveyLogs> SmileSSurveyLogs { get; set; }
        public virtual DbSet<SurveyResult> SurveyResult { get; set; }
        public virtual DbSet<SurveyResultAnswer> SurveyResultAnswer { get; set; }
        public virtual DbSet<TmpSurveyResult> TmpSurveyResult { get; set; }
        public virtual DbSet<Reference> Reference { get; set; }
        public virtual DbSet<Survey> Survey { get; set; }
        public virtual DbSet<SurveyAnswer> SurveyAnswer { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestion { get; set; }
        public virtual DbSet<SurveyViewLog> SurveyViewLog { get; set; }
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionXAnswer> QuestionXAnswer { get; set; }
    
        public virtual ObjectResult<usp_Survey_Select_Result> usp_Survey_Select(Nullable<int> surveyId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Survey_Select_Result>("usp_Survey_Select", surveyIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter);
        }
    
        public virtual ObjectResult<usp_SurveyAnswer_Select_Result> usp_SurveyAnswer_Select(Nullable<int> surveyQuestionId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail)
        {
            var surveyQuestionIdParameter = surveyQuestionId.HasValue ?
                new ObjectParameter("SurveyQuestionId", surveyQuestionId) :
                new ObjectParameter("SurveyQuestionId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyAnswer_Select_Result>("usp_SurveyAnswer_Select", surveyQuestionIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter);
        }
    
        public virtual ObjectResult<usp_SurveyQuestion_Select_Result> usp_SurveyQuestion_Select(Nullable<int> surveyId, Nullable<int> questionId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var questionIdParameter = questionId.HasValue ?
                new ObjectParameter("QuestionId", questionId) :
                new ObjectParameter("QuestionId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyQuestion_Select_Result>("usp_SurveyQuestion_Select", surveyIdParameter, questionIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter);
        }
    
        public virtual ObjectResult<usp_SurveyResult_Insert_Result> usp_SurveyResult_Insert(Nullable<int> surveyViewLogId, Nullable<int> surveyId, Nullable<int> createdBy)
        {
            var surveyViewLogIdParameter = surveyViewLogId.HasValue ?
                new ObjectParameter("SurveyViewLogId", surveyViewLogId) :
                new ObjectParameter("SurveyViewLogId", typeof(int));
    
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var createdByParameter = createdBy.HasValue ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyResult_Insert_Result>("usp_SurveyResult_Insert", surveyViewLogIdParameter, surveyIdParameter, createdByParameter);
        }
    
        public virtual ObjectResult<usp_TmpSurveyResult_Insert_Result> usp_TmpSurveyResult_Insert(Nullable<int> surveyViewLogId, Nullable<int> surveyId, Nullable<int> surveyQuestionId, Nullable<int> surveyAnswerId, string answerMore, Nullable<int> createdBy)
        {
            var surveyViewLogIdParameter = surveyViewLogId.HasValue ?
                new ObjectParameter("SurveyViewLogId", surveyViewLogId) :
                new ObjectParameter("SurveyViewLogId", typeof(int));
    
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var surveyQuestionIdParameter = surveyQuestionId.HasValue ?
                new ObjectParameter("SurveyQuestionId", surveyQuestionId) :
                new ObjectParameter("SurveyQuestionId", typeof(int));
    
            var surveyAnswerIdParameter = surveyAnswerId.HasValue ?
                new ObjectParameter("SurveyAnswerId", surveyAnswerId) :
                new ObjectParameter("SurveyAnswerId", typeof(int));
    
            var answerMoreParameter = answerMore != null ?
                new ObjectParameter("AnswerMore", answerMore) :
                new ObjectParameter("AnswerMore", typeof(string));
    
            var createdByParameter = createdBy.HasValue ?
                new ObjectParameter("CreatedBy", createdBy) :
                new ObjectParameter("CreatedBy", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_TmpSurveyResult_Insert_Result>("usp_TmpSurveyResult_Insert", surveyViewLogIdParameter, surveyIdParameter, surveyQuestionIdParameter, surveyAnswerIdParameter, answerMoreParameter, createdByParameter);
        }
    
        public virtual ObjectResult<usp_SurveyViewLog_Insert_Result> usp_SurveyViewLog_Insert(Nullable<int> surveyId, string iPAddress)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var iPAddressParameter = iPAddress != null ?
                new ObjectParameter("IPAddress", iPAddress) :
                new ObjectParameter("IPAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyViewLog_Insert_Result>("usp_SurveyViewLog_Insert", surveyIdParameter, iPAddressParameter);
        }
    
        public virtual ObjectResult<usp_SurveyMonitorResult_Select_Result> usp_SurveyMonitorResult_Select(Nullable<int> formId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail, string search)
        {
            var formIdParameter = formId.HasValue ?
                new ObjectParameter("FormId", formId) :
                new ObjectParameter("FormId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            var searchParameter = search != null ?
                new ObjectParameter("Search", search) :
                new ObjectParameter("Search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyMonitorResult_Select_Result>("usp_SurveyMonitorResult_Select", formIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter, searchParameter);
        }
    
        public virtual ObjectResult<usp_SurveyResult_Select_Result> usp_SurveyResult_Select(Nullable<int> surveyId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyResult_Select_Result>("usp_SurveyResult_Select", surveyIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter);
        }
    
        public virtual ObjectResult<usp_SurveyResultDuplicateCheck_Select_Result> usp_SurveyResultDuplicateCheck_Select(Nullable<int> surveyId)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveyResultDuplicateCheck_Select_Result>("usp_SurveyResultDuplicateCheck_Select", surveyIdParameter);
        }
    
        public virtual ObjectResult<usp_Form_Select_Result> usp_Form_Select(Nullable<int> formId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string search)
        {
            var formIdParameter = formId.HasValue ?
                new ObjectParameter("FormId", formId) :
                new ObjectParameter("FormId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchParameter = search != null ?
                new ObjectParameter("Search", search) :
                new ObjectParameter("Search", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Form_Select_Result>("usp_Form_Select", formIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchParameter);
        }
    
        public virtual ObjectResult<usp_Survey_Insert_Result> usp_Survey_Insert(Nullable<int> formId, string sFCaseId, Nullable<System.Guid> customerId, string phoneNo, string employeeCode, string refList, string sendBy, Nullable<System.DateTime> sendDate, Nullable<bool> isSendNow)
        {
            var formIdParameter = formId.HasValue ?
                new ObjectParameter("FormId", formId) :
                new ObjectParameter("FormId", typeof(int));
    
            var sFCaseIdParameter = sFCaseId != null ?
                new ObjectParameter("SFCaseId", sFCaseId) :
                new ObjectParameter("SFCaseId", typeof(string));
    
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("CustomerId", customerId) :
                new ObjectParameter("CustomerId", typeof(System.Guid));
    
            var phoneNoParameter = phoneNo != null ?
                new ObjectParameter("PhoneNo", phoneNo) :
                new ObjectParameter("PhoneNo", typeof(string));
    
            var employeeCodeParameter = employeeCode != null ?
                new ObjectParameter("EmployeeCode", employeeCode) :
                new ObjectParameter("EmployeeCode", typeof(string));
    
            var refListParameter = refList != null ?
                new ObjectParameter("RefList", refList) :
                new ObjectParameter("RefList", typeof(string));
    
            var sendByParameter = sendBy != null ?
                new ObjectParameter("SendBy", sendBy) :
                new ObjectParameter("SendBy", typeof(string));
    
            var sendDateParameter = sendDate.HasValue ?
                new ObjectParameter("SendDate", sendDate) :
                new ObjectParameter("SendDate", typeof(System.DateTime));
    
            var isSendNowParameter = isSendNow.HasValue ?
                new ObjectParameter("IsSendNow", isSendNow) :
                new ObjectParameter("IsSendNow", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Survey_Insert_Result>("usp_Survey_Insert", formIdParameter, sFCaseIdParameter, customerIdParameter, phoneNoParameter, employeeCodeParameter, refListParameter, sendByParameter, sendDateParameter, isSendNowParameter);
        }
    
        public virtual ObjectResult<usp_SurveySMS_Update_Result> usp_SurveySMS_Update(Nullable<int> surveyId, Nullable<int> smsId, string remark)
        {
            var surveyIdParameter = surveyId.HasValue ?
                new ObjectParameter("SurveyId", surveyId) :
                new ObjectParameter("SurveyId", typeof(int));
    
            var smsIdParameter = smsId.HasValue ?
                new ObjectParameter("SmsId", smsId) :
                new ObjectParameter("SmsId", typeof(int));
    
            var remarkParameter = remark != null ?
                new ObjectParameter("Remark", remark) :
                new ObjectParameter("Remark", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SurveySMS_Update_Result>("usp_SurveySMS_Update", surveyIdParameter, smsIdParameter, remarkParameter);
        }
    
        public virtual ObjectResult<usp_Survey_InsertV2_Result> usp_Survey_InsertV2(Nullable<int> formId, string sFCaseId, Nullable<System.Guid> customerId, string phoneNo, string employeeCode, string refList, string sendBy, Nullable<System.DateTime> sendDate, Nullable<bool> isSendNow)
        {
            var formIdParameter = formId.HasValue ?
                new ObjectParameter("FormId", formId) :
                new ObjectParameter("FormId", typeof(int));
    
            var sFCaseIdParameter = sFCaseId != null ?
                new ObjectParameter("SFCaseId", sFCaseId) :
                new ObjectParameter("SFCaseId", typeof(string));
    
            var customerIdParameter = customerId.HasValue ?
                new ObjectParameter("CustomerId", customerId) :
                new ObjectParameter("CustomerId", typeof(System.Guid));
    
            var phoneNoParameter = phoneNo != null ?
                new ObjectParameter("PhoneNo", phoneNo) :
                new ObjectParameter("PhoneNo", typeof(string));
    
            var employeeCodeParameter = employeeCode != null ?
                new ObjectParameter("EmployeeCode", employeeCode) :
                new ObjectParameter("EmployeeCode", typeof(string));
    
            var refListParameter = refList != null ?
                new ObjectParameter("RefList", refList) :
                new ObjectParameter("RefList", typeof(string));
    
            var sendByParameter = sendBy != null ?
                new ObjectParameter("SendBy", sendBy) :
                new ObjectParameter("SendBy", typeof(string));
    
            var sendDateParameter = sendDate.HasValue ?
                new ObjectParameter("SendDate", sendDate) :
                new ObjectParameter("SendDate", typeof(System.DateTime));
    
            var isSendNowParameter = isSendNow.HasValue ?
                new ObjectParameter("IsSendNow", isSendNow) :
                new ObjectParameter("IsSendNow", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_Survey_InsertV2_Result>("usp_Survey_InsertV2", formIdParameter, sFCaseIdParameter, customerIdParameter, phoneNoParameter, employeeCodeParameter, refListParameter, sendByParameter, sendDateParameter, isSendNowParameter);
        }
    
        public virtual ObjectResult<usp_SmileCRMSurveyResult_Select_Result1> usp_SmileCRMSurveyResult_Select(Nullable<System.DateTime> dateFrom, Nullable<System.DateTime> dateTo, Nullable<int> branchId, string serviceTypeName, Nullable<int> createdByEmployeeId, Nullable<int> indexStart, Nullable<int> pageSize, string sortField, string orderType, string searchDetail)
        {
            var dateFromParameter = dateFrom.HasValue ?
                new ObjectParameter("DateFrom", dateFrom) :
                new ObjectParameter("DateFrom", typeof(System.DateTime));
    
            var dateToParameter = dateTo.HasValue ?
                new ObjectParameter("DateTo", dateTo) :
                new ObjectParameter("DateTo", typeof(System.DateTime));
    
            var branchIdParameter = branchId.HasValue ?
                new ObjectParameter("BranchId", branchId) :
                new ObjectParameter("BranchId", typeof(int));
    
            var serviceTypeNameParameter = serviceTypeName != null ?
                new ObjectParameter("ServiceTypeName", serviceTypeName) :
                new ObjectParameter("ServiceTypeName", typeof(string));
    
            var createdByEmployeeIdParameter = createdByEmployeeId.HasValue ?
                new ObjectParameter("CreatedByEmployeeId", createdByEmployeeId) :
                new ObjectParameter("CreatedByEmployeeId", typeof(int));
    
            var indexStartParameter = indexStart.HasValue ?
                new ObjectParameter("IndexStart", indexStart) :
                new ObjectParameter("IndexStart", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            var sortFieldParameter = sortField != null ?
                new ObjectParameter("SortField", sortField) :
                new ObjectParameter("SortField", typeof(string));
    
            var orderTypeParameter = orderType != null ?
                new ObjectParameter("OrderType", orderType) :
                new ObjectParameter("OrderType", typeof(string));
    
            var searchDetailParameter = searchDetail != null ?
                new ObjectParameter("SearchDetail", searchDetail) :
                new ObjectParameter("SearchDetail", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<usp_SmileCRMSurveyResult_Select_Result1>("usp_SmileCRMSurveyResult_Select", dateFromParameter, dateToParameter, branchIdParameter, serviceTypeNameParameter, createdByEmployeeIdParameter, indexStartParameter, pageSizeParameter, sortFieldParameter, orderTypeParameter, searchDetailParameter);
        }
    }
}
