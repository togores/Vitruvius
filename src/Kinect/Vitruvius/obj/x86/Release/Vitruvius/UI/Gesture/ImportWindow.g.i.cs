﻿#pragma checksum "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B5C729AAB0762AEDF9FD6564BEBCED70"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
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


namespace Opus.Vitruvius.Views {
    
    
    /// <summary>
    /// ImportWindow
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class ImportWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel gestureStackPanel;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button importSelectedButton;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button importAllButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Vitruvius;component/vitruvius/ui/gesture/importwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 4 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            ((Opus.Vitruvius.Views.ImportWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 4 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            ((Opus.Vitruvius.Views.ImportWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.gestureStackPanel = ((System.Windows.Controls.StackPanel)(target));
            
            #line 12 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            this.gestureStackPanel.Loaded += new System.Windows.RoutedEventHandler(this.gestureStackPanel_Loaded);
            
            #line default
            #line hidden
            return;
            case 3:
            this.importSelectedButton = ((System.Windows.Controls.Button)(target));
            
            #line 16 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            this.importSelectedButton.Click += new System.Windows.RoutedEventHandler(this.importSelectedButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.importAllButton = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\..\..\..\..\Vitruvius\UI\Gesture\ImportWindow.xaml"
            this.importAllButton.Click += new System.Windows.RoutedEventHandler(this.importAllButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

