﻿using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI;


//TODO mettre un fond en fonction du module
//TODO VIRER TOUS LES ACCENTS DANS LE C#
//TODO enable le bouton quand pouvoir possible (dé 5 ou 6), disable quand pas possible

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
        private List<Panne> pannes;
        private List<Dé> lancer;
        private List<Dé> listeDéPiégés;
        private bool hardMode;
        private int nombreLancers = 3;
        private Vaisseau vaisseau;
        private int numeroSemaine;
        private bool pouvoirUtilisé = false;
        private bool utilisationPouvoirCapitaine = false;
        private bool gameStarted = true;

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

            //TODO interface : Afficher le nom du module et le nom du membre à l'interieur
            // ça devrait le faire mais exception??
            //rtTitreModule.Text = modules[indexCurrentModule].Type.ToString();
            //rtNomPersonnage.Text = membres[indexCurrentMembre].Role.ToString();

            imgD7.Source = new BitmapImage(new Uri("ms-appx:/Assets/D1.png", UriKind.RelativeOrAbsolute));
            imgD8.Source = new BitmapImage(new Uri("ms-appx:/Assets/D1.png", UriKind.RelativeOrAbsolute));
            imgD9.Source = new BitmapImage(new Uri("ms-appx:/Assets/D1.png", UriKind.RelativeOrAbsolute));

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
            pannes = parameters.Pannes;
            numeroSemaine = parameters.NumeroSemaine;

            btReparerValeur.Content = modules[indexCurrentModule].Panne.Dégat;

            if (hardMode)
            {
                // TODO Mettre les 3 dés piégés en image qui sont dans listeDéPiégés (c'est une liste d'objet Dé) chaque dé a sa valeur et son type de piege
                int nombreDésPiégés = RandomNumber(0, 4);

                listeDéPiégés = new List<Dé>(new Dé[nombreDésPiégés]);
                int index = 0;
                // TODO a verifier
                if (listeDéPiégés.Count > 0)
                {
                    for (int i = 0; i < nombreDésPiégés; i++)
                        listeDéPiégés[i] = new Dé();

                    foreach (Dé déPiégé in listeDéPiégés)
                    {
                        if (index < listeDéPiégés.Count - 1)
                        {
                            while (listeDéPiégés[index].Valeur == listeDéPiégés[index + 1].Valeur)
                            {
                                listeDéPiégés[index + 1].Valeur = RandomNumber(1, 7);
                            }
                        }
                        int random = RandomNumber(0, 3);
                        switch (random)
                        {
                            case 0:
                                déPiégé.Type = déType.Bléssure;
                                break;
                            case 1:
                                déPiégé.Type = déType.Caduc;
                                break;
                            case 2:
                                déPiégé.Type = déType.Stase;
                                break;
                        }
                        index++;
                    }
                }
            }
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
                    default:
                        i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[index].Valeur + ".png", UriKind.RelativeOrAbsolute));
                        i.Tag = "D" + lancer[index].Valeur + ".png";
                        break;
                }
                index++;
            }

            btReparerValeur.Content = modules[indexCurrentModule].Panne.Dégat;
        }

        private void ButtonTerminer_Click(object sender, RoutedEventArgs e)
        {
            membres[indexCurrentMembre].AJoué = true;
            var parameters = new CurrentParameters(membres, modules, pannes, indexCurrentMembre, indexCurrentModule, hardMode, vaisseau, numeroSemaine, gameStarted);

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
                        Debug.WriteLine("Lancer " + j + " : " + lancer[j].Valeur);
                    }
                    else
                    {
                        if (lancer[j].Type.Equals(déType.Normal))
                        {
                            lancer[j] = new Dé();
                            i.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + lancer[j].Valeur + ".png", UriKind.RelativeOrAbsolute));
                            i.Tag = "D" + lancer[j].Valeur + ".png";
                            Debug.WriteLine("Lancer " + j + " : " + lancer[j].Valeur);
                        }
                    }
                    j++;
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
                            if (dé.Valeur.Equals(lancer[i]))
                            {
                                switch (dé.Type)
                                {
                                    // TODO mettre des messages d'alertes quand on subit un malus
                                    case déType.Bléssure:
                                        lancer[i].Type = déType.Bléssure;
                                        Debug.WriteLine("Dé emplacement : " + i + " = bléssure, -1 pv pour le membre");
                                        membres[indexCurrentMembre].Pv--;
                                        break;
                                    case déType.Stase:
                                        lancer[i].Type = déType.Stase;
                                        Debug.WriteLine("Dé emplacement : " + i + " = stase, à rendre non relancable mais utilisable pour capacité ou réparation");
                                        // TODO Dé impossible à relancer mais utilisable pour capacité spéciale ou réparation (highlighté obligatoire quoi)
                                        break;
                                    case déType.Caduc:
                                        lancer[i].Type = déType.Caduc;
                                        Debug.WriteLine("Dé emplacement : " + i + " = caduc, à rendre non relancable, non utilisable");
                                        // TODO Griser le dé
                                        break;
                                }
                            }
                        }
                    }
                }

                nombreLancers--;
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
                // TODO : Bug sur la condition : quand un dés devient grisé(lock) apres l'avoir utiliser
                // pour réparer, quand on clic dessus  = il redevient highlight
                if (!s.Contains("HightLight") && !s.Contains("Lock"))
                {
                    b.Source = new BitmapImage(new Uri("ms-appx:/Assets/D" + s[1] + "_HightLight.png", UriKind.RelativeOrAbsolute));
                    b.Tag = "D" + s[1] + "_HightLight.png";
                    lancer[indexDé].Type = déType.Highlight;

                    // Surbrillance des actions possible et activation du bouton de réparation
                    btLancer1.Background = new SolidColorBrush(Colors.White);
                    btLancer1.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.Background = new SolidColorBrush(Colors.White);
                    btReparerValeur.Foreground = new SolidColorBrush(Colors.Black);
                    btReparerValeur.IsEnabled = true;
                }
                else
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
                        btLancer1.Background = btTerminer.Background;
                        btLancer1.Foreground = new SolidColorBrush(Colors.White);
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

        private void VerouillerDé()
        {
            //Griser le dé ciblé
            //Ne devra pas être relancé si on utilise le bouton relance
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
                    if (dé.Type.Equals(déType.Highlight))
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
        /// Utilisation du pouvoir spécial onTap
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bt_PouvoirSpe_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!pouvoirUtilisé)
            {
                bool is5InList = false;
                bool is6InList = false;

                int indexDé5 = 0;
                int indexDé6 = 0;
                int i = 0;
                foreach (Dé dé in lancer)
                {
                    if (dé.Valeur.Equals(5) && (dé.Type.Equals(déType.Normal) || dé.Type.Equals(déType.Highlight)))
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
                        if (dé.Valeur.Equals(6) && (dé.Type.Equals(déType.Normal) || dé.Type.Equals(déType.Highlight)))
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

