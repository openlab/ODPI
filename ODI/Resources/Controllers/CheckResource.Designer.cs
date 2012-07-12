﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17626
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ODI.Resources.Controllers {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CheckResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CheckResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ODI.Resources.Controllers.CheckResource", typeof(CheckResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not connect to Azure, Please check that your Subscription Id is correct..
        /// </summary>
        public static string ErrorCouldNotConnectToAzure {
            get {
                return ResourceManager.GetString("ErrorCouldNotConnectToAzure", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not make a secure connection to Azure.  Please make sure that you have uploaded the certificate using the Management Console..
        /// </summary>
        public static string ErrorCouldNotMakeSecureConnection {
            get {
                return ResourceManager.GetString("ErrorCouldNotMakeSecureConnection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connected securly to Azure, however there are no Hosted Services could be found for this Subscription.  Please make sure you have at least one Hosted Service set up..
        /// </summary>
        public static string ErrorNoHostedServiceFound {
            get {
                return ResourceManager.GetString("ErrorNoHostedServiceFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must provide the Primary Access Key.
        /// </summary>
        public static string ErrorPrimaryAccessKey {
            get {
                return ResourceManager.GetString("ErrorPrimaryAccessKey", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must provide the Storage Account Name.
        /// </summary>
        public static string ErrorStorageAccountName {
            get {
                return ResourceManager.GetString("ErrorStorageAccountName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must provide the Subscription Id.
        /// </summary>
        public static string ErrorSubscriptionId {
            get {
                return ResourceManager.GetString("ErrorSubscriptionId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Storage Account Exception :{0}&lt;br /&gt;Please recheck your credentials.
        /// </summary>
        public static string StorageAccountException {
            get {
                return ResourceManager.GetString("StorageAccountException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown Error: .
        /// </summary>
        public static string UnknownError {
            get {
                return ResourceManager.GetString("UnknownError", resourceCulture);
            }
        }
    }
}
