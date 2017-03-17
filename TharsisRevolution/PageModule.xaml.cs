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
using Windows.UI.Xaml.Controls.Primitives;

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
        private MessageDialog msgbox;

        private static readonly Random rdm = new Random();
        private static readonly object syncLock = new object();

        /// <summary>
        /// Fonction de random entre 2 entiers
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return rdm.Next(min, max);
            }
        }

        /// <summary>
        /// Constructeur, initialise les Component de l'UI
        /// </summary>
        public PageModule()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Fonction appelée quand on navigate vers cette page, 
        /// Initialisation des variables et possibles assignation des variables aux valeurs retransmises par les autres pages
        /// </summary>
        /// <param name="e"></param>
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

            BitmapImage pilotage = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Pilotage.png"));
            BitmapImage serre = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Serre.png"));
            BitmapImage inf = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Infirmerie.png"));
            BitmapImage detente = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Detente.png"));
            BitmapImage maint = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Maintenance.png"));
            BitmapImage labo = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Laboratoire.png"));
            BitmapImage survie = new BitmapImage(new Uri("ms-appx:///Assets/FD_Module_Survie.png"));

            switch (modules[indexCurrentModule].Type.ToString())
            {
                case "PostePilotage":
                    Background_PageModule.ImageSource = pilotage;
                    break;
                case "Serre":
                    Background_PageModule.ImageSource = serre;
                    break;
                case "SystemeSurvie":
                    Background_PageModule.ImageSource = survie;
                    break;
                case "Maintenance":
                    Background_PageModule.ImageSource = maint;
                    break;
                case "Infirmerie":
                    Background_PageModule.ImageSource = inf;
                    break;
                case "Détente":
                    Background_PageModule.ImageSource = detente;
                    break;
                case "Laboratoire":
                    Background_PageModule.ImageSource = labo;
                    break;
                default:
                    break;
            }

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
                if (listeDéPiégés.Count > 0)
                    TooltipPiege.Text = tooltipPiege;
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
            pg_Personnage.Value = membres[indexCurrentMembre].pv;

            btLancer1.Background = new SolidColorBrush(Colors.White);
            btLancer1.Foreground = new SolidColorBrush(Colors.Black);
        }

        /// <summary>
        /// Fonction à appeler à chaque changement dans l'UI
        /// </summary>
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

            pg_Personnage.Value = membres[indexCurrentMembre].pv;
            btReparerValeur.Content = modules[indexCurrentModule].Panne.Dégat;
        }

        /// <summary>
        /// Fonction déclencher lors du clique sur le bouton Finir le déploiement du module pour le retour à la page principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonTerminer_Click(object sender, RoutedEventArgs e)
        {
            membres[indexCurrentMembre].AJoué = true;
            var parameters = new CurrentParameters(membres, modules, indexCurrentMembre, indexCurrentModule, hardMode, vaisseau, numeroSemaine, gameStarted);

            this.Frame.Navigate(typeof(MainPage), parameters);
        }

        /// <summary>
        /// Fonction qui permet le lancement des Dés d'un personnage aléatoirement
        /// Vérifier si le pouvoir d'un personnage peut etre activé
        /// Et gestion des Dés en mode difficile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ButtonLancer_Click(object sender, RoutedEventArgs e)
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
                                    case déType.Bléssure:
                                        lancer[i].Type = déType.Bléssure;
                                        membres[indexCurrentMembre].Pv--;
                                        msgbox = new MessageDialog("Le dé " + dé.Valeur + " vous inflige 1 point de dégat.");
                                        await msgbox.ShowAsync();
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
            {
                msgbox = new MessageDialog("Vous n'avez plus de lancers !");
                await msgbox.ShowAsync();
            }
        }

        /// <summary>
        /// Fonction activée lors du clic sur un des dés de la barre de lancer
        ///Highlight et suppression de l'highlight en fonction de l'état du dé au moment du clic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void imDice_Tapped(object sender, TappedRoutedEventArgs e)
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
                    msgbox = new MessageDialog("Ce dé est inutilisable...");
                    await msgbox.ShowAsync();
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

        /// <summary>
        /// Fonction que permet de réparer la panne d'un module en lui indiquant la valeur des Dés selectionner pour reparer la panne
        /// </summary>
        /// <param name="valeur"></param>
        private async void Réparer(int valeur)
        {
            int dégatAvant = modules[indexCurrentModule].Panne.Dégat;
            int dégatAprès = modules[indexCurrentModule].Panne.Dégat - valeur;
            if (dégatAprès < 1)
            {
                modules[indexCurrentModule].Panne.Dégat = 0;
                modules[indexCurrentModule].EstEnPanne = false;
                BorderPanne.Visibility = Visibility.Collapsed;
                tbReparer.Text = "Panne réparée";
                msgbox = new MessageDialog("Panne réparée !");
                await msgbox.ShowAsync();
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

        /// <summary>
        /// Fonction qui permet de mettre à jour l'affichage du niveau de la barre de vie du personnage dans le module
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvPersonnage(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvPersonnage.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Fonction qui permet la vérification de l'utilisation du pouvoir du personnage dans le module
        /// </summary>
        /// <returns></returns>
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
        private async void bt_PouvoirSpe_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog msg = new MessageDialog("");
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
                    {
                        Réparer(10);
                        msg = new MessageDialog("Le Commandant a utilisé son pouvoir de réparation de la panne !","Pouvoir");
                    }

                    // Capitaine : +1 Dés pour chaque membre
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Capitaine))
                    {
                        foreach (Membre m in membres)
                        {
                            if (m.NombreDeDés < 6)
                                m.NombreDeDés++;
                        }
                        utilisationPouvoirCapitaine = true;
                        msg = new MessageDialog("Le Capitaine a utilisé son pouvoir et offre 1 Dé à chaques personnages !", "Pouvoir");
                    }

                    // Docteur : +1 pv pour chaque membre
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Docteur))
                    {
                        foreach (Membre m in membres)
                        {
                            if (m.Pv < 6 && m.Pv > 0)
                                m.Pv++;
                        }
                        msg = new MessageDialog("Le Docteur a utilisé son pouvoir de guérison !", "Pouvoir");
                    }

                    // Mécanicien : +1 pv au vaisseau
                    if (membres[indexCurrentMembre].Role.Equals(Membre.roleMembre.Mécanicien))
                    {
                        if (vaisseau.Pv < 10)
                            vaisseau.Pv++;
                        msg = new MessageDialog("Le Mécanicien a utilisé son pouvoir de réparation du vaisseau !", "Pouvoir");

                    }
                }
                UpdateUI();
                pouvoirUtilisé = true;
                await msg.ShowAsync();
            }
        }

        /// <summary>
        /// Fonction qui permet de fournir des informations sur des élements durant le jeu au passage de la souris
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fct_Information_PointerEnter(object sender, PointerRoutedEventArgs e)
        {
            Button btn = new Button(); 
            Image img = new Image(); 
            if(sender.GetType() == btn.GetType())
            {
                 btn = (Button)sender;
            }
            else
            {
                img = (Image)sender;
            }

            switch (btn.Name)
            {
                case "btLancer1":
                    Affichage.Text = "Lance les dés pour réparer la panne.";
                    break;
                case "bt_PouvoirSpe":
                    Affichage.Text = "Utilisation du pouvoir du personnage avec un dés de 5 ou 6.";
                    break;
                case "btReparerValeur":
                    Affichage.Text = "Nombre de points de panne restant à réparer.";
                    break;
                default:
                    break;
            }

            if (img.Name.Contains("imgD"))
            {
                Affichage.Text = "Cliquer sur un Dés pour le verrouiller ou l'utiliser.";
            }    
        }

        /// <summary>
        /// Fonction pour vider le textbox qui donne des information à la sortie de la souris de l'element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fct_Information_PointerExit(object sender, PointerRoutedEventArgs e)
        {
            Affichage.Text = "";
        }
    }
}

