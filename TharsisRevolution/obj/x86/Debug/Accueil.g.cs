﻿#pragma checksum "C:\Users\Alex\Source\Repos\TharsisRevolution2\TharsisRevolution\Accueil.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "010DBDAF560EFFD71AE14220E9308614"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TharsisRevolution
{
    partial class Accueil : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.btn_Jouer = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 17 "..\..\..\Accueil.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Jouer).Click += this.btn_Jouer_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.cb_Difficulté = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                }
                break;
            case 3:
                {
                    this.textBox = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.MusiqueIntro = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                }
                break;
            case 5:
                {
                    this.FDNoir = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 6:
                {
                    this.VideoIntro = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                    #line 30 "..\..\..\Accueil.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.VideoIntro).MediaEnded += this.video_Ended;
                    #line 30 "..\..\..\Accueil.xaml"
                    ((global::Windows.UI.Xaml.Controls.MediaElement)this.VideoIntro).Tapped += this.video_clicked;
                    #line default
                }
                break;
            case 7:
                {
                    this.btn_Load = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 33 "..\..\..\Accueil.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Load).Tapped += this.btn_Load_Tapped;
                    #line default
                }
                break;
            case 8:
                {
                    this.btn_Quitter = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 36 "..\..\..\Accueil.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Quitter).Tapped += this.btn_Quitter_Tapped;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

