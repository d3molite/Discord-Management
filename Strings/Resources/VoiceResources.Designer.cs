﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Strings.Resources {
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
    internal class VoiceResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal VoiceResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Strings.Resources.VoiceResources", typeof(VoiceResources).Assembly);
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
        ///   Looks up a localized string similar to Channel **{0}** has been deleted due to inactivity..
        /// </summary>
        internal static string channel_deleted {
            get {
                return ResourceManager.GetString("channel_deleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This command can only be used in &lt;#{0}&gt;..
        /// </summary>
        internal static string error_channel {
            get {
                return ResourceManager.GetString("error_channel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Voice is not configured for this guild..
        /// </summary>
        internal static string error_config {
            get {
                return ResourceManager.GetString("error_config", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must use this command in a guild!.
        /// </summary>
        internal static string error_guild {
            get {
                return ResourceManager.GetString("error_guild", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channel **{0}** has been created! \nTap &lt;#{1}&gt; to join..
        /// </summary>
        internal static string voice_created {
            get {
                return ResourceManager.GetString("voice_created", resourceCulture);
            }
        }
    }
}