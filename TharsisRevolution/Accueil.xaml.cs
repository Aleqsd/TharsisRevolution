﻿using Microsoft.Toolkit.Uwp;
using System.Collections.Generic;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace TharsisRevolution
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Accueil : Page
    {
        private bool hardMode = false;

        public Accueil()
        {
            this.InitializeComponent();

            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            List<string> sDifficulte = new List<string>();
            sDifficulte.Add("Normal");
            sDifficulte.Add("Difficile");
            cb_Difficulté.ItemsSource = sDifficulte;
            cb_Difficulté.SelectedIndex = 0;
        }

        private void btn_Quitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void btn_Jouer_Click(object sender, RoutedEventArgs e)
        {
            // Si l'index est 1, mode difficile, sinon normal
            if (cb_Difficulté.SelectedIndex == 1)
                hardMode = true;
            else
                hardMode = false;
            var parameters = new GameParameters(hardMode);

            this.Frame.Navigate(typeof(MainPage),parameters);
        }

        private void video_clicked(object sender, TappedRoutedEventArgs e)
        {
            VideoIntro.Stop();
            VideoIntro.IsFullWindow = false;
            FDNoir.Visibility = Visibility.Collapsed;
            VideoIntro.Visibility = Visibility.Collapsed;
            MusiqueIntro.Play();
        }

        private void video_Ended(object sender, RoutedEventArgs e)
        {
            VideoIntro.Stop();
            VideoIntro.IsFullWindow = false;
            FDNoir.Visibility = Visibility.Collapsed;
            VideoIntro.Visibility = Visibility.Collapsed;
            MusiqueIntro.Play();
        }

        private void btn_Load_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var helper = new LocalObjectStorageHelper();

            Vaisseau tempVaisseau = new Vaisseau();
            if (helper.KeyExists("vaisseau"))
                tempVaisseau = helper.Read<Vaisseau>("vaisseau");

            List<Membre> tempMembres = new List<Membre>();
            for (int i = 0; i<4; i++)
            {
                if (helper.KeyExists("membre"+i))
                    tempMembres.Add(helper.Read<Membre>("membre" + i));
            }

            List<Module> tempModules = new List<Module>();
            for (int i = 0; i < 7; i++)
            {
                if (helper.KeyExists("module" + i))
                    tempModules.Add(helper.Read<Module>("module" + i));
            }

            int tempNumeroSemaine = 0;
            if (helper.KeyExists("numeroSemaine"))
                tempNumeroSemaine = helper.Read<int>("numeroSemaine");

            bool tempHardMode = false;
            if (helper.KeyExists("hardMode"))
                tempHardMode = helper.Read<bool>("hardMode");

            bool tempGameStarted = false;
            if (helper.KeyExists("gameStarted"))
                tempGameStarted = helper.Read<bool>("gameStarted");

            int tempIndexCurrentClickMembre = 0;
            if (helper.KeyExists("indexCurrentClickMembre"))
                tempIndexCurrentClickMembre = helper.Read<int>("indexCurrentClickMembre");

            int tempIndexCurrentClickModule = 0;
            if (helper.KeyExists("indexCurrentClickModule"))
                tempIndexCurrentClickModule = helper.Read<int>("indexCurrentClickModule");

            CurrentParameters parameters = new CurrentParameters(tempMembres, tempModules, tempIndexCurrentClickMembre, tempIndexCurrentClickModule, tempHardMode, tempVaisseau, tempNumeroSemaine, tempGameStarted);
            this.Frame.Navigate(typeof(MainPage), parameters);
        }
    }
}
