﻿#pragma checksum "..\..\..\UserTypeSelection.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F270FAFB6AB23D8C99DBE9001426B2C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace ChildcareApplication {
    
    
    /// <summary>
    /// UserSelection
    /// </summary>
    public partial class UserSelection : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\UserTypeSelection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ParentUse;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\UserTypeSelection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_AdminLogin;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UserTypeSelection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Exit;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\UserTypeSelection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_Help;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UserTypeSelection.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_About;
        
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
            System.Uri resourceLocater = new System.Uri("/ChildcareApplication;component/usertypeselection.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserTypeSelection.xaml"
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
            this.btn_ParentUse = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\UserTypeSelection.xaml"
            this.btn_ParentUse.Click += new System.Windows.RoutedEventHandler(this.btn_ParentUse_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_AdminLogin = ((System.Windows.Controls.Button)(target));
            
            #line 12 "..\..\..\UserTypeSelection.xaml"
            this.btn_AdminLogin.Click += new System.Windows.RoutedEventHandler(this.btn_AdminLogin_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_Exit = ((System.Windows.Controls.Button)(target));
            
            #line 13 "..\..\..\UserTypeSelection.xaml"
            this.btn_Exit.Click += new System.Windows.RoutedEventHandler(this.btn_Exit_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btn_Help = ((System.Windows.Controls.Button)(target));
            
            #line 14 "..\..\..\UserTypeSelection.xaml"
            this.btn_Help.Click += new System.Windows.RoutedEventHandler(this.btn_Help_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btn_About = ((System.Windows.Controls.Button)(target));
            
            #line 15 "..\..\..\UserTypeSelection.xaml"
            this.btn_About.Click += new System.Windows.RoutedEventHandler(this.btn_About_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

