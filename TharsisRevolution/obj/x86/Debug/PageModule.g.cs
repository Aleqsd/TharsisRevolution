﻿#pragma checksum "C:\Users\louis\Source\Repos\TharsisRevolution\TharsisRevolution\PageModule.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F8458253252185C2BE7AE71F1C54E819"
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
    partial class PageModule : 
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
                    this.RelativeMain = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                }
                break;
            case 2:
                {
                    this.Background_PageModule = (global::Windows.UI.Xaml.Media.ImageBrush)(target);
                }
                break;
            case 3:
                {
                    this.rtTitreModule = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4:
                {
                    this.rtNomPersonnage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.pg_Personnage = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 17 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pg_Personnage).ValueChanged += this.progressBar_PvPersonnage;
                    #line default
                }
                break;
            case 6:
                {
                    this.lbl_PvPersonnage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.tbLancesRestant = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.btLancer = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 9:
                {
                    this.Affichage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.BorderPouvoir = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 11:
                {
                    this.DetailPouvoir = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 12:
                {
                    this.BorderPanne = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 13:
                {
                    this.BorderBottom = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 14:
                {
                    this.BorderPiege = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 15:
                {
                    this.TooltipPiege = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 16:
                {
                    this.grDesHardMode = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 17:
                {
                    this.imgD9 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 18:
                {
                    this.imgD8 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 19:
                {
                    this.imgD7 = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 20:
                {
                    this.grPilotage = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 21:
                {
                    this.boD1 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 22:
                {
                    this.boD2 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 23:
                {
                    this.boD3 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 24:
                {
                    this.boD4 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 25:
                {
                    this.boD5 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 26:
                {
                    this.boD6 = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 27:
                {
                    this.imgD6 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 77 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD6).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 28:
                {
                    this.imgD5 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 74 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD5).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 29:
                {
                    this.imgD4 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 71 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD4).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 30:
                {
                    this.imgD3 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 68 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD3).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 31:
                {
                    this.imgD2 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 65 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD2).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 32:
                {
                    this.imgD1 = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 62 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.imgD1).Tapped += this.imDice_Tapped;
                    #line default
                }
                break;
            case 33:
                {
                    this.tbReparer = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 34:
                {
                    this.btReparerValeur = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 42 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btReparerValeur).Tapped += this.btReparerValeur_Tapped;
                    #line default
                }
                break;
            case 35:
                {
                    this.bt_PouvoirSpe = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 35 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.bt_PouvoirSpe).Tapped += this.bt_PouvoirSpe_Tapped;
                    #line default
                }
                break;
            case 36:
                {
                    this.btLancer1 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 31 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btLancer1).Click += this.ButtonLancer_Click;
                    #line default
                }
                break;
            case 37:
                {
                    this.btTerminer = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 22 "..\..\..\PageModule.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btTerminer).Click += this.ButtonTerminer_Click;
                    #line default
                }
                break;
            case 38:
                {
                    this.imFleche = (global::Windows.UI.Xaml.Controls.Image)(target);
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

