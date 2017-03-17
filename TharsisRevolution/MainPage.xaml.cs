using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace TharsisRevolution
{
    /// <summary>
    /// Page principale du jeu
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Vaisseau vaisseau;
        private List<Module> modules;
        private List<Membre> membres;
        private int numeroSemaine;
        private bool hardMode;
        private bool gameStarted = false;

        private int indexCurrentClickMembre;
        private int indexCurrentClickModule;
        private int rowCurrentModule;
        private int columnCurrentModule;
        SolidColorBrush color_Blanc = new SolidColorBrush(Colors.White);
        SolidColorBrush color_Black = new SolidColorBrush(Colors.Black);
        // Bonne fonction randon
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
        /// Fonction appelée quand on navigate vers cette page, 
        /// Initialisation des variables et possibles assignation des variables aux valeurs retransmises par les autres pages
        /// Update de l'UI
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Initialiser();

            if (e.Parameter is GameParameters)
            {
                var parameters = (GameParameters)e.Parameter;
                hardMode = parameters.HardMode;
            }
            if (e.Parameter is CurrentParameters)
            {
                var parameters = (CurrentParameters)e.Parameter;
                membres = parameters.Membres;
                modules = parameters.Modules;
                indexCurrentClickMembre = parameters.IndexCurrentMembre;
                indexCurrentClickModule = parameters.IndexCurrentModule;
                vaisseau = parameters.Vaisseau;
                numeroSemaine = parameters.NumeroSemaine;
                gameStarted = parameters.GameStarted;
                hardMode = parameters.HardMode;
            }
            if (!gameStarted)
                NouvelleSemaine();
            gameStarted = true;

            UpdateUI();
        }

        /// <summary>
        /// Constructeur, initialise les Component de l'UI
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Positionne les cadres des personnages avec leurs PV et Dés
        /// </summary>
        private void InitialiserValeursPersonnages()
        {
            for (int i = 0; i < 4; i++)
            {
                //Initialisation emplacement personnage visuelement avec une fonction prévu spécialement pour l'initialisation des positions de départ
                Deplacement_PersonnageToCurrentModule(membres[i].Role, membres[i].Position);
                Debug.WriteLine("Membre " + membres[i].Role + " à la position " + membres[i].Position.Type);
            }

            //initialisation des paramètres du Personnage Commandant
            pb_PvCommandant.Value = membres[0].Pv;
            switch (membres[0].NombreDeDés)
            {
                case 0:
                    Com_d1.Fill = color_Black;
                    Com_d2.Fill = color_Black;
                    Com_d3.Fill = color_Black;
                    Com_d4.Fill = color_Black;
                    Com_d5.Fill = color_Black;
                    Com_d6.Fill = color_Black;
                    break;
                case 1:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Black;
                    Com_d3.Fill = color_Black;
                    Com_d4.Fill = color_Black;
                    Com_d5.Fill = color_Black;
                    Com_d6.Fill = color_Black;
                    break;
                case 2:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Blanc;
                    Com_d3.Fill = color_Black;
                    Com_d4.Fill = color_Black;
                    Com_d5.Fill = color_Black;
                    Com_d6.Fill = color_Black;
                    break;
                case 3:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Blanc;
                    Com_d3.Fill = color_Blanc;
                    Com_d4.Fill = color_Black;
                    Com_d5.Fill = color_Black;
                    Com_d6.Fill = color_Black;
                    break;
                case 4:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Blanc;
                    Com_d3.Fill = color_Blanc;
                    Com_d4.Fill = color_Blanc;
                    Com_d5.Fill = color_Black;
                    Com_d6.Fill = color_Black;
                    break;
                case 5:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Blanc;
                    Com_d3.Fill = color_Blanc;
                    Com_d4.Fill = color_Blanc;
                    Com_d5.Fill = color_Blanc;
                    Com_d6.Fill = color_Black;
                    break;
                case 6:
                    Com_d1.Fill = color_Blanc;
                    Com_d2.Fill = color_Blanc;
                    Com_d3.Fill = color_Blanc;
                    Com_d4.Fill = color_Blanc;
                    Com_d5.Fill = color_Blanc;
                    Com_d6.Fill = color_Blanc;
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Capitaine
            pb_PvCapitaine.Value = membres[1].Pv;
            switch (membres[1].NombreDeDés)
            {
                case 0:
                    Cap_d1.Fill = color_Black;
                    Cap_d2.Fill = color_Black;
                    Cap_d3.Fill = color_Black;
                    Cap_d4.Fill = color_Black;
                    Cap_d5.Fill = color_Black;
                    Cap_d6.Fill = color_Black;
                    break;
                case 1:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Black;
                    Cap_d3.Fill = color_Black;
                    Cap_d4.Fill = color_Black;
                    Cap_d5.Fill = color_Black;
                    Cap_d6.Fill = color_Black;
                    break;
                case 2:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Blanc;
                    Cap_d3.Fill = color_Black;
                    Cap_d4.Fill = color_Black;
                    Cap_d5.Fill = color_Black;
                    Cap_d6.Fill = color_Black;
                    break;
                case 3:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Blanc;
                    Cap_d3.Fill = color_Blanc;
                    Cap_d4.Fill = color_Black;
                    Cap_d5.Fill = color_Black;
                    Cap_d6.Fill = color_Black;
                    break;
                case 4:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Blanc;
                    Cap_d3.Fill = color_Blanc;
                    Cap_d4.Fill = color_Blanc;
                    Cap_d5.Fill = color_Black;
                    Cap_d6.Fill = color_Black;
                    break;
                case 5:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Blanc;
                    Cap_d3.Fill = color_Blanc;
                    Cap_d4.Fill = color_Blanc;
                    Cap_d5.Fill = color_Blanc;
                    Cap_d6.Fill = color_Black;
                    break;
                case 6:
                    Cap_d1.Fill = color_Blanc;
                    Cap_d2.Fill = color_Blanc;
                    Cap_d3.Fill = color_Blanc;
                    Cap_d4.Fill = color_Blanc;
                    Cap_d5.Fill = color_Blanc;
                    Cap_d6.Fill = color_Blanc;
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Docteur
            pb_PvDocteur.Value = membres[2].Pv;
            switch (membres[2].NombreDeDés)
            {
                case 0:
                    doc_d1.Fill = color_Black;
                    doc_d2.Fill = color_Black;
                    doc_d3.Fill = color_Black;
                    doc_d4.Fill = color_Black;
                    doc_d5.Fill = color_Black;
                    doc_d6.Fill = color_Black;
                    break;
                case 1:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Black;
                    doc_d3.Fill = color_Black;
                    doc_d4.Fill = color_Black;
                    doc_d5.Fill = color_Black;
                    doc_d6.Fill = color_Black;
                    break;
                case 2:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Blanc;
                    doc_d3.Fill = color_Black;
                    doc_d4.Fill = color_Black;
                    doc_d5.Fill = color_Black;
                    doc_d6.Fill = color_Black;
                    break;
                case 3:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Blanc;
                    doc_d3.Fill = color_Blanc;
                    doc_d4.Fill = color_Black;
                    doc_d5.Fill = color_Black;
                    doc_d6.Fill = color_Black;
                    break;
                case 4:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Blanc;
                    doc_d3.Fill = color_Blanc;
                    doc_d4.Fill = color_Blanc;
                    doc_d5.Fill = color_Black;
                    doc_d6.Fill = color_Black;
                    break;
                case 5:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Blanc;
                    doc_d3.Fill = color_Blanc;
                    doc_d4.Fill = color_Blanc;
                    doc_d5.Fill = color_Blanc;
                    doc_d6.Fill = color_Black;
                    break;
                case 6:
                    doc_d1.Fill = color_Blanc;
                    doc_d2.Fill = color_Blanc;
                    doc_d3.Fill = color_Blanc;
                    doc_d4.Fill = color_Blanc;
                    doc_d5.Fill = color_Blanc;
                    doc_d6.Fill = color_Blanc;
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Mecanicien
            pbPvMeca.Value = membres[3].Pv;
            switch (membres[3].NombreDeDés)
            {
                case 0:
                    Meca_d1.Fill = color_Black;
                    Meca_d2.Fill = color_Black;
                    Meca_d3.Fill = color_Black;
                    Meca_d4.Fill = color_Black;
                    Meca_d5.Fill = color_Black;
                    Meca_d6.Fill = color_Black;
                    break;
                case 1:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Black;
                    Meca_d3.Fill = color_Black;
                    Meca_d4.Fill = color_Black;
                    Meca_d5.Fill = color_Black;
                    Meca_d6.Fill = color_Black;
                    break;
                case 2:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Blanc;
                    Meca_d3.Fill = color_Black;
                    Meca_d4.Fill = color_Black;
                    Meca_d5.Fill = color_Black;
                    Meca_d6.Fill = color_Black;
                    break;
                case 3:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Blanc;
                    Meca_d3.Fill = color_Blanc;
                    Meca_d4.Fill = color_Black;
                    Meca_d5.Fill = color_Black;
                    Meca_d6.Fill = color_Black;
                    break;
                case 4:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Blanc;
                    Meca_d3.Fill = color_Blanc;
                    Meca_d4.Fill = color_Blanc;
                    Meca_d5.Fill = color_Black;
                    Meca_d6.Fill = color_Black;
                    break;
                case 5:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Blanc;
                    Meca_d3.Fill = color_Blanc;
                    Meca_d4.Fill = color_Blanc;
                    Meca_d5.Fill = color_Blanc;
                    Meca_d6.Fill = color_Black;
                    break;
                case 6:
                    Meca_d1.Fill = color_Blanc;
                    Meca_d2.Fill = color_Blanc;
                    Meca_d3.Fill = color_Blanc;
                    Meca_d4.Fill = color_Blanc;
                    Meca_d5.Fill = color_Blanc;
                    Meca_d6.Fill = color_Blanc;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Positionne les pannes dans l'interface
        /// </summary>
        private void InitialiserUIPannes()
        {
            BitmapImage imagePanne = new BitmapImage(new Uri("ms-appx:///Assets/Ambox_warning_red.png"));
            Image img_Panne = new Image();

            for (int i = 9; i < Grid_Jeu.Children.Count; i++) // Supprimer les pannes
            {
                UIElement item = Grid_Jeu.Children.ElementAt(i);
                if (item.GetType() == img_Panne.GetType())
                {
                    Grid_Jeu.Children.Remove(item);
                }
            }

            foreach (Module module in modules)
            {
                if (module.EstEnPanne)
                {
                    Debug.WriteLine("Module " + module.Type.ToString() + " est en " + module.Panne.TaillePanne + " panne");

                    img_Panne = new Image();
                    img_Panne.Source = imagePanne;
                    img_Panne.Name = "panne";
                    img_Panne.AccessKey = "panne";
                    img_Panne.Width = 40;
                    img_Panne.Height = 40;
                    img_Panne.IsTapEnabled = true;
                    img_Panne.PointerEntered += imgPointerEnter;
                    img_Panne.PointerExited += imgPointerExit;
                    img_Panne.Tapped += informationPanne_OnClick;

                    //positionnement de l'image de panne
                    Grid.SetColumn(img_Panne, module.EmplacementX);
                    Grid.SetRow(img_Panne, module.EmplacementY);
                    this.Grid_Jeu.Children.Add(img_Panne);
                }
            }
        }

        /// <summary>
        /// Permet d'afficher la main pour le passage au dessus des pannes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgPointerEnter(object sender, PointerRoutedEventArgs e)
        {
            CoreCursor handCursor = new CoreCursor(CoreCursorType.Hand, 1);
            Window.Current.CoreWindow.PointerCursor = handCursor;
        }

        /// <summary>
        /// Permet de rafficher la souris lors que l'on sort de l'image de la panne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgPointerExit(object sender, PointerRoutedEventArgs e)
        {
            CoreCursor arrowCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            Window.Current.CoreWindow.PointerCursor = arrowCursor;
        }

        /// <summary>
        /// Fonction à appeler à chaque changement dans l'UI
        /// </summary>
        private void UpdateUI()
        {
            this.pg_PvShip.Value = vaisseau.Pv;
            this.slider_Semaines.Value = numeroSemaine;

            InitialiserValeursPersonnages();
            InitialiserUIPersonnages();
            InitialiserUIPannes();
        }

        /// <summary>
        /// Initialisation de la partie, création des objets, placement des membres à leurs positions initiales randomisées et différentes
        /// </summary>
        private void Initialiser()
        {
            //Initialisation Vaisseau
            vaisseau = new Vaisseau();

            //Initialisation des Moduless //modif thomas (proposition)
            modules = new List<Module> {
                new Module(Module.moduleType.PostePilotage,Grid.GetColumn(PostePilotage),Grid.GetRow(PostePilotage)),
                new Module(Module.moduleType.Serre,Grid.GetColumn(Serre),Grid.GetRow(Serre)),
                new Module(Module.moduleType.Infirmerie,Grid.GetColumn(Infirmerie),Grid.GetRow(Infirmerie)),
                new Module(Module.moduleType.Laboratoire,Grid.GetColumn(Laboratoire),Grid.GetRow(Laboratoire)),
                new Module(Module.moduleType.Détente,Grid.GetColumn(Détente),Grid.GetRow(Détente)),
                new Module(Module.moduleType.SystemeSurvie,Grid.GetColumn(SystemeSurvie),Grid.GetRow(SystemeSurvie)),
                new Module(Module.moduleType.Maintenance,Grid.GetColumn(Maintenance),Grid.GetRow(Maintenance)),
            };

            //Initialisation des Membres
            membres = new List<Membre> {
                new Membre(Membre.roleMembre.Commandant,1),
                new Membre(Membre.roleMembre.Capitaine,2),
                new Membre(Membre.roleMembre.Docteur,3),
                new Membre(Membre.roleMembre.Mécanicien,4)
            };

            int randomModule = RandomNumber(0, 7); //va de 0 à 6, à verifier

            // Positionnement des membres dans les modules // modif Thomas (pas certain mais proposition)
            for (int i = 0; i < 4; i++)
            {
                //Tant qu'on tombe sur un module avec um membre à l'interieur, recommencer.
                while (modules[randomModule].PresenceMembre)
                    randomModule = RandomNumber(0, 7);
                membres[i].Position = modules[randomModule];
                modules[randomModule].PresenceMembre = true; //thomas (permet de faire fonctionner ta boucle while au dessus)
            }

            //Initialisation Semaine
            numeroSemaine = 1;
        }

        /// <summary>
        /// Fonction pour afficher des informations sur la panne au click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void informationPanne_OnClick(object sender, TappedRoutedEventArgs e)
        {
            int x = Grid.GetColumn((FrameworkElement)sender);
            int y = Grid.GetRow((FrameworkElement)sender);
            string coordonnee = x.ToString() + y.ToString();
            MessageDialog msgbox;

            switch (coordonnee)
            {
                case "02":
                    //pilotage
                    msgbox = new MessageDialog("Panne " + modules[0].Panne.TaillePanne + " de " + modules[0].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "12":
                    //serre
                    msgbox = new MessageDialog("Panne " + modules[1].Panne.TaillePanne + " de " + modules[1].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "22":
                    //infi
                    msgbox = new MessageDialog("Panne " + modules[2].Panne.TaillePanne + " de " + modules[2].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "32":
                    //detente
                    msgbox = new MessageDialog("Panne " + modules[4].Panne.TaillePanne + " de " + modules[4].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "42":
                    //maintenance
                    msgbox = new MessageDialog("Panne " + modules[6].Panne.TaillePanne + " de " + modules[6].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "21":
                    //labo
                    msgbox = new MessageDialog("Panne " + modules[3].Panne.TaillePanne + " de " + modules[3].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "33":
                    //survie
                    msgbox = new MessageDialog("Panne " + modules[5].Panne.TaillePanne + " de " + modules[5].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Renvoi true si le vaisseau est rempli de panne
        /// </summary>
        /// <param name="numeroSemaine"></param>
        private bool RempliDePanne()
        {
            foreach (Module module in modules)
            {
                if(!module.EstEnPanne)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Création d'un certain nombre de panne en fonction du jour de la semaine et assignation de la panne à son module
        /// </summary>
        /// <param name="numeroSemaine"></param>
        private void CreationPannes(int numeroSemaine)
        {
            if (!RempliDePanne())
            {
                int random = RandomNumber(0, 7);
                Debug.WriteLine("Création de panne pour la semaine " + numeroSemaine);
                switch (numeroSemaine)
                {
                    case 1:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(22, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(23, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 2:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(20, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(21, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 3:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(17, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(18, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(19, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 4:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(15, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(16, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 5:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(13, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(14, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 6:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(10, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(11, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(12, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 7:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(8, Panne.taille.Grosse, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(9, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 8:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(5, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(6, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(7, Panne.taille.Petite, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 9:
                        random = RandomNumber(0, 7);
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(2, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(3, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        while (modules[random].EstEnPanne)
                            random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(4, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    case 10:
                        random = RandomNumber(0, 7);
                        modules[random].Panne = new Panne(1, Panne.taille.Moyenne, hardMode);
                        modules[random].EstEnPanne = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Dégat infligé par une Panne (fait en fin de semaine)
        /// </summary>
        /// <param name="module"></param>
        private async void PanneInfligeDégat(Module module)
        {            
            int typeDegat = RandomNumber(1, 4);
            MessageDialog infobox = new MessageDialog("");
            switch (typeDegat)
            {                             
                case 1: //Perte de vie membres
                    int dégatMembre = RandomNumber(1, 4);
                    infobox = new MessageDialog("Une panne restante a infligé "+dégatMembre+" point(s) de Dégat à tous les personnages !","Information");
                    foreach (Membre membre in membres)
                        membre.Pv = membre.Pv - dégatMembre;

                    break;
                case 2: //Perte de vie vaisseau
                    int dégatVaisseau = RandomNumber(1, 4);
                    infobox = new MessageDialog("Une panne restante a infligé "+dégatVaisseau+" point(s) de Dégat à votre vaisseau !","Information");
                    vaisseau.Pv = vaisseau.Pv - dégatVaisseau;

                    break;
                case 3: //Perte de dés membres
                    int perteDés = RandomNumber(1, 4);
                    foreach (Membre membre in membres)
                    {
                        if (membre.NombreDeDés - perteDés < 1)
                        {
                            membre.NombreDeDés = 1;
                        }
                        else
                        {
                            infobox = new MessageDialog("Une panne restante a fait perdre "+perteDés+" Dé(s) à tous les personnages !","Information");
                            membre.NombreDeDés = membre.NombreDeDés - perteDés;

                        }
                    }
                    break;
                default:
                    infobox = new MessageDialog("Vous n'avez subis aucun dommage en fin de semaine.","Information");
                    break;
            }
            if(infobox.Content.ToString() != "")
            {
                await infobox.ShowAsync();
            }

        }

        /// <summary>
        /// Activation au début d'une nouvelle semaine
        /// </summary>
        private void NouvelleSemaine()
        {
            Debug.WriteLine("Nouvelle semaine");
            foreach (Membre membre in membres)
                membre.AJoué = false;

            CreationPannes(numeroSemaine);
        }

        /// <summary>
        /// Bool renvoyant true si tous les membres ont joué cette semaine
        /// </summary>
        /// <returns></returns>
        private bool MembresOntJoué()
        {
            int nombreMembreQuiOntJoué = 0;
            foreach (Membre membre in membres)
            {
                if (membre.AJoué)
                    nombreMembreQuiOntJoué++;
            }
            if (nombreMembreQuiOntJoué == membres.Count)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gere les evenements arrivant en fin de semaine, une fois que tous les membres ont joués
        /// A appeler si MembresOntJoué()
        /// </summary>
        private async void FinSemaine()
        {
            Debug.WriteLine("Fin semaine");
            MessageDialog infobox = new MessageDialog("");
            foreach (Membre membre in membres)
            {
                if (membre.NombreDeDés > 1)
                {
                    infobox = new MessageDialog("Les personnages perdent leur dé de fin de semaine...","Fin de semaine");
                    membre.NombreDeDés--;                    
                }
            }

            foreach (Module module in modules)
            {
                if (module.EstEnPanne)
                    PanneInfligeDégat(module);
            }

            if (vaisseau.Pv < 1 || !unMembreEnVie())
                Defaite();

            if (numeroSemaine == 10 && vaisseau.Pv < 1 && unMembreEnVie())
                Victoire();

            numeroSemaine++;
            await infobox.ShowAsync();
        }

        /// <summary>
        /// Renvoi true au moins un membre est en vie (condition vicotire)
        /// </summary>
        /// <returns></returns>
        private bool unMembreEnVie()
        {
            foreach (Membre membre in membres)
            {
                if (membre.Pv > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Declenche la Défaite
        /// </summary>
        private async void Defaite()
        {
            MessageDialog msgbox = new MessageDialog("Vous avez perdu !","Défaite");
            await msgbox.ShowAsync();
            this.Frame.Navigate(typeof(Accueil));
        }

        /// <summary>
        /// Déclenche la Victoire
        /// </summary>
        private async void Victoire()
        {
            MessageDialog msgbox = new MessageDialog("Bravo, vous avez gagner !", "Victoire");
            await msgbox.ShowAsync();
            this.Frame.Navigate(typeof(Accueil));
        }
                
        /// <summary>
        /// Permet de modifier le texte des semaines en fonction de la valeur du slider du temps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_TimeSemaine_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            this.lbl_TimeSemaine.Text = msg;
        }

        /// <summary>
        /// Permet de modifier l'indicateur du niveau de la progressebar du vaisseau
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvShip(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvShip.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Permet de modifier l'indicateur du niveau de la progressebar du personnage commandant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvCommandant(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvCommandant.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Permet de modifier l'indicateur du niveau de la progressebar du personnage capitaine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvCapitaine(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvCapitaine.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Permet de modifier l'indicateur du niveau de la progressebar du personnage Docteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvDocteur(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvDocteur.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Permet de modifier l'indicateur du niveau de la progressebar du personnage Mecanicien 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressBar_PvMeca(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            try
            {
                this.lbl_PvMeca.Text = msg;
            }
            catch (NullReferenceException exe)
            {
                Debug.WriteLine("Message exeption :" + exe);
            }
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage MEcanicien
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Meca_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            if(membres[3].Pv > 0)
            {
                HightLight_Personnage(Membre.roleMembre.Mécanicien);
                indexCurrentClickMembre = 3;
            }
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Docteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Doc_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            if(membres[2].pv > 0)
            {
                HightLight_Personnage(Membre.roleMembre.Docteur);
                indexCurrentClickMembre = 2;
            }
            
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Capitaine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capitaine_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            if (membres[1].Pv > 0)
            {
                HightLight_Personnage(Membre.roleMembre.Capitaine);
                indexCurrentClickMembre = 1;
            }
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Commandant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commandant_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            if (membres[0].Pv > 0)
            {
                HightLight_Personnage(Membre.roleMembre.Commandant);
                indexCurrentClickMembre = 0;
            }
        }

        /// <summary>
        /// Fonction qui va permettre d'appliquer le hightlight sur un personnage qui sera selectionné et qu'il soit le seul lumineux montrant ainsi sa sélection
        /// </summary>
        /// <param name="perso"></param>
        private void HightLight_Personnage(Membre.roleMembre perso)
        {
            BitmapImage doc = new BitmapImage(new Uri("ms-appx:///Assets/img_Doctor.png"));
            BitmapImage meca = new BitmapImage(new Uri("ms-appx:///Assets/img_meca.png"));
            BitmapImage commandant = new BitmapImage(new Uri("ms-appx:///Assets/img_commandant.png"));
            BitmapImage capitaine = new BitmapImage(new Uri("ms-appx:///Assets/img_capitaine.png"));

            BitmapImage doc_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/img_Doctor_HightLight.png"));
            BitmapImage meca_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/img_meca_HightLight.png"));
            BitmapImage capitaine_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/img_capitaine_HightLight.png"));
            BitmapImage commandant_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/img_commandant_HightLight.png"));


            if (perso == Membre.roleMembre.Capitaine)
            {
                this.imLogoCapitaine.Source = capitaine_HightLight;
                this.reCapitaine.BorderBrush = new SolidColorBrush(Colors.Red);

                Canvas.SetZIndex(reCapitaine, 1);
                Canvas.SetZIndex(reCommandant, 0);
                Canvas.SetZIndex(reDocteur, 0);
                Canvas.SetZIndex(reMeca, 0);

                if(membres[0].Pv > 0)                
                    this.imLogoCommandant.Source = commandant;                
                if(membres[2].Pv > 0)                
                    this.imLogoDocteur.Source = doc;                
                if(membres[3].Pv > 0)
                    this.imLogoMeca.Source = meca;
                

                this.reDocteur.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reCommandant.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reMeca.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            else if (perso == Membre.roleMembre.Commandant)
            {
                this.imLogoCommandant.Source = commandant_HightLight;
                this.reCommandant.BorderBrush = new SolidColorBrush(Colors.Red);

                Canvas.SetZIndex(reCapitaine, 0);
                Canvas.SetZIndex(reCommandant, 1);
                Canvas.SetZIndex(reDocteur, 0);
                Canvas.SetZIndex(reMeca, 0);
                                
                if (membres[1].Pv > 0)
                    this.imLogoCapitaine.Source = capitaine;
                if (membres[2].Pv > 0)
                    this.imLogoDocteur.Source = doc;
                if (membres[3].Pv > 0)
                    this.imLogoMeca.Source = meca;

                this.reCapitaine.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reDocteur.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reMeca.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            else if (perso == Membre.roleMembre.Docteur)
            {
                this.imLogoDocteur.Source = doc_HightLight;
                this.reDocteur.BorderBrush = new SolidColorBrush(Colors.Red);

                Canvas.SetZIndex(reCapitaine, 0);
                Canvas.SetZIndex(reCommandant, 0);
                Canvas.SetZIndex(reDocteur, 1);
                Canvas.SetZIndex(reMeca, 0);

                if (membres[0].Pv > 0)
                    this.imLogoCommandant.Source = commandant;
                if (membres[1].Pv > 0)
                    this.imLogoCapitaine.Source = capitaine;
                if (membres[3].Pv > 0)
                    this.imLogoMeca.Source = meca;

                this.reCommandant.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reCapitaine.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reMeca.BorderBrush = new SolidColorBrush(Colors.Black);
            }
            else if (perso == Membre.roleMembre.Mécanicien)
            {
                this.imLogoMeca.Source = meca_HightLight;
                this.reMeca.BorderBrush = new SolidColorBrush(Colors.Red);

                Canvas.SetZIndex(reCapitaine, 0);
                Canvas.SetZIndex(reCommandant, 0);
                Canvas.SetZIndex(reDocteur, 0);
                Canvas.SetZIndex(reMeca, 1);

                if (membres[0].Pv > 0)
                    this.imLogoCommandant.Source = commandant;
                if (membres[1].Pv > 0)
                    this.imLogoCapitaine.Source = capitaine;
                if (membres[2].Pv > 0)
                    this.imLogoDocteur.Source = doc;

                this.reDocteur.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reCapitaine.BorderBrush = new SolidColorBrush(Colors.Black);
                this.reCommandant.BorderBrush = new SolidColorBrush(Colors.Black);
            }
        }

        /// <summary>
        /// Fonction qui va permettre d'appliquer le highlight sur un module qui sera selectionné et qu'il soit le seul lumineux montrant ainsi sa sélection + mise en place des coordonnée x,y de l'element selectionner
        /// </summary>
        /// <param name="module"></param>
        private void HightLight_Module(Module.moduleType module)
        {
            BitmapImage pilotage = new BitmapImage(new Uri("ms-appx:///Assets/Module_Pilotage2.png"));
            BitmapImage serre = new BitmapImage(new Uri("ms-appx:///Assets/Module_Serre2.png"));
            BitmapImage inf = new BitmapImage(new Uri("ms-appx:///Assets/Module_Infirmerie2.png"));
            BitmapImage detente = new BitmapImage(new Uri("ms-appx:///Assets/Module_Detente2.png"));
            BitmapImage maint = new BitmapImage(new Uri("ms-appx:///Assets/Module_Maintenance2.png"));
            BitmapImage labo = new BitmapImage(new Uri("ms-appx:///Assets/Module_Laboratoire2.png"));
            BitmapImage survie = new BitmapImage(new Uri("ms-appx:///Assets/Module_Survie2.png"));


            BitmapImage serre_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Serre_HightLight.png"));
            BitmapImage pilotage_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Pilotage_HightLight.png"));
            BitmapImage inf_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Infirmerie_HightLight.png"));
            BitmapImage detente_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Detente_HightLight.png"));
            BitmapImage maint_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Maintenance_HightLight.png"));
            BitmapImage labo_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Laboratoire_HightLight.png"));
            BitmapImage survie_HightLight = new BitmapImage(new Uri("ms-appx:///Assets/Module_Survie_HightLight.png"));

            if (module == Module.moduleType.Détente)
            {
                this.Détente.Source = detente_HightLight;
                rowCurrentModule = Grid.GetRow(Détente);
                columnCurrentModule = Grid.GetColumn(Détente);
                indexCurrentClickModule = 4;

                this.PostePilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.SystemeSurvie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Infirmerie)
            {
                this.Infirmerie.Source = inf_HightLight;
                rowCurrentModule = Grid.GetRow(Infirmerie);
                columnCurrentModule = Grid.GetColumn(Infirmerie);
                indexCurrentClickModule = 2;

                this.PostePilotage.Source = pilotage;
                this.Détente.Source = detente;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.SystemeSurvie.Source = survie;
                this.Maintenance.Source = maint; ;
            }
            else if (module == Module.moduleType.Laboratoire)
            {
                this.Laboratoire.Source = labo_HightLight;
                rowCurrentModule = Grid.GetRow(Laboratoire);
                columnCurrentModule = Grid.GetColumn(Laboratoire);
                indexCurrentClickModule = 3;

                this.PostePilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Détente.Source = detente;
                this.SystemeSurvie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Maintenance)
            {
                this.Maintenance.Source = maint_HightLight;
                rowCurrentModule = Grid.GetRow(Maintenance);
                columnCurrentModule = Grid.GetColumn(Maintenance);
                indexCurrentClickModule = 6;

                this.PostePilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.SystemeSurvie.Source = survie;
                this.Détente.Source = detente;
            }
            else if (module == Module.moduleType.PostePilotage)
            {
                this.PostePilotage.Source = pilotage_HightLight;
                rowCurrentModule = Grid.GetRow(PostePilotage);
                columnCurrentModule = Grid.GetColumn(PostePilotage);
                indexCurrentClickModule = 0;

                this.Détente.Source = detente;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.SystemeSurvie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Serre)
            {
                this.Serre.Source = serre_HightLight;
                rowCurrentModule = Grid.GetRow(Serre);
                columnCurrentModule = Grid.GetColumn(Serre);
                indexCurrentClickModule = 1;

                this.PostePilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Détente.Source = detente;
                this.Laboratoire.Source = labo;
                this.SystemeSurvie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.SystemeSurvie)
            {
                this.SystemeSurvie.Source = survie_HightLight;
                rowCurrentModule = Grid.GetRow(SystemeSurvie);
                columnCurrentModule = Grid.GetColumn(SystemeSurvie);
                indexCurrentClickModule = 5;

                this.PostePilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Détente.Source = detente;
                this.Maintenance.Source = maint;
            }
        }

        /// <summary>
        /// Déplacement d'un membre à un Module avec gestion des dégats si il traverse une panne
        /// </summary>
        /// <param name="membre"></param>
        /// <param name="moduleDestination"></param>
        private async void Deplacement(Membre membre, Module moduleDestination)
        {
            // Variable du deplacement du personnage

            int indexDeLaSalleDeDepart;
            int indexDeLaSalleChoisie;
            MessageDialog infobox;

            switch (membre.Position.Type)
            {
                case Module.moduleType.PostePilotage:
                    indexDeLaSalleDeDepart = 1;
                    break;
                case Module.moduleType.Serre:
                    indexDeLaSalleDeDepart = 2;
                    break;
                case Module.moduleType.Infirmerie:
                    indexDeLaSalleDeDepart = 3;
                    break;
                case Module.moduleType.Laboratoire:
                    indexDeLaSalleDeDepart = 4;
                    break;
                case Module.moduleType.Détente:
                    indexDeLaSalleDeDepart = 5;
                    break;
                case Module.moduleType.SystemeSurvie:
                    indexDeLaSalleDeDepart = 6;
                    break;
                case Module.moduleType.Maintenance:
                    indexDeLaSalleDeDepart = 7;
                    break;
                default:
                    indexDeLaSalleDeDepart = 1;
                    break;
            }

            switch (moduleDestination.Type)
            {
                case Module.moduleType.PostePilotage:
                    indexDeLaSalleChoisie = 1;
                    break;
                case Module.moduleType.Serre:
                    indexDeLaSalleChoisie = 2;
                    break;
                case Module.moduleType.Infirmerie:
                    indexDeLaSalleChoisie = 3;
                    break;
                case Module.moduleType.Laboratoire:
                    indexDeLaSalleChoisie = 4;
                    break;
                case Module.moduleType.Détente:
                    indexDeLaSalleChoisie = 5;
                    break;
                case Module.moduleType.SystemeSurvie:
                    indexDeLaSalleChoisie = 6;
                    break;
                case Module.moduleType.Maintenance:
                    indexDeLaSalleChoisie = 7;
                    break;
                default:
                    indexDeLaSalleChoisie = 1;
                    break;
            }


            Debug.WriteLine("Membre " + membre.Role + " PV avant déplacement " + membre.Pv);

            //Gestion de tous les cas pouvant générer des dégats au passage
            if (modules[1].EstEnPanne) // Serre en Panne
            {
                if (indexDeLaSalleChoisie == 1 && indexDeLaSalleDeDepart > 2) //Direction Pilotage depuis Infirmerie ou plus loin
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Serre en panne", "Information");
                   // Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Serre en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
                if (indexDeLaSalleChoisie > 2 && indexDeLaSalleDeDepart == 1) //Depart de Pilotage vers Infirmerie ou plus loin
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Serre en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Serre en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
            }
            if (modules[2].EstEnPanne) // Infirmerie en Panne
            {
                if (indexDeLaSalleChoisie == 4 && (indexDeLaSalleDeDepart != 3 && indexDeLaSalleDeDepart != 4)) // Direction Laboratoire si départ n'est pas Infirmerie ou Laboratoire
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
                if (indexDeLaSalleChoisie > 4 && (indexDeLaSalleDeDepart < 3 || indexDeLaSalleDeDepart == 4)) //Direction à droite de Infirmerie, départ à gauche Infirmerie ou Laboratoire
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
                if (indexDeLaSalleChoisie < 3 && indexDeLaSalleDeDepart > 3) // Direction à gauche de l'Infirmerie, départ à droite de l'Infirmerie ou Laboratoire
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant l'Infirmerie en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
            }
            if (modules[4].EstEnPanne) // Détente en Panne
            {
                if (indexDeLaSalleChoisie == 7 && (indexDeLaSalleDeDepart < 5 || indexDeLaSalleDeDepart == 6)) // Direction Maintenance, Départ Survie ou à gauche de Détente
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
                if (indexDeLaSalleChoisie == 6 && (indexDeLaSalleDeDepart < 5 || indexDeLaSalleDeDepart == 7)) // Direction Survie, Départ Maintenance ou à gauche de Détente
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
                if (indexDeLaSalleChoisie < 5 && (indexDeLaSalleDeDepart > 5)) // Direction gauche de Détente, Départ Maintenance ou Survie
                {
                    infobox = new MessageDialog("Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne", "Information");
                    //Affichage.Text = "Le " + membres[membre.Id - 1].Role.ToString() + " a subit 1 dégat en traversant la Salle de détente en panne";
                    membres[membre.Id - 1].Pv--;
                    await infobox.ShowAsync();

                }
            }

            Debug.WriteLine("Membre " + membre.Role + " PV après déplacement " + membre.Pv);

            // Défaite si on traverse une panne avec son dernier membre en vie avec un Pv...
            if (!unMembreEnVie())
                Defaite();
            
            // Déplacement
            membres[membre.Id - 1].Position = modules[indexDeLaSalleChoisie - 1];

            Debug.WriteLine("Membre " + membres[membre.Id - 1].Role + " déplacé à la position " + membres[membre.Id - 1].Position.Type.ToString());
            UpdateUI();
        }

        /// <summary>
        /// Fonction de déplacement des personnages vers un module
        /// </summary>
        private async void Deplacement_PersonnageToCurrentModule()
        {
            if (membres[indexCurrentClickMembre].AJoué) // A déjà joué
            {
                MessageDialog infoJouer = new MessageDialog("Le " + membres[indexCurrentClickMembre].Role.ToString() + " a déjà joué.", "Information");
                // Affichage.Text = "Le " + membres[indexCurrentClickMembre].Role.ToString() + " a déjà joué";
                await infoJouer.ShowAsync();
            }
            else if (membres[indexCurrentClickMembre].Pv == 0) // Mort
            {
                MessageDialog infoMort = new MessageDialog("Le " + membres[indexCurrentClickMembre].Role.ToString() + " est mort.", "Information");
                //Affichage.Text = "Le " + membres[indexCurrentClickMembre].Role.ToString() + " est mort";
                await infoMort.ShowAsync();
            }
            else
            {
                MessageDialog msgbox = new MessageDialog("Voulez vous déplacer le " + membres[indexCurrentClickMembre].Role.ToString() + " dans le module : '" + modules[indexCurrentClickModule].Type.ToString() + "' ?", "Déplacement Personnage ?");

                msgbox.Commands.Clear();
                msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
                msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });

                switch (indexCurrentClickMembre)
                {
                    case 2: //Docteur
                        var resDoc = await msgbox.ShowAsync();
                        if ((int)resDoc.Id == 0)
                        {
                            Deplacement(membres[2], modules[indexCurrentClickModule]); // Déplacement

                            if (modules[indexCurrentClickModule].EstEnPanne && (membres[2].Pv > 0)) // Déploiement seulemetn si le module est en panne et le personnage non mort
                                Creation_Btn_Deploiement();
                        }
                        break;
                    case 3: // Mécanicien
                        var resMeca = await msgbox.ShowAsync();
                        if ((int)resMeca.Id == 0)
                        {
                            Deplacement(membres[3], modules[indexCurrentClickModule]);

                            if (modules[indexCurrentClickModule].EstEnPanne && (membres[3].Pv > 0))
                                Creation_Btn_Deploiement();
                        }
                        break;
                    case 1: //Capitaine
                        var resCap = await msgbox.ShowAsync();
                        if ((int)resCap.Id == 0)
                        {
                            Deplacement(membres[1], modules[indexCurrentClickModule]);

                            if (modules[indexCurrentClickModule].EstEnPanne && (membres[1].Pv > 0))
                                Creation_Btn_Deploiement();
                        }
                        break;
                    case 0: //Commandant
                        var resCom = await msgbox.ShowAsync();
                        if ((int)resCom.Id == 0)
                        {
                            Deplacement(membres[0], modules[indexCurrentClickModule]);

                            if (modules[indexCurrentClickModule].EstEnPanne && (membres[0].Pv > 0))
                                Creation_Btn_Deploiement();
                        }
                        break;
                    default:
                        break;
                }
                UpdateUI();
            }
        }

        /// <summary>
        /// Position l'UI des personnages
        /// </summary>
        private void InitialiserUIPersonnages()
        {
            BitmapImage imgMort = new BitmapImage(new Uri("ms-appx:///Assets/mort.jpg"));
            int marginLevel = 0;
            Thickness margin;
            foreach (Membre membre in membres)
            {
                switch (membre.Role)
                {
                    case Membre.roleMembre.Commandant:
                        if (membre.Position.Type.Equals(Module.moduleType.Infirmerie) || membre.Position.Type.Equals(Module.moduleType.SystemeSurvie)) // Infirmerie || Système de survie
                            Grid.SetRow(reCommandant, membre.Position.EmplacementY + 1);
                        else
                            Grid.SetRow(reCommandant, membre.Position.EmplacementY - 1);

                        Grid.SetColumn(reCommandant, membre.Position.EmplacementX);

                        if (membre.AJoué)
                            lbl_Commandant.Foreground = new SolidColorBrush(Colors.DimGray);
                        else
                            lbl_Commandant.Foreground = new SolidColorBrush(Colors.White);

                        if (membre.Pv < 1)
                        {
                            imLogoCommandant.Source = imgMort;
                            reCommandant.IsTapEnabled = false;
                        }

                        break;
                    case Membre.roleMembre.Capitaine:
                        marginLevel = 0;
                        if (membres[0].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        margin = reCapitaine.Margin;
                        margin.Top = -15 * marginLevel;
                        margin.Right = -15 * marginLevel;
                        reCapitaine.Margin = margin;

                        if (membre.Position.Type.Equals(Module.moduleType.Infirmerie) || membre.Position.Type.Equals(Module.moduleType.SystemeSurvie)) // Infirmerie || Système de survie
                            Grid.SetRow(reCapitaine, membre.Position.EmplacementY + 1);
                        else
                            Grid.SetRow(reCapitaine, membre.Position.EmplacementY - 1);

                        Grid.SetColumn(reCapitaine, membre.Position.EmplacementX);

                        if (membre.AJoué)
                            lbl_Capitaine.Foreground = new SolidColorBrush(Colors.DimGray);
                        else
                            lbl_Capitaine.Foreground = new SolidColorBrush(Colors.White);

                        if (membre.Pv < 1)
                        {
                            imLogoCapitaine.Source = imgMort;
                            reCapitaine.IsTapEnabled = false;
                        }

                        break;
                    case Membre.roleMembre.Docteur:
                        marginLevel = 0;
                        if (membres[0].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        if (membres[1].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        margin = reDocteur.Margin;
                        margin.Top = -15 * marginLevel;
                        margin.Right = -15 * marginLevel;
                        reDocteur.Margin = margin;

                        if (membre.Position.Type.Equals(Module.moduleType.Infirmerie) || membre.Position.Type.Equals(Module.moduleType.SystemeSurvie)) // Infirmerie || Système de survie
                            Grid.SetRow(reDocteur, membre.Position.EmplacementY + 1);
                        else
                            Grid.SetRow(reDocteur, membre.Position.EmplacementY - 1);

                        Grid.SetColumn(reDocteur, membre.Position.EmplacementX);

                        if (membre.AJoué)
                            lbl_Docteur.Foreground = new SolidColorBrush(Colors.DimGray);
                        else
                            lbl_Docteur.Foreground = new SolidColorBrush(Colors.White);

                        if (membre.Pv < 1)
                        {
                            imLogoDocteur.Source = imgMort;
                            reDocteur.IsTapEnabled = false;

                        }

                        break;
                    case Membre.roleMembre.Mécanicien:
                        marginLevel = 0;
                        if (membres[0].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        if (membres[1].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        if (membres[2].Position.Type.Equals(membre.Position.Type))
                            marginLevel++;
                        margin = reMeca.Margin;
                        margin.Top = -15 * marginLevel;
                        margin.Right = -15 * marginLevel;
                        reMeca.Margin = margin;

                        if (membre.Position.Type.Equals(Module.moduleType.Infirmerie) || membre.Position.Type.Equals(Module.moduleType.SystemeSurvie)) // Infirmerie || Système de survie
                            Grid.SetRow(reMeca, membre.Position.EmplacementY + 1);
                        else
                            Grid.SetRow(reMeca, membre.Position.EmplacementY - 1);

                        Grid.SetColumn(reMeca, membre.Position.EmplacementX);

                        if (membre.AJoué)
                            lbl_Mecanicien.Foreground = new SolidColorBrush(Colors.DimGray);
                        else
                            lbl_Mecanicien.Foreground = new SolidColorBrush(Colors.White);

                        if (membre.Pv < 1)
                        {
                            imLogoMeca.Source = imgMort;
                            reMeca.IsTapEnabled = false;

                        }

                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Surcharge de la fonction de deplacement des personnages lors de leur initalisation des positions de départ
        /// </summary>
        /// <param name="personnage"></param>
        /// <param name="module"></param>
        private void Deplacement_PersonnageToCurrentModule(Membre.roleMembre personnage, Module module)
        {
            switch (personnage.ToString())
            {
                case "Docteur":
                    if (module.Type == Module.moduleType.Infirmerie)
                    {
                        Grid.SetRow(reDocteur, module.EmplacementY + 1);
                        Grid.SetColumn(reDocteur, module.EmplacementX);
                    }
                    else if (module.Type == Module.moduleType.SystemeSurvie)
                    {
                        Grid.SetRow(reDocteur, module.EmplacementY + 1);
                        Grid.SetColumn(reDocteur, module.EmplacementX);
                    }
                    else
                    {
                        Grid.SetRow(reDocteur, module.EmplacementY - 1);
                        Grid.SetColumn(reDocteur, module.EmplacementX);
                    }
                    break;
                case "Mécanicien":
                    if (module.Type == Module.moduleType.Infirmerie)
                    {
                        Grid.SetRow(reMeca, module.EmplacementY + 1);
                        Grid.SetColumn(reMeca, module.EmplacementX);
                    }
                    else if (module.Type == Module.moduleType.SystemeSurvie)
                    {
                        Grid.SetRow(reMeca, module.EmplacementY + 1);
                        Grid.SetColumn(reMeca, module.EmplacementX);
                    }
                    else
                    {
                        Grid.SetRow(reMeca, module.EmplacementY - 1);
                        Grid.SetColumn(reMeca, module.EmplacementX);
                    }
                    break;
                case "Capitaine":
                    if (module.Type == Module.moduleType.Infirmerie)
                    {
                        Grid.SetRow(reCapitaine, module.EmplacementY + 1);
                        Grid.SetColumn(reCapitaine, module.EmplacementX);
                    }
                    else if (module.Type == Module.moduleType.SystemeSurvie)
                    {
                        Grid.SetRow(reCapitaine, module.EmplacementY + 1);
                        Grid.SetColumn(reCapitaine, module.EmplacementX);
                    }
                    else
                    {
                        Grid.SetRow(reCapitaine, module.EmplacementY - 1);
                        Grid.SetColumn(reCapitaine, module.EmplacementX);
                    }
                    break;
                case "Commandant":
                    if (module.Type == Module.moduleType.Infirmerie)
                    {
                        Grid.SetRow(reCommandant, module.EmplacementY + 1);
                        Grid.SetColumn(reCommandant, module.EmplacementX);
                    }
                    else if (module.Type == Module.moduleType.SystemeSurvie)
                    {
                        Grid.SetRow(reCommandant, module.EmplacementY + 1);
                        Grid.SetColumn(reCommandant, module.EmplacementX);
                    }
                    else
                    {
                        Grid.SetRow(reCommandant, module.EmplacementY - 1);
                        Grid.SetColumn(reCommandant, module.EmplacementX);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fonction de création du bouton de déploiment pour permettre le déplacement d'une personnage
        /// </summary>
        private void Creation_Btn_Deploiement()
        {
            Button btn_deployment = new Button();
            btn_deployment.Name = "btn_deployment";
            btn_deployment.Height = 50;
            btn_deployment.Width = 240;
            btn_deployment.VerticalAlignment = (VerticalAlignment)AlignmentY.Bottom;
            btn_deployment.Content = "Déploiement";
            btn_deployment.BorderBrush = new SolidColorBrush(Colors.Black);
            btn_deployment.BorderThickness = new Thickness(5);
            btn_deployment.FontSize = 25;
            btn_deployment.Foreground = new SolidColorBrush(Colors.White);
            btn_deployment.Background = new SolidColorBrush(Colors.Gray);

            btn_deployment.IsTapEnabled = true;
            btn_deployment.Tapped += btn_deployment_OnClick;

            Grid.SetRow(btn_deployment, rowCurrentModule);
            Grid.SetColumn(btn_deployment, columnCurrentModule);

            for (int i = 9; i < Grid_Jeu.Children.Count; i++) // Supprimer les deploiements
            {
                UIElement item = Grid_Jeu.Children.ElementAt(i);
                if (item.GetType().Equals(btn_deployment.GetType()))
                {
                    Grid_Jeu.Children.Remove(item);
                }
            }

            this.Grid_Jeu.Children.Add(btn_deployment); // puis on creer le bouton deploiement
        }

        /// <summary>
        /// Fonction de deploiement d'un personnage dans un module au click du bouton deploiement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_deployment_OnClick(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog msgbox = new MessageDialog("Voulez vous déploier le " + membres[indexCurrentClickMembre].Role.ToString() + " dans le module : '" + modules[indexCurrentClickModule].Type.ToString() + "' ?", "Déploiment de " + membres[indexCurrentClickMembre].Role.ToString() + " ?");
            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });

            var parameters = new CurrentParameters(membres, modules, indexCurrentClickMembre, indexCurrentClickModule, hardMode, vaisseau, numeroSemaine, gameStarted);

            var res = await msgbox.ShowAsync();
            if ((int)res.Id == 0)
            {
                if (!modules[indexCurrentClickModule].EstEnPanne)
                    Affichage.Text = "Le module " + modules[indexCurrentClickModule].Type.ToString() + " n'est pas en panne";
                else
                    this.Frame.Navigate(typeof(PageModule), parameters);
            }
        }

        /// <summary>
        /// Fonction d'hightlight du module Pilotage et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pilotage_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.PostePilotage);
            Deplacement_PersonnageToCurrentModule();
        }

        /// <summary>
        /// Fonction d'hightlight du module Serre et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Serre_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Serre);
            Deplacement_PersonnageToCurrentModule();
        }

        /// <summary>
        /// Fonction d'hightlight du module Infiremerie et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Infirmerie_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Infirmerie);
            Deplacement_PersonnageToCurrentModule();

        }

        /// <summary>
        /// Fonction d'hightlight du module Détente et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Detente_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Détente);
            Deplacement_PersonnageToCurrentModule();

        }

        /// <summary>
        /// Fonction d'hightlight du module Maintenance et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maintenance_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Maintenance);
            Deplacement_PersonnageToCurrentModule();

        }

        /// <summary>
        /// Fonction d'hightlight du module Laboratoire et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Labo_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Laboratoire);
            Deplacement_PersonnageToCurrentModule();

        }

        /// <summary>
        /// Fonction d'hightlight du module Survie et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Survie_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.SystemeSurvie);
            Deplacement_PersonnageToCurrentModule();

        }

        /// <summary>
        /// Fonction déclencher par l'appuie sur le bouton de fin de semaine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnFinSemaine_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MessageDialog msgbox = new MessageDialog(" Voulez-vous terminer la semaine ?");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });

            var res = await msgbox.ShowAsync();

            if ((int)res.Id == 0)
            {
                FinSemaine();
                NouvelleSemaine();
                UpdateUI();
            }
        }

        /// <summary>
        /// Fonction qui permet d'ajuster le volume de la musique de fond
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeVolume(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (song != null)
                song.Volume = (double)volumeSlider.Value / 100;
        }

        /// <summary>
        /// Fonction déclencher lors de l'appuie du bouton de sauvegarde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSauvegarde_Click(object sender, RoutedEventArgs e)
        {
            var helper = new LocalObjectStorageHelper();

            helper.Save("vaisseau", vaisseau);

            int i = 0;
            foreach (Module module in modules)
            {
                helper.Save("module" + i, module);
                i++;
            }

            i = 0;
            foreach (Membre membre in membres)
            {
                helper.Save("membre" + i, membre);
                i++;
            }

            helper.Save("numeroSemaine", numeroSemaine);
            helper.Save("hardMode", hardMode);
            helper.Save("gameStarted", gameStarted);

            helper.Save("indexCurrentClickMembre", indexCurrentClickMembre);
            helper.Save("indexCurrentClickModule", indexCurrentClickModule);

            MessageDialog msg = new MessageDialog("Partie sauvegardé !","Sauvegarde");
            await msg.ShowAsync();
        }

        /// <summary>
        /// Fonction déclencher lorsque le pointer de la souris entre dans la zone d'un personnage pour fournir des informations au joueur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rePerso_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement)
            {
                RelativePanel r = e.OriginalSource as RelativePanel; // tentative de cast
                // Si le cast échoue, aucune exception n'est provoquée
                // Cette opération a dû être réalisée car des exceptions apparaissaient
                // quand l'utilisateur bougait sa souris de partout sur l'application
                if (r != null)
                {
                    tbPersoPouvoirTip.Text = r.Name.Substring(2) + " : \n" + "Pouvoir Spécial : \n" + r.Tag.ToString();
                    boPersoTip.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Fonction déclencher lorsque le pointer de la souris sort dans la zone d'un personnage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rePerso_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            boPersoTip.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// bouton pour revenir au menu principal lors de l'anbandon du joueur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAbandonner_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgbox = new MessageDialog("Etes vous certains de vouloir abandonner ?","Abandonner ?");
            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });
            
            var res = await msgbox.ShowAsync();
            if ((int)res.Id == 0)
            {
                Defaite();
            }
            else
            {
                msgbox = new MessageDialog("Vous n'avez pas cédez à l'abandon ! En route vers Mars !");
                await msgbox.ShowAsync();
            }
        }
    }
}
