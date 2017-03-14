using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Popups;

namespace TharsisRevolution
{
    /// <summary>
    /// Page d'un module
    /// </summary>
    public sealed partial class PageModule : Page
    {
        private int indexCurrentMembre;
        private int indexCurrentModule;
        private List<Membre> membres;
        private List<Module> modules;
        private List<Dé> lancer;
        private List<Dé> listeDéPiégés;
        private bool hardMode;
        private int nombreLancers = 3;
        private Vaisseau vaisseau;
        private int numeroSemaine;
        private bool pouvoirUtilisé = false;
        private bool utilisationPouvoirCapitaine = false;
        private bool gameStarted = true;
        private string tooltipPiege;

        private static readonly Random rdm = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return rdm.Next(min, max);
            }
        }

        public PageModule()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var parameters = (CurrentParameters)e.Parameter;

            indexCurrentMembre = parameters.IndexCurrentMembre;
            indexCurrentModule = parameters.IndexCurrentModule;
            membres = parameters.Membres;
            modules = parameters.Modules;
            hardMode = parameters.HardMode;
            vaisseau = parameters.Vaisseau;
            numeroSemaine = parameters.NumeroSemaine;

            btReparerValeur.Content = modules[indexCurrentModule].Panne.Dégat;

            if (hardMode)
            {
                listeDéPiégés = modules[indexCurrentModule].Panne.DésPiégés;

                foreach (Dé dé in listeDéPiégés)
                    tooltipPiege += "Dé " + dé.Valeur + " = " + dé.Type + "\n";

                List<Image> imageListDésPiégés;
                switch (listeDéPiégés.Count)
                {
                    case 0:
                        imageListDésPiégés = new List<Image>();
                        break;
                    case 1:
                        imageListDésPiégés = new List<Image> { imgD7 };
                        break;
                    case 2:
                        imageListDésPiégés = new List<Image> { imgD7, imgD8 };
                        break;
                    case 3:
                        imageListDésPiégés = new List<Image> { imgD7, imgD8, imgD9 };
                        break;
                    default:
                        imageListDésPiégés = new List<Image>();
                        break;
                }

                int index2 = 0;
                foreach (Image i in imageListDésPiégés)
                {
                    switch (listeDéPiégés[index2].Type)
                    {
                        case déType.Bléssure:
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + listeDéPiégés[index2].Valeur + "_Blessure.png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + listeDéPiégés[index2].Valeur + "_Blessure.png";
                            break;
                        case déType.Caduc:
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + listeDéPiégés[index2].Valeur + "_Caduc.png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + listeDéPiégés[index2].Valeur + "_Caduc.png";
                            break;
                        case déType.Stase:
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + listeDéPiégés[index2].Valeur + "_Stase.png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + listeDéPiégés[index2].Valeur + "_Stase.png";
                            break;
                        default:
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + listeDéPiégés[index2].Valeur + ".png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + listeDéPiégés[index2].Valeur + ".png";
                            break;
                    }
                    index2++;
                }
            }
            else
                BorderPiege.Visibility = Visibility.Collapsed;


            if (nombreLancers > 1)
                tbLancesRestant.Text = nombreLancers + " restants";
            else
                tbLancesRestant.Text = nombreLancers + " restant";

            switch (membres[indexCurrentMembre].Role)
            {
                case Membre.roleMembre.Capitaine:
                    DetailPouvoir.Text = "+1 Dé pour chaque membre";
                    break;
                case Membre.roleMembre.Commandant:
                    DetailPouvoir.Text = "+10 Réparation";
                    break;
                case Membre.roleMembre.Mécanicien:
                    DetailPouvoir.Text = "+1 PV pour le vaisseau";
                    break;
                case Membre.roleMembre.Docteur:
                    DetailPouvoir.Text = "+1 PV pour chaque membre";
                    break;
                default:
                    break;
            }

            rtTitreModule.Text = modules[indexCurrentModule].Type.ToString();
            rtNomPersonnage.Text = membres[indexCurrentMembre].Role.ToString();

            btLancer1.Background = new SolidColorBrush(Colors.White);
            btLancer1.Foreground = new SolidColorBrush(Colors.Black);

            if (listeDéPiégés.Count > 0)
                TooltipPiege.Text = tooltipPiege;

        }

        private void UpdateUI()
        {
            List<Image> imageList;
            switch (membres[indexCurrentMembre].NombreDeDés)
            {
                case 1:
                    imageList = new List<Image> { imgD1 };
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

            if (utilisationPouvoirCapitaine)
            {
                lancer.Add(new Dé());
                utilisationPouvoirCapitaine = false;
            }

            int index = 0;
            foreach (Image i in imageList)
            {
                switch (lancer[index].Type)
                {
                    case déType.Normal:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + ".png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + ".png";
                        break;
                    case déType.Highlight:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_HightLight.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_HightLight.png";
                        break;
                    case déType.Grisé:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Lock.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Lock.png";
                        break;
                    case déType.Bléssure:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Blessure.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Blessure.png";
                        break;
                    case déType.Caduc:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Caduc.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Caduc.png";
                        break;
                    case déType.Stase:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Stase.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Stase.png";
                        break;
                    case déType.StaseHighlight:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Stase_HightLight.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Stase_HightLight.png";
                        break;
                    case déType.BléssureHighlight:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + "_Blessure_HightLight.png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + "_Blessure_HightLight.png";
                        break;
                    default:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + ".png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + ".png";
                        break;
                }
                index++;

                if (PouvoirUtilisable())
                {
                    bt_PouvoirSpe.Background = new SolidColorBrush(Colors.White);
                    bt_PouvoirSpe.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bt_PouvoirSpe.Background = btTerminer.Background;
                    bt_PouvoirSpe.Foreground = new SolidColorBrush(Colors.White);
                }
            }

            btReparerValeur.Content = modules[indexCurrentModule].Panne.Dégat;
        }

        private void ButtonTerminer_Click(object sender, RoutedEventArgs e)
        {
            membres[indexCurrentMembre].AJoué = true;
            var parameters = new CurrentParameters(membres, modules, indexCurrentMembre, indexCurrentModule, hardMode, vaisseau, numeroSemaine, gameStarted);

            this.Frame.Navigate(typeof(MainPage), parameters);
        }

        private void ButtonLancer_Click(object sender, RoutedEventArgs e)
        {
            btLancer1.Content = "Relancer";

            if (nombreLancers == 3)
            {
                lancer = new List<Dé>(new Dé[membres[indexCurrentMembre].NombreDeDés]);
                Debug.WriteLine("Lancer de " + membres[indexCurrentMembre].NombreDeDés + " dés");
            }

            if (nombreLancers == 1)
            {
                btLancer1.Background = btTerminer.Background;
                btLancer1.Foreground = new SolidColorBrush(Colors.White);
            }

            if (nombreLancers > 0)
            {
                List<Image> imageList;
                switch (membres[indexCurrentMembre].NombreDeDés)
                {
                    case 1:
                        imageList = new List<Image> { imgD1 };
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

                int j = 0;

                foreach (Image i in imageList)
                {
                    //Griser Dé si le dé est piégé en hardmode, animation, ou message ?

                    // new Dé cré un dé à valeur random
                    if (nombreLancers == 3)
                    {
                        // Création de la liste de dé, de X dés en fonction du nombre de dés du membre
                        lancer[j] = new Dé();
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[j].Valeur + ".png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[j].Valeur + ".png";
                    }
                    else
                    {
                        if (lancer[j].Type.Equals(déType.Normal) || lancer[j].Type.Equals(déType.Bléssure))
                        {
                            lancer[j] = new Dé();
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[j].Valeur + ".png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + lancer[j].Valeur + ".png";
                        }
                    }
                    j++;
                }

                if (PouvoirUtilisable())
                {
                    bt_PouvoirSpe.Background = new SolidColorBrush(Colors.White);
                    bt_PouvoirSpe.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    bt_PouvoirSpe.Background = btTerminer.Background;
                    bt_PouvoirSpe.Foreground = new SolidColorBrush(Colors.White);
                }

                // TODO a verifier
                if (hardMode)
                {
                    // Pour chaque dés dans le lancer
                    for (int i = 0; i < membres[indexCurrentMembre].NombreDeDés; i++)
                    {
                        // Pour chaque dé piégé, regarder si c'est la même valeur que le dé du lancer
                        foreach (Dé dé in listeDéPiégés)
                        {
                            if (dé.Valeur.Equals(lancer[i].Valeur) && (!lancer[i].Type.Equals(déType.Grisé) && !lancer[i].Type.Equals(déType.Caduc)))
                            {
                                switch (dé.Type)
                                {
                                    // TODO mettre des messages d'alertes quand on subit un malus
                                    case déType.Bléssure:
                                        lancer[i].Type = déType.Bléssure;
                                        membres[indexCurrentMembre].Pv--;
                                        Affichage.Text = "Le dé " + dé.Valeur + " vous inflige une bléssure (PV actuel : " + membres[indexCurrentMembre].Pv + ").";
                                        break;
                                    case déType.Stase:
                                        lancer[i].Type = déType.Stase;
                                        break;
                                    case déType.Caduc:
                                        lancer[i].Type = déType.Caduc;
                                        break;
                                }
                            }
                        }
                    }
                }

                nombreLancers--;

                if (membres[indexCurrentMembre].Pv < 1)
                    nombreLancers = 0;
                // Mise à jour du nombre de dés restant affichés
                tbLancesRestant.Text = nombreLancers + " restant";
                UpdateUI();
            }
            else
                Affichage.Text = "Vous n'avez plus de lancers";
        }


        private void imDice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement)
            {
                Image b = ((Image)e.OriginalSource);
                int indexDé = 0;

                switch (b.Name)
                {
                    case "imgD1":
                        indexDé = 0;
                        break;
                    case "imgD2":
                        indexDé = 1;
                        break;
                    case "imgD3":
                        indexDé = 2;
                        break;
                    case "imgD4":
                        indexDé = 3;
                        break;
                    case "imgD5":
                        indexDé = 4;
                        break;
                    case "imgD6":
                        indexDé = 7;
                        break;
                    default:
                        break;
                }

                string s = b.Tag.ToString();
                if (s.Contains("Lock") || s.Contains("Caduc")) //Gris et Caduc
                {
                    Affichage.Text = "Ce dé est inutilisable";
                }
                else if (s.Contains("Blessure.png")) //Blessure
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_Blessure_HightLight.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_Blessure_HightLight.png";
                    lancer[indexDé].Type = déType.BléssureHighlight;

                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else if (s.Contains("Stase.png")) //Stase
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_Stase_HightLight.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_Stase_HightLight.png";
                    lancer[indexDé].Type = déType.StaseHighlight;

                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else if (s.Contains("Blessure_HightLight")) // Blessure Highlight
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_Blessure.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_Blessure.png";
                    lancer[indexDé].Type = déType.Bléssure;

                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else if (s.Contains("Stase_HightLight")) // Stase Highlight
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_Stase.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_Stase.png";
                    lancer[indexDé].Type = déType.Stase;

                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else if (!s.Contains("HightLight")) // Blanc
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_HightLight.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_HightLight.png";
                    lancer[indexDé].Type = déType.Highlight;

                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else //Highlighté
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + ".png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + ".png";
                    lancer[indexDé].Type = déType.Normal;

                    List<Boolean> SelectedDices = new List<Boolean>{ imgD1.Tag.ToString().Contains("HightLight"),
                                                            imgD2.Tag.ToString().Contains("HightLight"),
                                                            imgD3.Tag.ToString().Contains("HightLight"),
                                                            imgD4.Tag.ToString().Contains("HightLight"),
                                                            imgD5.Tag.ToString().Contains("HightLight"),
                                                            imgD6.Tag.ToString().Contains("HightLight")};
                    if (!SelectedDices.Contains(true))
                    {
                        btReparerValeur.Background = btTerminer.Background;
                        btReparerValeur.Foreground = new SolidColorBrush(Colors.White);
                        btReparerValeur.IsEnabled = false;
                    }
                }
            }
        }

        private void Réparer(int valeur)
        {
            int dégatAvant = modules[indexCurrentModule].Panne.Dégat;
            int dégatAprès = modules[indexCurrentModule].Panne.Dégat - valeur;
            if (dégatAprès < 1)
            {
                modules[indexCurrentModule].Panne.Dégat = 0;
                modules[indexCurrentModule].EstEnPanne = false;
                BorderPanne.Visibility = Visibility.Collapsed;
                tbReparer.Text = "Panne réparée";
                Affichage.Text = "Panne réparée";
            }
            else
                modules[indexCurrentModule].Panne.Dégat = dégatAprès;
        }

        /// <summary>
        /// OnClick listener du bouton servant à réparer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btReparerValeur_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // Liste des dés selectionnés par le joueur
            List<Boolean> SelectedDices = new List<Boolean>{ imgD1.Tag.ToString().Contains("HightLight"),
                                                            imgD2.Tag.ToString().Contains("HightLight"),
                                                            imgD3.Tag.ToString().Contains("HightLight"),
                                                            imgD4.Tag.ToString().Contains("HightLight"),
                                                            imgD5.Tag.ToString().Contains("HightLight"),
                                                            imgD6.Tag.ToString().Contains("HightLight")};


            // Si le joueur a lancé les dés et en a sélectionné au moins 1
            if (nombreLancers < 3 & SelectedDices.Contains(true))
            {
                int ValeurAReparer = 0;
                foreach (Dé dé in lancer)
                {
                    if (dé.Type.Equals(déType.Highlight) || dé.Type.Equals(déType.BléssureHighlight) || dé.Type.Equals(déType.StaseHighlight))
                    {
                        ValeurAReparer += dé.Valeur;
                        dé.Type = déType.Grisé;
                    }
                }
                this.Réparer(ValeurAReparer);
                UpdateUI();
            }
        }

        private bool PouvoirUtilisable()
        {
            if (nombreLancers == 3)
                return false;
            if (pouvoirUtilisé)
                return false;
            foreach (Dé dé in lancer)
            {
                if ((dé.Valeur.Equals(5) || dé.Valeur.Equals(6)) && (dé.Type.Equals(déType.Normal) || dé.Type.Equals(déType.Highlight) || dé.Type.Equals(déType.BléssureHighlight) || dé.Type.Equals(déType.StaseHighlight) || dé.Type.Equals(déType.Bléssure) || dé.Type.Equals(déType.Stase)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Utilisation du pouvoir spécial onTap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_PouvoirSpe_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (PouvoirUtilisable())
            {
                bool is5InList = false;
                bool is6InList = false;

                int indexDé5 = 0;
                int indexDé6 = 0;
                int i = 0;
                foreach (Dé dé in lancer)
                {
                    if (dé.Valeur.Equals(5) && (dé.Type.Equals(déType.Normal) || dé.Type.Equals(déType.Highlight) || dé.Type.Equals(déType.BléssureHighlight) || dé.Type.Equals(déType.StaseHighlight) || dé.Type.Equals(déType.Bléssure) || dé.Type.Equals(déType.Stase)))
                    {
                        is5InList = true;
                        indexDé5 = i;
                        break;
                    }
                    i++;
                }
                if (!is5InList)
                {
                    i = 0;
                    foreach (Dé dé in lancer)
                    {
                        if (dé.Valeur.Equals(6) && (dé.Type.Equals(déType.Normal) || dé.Type.Equals(déType.Highlight) || dé.Type.Equals(déType.BléssureHighlight) || dé.Type.Equals(déType.StaseHighlight) || dé.Type.Equals(déType.Bléssure) || dé.Type.Equals(déType.Stase)))
                        {
                            is6InList = true;
                            indexDé6 = i;
                            break;
                        }
                        i++;
                    }
                }

                Debug.WriteLine("Lancer a au moins un 5 ? " + is5InList.ToString());
                Debug.WriteLine("Lancer a au moins un 6 ? " + is5InList.ToString());

                if (is5InList || is6InList)
                {
                    if (is5InList)
                        lancer[indexDé5].Type = déType.Grisé;
                    if (is6InList)
                        lancer[indexDé6].Type = déType.Grisé;

                    Debug.WriteLine("Le " + membres[indexCurrentMembre].Role.ToString() + " utilise son pouvoir !!!");

                    // Commandant : 10 de réparation
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Commandant))
                        Réparer(10);

                    // Capitaine : +1 Dés pour chaque membre
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Capitaine))
                    {
                        foreach (Membre m in membres)
                        {
                            if (m.NombreDeDés < 6)
                                m.NombreDeDés++;
                        }
                        utilisationPouvoirCapitaine = true;
                    }

                    // Docteur : +1 pv pour chaque membre
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Capitaine))
                    {
                        foreach (Membre m in membres)
                        {
                            if (m.Pv < 6 && m.Pv > 0)
                                m.Pv++;
                        }
                    }

                    // Mécanicien : +1 pv au vaisseau
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Mécanicien))
                    {
                        if (vaisseau.Pv < 10)
                            vaisseau.Pv++;
                    }
                }
                UpdateUI();
                pouvoirUtilisé = true;
            }
        }
    }
}

