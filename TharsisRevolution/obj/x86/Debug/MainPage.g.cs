﻿#pragma checksum "C:\Users\Alex\Source\Repos\TharsisRevolution2\TharsisRevolution\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "973BAA5A4D9E5FDDA0FA36F3BCD1A820"
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
    partial class MainPage : 
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
                    this.block_PvShip = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 2:
                {
                    this.pg_PvShip = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 21 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pg_PvShip).ValueChanged += this.progressBar_PvShip;
                    #line default
                }
                break;
            case 3:
                {
                    this.block_Semaines = (global::Windows.UI.Xaml.Controls.Border)(target);
                }
                break;
            case 4:
                {
                    this.lbl_TimeSemaine = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5:
                {
                    this.lbl_TimeSemaineMax = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.slider_Semaines = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 28 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.slider_Semaines).ValueChanged += this.slider_TimeSemaine_ValueChanged;
                    #line default
                }
                break;
            case 7:
                {
                    this.lbl_txtSemaine = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.Grid_Jeu = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 9:
                {
                    this.lbl_Ship = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 10:
                {
                    this.lbl_PvShip = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 11:
                {
                    this.song = (global::Windows.UI.Xaml.Controls.MediaElement)(target);
                }
                break;
            case 12:
                {
                    this.Affichage = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 13:
                {
                    this.btnSauvegarde = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 70 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnSauvegarde).Click += this.btnSauvegarde_Click;
                    #line default
                }
                break;
            case 14:
                {
                    this.btnFinSemaine = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 71 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btnFinSemaine).Tapped += this.btnFinSemaine_Tapped;
                    #line default
                }
                break;
            case 15:
                {
                    this.PostePilotage = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 72 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.PostePilotage).Tapped += this.Pilotage_HightLight_OnClick;
                    #line default
                }
                break;
            case 16:
                {
                    this.Serre = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 73 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.Serre).Tapped += this.Serre_HightLight_OnClick;
                    #line default
                }
                break;
            case 17:
                {
                    this.Infirmerie = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 74 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.Infirmerie).Tapped += this.Infirmerie_HightLight_OnClick;
                    #line default
                }
                break;
            case 18:
                {
                    this.Détente = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 75 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.Détente).Tapped += this.Detente_HightLight_OnClick;
                    #line default
                }
                break;
            case 19:
                {
                    this.Maintenance = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 76 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.Maintenance).Tapped += this.Maintenance_HightLight_OnClick;
                    #line default
                }
                break;
            case 20:
                {
                    this.Laboratoire = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 77 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.Laboratoire).Tapped += this.Labo_HightLight_OnClick;
                    #line default
                }
                break;
            case 21:
                {
                    this.SystemeSurvie = (global::Windows.UI.Xaml.Controls.Image)(target);
                    #line 78 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Image)this.SystemeSurvie).Tapped += this.Survie_HightLight_OnClick;
                    #line default
                }
                break;
            case 22:
                {
                    this.reMeca = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                    #line 82 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RelativePanel)this.reMeca).Tapped += this.Meca_HightLight_OnClick;
                    #line default
                }
                break;
            case 23:
                {
                    this.reDocteur = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                    #line 121 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RelativePanel)this.reDocteur).Tapped += this.Doc_HightLight_OnClick;
                    #line default
                }
                break;
            case 24:
                {
                    this.reCapitaine = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                    #line 160 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RelativePanel)this.reCapitaine).Tapped += this.Capitaine_HightLight_OnClick;
                    #line default
                }
                break;
            case 25:
                {
                    this.reCommandant = (global::Windows.UI.Xaml.Controls.RelativePanel)(target);
                    #line 199 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.RelativePanel)this.reCommandant).Tapped += this.Commandant_HightLight_OnClick;
                    #line default
                }
                break;
            case 26:
                {
                    this.volumeSlider = (global::Windows.UI.Xaml.Controls.Slider)(target);
                    #line 238 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Slider)this.volumeSlider).ValueChanged += this.ChangeVolume;
                    #line default
                }
                break;
            case 27:
                {
                    this.textBlock = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 28:
                {
                    this.lbl_Commandant = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 29:
                {
                    this.imLogoCommandant = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 30:
                {
                    this.pb_PvCommandant = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 202 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pb_PvCommandant).ValueChanged += this.progressBar_PvCommandant;
                    #line default
                }
                break;
            case 31:
                {
                    this.lbl_PvCommandant = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 32:
                {
                    this.gr_DesCommandant = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 33:
                {
                    this.Com_d6 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 34:
                {
                    this.Com_d5 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 35:
                {
                    this.Com_d4 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 36:
                {
                    this.Com_d3 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 37:
                {
                    this.Com_d2 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 38:
                {
                    this.Com_d1 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 39:
                {
                    this.lbl_Capitaine = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 40:
                {
                    this.imLogoCapitaine = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 41:
                {
                    this.pb_PvCapitaine = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 163 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pb_PvCapitaine).ValueChanged += this.progressBar_PvCapitaine;
                    #line default
                }
                break;
            case 42:
                {
                    this.lbl_PvCapitaine = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 43:
                {
                    this.gr_DesCapitaine = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 44:
                {
                    this.Cap_d6 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 45:
                {
                    this.Cap_d5 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 46:
                {
                    this.Cap_d4 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 47:
                {
                    this.Cap_d3 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 48:
                {
                    this.Cap_d2 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 49:
                {
                    this.Cap_d1 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 50:
                {
                    this.lbl_Docteur = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 51:
                {
                    this.imLogoDocteur = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 52:
                {
                    this.pb_PvDocteur = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 124 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pb_PvDocteur).ValueChanged += this.progressBar_PvDocteur;
                    #line default
                }
                break;
            case 53:
                {
                    this.lbl_PvDocteur = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 54:
                {
                    this.gr_DesDocteur = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 55:
                {
                    this.doc_d6 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 56:
                {
                    this.doc_d5 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 57:
                {
                    this.doc_d4 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 58:
                {
                    this.doc_d3 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 59:
                {
                    this.doc_d2 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 60:
                {
                    this.doc_d1 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 61:
                {
                    this.lbl_Mecanicien = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 62:
                {
                    this.imLogoMeca = (global::Windows.UI.Xaml.Controls.Image)(target);
                }
                break;
            case 63:
                {
                    this.pbPvMeca = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                    #line 85 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ProgressBar)this.pbPvMeca).ValueChanged += this.progressBar_PvMeca;
                    #line default
                }
                break;
            case 64:
                {
                    this.lbl_PvMeca = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 65:
                {
                    this.grDesMeca = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 66:
                {
                    this.Meca_d6 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 67:
                {
                    this.Meca_d5 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 68:
                {
                    this.Meca_d4 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 69:
                {
                    this.Meca_d3 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 70:
                {
                    this.Meca_d2 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
                }
                break;
            case 71:
                {
                    this.Meca_d1 = (global::Windows.UI.Xaml.Shapes.Rectangle)(target);
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

