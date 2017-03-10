using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TharsisRevolution
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageModule : Page
    {
        Membre jack;
        public PageModule()
        {
            this.InitializeComponent();
            Module m = new Module(Module.moduleType.Détente,1,1);
            m.PresenceMembre = true;
            jack = new Membre(Membre.roleMembre.Docteur, 1);
            jack.Position = m;
            jack.NombreDeDés = 3;



        }

        private void ButtonTerminer_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        private void ButtonLancer_Click(object sender, RoutedEventArgs e)
        {
            List<Image> imageList;
            switch (jack.NombreDeDés)
            {
                case 1:
                    imageList = new List<Image> { imgD1};
                    break;
                case 2:
                    imageList = new List<Image> { imgD1, imgD2 };
                    break;
                case 3:
                    imageList = new List<Image> { imgD1, imgD2, imgD3 };
                    break;
                case 4:
                    imageList = new List<Image> { imgD1, imgD2, imgD3, imgD4 };
                    break;
                case 5:
                    imageList = new List<Image> { imgD1, imgD2, imgD3, imgD4, imgD5 };
                    break;
                case 6:
                    imageList = new List<Image> { imgD1, imgD2, imgD3, imgD4, imgD5, imgD6 };
                    break;
                default:
                    imageList = new List<Image> { imgD1, imgD2, imgD3, imgD4, imgD5, imgD6 };
                    break;
            }

            Random rnd = new Random();
            foreach (Image i in imageList)
            {
                
                int dice = rnd.Next(1, 7);
                i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D"+dice+".png", UriKind.RelativeOrAbsolute));
                i.Tag = "D"+dice+".png";
                

            }

        }

        private void imDice_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (e.OriginalSource is FrameworkElement)
            {
                Image b = ((Image)e.OriginalSource);
                string s = b.Tag.ToString();
                Debug.WriteLine(s[2]);
                if (!s[2].Equals("_"))
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_HightLight.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_HightLight.png";
                }
                else//ne veut pas de-highlight l'image
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + ".png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + ".png";
                }
            }
            
        }
    }
}
