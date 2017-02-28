using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TharsisRevolution
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random rdm;
        private Vaisseau vaisseau;
        private List<Module> modules;
        private List<Membre> membres;
        private List<Panne> pannes;
        private int numeroSemaine;

        private Membre.roleMembre currentClickPersonnage;

        private bool hightLight_pilotage = false;
        private bool hightLight_detente = false;
        private bool hightLight_serre = false;
        private bool hightLight_survie = false;
        private bool hightLight_infirimerie = false;
        private bool hightLight_maintenance = false;
        private bool hightLight_labo = false;


        public MainPage()
        {
            this.InitializeComponent();
            //INITIALISATION DU JEU
            Initialiser();

            // ---------------------------------------------------
            //NOUVELLE SEMAINE
            NouvelleSemaine();

            //CREATION PANNE
            CreationPannes(numeroSemaine);

            // =========================
            //SELECTION D'UN MEMBRE = UI

            //DEPLACEMENT 
            Deplacement(membres[0], modules[1]);

            //LANCER DE DES 
            List<int> monLancer = LancerDés(5);

            //Facultatif : RELANCER LES dés voulus (modification du tableau int[] reçu de LancerDés avec LancerDé à certaines positions)
            LancerDé();

            //Facultatif : UTILISATION POUVOIR
            UtiliserPouvoir(membres[0], modules[1], monLancer);

            //Facultatif : REPARATION MODULE
            ReparationPanneDuModule(modules[1], 5);

            //FINDUTOUR pour ce membre
            FinTour(membres[0]);
            // ==========================

            // ==========================
            //SELECTION D'UN AUTRE MEMBRE
            //
            FinTour(membres[1]);
            FinTour(membres[2]);
            FinTour(membres[3]);

            // ...
            // ==========================

            //FIN SEMAINE (détecte défaite)
            FinSemaine();
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

            //Initialisation des Moduless
            modules = new List<Module> {
                new Module(Module.moduleType.PostePilotage,1),
                new Module(Module.moduleType.Serre,2),
                new Module(Module.moduleType.Infirmerie,3),
                new Module(Module.moduleType.Laboratoire,4),
                new Module(Module.moduleType.Détente,5),
                new Module(Module.moduleType.SystemeSurvie,6),
                new Module(Module.moduleType.Maintenance,7),
            };

            //Initialisation des Membres
            membres = new List<Membre> {
                new Membre(Membre.roleMembre.Commandant,1),
                new Membre(Membre.roleMembre.Capitaine,2),
                new Membre(Membre.roleMembre.Docteur,3),
                new Membre(Membre.roleMembre.Mécanicien,4)
            };


            int randomModule = rdm.Next(1, 7);

            // Positionnement des membres dans les modules
            for (int i = 0; i < 4; i++)
            {
                //Tant qu'on tombe sur un module avec um membre à l'interieur, recommencer.
                while (modules[randomModule].PresenceMembre)
                    randomModule = rdm.Next(1, 8);
                membres[i].Position = modules[randomModule];
                Debug.WriteLine("Membre " + membres[i].Role + " à la position " + membres[i].Position);

            }

            //Initialisation Semaine
            numeroSemaine = 1;
            Debug.WriteLine("Semaine 1");

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
            int indexDeLaSalleDeDepart = membres[membre.Id].Position.Emplacement;
            int indexDeLaSalleChoisie = moduleDestination.Emplacement;

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
        /// Lancer de X dés
        /// </summary>
        /// <param name="nombreDés"></param>
        /// <returns></returns>
        private List<int> LancerDés(int nombreDés)
        {
            List<int> lancers = new List<int>(new int[nombreDés]);
            Debug.WriteLine("Lancer de " + nombreDés + " dés");

            for (int i = 0; i < nombreDés; i++)
            {
                lancers[i] = rdm.Next(1, 7);
                Debug.WriteLine("Lancer " + i + " : " + lancers[i]);
            }

            return lancers;
        }

        /// <summary>
        /// Lancer d'un dé
        /// </summary>
        /// <returns></returns>
        private int LancerDé()
        {
            Debug.WriteLine("Lancer d'un dé");
            return rdm.Next(1, 7);
        }

        /// <summary>
        /// Utilisation d'un pouvoir d'un membre à un emplacement de panne donné pour un lancer de dés donné
        /// </summary>
        /// <param name="membre"></param>
        /// <param name="module"></param>
        /// <param name="lancers"></param>
        private void UtiliserPouvoir(Membre membre, Module module, List<int> lancers)
        {
            bool is5InList = lancers.IndexOf(5) != -1;
            bool is6InList = lancers.IndexOf(6) != -1;

            Debug.WriteLine("Membre " + membre.Role + " utilise son pouvoir");
            Debug.WriteLine("Lancer a au moins un 5 ? " + is5InList.ToString());
            Debug.WriteLine("Lancer a au moins un 6 ? " + is5InList.ToString());
            Debug.WriteLine("Le Module actuel est il en panne ? " + module.EstEnPanne.ToString());


            // On peut récupérer l'index du dé 5 ou 6 avec lancers.IndexOf(5)

            if ((is5InList || is6InList) && module.EstEnPanne)
            {
                Debug.WriteLine("Membre " + membre.Role + " utilise son pouvoir !!!");

                // Commandant : 10 de réparation
                if (membre.Role.Equals(Membre.roleMembre.Commandant))
                    module.Panne.Dégat = module.Panne.Dégat - 10;

                // Capitaine : +1 Dés pour chaque membre
                if (membre.Role.Equals(Membre.roleMembre.Capitaine))
                {
                    foreach (Membre m in membres)
                    {
                        if (m.NombreDeDés < 6)
                            m.NombreDeDés++;
                    }
                }

                // Docteur : +1 pv pour chaque membre
                if (membre.Role.Equals(Membre.roleMembre.Capitaine))
                {
                    foreach (Membre m in membres)
                    {
                        if (m.Pv < 6)
                            m.Pv++;
                    }
                }

                // Mécanicien : +1 pv au vaisseau
                if (membre.Role.Equals(Membre.roleMembre.Mécanicien))
                {
                    if (vaisseau.Pv < 10)
                        vaisseau.Pv++;
                }
            }
        }

        /// <summary>
        /// Création d'un certain nombre de panne en fonction du jour de la semaine et assignation de la panne à son module
        /// </summary>
        /// <param name="numeroSemaine"></param>
        private void CreationPannes(int numeroSemaine)
        {
            int random = rdm.Next(0, 7);
            Debug.WriteLine("Création de panne pour la semaine " + numeroSemaine);
            switch (numeroSemaine)
            {
                case 1:
                    pannes.Add(new Panne(1, Panne.taille.Moyenne));
                    modules[random].Panne = pannes.Where(p => p.Id == 1).FirstOrDefault();
                    break;
                case 2:
                    pannes.Add(new Panne(2, Panne.taille.Moyenne));
                    pannes.Add(new Panne(3, Panne.taille.Moyenne));
                    pannes.Add(new Panne(4, Panne.taille.Moyenne));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 2).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 3).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 4).FirstOrDefault();
                    break;
                case 3:
                    pannes.Add(new Panne(5, Panne.taille.Moyenne));
                    pannes.Add(new Panne(6, Panne.taille.Moyenne));
                    pannes.Add(new Panne(7, Panne.taille.Petite));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 5).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 6).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 7).FirstOrDefault();
                    break;
                case 4:
                    pannes.Add(new Panne(8, Panne.taille.Grosse));
                    pannes.Add(new Panne(9, Panne.taille.Petite));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 8).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 9).FirstOrDefault();
                    break;
                case 5:
                    pannes.Add(new Panne(10, Panne.taille.Grosse));
                    pannes.Add(new Panne(11, Panne.taille.Grosse));
                    pannes.Add(new Panne(12, Panne.taille.Moyenne));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 10).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 11).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 12).FirstOrDefault();
                    break;
                case 6:
                    pannes.Add(new Panne(13, Panne.taille.Grosse));
                    pannes.Add(new Panne(14, Panne.taille.Moyenne));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 13).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 14).FirstOrDefault();
                    break;
                case 7:
                    pannes.Add(new Panne(15, Panne.taille.Moyenne));
                    pannes.Add(new Panne(16, Panne.taille.Moyenne));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 15).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 16).FirstOrDefault();
                    break;
                case 8:
                    pannes.Add(new Panne(17, Panne.taille.Grosse));
                    pannes.Add(new Panne(18, Panne.taille.Petite));
                    pannes.Add(new Panne(19, Panne.taille.Petite));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 17).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 18).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 19).FirstOrDefault();
                    break;
                case 9:
                    pannes.Add(new Panne(20, Panne.taille.Moyenne));
                    pannes.Add(new Panne(21, Panne.taille.Petite));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 20).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 21).FirstOrDefault();
                    break;
                case 10:
                    pannes.Add(new Panne(22, Panne.taille.Grosse));
                    pannes.Add(new Panne(23, Panne.taille.Petite));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 22).FirstOrDefault();
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 23).FirstOrDefault();
                    break;
                default:
                    break;
            }
            Debug.WriteLine("Liste des pannes");
            foreach (Panne panne in pannes)
                Debug.WriteLine("Panne : " + panne.TaillePanne);
        }

        /// <summary>
        /// Reparation totale ou partielle d'une panne à un module donné
        /// </summary>
        /// <param name="module"></param>
        /// <param name="montantDeLaReparation"></param>
        private void ReparationPanneDuModule(Module module, int montantDeLaReparation)
        {
            Debug.WriteLine("Réparation du module " + module.Emplacement + " de " + montantDeLaReparation);
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
            Debug.WriteLine("Panne du module inflige des dégats " + module.Emplacement);
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
        /// Fin du tour d'un membre
        /// </summary>
        /// <param name="membre"></param>
        private void FinTour(Membre membre)
        {
            Debug.WriteLine("Fin de tour du membre " + membre.Role);

            membre.AJoué = true;
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
        /// Permet de modifier le texte en fonction de la valeur du slider du temps
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void slider_TimeSemaine_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            this.lbl_TimeSemaine.Text = msg;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage MEcanicien
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Meca_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Mécanicien);
            currentClickPersonnage = Membre.roleMembre.Mécanicien;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Docteur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Doc_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Docteur);
            currentClickPersonnage = Membre.roleMembre.Docteur;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Capitaine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Capitaine_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Capitaine);
            currentClickPersonnage = Membre.roleMembre.Capitaine;
        }

        /// <summary>
        /// Fonction pour affichage lumineux à la selection du personnage Commandant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Commandant_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Personnage(Membre.roleMembre.Commandant);
            currentClickPersonnage = Membre.roleMembre.Commandant;
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
            else if(perso == Membre.roleMembre.Commandant)
            {                
                this.imLogoCommandant.Source = commandant_HightLight;

                this.imLogoCapitaine.Source = capitaine;
                this.imLogoDocteur.Source = doc;
                this.imLogoMeca.Source = meca;
            }
            else if(perso == Membre.roleMembre.Docteur)
            {
                this.imLogoDocteur.Source = doc_HightLight;

                this.imLogoCommandant.Source = commandant;
                this.imLogoCapitaine.Source = capitaine;
                this.imLogoMeca.Source = meca;
            }
            else if(perso == Membre.roleMembre.Mécanicien)
            {             
                this.imLogoMeca.Source = meca_HightLight;

                this.imLogoCommandant.Source = commandant;
                this.imLogoDocteur.Source = doc;
                this.imLogoCapitaine.Source = capitaine;
            }
        }

        /// <summary>
        /// Fonction qui va permettre d'appliquer le hightlight sur un module qui sera selectionner et qu'il sois le seul lumineux montrant ainsi sa selection
        /// </summary>
        /// <param name="module"></param>
        private void HightLight_Module(Module.moduleType module)
        {
            BitmapImage pilotage = new BitmapImage(new Uri("ms-appx:///Assets/Module_Pilotage2.png"));
            BitmapImage serre= new BitmapImage(new Uri("ms-appx:///Assets/Module_Serre2.png"));
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
                this.Detente.Source = detente_HightLight;

                this.Pilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Survie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Infirmerie)
            {
                this.Infirmerie.Source = inf_HightLight;

                this.Pilotage.Source = pilotage;
                this.Detente.Source = detente;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Survie.Source = survie;
                this.Maintenance.Source = maint; ;
            }
            else if (module == Module.moduleType.Laboratoire)
            {
                this.Laboratoire.Source = labo_HightLight;

                this.Pilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Detente.Source = detente;
                this.Survie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Maintenance)
            {
                this.Maintenance.Source = maint_HightLight;

                this.Pilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Survie.Source = survie;
                this.Detente.Source = detente;
            }
            else if (module == Module.moduleType.PostePilotage)
            {
                this.Pilotage.Source = pilotage_HightLight;

                this.Detente.Source = detente;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Survie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.Serre)
            {
                this.Serre.Source = serre_HightLight;

                this.Pilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Detente.Source = detente;
                this.Laboratoire.Source = labo;
                this.Survie.Source = survie;
                this.Maintenance.Source = maint;
            }
            else if (module == Module.moduleType.SystemeSurvie)
            {
                this.Survie.Source = survie_HightLight;

                this.Pilotage.Source = pilotage;
                this.Infirmerie.Source = inf;
                this.Serre.Source = serre;
                this.Laboratoire.Source = labo;
                this.Detente.Source = detente;
                this.Maintenance.Source = maint;
            }
        }

        /// <summary>
        /// Fonction de déplacement des personnages vers un module
        /// </summary>
        private async void Deplacement_PersonnageToModule(Module.moduleType moduleSelected)
        {
            string personnage = currentClickPersonnage.ToString();

            MessageDialog msgbox = new MessageDialog("Voulez vous déplacer le "+personnage+" dans le module : '" + moduleSelected + "' ?", "Déplacement Personnage ?");

            msgbox.Commands.Clear();
            msgbox.Commands.Add(new UICommand { Label = "Oui", Id = 0 });
            msgbox.Commands.Add(new UICommand { Label = "Non", Id = 1 });
            msgbox.Commands.Add(new UICommand { Label = "Annuler", Id = 2 });

            var res = await msgbox.ShowAsync();

            if ((int)res.Id == 0)
            {
                this.Frame.Navigate(typeof(PageModule));
            }

            if ((int)res.Id == 2 || (int)res.Id == 1)
            {
                MessageDialog msgbox2 = new MessageDialog("Votre déplacement a été annulé...");
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
            Deplacement_PersonnageToModule(Module.moduleType.PostePilotage);
        }

        /// <summary>
        /// Fonction d'hightlight du module Serre et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Serre_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Serre);
            Deplacement_PersonnageToModule(Module.moduleType.Serre);
        }

        /// <summary>
        /// Fonction d'hightlight du module Infiremerie et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Infirmerie_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Infirmerie);
            Deplacement_PersonnageToModule(Module.moduleType.Infirmerie);
        }

        /// <summary>
        /// Fonction d'hightlight du module Détente et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Detente_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Détente);
            Deplacement_PersonnageToModule(Module.moduleType.Détente);
        }

        /// <summary>
        /// Fonction d'hightlight du module Maintenance et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maintenance_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Maintenance);
            Deplacement_PersonnageToModule(Module.moduleType.Maintenance);
        }

        /// <summary>
        /// Fonction d'hightlight du module Laboratoire et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Labo_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.Laboratoire);
            Deplacement_PersonnageToModule(Module.moduleType.Laboratoire);
        }

        /// <summary>
        /// Fonction d'hightlight du module Survie et permet le lancement d'un personnage dans ce module même
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Survie_HightLight_OnClick(object sender, TappedRoutedEventArgs e)
        {
            HightLight_Module(Module.moduleType.SystemeSurvie);
            Deplacement_PersonnageToModule(Module.moduleType.SystemeSurvie);
        }
    }

}