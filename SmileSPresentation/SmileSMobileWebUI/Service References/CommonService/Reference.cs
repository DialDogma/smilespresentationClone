﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmileSMobileWebUI.CommonService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="usp_MasterFilterList_Select_Result", Namespace="http://schemas.datacontract.org/2004/07/SmileSMobileAppService.Model")]
    [System.SerializableAttribute()]
    public partial class usp_MasterFilterList_Select_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DisplayField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ValueField;
        
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
        public string Display {
            get {
                return this.DisplayField;
            }
            set {
                if ((object.ReferenceEquals(this.DisplayField, value) != true)) {
                    this.DisplayField = value;
                    this.RaisePropertyChanged("Display");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Value {
            get {
                return this.ValueField;
            }
            set {
                if ((object.ReferenceEquals(this.ValueField, value) != true)) {
                    this.ValueField = value;
                    this.RaisePropertyChanged("Value");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="usp_MobileVersion_Select_Result", Namespace="http://schemas.datacontract.org/2004/07/SmileSMobileAppService.Model")]
    [System.SerializableAttribute()]
    public partial class usp_MobileVersion_Select_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastVersionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MSGField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string linkDWField;
        
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
        public string LastVersion {
            get {
                return this.LastVersionField;
            }
            set {
                if ((object.ReferenceEquals(this.LastVersionField, value) != true)) {
                    this.LastVersionField = value;
                    this.RaisePropertyChanged("LastVersion");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MSG {
            get {
                return this.MSGField;
            }
            set {
                if ((object.ReferenceEquals(this.MSGField, value) != true)) {
                    this.MSGField = value;
                    this.RaisePropertyChanged("MSG");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string linkDW {
            get {
                return this.linkDWField;
            }
            set {
                if ((object.ReferenceEquals(this.linkDWField, value) != true)) {
                    this.linkDWField = value;
                    this.RaisePropertyChanged("linkDW");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="usp_AgentLocation_Insert_Result", Namespace="http://schemas.datacontract.org/2004/07/SmileSMobileAppService.Model")]
    [System.SerializableAttribute()]
    public partial class usp_AgentLocation_Insert_Result : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CurrentLocationIDField;
        
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
        public System.Nullable<int> CurrentLocationID {
            get {
                return this.CurrentLocationIDField;
            }
            set {
                if ((this.CurrentLocationIDField.Equals(value) != true)) {
                    this.CurrentLocationIDField = value;
                    this.RaisePropertyChanged("CurrentLocationID");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CommonService.ICommonService")]
    public interface ICommonService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/GetMasterFilterList", ReplyAction="http://tempuri.org/ICommonService/GetMasterFilterListResponse")]
        SmileSMobileWebUI.CommonService.usp_MasterFilterList_Select_Result[] GetMasterFilterList(int filterTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/GetMasterFilterList", ReplyAction="http://tempuri.org/ICommonService/GetMasterFilterListResponse")]
        System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_MasterFilterList_Select_Result[]> GetMasterFilterListAsync(int filterTypeId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/GetMobileVersion", ReplyAction="http://tempuri.org/ICommonService/GetMobileVersionResponse")]
        SmileSMobileWebUI.CommonService.usp_MobileVersion_Select_Result GetMobileVersion();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/GetMobileVersion", ReplyAction="http://tempuri.org/ICommonService/GetMobileVersionResponse")]
        System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_MobileVersion_Select_Result> GetMobileVersionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/SetAgentLocation", ReplyAction="http://tempuri.org/ICommonService/SetAgentLocationResponse")]
        SmileSMobileWebUI.CommonService.usp_AgentLocation_Insert_Result SetAgentLocation(string gps, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICommonService/SetAgentLocation", ReplyAction="http://tempuri.org/ICommonService/SetAgentLocationResponse")]
        System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_AgentLocation_Insert_Result> SetAgentLocationAsync(string gps, int userId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICommonServiceChannel : SmileSMobileWebUI.CommonService.ICommonService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CommonServiceClient : System.ServiceModel.ClientBase<SmileSMobileWebUI.CommonService.ICommonService>, SmileSMobileWebUI.CommonService.ICommonService {
        
        public CommonServiceClient() {
        }
        
        public CommonServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CommonServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CommonServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public SmileSMobileWebUI.CommonService.usp_MasterFilterList_Select_Result[] GetMasterFilterList(int filterTypeId) {
            return base.Channel.GetMasterFilterList(filterTypeId);
        }
        
        public System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_MasterFilterList_Select_Result[]> GetMasterFilterListAsync(int filterTypeId) {
            return base.Channel.GetMasterFilterListAsync(filterTypeId);
        }
        
        public SmileSMobileWebUI.CommonService.usp_MobileVersion_Select_Result GetMobileVersion() {
            return base.Channel.GetMobileVersion();
        }
        
        public System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_MobileVersion_Select_Result> GetMobileVersionAsync() {
            return base.Channel.GetMobileVersionAsync();
        }
        
        public SmileSMobileWebUI.CommonService.usp_AgentLocation_Insert_Result SetAgentLocation(string gps, int userId) {
            return base.Channel.SetAgentLocation(gps, userId);
        }
        
        public System.Threading.Tasks.Task<SmileSMobileWebUI.CommonService.usp_AgentLocation_Insert_Result> SetAgentLocationAsync(string gps, int userId) {
            return base.Channel.SetAgentLocationAsync(gps, userId);
        }
    }
}
