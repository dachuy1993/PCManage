﻿#pragma checksum "..\..\Window_ListIP.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "22EAA35517A530117E219DF22A0D7D94D4CE1F5EBF1BFD53E6503957DFDB0DE8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PcManage;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PcManage {
    
    
    /// <summary>
    /// Window_ListIP
    /// </summary>
    public partial class Window_ListIP : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\Window_ListIP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtFilterInfo;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\Window_ListIP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFilterInfo;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\Window_ListIP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbQtyIP;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\Window_ListIP.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvListIP;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PcManage;component/window_listip.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Window_ListIP.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\Window_ListIP.xaml"
            ((PcManage.Window_ListIP)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txtFilterInfo = ((System.Windows.Controls.TextBox)(target));
            
            #line 26 "..\..\Window_ListIP.xaml"
            this.txtFilterInfo.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtFilterInfo_KeyDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btnFilterInfo = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\Window_ListIP.xaml"
            this.btnFilterInfo.Click += new System.Windows.RoutedEventHandler(this.btnFilterInfo_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lbQtyIP = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lvListIP = ((System.Windows.Controls.ListView)(target));
            
            #line 40 "..\..\Window_ListIP.xaml"
            this.lvListIP.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lvListIP_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

