﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSCustomerBase.SSSDocumentService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Document", Namespace="http://schemas.datacontract.org/2004/07/SmileSDocService.Model")]
    [System.SerializableAttribute()]
    public partial class Document : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentFileIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentTypeIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DocumentTypeNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PathFullDocField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PathThumbnailImgField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int RunningNoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentFileId {
            get {
                return this.DocumentFileIdField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentFileIdField, value) != true)) {
                    this.DocumentFileIdField = value;
                    this.RaisePropertyChanged("DocumentFileId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentId {
            get {
                return this.DocumentIdField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentIdField, value) != true)) {
                    this.DocumentIdField = value;
                    this.RaisePropertyChanged("DocumentId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentTypeId {
            get {
                return this.DocumentTypeIdField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentTypeIdField, value) != true)) {
                    this.DocumentTypeIdField = value;
                    this.RaisePropertyChanged("DocumentTypeId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DocumentTypeName {
            get {
                return this.DocumentTypeNameField;
            }
            set {
                if ((object.ReferenceEquals(this.DocumentTypeNameField, value) != true)) {
                    this.DocumentTypeNameField = value;
                    this.RaisePropertyChanged("DocumentTypeName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PathFullDoc {
            get {
                return this.PathFullDocField;
            }
            set {
                if ((object.ReferenceEquals(this.PathFullDocField, value) != true)) {
                    this.PathFullDocField = value;
                    this.RaisePropertyChanged("PathFullDoc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PathThumbnailImg {
            get {
                return this.PathThumbnailImgField;
            }
            set {
                if ((object.ReferenceEquals(this.PathThumbnailImgField, value) != true)) {
                    this.PathThumbnailImgField = value;
                    this.RaisePropertyChanged("PathThumbnailImg");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int RunningNo {
            get {
                return this.RunningNoField;
            }
            set {
                if ((this.RunningNoField.Equals(value) != true)) {
                    this.RunningNoField = value;
                    this.RaisePropertyChanged("RunningNo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SSSDocumentService.IDocumentService")]
    public interface IDocumentService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/GetDocument", ReplyAction="http://tempuri.org/IDocumentService/GetDocumentResponse")]
        SmileSCustomerBase.SSSDocumentService.Document[] GetDocument(string refCode, string empCode, int pageSize, int indexStart, string sortField, string orderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/GetDocument", ReplyAction="http://tempuri.org/IDocumentService/GetDocumentResponse")]
        System.Threading.Tasks.Task<SmileSCustomerBase.SSSDocumentService.Document[]> GetDocumentAsync(string refCode, string empCode, int pageSize, int indexStart, string sortField, string orderType);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_Test", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_TestResponse")]
        bool Set_Document_Test(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7, string documentIndexData8, string documentIndexData9, string documentIndexData10);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_Test", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_TestResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_TestAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7, string documentIndexData8, string documentIndexData9, string documentIndexData10);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PHNewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PHNewAppResponse")]
        bool Set_Document_PHNewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PHNewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PHNewAppResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_PHNewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PHStatement", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PHStatementResponse")]
        bool Set_Document_PHStatement(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PHStatement", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PHStatementResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_PHStatementAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PA30NewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PA30NewAppResponse")]
        bool Set_Document_PA30NewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_PA30NewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_PA30NewAppResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_PA30NewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_LifeNewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_LifeNewAppResponse")]
        bool Set_Document_LifeNewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_LifeNewApp", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_LifeNewAppResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_LifeNewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_LifeETC", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_LifeETCResponse")]
        bool Set_Document_LifeETC(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/Set_Document_LifeETC", ReplyAction="http://tempuri.org/IDocumentService/Set_Document_LifeETCResponse")]
        System.Threading.Tasks.Task<bool> Set_Document_LifeETCAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/DeleteDocument", ReplyAction="http://tempuri.org/IDocumentService/DeleteDocumentResponse")]
        string DeleteDocument(string documentFileId, string empCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/DeleteDocument", ReplyAction="http://tempuri.org/IDocumentService/DeleteDocumentResponse")]
        System.Threading.Tasks.Task<string> DeleteDocumentAsync(string documentFileId, string empCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/GetDocumentV2", ReplyAction="http://tempuri.org/IDocumentService/GetDocumentV2Response")]
        SmileSCustomerBase.SSSDocumentService.Document[] GetDocumentV2(string refCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IDocumentService/GetDocumentV2", ReplyAction="http://tempuri.org/IDocumentService/GetDocumentV2Response")]
        System.Threading.Tasks.Task<SmileSCustomerBase.SSSDocumentService.Document[]> GetDocumentV2Async(string refCode);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IDocumentServiceChannel : SmileSCustomerBase.SSSDocumentService.IDocumentService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DocumentServiceClient : System.ServiceModel.ClientBase<SmileSCustomerBase.SSSDocumentService.IDocumentService>, SmileSCustomerBase.SSSDocumentService.IDocumentService {
        
        public DocumentServiceClient() {
        }
        
        public DocumentServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DocumentServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DocumentServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DocumentServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SmileSCustomerBase.SSSDocumentService.Document[] GetDocument(string refCode, string empCode, int pageSize, int indexStart, string sortField, string orderType) {
            return base.Channel.GetDocument(refCode, empCode, pageSize, indexStart, sortField, orderType);
        }
        
        public System.Threading.Tasks.Task<SmileSCustomerBase.SSSDocumentService.Document[]> GetDocumentAsync(string refCode, string empCode, int pageSize, int indexStart, string sortField, string orderType) {
            return base.Channel.GetDocumentAsync(refCode, empCode, pageSize, indexStart, sortField, orderType);
        }
        
        public bool Set_Document_Test(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7, string documentIndexData8, string documentIndexData9, string documentIndexData10) {
            return base.Channel.Set_Document_Test(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6, documentIndexData7, documentIndexData8, documentIndexData9, documentIndexData10);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_TestAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7, string documentIndexData8, string documentIndexData9, string documentIndexData10) {
            return base.Channel.Set_Document_TestAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6, documentIndexData7, documentIndexData8, documentIndexData9, documentIndexData10);
        }
        
        public bool Set_Document_PHNewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7) {
            return base.Channel.Set_Document_PHNewApp(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6, documentIndexData7);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_PHNewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6, string documentIndexData7) {
            return base.Channel.Set_Document_PHNewAppAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6, documentIndexData7);
        }
        
        public bool Set_Document_PHStatement(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_PHStatement(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_PHStatementAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_PHStatementAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public bool Set_Document_PA30NewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_PA30NewApp(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_PA30NewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_PA30NewAppAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public bool Set_Document_LifeNewApp(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6) {
            return base.Channel.Set_Document_LifeNewApp(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_LifeNewAppAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5, string documentIndexData6) {
            return base.Channel.Set_Document_LifeNewAppAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5, documentIndexData6);
        }
        
        public bool Set_Document_LifeETC(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_LifeETC(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public System.Threading.Tasks.Task<bool> Set_Document_LifeETCAsync(int branchId, System.DateTime documentDate, string employeeCode, string attachmentUrl, string documentIndexData1, string documentIndexData2, string documentIndexData3, string documentIndexData4, string documentIndexData5) {
            return base.Channel.Set_Document_LifeETCAsync(branchId, documentDate, employeeCode, attachmentUrl, documentIndexData1, documentIndexData2, documentIndexData3, documentIndexData4, documentIndexData5);
        }
        
        public string DeleteDocument(string documentFileId, string empCode) {
            return base.Channel.DeleteDocument(documentFileId, empCode);
        }
        
        public System.Threading.Tasks.Task<string> DeleteDocumentAsync(string documentFileId, string empCode) {
            return base.Channel.DeleteDocumentAsync(documentFileId, empCode);
        }
        
        public SmileSCustomerBase.SSSDocumentService.Document[] GetDocumentV2(string refCode) {
            return base.Channel.GetDocumentV2(refCode);
        }
        
        public System.Threading.Tasks.Task<SmileSCustomerBase.SSSDocumentService.Document[]> GetDocumentV2Async(string refCode) {
            return base.Channel.GetDocumentV2Async(refCode);
        }
    }
}
