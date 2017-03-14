using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

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

            List<string> sDifficulte = new List<string>();
            sDifficulte.Add("Normal");
            sDifficulte.Add("Difficile");
            cb_Difficulté.ItemsSource = sDifficulte;
            cb_Difficulté.SelectedIndex = 0;
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
            FDNoir.Visibility = Visibility.Collapsed;
            VideoIntro.Visibility = Visibility.Collapsed;
            MusiqueIntro.Play();
        }        
    }
}
