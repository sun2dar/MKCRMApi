//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class LetterEntry {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LetterEntry() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.LetterEntry", global::System.Reflection.Assembly.Load("App_GlobalResources"));
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Branch is not found..
        /// </summary>
        internal static string Alert_BranchNotFound {
            get {
                return ResourceManager.GetString("Alert_BranchNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please choose attachment type..
        /// </summary>
        internal static string Alert_ChooseAttachmentType {
            get {
                return ResourceManager.GetString("Alert_ChooseAttachmentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please choose the CC..
        /// </summary>
        internal static string Alert_ChooseBranch {
            get {
                return ResourceManager.GetString("Alert_ChooseBranch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please choose the Letter Template..
        /// </summary>
        internal static string Alert_ChooseTemplate {
            get {
                return ResourceManager.GetString("Alert_ChooseTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error when generating Letter for Request #{0}. {1}..
        /// </summary>
        internal static string Alert_GenerateLetterError {
            get {
                return ResourceManager.GetString("Alert_GenerateLetterError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success when generating Letter for Request #{0}..
        /// </summary>
        internal static string Alert_GenerateLetterSuccess {
            get {
                return ResourceManager.GetString("Alert_GenerateLetterSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field LetterNo harus berupa angka..
        /// </summary>
        internal static string Alert_LetterNo {
            get {
                return ResourceManager.GetString("Alert_LetterNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tidak dapat membuat Letter Entry karena Request kosong..
        /// </summary>
        internal static string Alert_NoRequest {
            get {
                return ResourceManager.GetString("Alert_NoRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tidak dapat membuat Letter Entry karena Request lebih dari 5..
        /// </summary>
        internal static string Alert_RequestMoreThanFive {
            get {
                return ResourceManager.GetString("Alert_RequestMoreThanFive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have no longer session. Please re-login..
        /// </summary>
        internal static string Alert_SessionIsNull {
            get {
                return ResourceManager.GetString("Alert_SessionIsNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Letter Entry berhasil disimpan..
        /// </summary>
        internal static string Alert_Success {
            get {
                return ResourceManager.GetString("Alert_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Field Template harus diisi..
        /// </summary>
        internal static string Alert_Template {
            get {
                return ResourceManager.GetString("Alert_Template", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Letter Entry created without an attachment, because Letter Template does not have an attachment document. You can attach the document manualy..
        /// </summary>
        internal static string Alert_TemplateNoAttachment {
            get {
                return ResourceManager.GetString("Alert_TemplateNoAttachment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Letter Template Type hasn&apos;t been defined..
        /// </summary>
        internal static string Alert_TemplateUndefined {
            get {
                return ResourceManager.GetString("Alert_TemplateUndefined", resourceCulture);
            }
        }
    }
}
