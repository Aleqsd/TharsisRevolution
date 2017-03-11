using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

// TODO faire gaffe aux résolutions
// TODO Garder le gif et y insérer en plus un fond d'écran immobile ?
// TODO Ajouter du fun (un nyan cat ?)
// TODO supprimer les using ou fonctions inutilisées
// TODO commenter tous le code

namespace TharsisRevolution
{
    /// <summary>
    /// Page principale
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random rdm;
        private Vaisseau vaisseau;
        private List<Module> modules;
        private List<Membre> membres;
        private List<Panne> pannes;
        private int numeroSemaine;
        private bool hardMode = false;

        private int indexCurrentClickMembre;
        private int indexCurrentClickModule;
        private int rowCurrentModule;
        private int columnCurrentModule;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is GameParameters)
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
            }

        }
        public MainPage()
        {
            this.InitializeComponent();
            //INITIALISATION DU JEU
            hardMode = true;
            Initialiser();

            // ---------------------------------------------------
            //NOUVELLE SEMAINE
            NouvelleSemaine();

            //CREATION PANNE
            CreationPannes(numeroSemaine);

            // =========================
            //SELECTION D'UN MEMBRE = UI

            //DEPLACEMENT 
            //Deplacement(membres[0], modules[1]);

            //LANCER DE DES 
            //List<Dé> monLancer = LancerDés(5, modules[1].Panne, membres[0]);

            //Facultatif : RELANCER LES dés voulus (modification du tableau int[] reçu de LancerDés avec LancerDé à certaines positions)
            //LancerDé();

            //Facultatif : UTILISATION POUVOIR
            //UtiliserPouvoir(membres[0], modules[1], monLancer);

            //Facultatif : REPARATION MODULE
            //ReparationPanneDuModule(modules[1], 5);

            //FINDUTOUR pour ce membre
            //FinTour(membres[0]);
            // ==========================

            // ==========================
            //SELECTION D'UN AUTRE MEMBRE
            //
            //FinTour(membres[1]);
            //FinTour(membres[2]);
            //FinTour(membres[3]);

            // ...
            // ==========================

            //FIN SEMAINE (détecte défaite)
            //FinSemaine();
            // ---------------------------------------------------

            //FIN SEMAINE (même fonction, détecte si semaine 10)
        }



        /// <summary>
        /// Initialisation de la partie, création des objets, placement des membres à leurs positions initiales randomisées
        /// </summary>
        private void Initialiser()
        {
            rdm = new Random();

            //Initialisation Vaisseau
            vaisseau = new Vaisseau();
            this.pg_PvShip.Value = vaisseau.Pv;

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

            //initialisation des paramètres du Personnage Commandant
            pb_PvCommandant.Value = membres[0].Pv;
            switch(membres[0].NombreDeDés)
            {
                case 0:
                    Com_d1.Fill = new SolidColorBrush( Colors.Black);
                    Com_d2.Fill = new SolidColorBrush(Colors.Black);
                    Com_d3.Fill = new SolidColorBrush(Colors.Black);
                    Com_d4.Fill = new SolidColorBrush(Colors.Black);
                    Com_d5.Fill = new SolidColorBrush(Colors.Black);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 1:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.Black);
                    Com_d3.Fill = new SolidColorBrush(Colors.Black);
                    Com_d4.Fill = new SolidColorBrush(Colors.Black);
                    Com_d5.Fill = new SolidColorBrush(Colors.Black);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 2:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.White);
                    Com_d3.Fill = new SolidColorBrush(Colors.Black);
                    Com_d4.Fill = new SolidColorBrush(Colors.Black);
                    Com_d5.Fill = new SolidColorBrush(Colors.Black);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 3:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.White);
                    Com_d3.Fill = new SolidColorBrush(Colors.White);
                    Com_d4.Fill = new SolidColorBrush(Colors.Black);
                    Com_d5.Fill = new SolidColorBrush(Colors.Black);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 4:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.White);
                    Com_d3.Fill = new SolidColorBrush(Colors.White);
                    Com_d4.Fill = new SolidColorBrush(Colors.White);
                    Com_d5.Fill = new SolidColorBrush(Colors.Black);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 5:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.White);
                    Com_d3.Fill = new SolidColorBrush(Colors.White);
                    Com_d4.Fill = new SolidColorBrush(Colors.White);
                    Com_d5.Fill = new SolidColorBrush(Colors.White);
                    Com_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 6:
                    Com_d1.Fill = new SolidColorBrush(Colors.White);
                    Com_d2.Fill = new SolidColorBrush(Colors.White);
                    Com_d3.Fill = new SolidColorBrush(Colors.White);
                    Com_d4.Fill = new SolidColorBrush(Colors.White);
                    Com_d5.Fill = new SolidColorBrush(Colors.White);
                    Com_d6.Fill = new SolidColorBrush(Colors.White);
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Capitaine
            pb_PvCapitaine.Value = membres[1].Pv;
            switch (membres[1].NombreDeDés)
            {
                case 0:
                    Cap_d1.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d2.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d3.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d4.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d5.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 1:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d3.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d4.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d5.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 2:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.White);
                    Cap_d3.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d4.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d5.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 3:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.White);
                    Cap_d3.Fill = new SolidColorBrush(Colors.White);
                    Cap_d4.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d5.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 4:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.White);
                    Cap_d3.Fill = new SolidColorBrush(Colors.White);
                    Cap_d4.Fill = new SolidColorBrush(Colors.White);
                    Cap_d5.Fill = new SolidColorBrush(Colors.Black);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 5:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.White);
                    Cap_d3.Fill = new SolidColorBrush(Colors.White);
                    Cap_d4.Fill = new SolidColorBrush(Colors.White);
                    Cap_d5.Fill = new SolidColorBrush(Colors.White);
                    Cap_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 6:
                    Cap_d1.Fill = new SolidColorBrush(Colors.White);
                    Cap_d2.Fill = new SolidColorBrush(Colors.White);
                    Cap_d3.Fill = new SolidColorBrush(Colors.White);
                    Cap_d4.Fill = new SolidColorBrush(Colors.White);
                    Cap_d5.Fill = new SolidColorBrush(Colors.White);
                    Cap_d6.Fill = new SolidColorBrush(Colors.White);
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Docteur
            pb_PvDocteur.Value = membres[2].Pv;
            switch (membres[2].NombreDeDés)
            {
                case 0:
                    doc_d1.Fill = new SolidColorBrush(Colors.Black);
                    doc_d2.Fill = new SolidColorBrush(Colors.Black);
                    doc_d3.Fill = new SolidColorBrush(Colors.Black);
                    doc_d4.Fill = new SolidColorBrush(Colors.Black);
                    doc_d5.Fill = new SolidColorBrush(Colors.Black);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 1:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.Black);
                    doc_d3.Fill = new SolidColorBrush(Colors.Black);
                    doc_d4.Fill = new SolidColorBrush(Colors.Black);
                    doc_d5.Fill = new SolidColorBrush(Colors.Black);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 2:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.White);
                    doc_d3.Fill = new SolidColorBrush(Colors.Black);
                    doc_d4.Fill = new SolidColorBrush(Colors.Black);
                    doc_d5.Fill = new SolidColorBrush(Colors.Black);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 3:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.White);
                    doc_d3.Fill = new SolidColorBrush(Colors.White);
                    doc_d4.Fill = new SolidColorBrush(Colors.Black);
                    doc_d5.Fill = new SolidColorBrush(Colors.Black);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 4:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.White);
                    doc_d3.Fill = new SolidColorBrush(Colors.White);
                    doc_d4.Fill = new SolidColorBrush(Colors.White);
                    doc_d5.Fill = new SolidColorBrush(Colors.Black);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 5:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.White);
                    doc_d3.Fill = new SolidColorBrush(Colors.White);
                    doc_d4.Fill = new SolidColorBrush(Colors.White);
                    doc_d5.Fill = new SolidColorBrush(Colors.White);
                    doc_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 6:
                    doc_d1.Fill = new SolidColorBrush(Colors.White);
                    doc_d2.Fill = new SolidColorBrush(Colors.White);
                    doc_d3.Fill = new SolidColorBrush(Colors.White);
                    doc_d4.Fill = new SolidColorBrush(Colors.White);
                    doc_d5.Fill = new SolidColorBrush(Colors.White);
                    doc_d6.Fill = new SolidColorBrush(Colors.White);
                    break;
                default:
                    break;
            }

            //Initialisation des paramètres du personnage Mecanicien
            pbPvMeca.Value = membres[3].Pv;
            switch (membres[3].NombreDeDés)
            {
                case 0:
                    Meca_d1.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d2.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d3.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d4.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d5.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 1:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d3.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d4.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d5.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 2:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.White);
                    Meca_d3.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d4.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d5.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 3:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.White);
                    Meca_d3.Fill = new SolidColorBrush(Colors.White);
                    Meca_d4.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d5.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 4:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.White);
                    Meca_d3.Fill = new SolidColorBrush(Colors.White);
                    Meca_d4.Fill = new SolidColorBrush(Colors.White);
                    Meca_d5.Fill = new SolidColorBrush(Colors.Black);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 5:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.White);
                    Meca_d3.Fill = new SolidColorBrush(Colors.White);
                    Meca_d4.Fill = new SolidColorBrush(Colors.White);
                    Meca_d5.Fill = new SolidColorBrush(Colors.White);
                    Meca_d6.Fill = new SolidColorBrush(Colors.Black);
                    break;
                case 6:
                    Meca_d1.Fill = new SolidColorBrush(Colors.White);
                    Meca_d2.Fill = new SolidColorBrush(Colors.White);
                    Meca_d3.Fill = new SolidColorBrush(Colors.White);
                    Meca_d4.Fill = new SolidColorBrush(Colors.White);
                    Meca_d5.Fill = new SolidColorBrush(Colors.White);
                    Meca_d6.Fill = new SolidColorBrush(Colors.White);
                    break;
                default:
                    break;
            }


            int randomModule = rdm.Next(0, 6); //dans la liste les item vont de 0 et 6 et pas de 1 à 7, car sur le 7 j'ai toujours un outofrange :/
            
            // Positionnement des membres dans les modules // modif Thomas (pas certain mais proposition)
            for (int i = 0; i < 4; i++)
            {
                //Tant qu'on tombe sur un module avec um membre à l'interieur, recommencer.
                while (modules[randomModule].PresenceMembre)
                    randomModule = rdm.Next(0, 7);
                membres[i].Position = modules[randomModule];
                modules[randomModule].PresenceMembre = true; //thomas (permet de faire fonctionner ta boucle while au dessus)

                //Thomas : Initialisation emplacement personnage visuelement avec une fonction prévu spécialement pour l'initialisation des positions de départ
                Deplacement_PersonnageToCurrentModule(membres[i].Role , membres[i].Position);
                Debug.WriteLine("Membre " + membres[i].Role + " à la position " + membres[i].Position.Type);
            }
            

            //Initialisation Semaine
            numeroSemaine = 1;
            Debug.WriteLine("Semaine 1");
            this.slider_Semaines.Value = numeroSemaine;

            //Initialisation des Pannes avec la première panne moyenne du tour 1
            pannes = new List<Panne>();
        }

        /// <summary>
        /// Déplacement d'un membre à un Module avec gestion des dégats si il traverse une panne
        /// </summary>
        /// <param name="membre"></param>
        /// <param name="moduleDestination"></param>
        private void Deplacement(Membre membre, Module moduleDestination)
        {
            // Variable du deplacement du personnage
            int indexDeLaSalleDeDepart = membres[membre.Id].Position.EmplacementX;
            int indexDeLaSalleChoisie = moduleDestination.EmplacementX;

            Debug.WriteLine("Membre " + membre.Role + " PV avant déplacement " + membre.Pv);

            //Gestion de tous les cas pouvant générer des dégats au passage
            if (modules[1].EstEnPanne) // Serre en Panne
            {
                if (indexDeLaSalleChoisie == 0 && indexDeLaSalleDeDepart > 2) //Direction Pilotage depuis Infirmerie ou plus loin
                    membres[membre.Id].Pv--;
                if (indexDeLaSalleChoisie > 2 && indexDeLaSalleDeDepart == 0) //Depart de Pilotage vers Infirmerie ou plus loin
                    membres[membre.Id].Pv--;
            }
            if (modules[2].EstEnPanne) // Infirmerie en Panne
            {
                if (indexDeLaSalleChoisie == 4 && (indexDeLaSalleDeDepart != 3 && indexDeLaSalleDeDepart != 4)) // Direction Laboratoire si départ n'est pas Infirmerie ou Laboratoire
                    membres[membre.Id].Pv--;
                if (indexDeLaSalleChoisie > 4 && (indexDeLaSalleDeDepart > 3 || indexDeLaSalleDeDepart == 4)) //Direction à droite de Infirmerie, départ à gauche Infirmerie ou Laboratoire
                    membres[membre.Id].Pv--;
                if (indexDeLaSalleChoisie < 3 && indexDeLaSalleDeDepart > 3) // Direction à gauche de l'Infirmerie, départ à droite de l'Infirmerie ou Laboratoire
                    membres[membre.Id].Pv--;
            }
            if (modules[3].EstEnPanne) // Détente en Panne
            {
                if (indexDeLaSalleChoisie == 7 && (indexDeLaSalleDeDepart < 5 || indexDeLaSalleDeDepart == 6)) // Direction Maintenance, Départ Survie ou à gauche de Détente
                    membres[membre.Id].Pv--;
                if (indexDeLaSalleChoisie == 6 && (indexDeLaSalleDeDepart < 5 || indexDeLaSalleDeDepart == 7)) // Direction Survie, Départ Maintenance ou à gauche de Détente
                    membres[membre.Id].Pv--;
                if (indexDeLaSalleChoisie < 5 && (indexDeLaSalleDeDepart > 5)) // Direction gauche de Détente, Départ Maintenance ou Survie
                    membres[membre.Id].Pv--;
            }

            Debug.WriteLine("Membre " + membre.Role + " PV après déplacement " + membre.Pv);

            // Défaite si on traverse une panne avec son dernier membre en vie avec un Pv...
            if (!unMembreEnVie())
                Défaite();

            // Déplacement
            membres[membre.Id].Position = modules[indexDeLaSalleChoisie];

            Debug.WriteLine("Membre " + membre.Role + " déplacé à la position " + membre.Position);
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
                    msgbox = new MessageDialog("Panne " + modules[4].Panne.TaillePanne + " de " + modules[4].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "32":
                    //detente
                    msgbox = new MessageDialog("Panne " + modules[5].Panne.TaillePanne + " de " + modules[5].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "42":
                    //maintenance
                    msgbox = new MessageDialog("Panne " + modules[3].Panne.TaillePanne + " de " + modules[3].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "21":
                    //labo
                    msgbox = new MessageDialog("Panne " + modules[6].Panne.TaillePanne + " de " + modules[6].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                case "33":
                    //survie
                    msgbox = new MessageDialog("Panne " + modules[2].Panne.TaillePanne + " de " + modules[2].Panne.Dégat + " points de dégats !", "Information sur la Panne.");
                    await msgbox.ShowAsync();
                    break;
                default:
                    break;
            }            
        }

        /// <summary>
        /// Création d'un certain nombre de panne en fonction du jour de la semaine et assignation de la panne à son module
        /// </summary>
        /// <param name="numeroSemaine"></param>
        private void CreationPannes(int numeroSemaine)
        {
            //creation image Panne (Thomas)
            //TODO gérer plusieurs pannes
            BitmapImage imagePanne = new BitmapImage(new Uri("ms-appx:///Assets/Ambox_warning_red.png"));
            Image img_Panne = new Image();
            img_Panne.Source = imagePanne;
            img_Panne.Width = 40;
            img_Panne.Height = 40;
            img_Panne.IsTapEnabled = true;
            img_Panne.Tapped += informationPanne_OnClick;
            
            int random = rdm.Next(0, 7);
            Debug.WriteLine("Création de panne pour la semaine " + numeroSemaine);
            switch (numeroSemaine)
            {
                case 1:
                    random = rdm.Next(0, 7);
                    pannes.Add(new Panne(1, Panne.taille.Moyenne, hardMode));                    
                    modules[random].Panne = pannes.Where(p => p.Id == 1).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 2:
                    pannes.Add(new Panne(2, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(3, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(4, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 2).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 3).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 4).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 3:
                    pannes.Add(new Panne(5, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(6, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(7, Panne.taille.Petite, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 5).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 6).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 7).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 4:
                    pannes.Add(new Panne(8, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(9, Panne.taille.Petite, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 8).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 9).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 5:
                    pannes.Add(new Panne(10, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(11, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(12, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 10).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 11).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 12).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 6:
                    pannes.Add(new Panne(13, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(14, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 13).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 14).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 7:
                    pannes.Add(new Panne(15, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(16, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 15).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 16).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 8:
                    pannes.Add(new Panne(17, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(18, Panne.taille.Petite, hardMode));
                    pannes.Add(new Panne(19, Panne.taille.Petite, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 17).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 18).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 19).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 9:
                    pannes.Add(new Panne(20, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(21, Panne.taille.Petite, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 20).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 21).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 10:
                    pannes.Add(new Panne(22, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(23, Panne.taille.Petite, hardMode));
                    random = rdm.Next(0, 7);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 22).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(0, 7);
                    modules[random].Panne = pannes.Where(p => p.Id == 23).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                default:
                    break;
            }
            foreach (Module module in modules)
            {
                if (module.EstEnPanne)
                {
                    Debug.WriteLine("Module " + module.Type.ToString() + " est en " + module.Panne.TaillePanne + " panne");

                    //positionnement de l'image de panne
                    Grid.SetColumn(img_Panne, module.EmplacementX);
                    Grid.SetRow(img_Panne, module.EmplacementY);
                    this.Grid_Jeu.Children.Add(img_Panne);
                }
            }

            Debug.WriteLine("Liste des pannes");
            foreach (Panne panne in pannes)
            {
                Debug.WriteLine("Panne "+ panne.Id+ " : " + panne.TaillePanne);
                if (hardMode)
                {
                    Debug.WriteLine("Panne a " + panne.NombreDésPiégés + " dés piégés");
                    foreach (Dé dé in panne.DésPiégés)
                    {
                        Debug.WriteLine(dé.Valeur + " = " + dé.Type.ToString());
                    }
                }
            }
        }
        
        /// <summary>
        /// Reparation totale ou partielle d'une panne à un module donné
        /// </summary>
        /// <param name="module"></param>
        /// <param name="montantDeLaReparation"></param>
        private void ReparationPanneDuModule(Module module, int montantDeLaReparation)
        {
            Debug.WriteLine("Réparation du module " + module.EmplacementX + " de " + montantDeLaReparation);
            if (module.EstEnPanne)
            {
                if (module.Panne.Dégat <= montantDeLaReparation)
                {
                    module.Panne.Dégat = 0;
                    module.Panne = null;
                    module.EstEnPanne = false;
                }
                else
                    module.Panne.Dégat = module.Panne.Dégat - montantDeLaReparation;
            }
        }

        /// <summary>
        /// Dégat infligé par une Panne (fait en fin de semaine)
        /// </summary>
        /// <param name="module"></param>
        private void PanneInfligeDégat(Module module)
        {
            Debug.WriteLine("Panne du module inflige des dégats " + module.EmplacementX);
            int typeDegat = rdm.Next(1, 4);
            switch (typeDegat)
            {
                case 1: //Perte de vie membres
                    int dégatMembre = rdm.Next(1, 4);
                    foreach (Membre membre in membres)
                        membre.Pv = membre.Pv - dégatMembre;
                    break;
                case 2: //Perte de vie vaisseau
                    int dégatVaisseau = rdm.Next(1, 4);
                    vaisseau.Pv = vaisseau.Pv - dégatVaisseau;
                    break;
                case 3: //Perte de dés membres
                    int perteDés = rdm.Next(1, 4);
                    foreach (Membre membre in membres)
                    {
                        if (membre.NombreDeDés - perteDés < 1)
                            membre.NombreDeDés = 1;
                        else
                            membre.NombreDeDés = membre.NombreDeDés - perteDés;
                    }
                    break;
                default:
                    break;
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
        }

        /// <summary>
        /// Gere les evenements arrivant en fin de semaine, une fois que tous les membres ont joués
        /// </summary>
        private void FinSemaine()
        {
            int nombreMembreQuiOntJoué = 0;
            Debug.WriteLine("Fin semaine");
            foreach (Membre membre in membres)
            {
                if (membre.AJoué)
                    nombreMembreQuiOntJoué++;
            }
            if (nombreMembreQuiOntJoué == 4) //Fin de semaine
            {
                foreach (Membre membre in membres)
                {
                    if (membre.NombreDeDés > 1)
                        membre.NombreDeDés--;
                }

                foreach (Module module in modules)
                {
                    if (module.EstEnPanne)
                    {
                        PanneInfligeDégat(module);
                    }
                }

                if (vaisseau.Pv < 1)
                    Défaite();

                if (numeroSemaine == 10 && vaisseau.Pv < 1 && unMembreEnVie())
                    Victoire();

                numeroSemaine++;
            }
        }

        /// <summary>
        /// Check si un membre au moins est en vie (condition vicotire)
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
        private void Défaite()
        {
            //DEFAITE
        }

        /// <summary>
        /// Déclenche la Victoire
        /// </summary>
        private void Victoire()
        {
            //VICTOIRE
        }

        //############################################################################# IHM ##############################################################################
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
            HightLight_Personnage(Membre.roleMembre.Mécanicien);
            indexCurrentClickMembre = 3;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Docteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Doc_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Docteur);
            indexCurrentClickMembre = 2;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Capitaine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capitaine_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Capitaine);
            indexCurrentClickMembre = 1;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Commandant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commandant_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Commandant);
            indexCurrentClickMembre = 0;
        }

        /// <summary>
        /// Fonction qui va permettre d'appliquer le hightlight sur un personnage qui sera selectionner et qu'il sois le seul lumineux montrant ainsi sa selection
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

                this.imLogoCommandant.Source = commandant;
                this.imLogoDocteur.Source = doc;
                this.imLogoMeca.Source = meca;
            }
            else if (perso == Membre.roleMembre.Commandant)
            {
                this.imLogoCommandant.Source = commandant_HightLight;

                this.imLogoCapitaine.Source = capitaine;
                this.imLogoDocteur.Source = doc;
                this.imLogoMeca.Source = meca;
            }
            else if (perso == Membre.roleMembre.Docteur)
            {
                this.imLogoDocteur.Source = doc_HightLight;

                this.imLogoCommandant.Source = commandant;
                this.imLogoCapitaine.Source = capitaine;
                this.imLogoMeca.Source = meca;
            }
            else if (perso == Membre.roleMembre.Mécanicien)
            {
                this.imLogoMeca.Source = meca_HightLight;

                this.imLogoCommandant.Source = commandant;
                this.imLogoDocteur.Source = doc;
                this.imLogoCapitaine.Source = capitaine;
            }
        }

        /// <summary>
        /// Fonction qui va permettre d'appliquer le hightlight sur un module qui sera selectionner et qu'il sois le seul lumineux montrant ainsi sa selection + mise en place des coordonnée x,y de l'element selectionner
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
        /// Fonction de déplacement des personnages vers un module
        /// </summary>
        private async void Deplacement_PersonnageToCurrentModule()
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
                        if(indexCurrentClickModule == 2) // Infirmerie
                        {
                            Grid.SetRow(reDocteur, rowCurrentModule + 1);
                            Grid.SetColumn(reDocteur, columnCurrentModule);
                        }
                        else if(indexCurrentClickModule == 5) // SystèmeSurvie
                        {
                            Grid.SetRow(reDocteur, rowCurrentModule + 1);
                            Grid.SetColumn(reDocteur, columnCurrentModule);
                        }
                        else
                        {
                            Grid.SetRow(reDocteur, rowCurrentModule - 1);
                            Grid.SetColumn(reDocteur, columnCurrentModule);
                        }

                        Creation_Btn_Deploiement();
                    }

                    if ((int)resDoc.Id == 1)
                    {
                        MessageDialog msgbox2 = new MessageDialog("Votre déplacement a été annulé...");
                        await msgbox2.ShowAsync();
                    }                    
                    break;
                case 3: // Mécanicien
                    var resMeca = await msgbox.ShowAsync();
                    if ((int)resMeca.Id == 0)
                    {
                        if (indexCurrentClickModule == 2) // Infirmerie
                        {
                            Grid.SetRow(reMeca, rowCurrentModule + 1);
                            Grid.SetColumn(reMeca, columnCurrentModule);
                        }
                        else if (indexCurrentClickModule == 5) // SystèmeSurvie
                        {
                            Grid.SetRow(reMeca, rowCurrentModule + 1);
                            Grid.SetColumn(reMeca, columnCurrentModule);
                        }
                        else
                        {
                            Grid.SetRow(reMeca, rowCurrentModule - 1);
                            Grid.SetColumn(reMeca, columnCurrentModule);
                        }
                        
                        Creation_Btn_Deploiement();
                    }

                    if ((int)resMeca.Id == 1)
                    {
                        MessageDialog msgbox2 = new MessageDialog("Votre déplacement a été annulé...");
                        await msgbox2.ShowAsync();
                    }
                    break;
                case 1: //Capitaine
                    var resCap = await msgbox.ShowAsync();
                    if ((int)resCap.Id == 0)
                    {
                        if (indexCurrentClickModule == 2) // Infirmerie
                        {
                            Grid.SetRow(reCapitaine, rowCurrentModule + 1);
                            Grid.SetColumn(reCapitaine, columnCurrentModule);
                        }
                        else if (indexCurrentClickModule == 5) // SystèmeSurvie
                        {
                            Grid.SetRow(reCapitaine, rowCurrentModule + 1);
                            Grid.SetColumn(reCapitaine, columnCurrentModule);
                        }
                        else
                        {
                            Grid.SetRow(reCapitaine, rowCurrentModule - 1);
                            Grid.SetColumn(reCapitaine, columnCurrentModule);
                        }
                        
                        Creation_Btn_Deploiement();
                    }

                    if ((int)resCap.Id == 1)
                    {
                        MessageDialog msgbox2 = new MessageDialog("Votre déplacement a été annulé...");
                        await msgbox2.ShowAsync();
                    }
                    break;
                case 0: //Commandant
                    var resCom = await msgbox.ShowAsync();
                    if ((int)resCom.Id == 0)
                    {
                        if (indexCurrentClickModule == 2) // Infirmerie
                        {
                            Grid.SetRow(reCommandant, rowCurrentModule + 1);
                            Grid.SetColumn(reCommandant, columnCurrentModule);
                        }
                        else if (indexCurrentClickModule == 5) // SystèmeSurvie
                        {
                            Grid.SetRow(reCommandant, rowCurrentModule + 1);
                            Grid.SetColumn(reCommandant, columnCurrentModule);
                        }
                        else
                        {
                            Grid.SetRow(reCommandant, rowCurrentModule - 1);
                            Grid.SetColumn(reCommandant, columnCurrentModule);
                        }
                        
                        Creation_Btn_Deploiement();
                    }

                    if ((int)resCom.Id == 1)
                    {
                        MessageDialog msgbox2 = new MessageDialog("Votre déplacement a été annulé...");
                        await msgbox2.ShowAsync();
                    }
                    break;
                default:
                    break;
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
            btn_deployment.BorderBrush = new SolidColorBrush(Colors.White);
            btn_deployment.BorderThickness = new Thickness(5);
            btn_deployment.FontSize = 25;
            btn_deployment.Foreground = new SolidColorBrush(Colors.White);

            btn_deployment.IsTapEnabled = true;
            btn_deployment.Tapped += btn_deployment_OnClick;            

            Grid.SetRow(btn_deployment, rowCurrentModule);
            Grid.SetColumn(btn_deployment, columnCurrentModule);

            /*
            for (int i = 0; i < Grid_Jeu.Children.Count; i++)
            {
                Debug.WriteLine(Grid_Jeu.Children.ElementAt(i).ToString());
            }*/

            UIElement lastitem = Grid_Jeu.Children.Last(); //recuperation du dernier element creer          
            if(lastitem.GetType() == btn_deployment.GetType()) //si le dernier element est un bouton
            {
                Grid_Jeu.Children.Remove(lastitem); //on supprime le dernier bouton
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
            MessageDialog msgbox = new MessageDialog("Voulez vous déploier le " + membres[indexCurrentClickMembre].Role.ToString() + " dans le module : '" + modules[indexCurrentClickModule].Type.ToString() + "' ?", "Déploiment de "+ membres[indexCurrentClickMembre].Role.ToString()+" ?");
            // TODO gérer l'impossibilité de déployer dans un module sans panne
            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });

            var parameters = new CurrentParameters(membres, modules, indexCurrentClickMembre, indexCurrentClickModule,hardMode,vaisseau,numeroSemaine);

            var res = await msgbox.ShowAsync();
            if ((int)res.Id == 0)
            {
                this.Frame.Navigate(typeof(PageModule),parameters);
            }

            if ((int)res.Id == 1)
            {
                MessageDialog msgbox2 = new MessageDialog("Votre déploiment a été annulé...");
                await msgbox2.ShowAsync();
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
        
    }

}