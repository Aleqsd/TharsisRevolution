using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool hardMode = false;

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
            Deplacement(membres[0], modules[1]);

            //LANCER DE DES 
            List<Dé> monLancer = LancerDés(5, modules[1].Panne, membres[0]);

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
        private List<Dé> LancerDés(int nombreDés, Panne panne, Membre membre)
        {
            List<Dé> lancers = new List<Dé>(new Dé[nombreDés]);
            Debug.WriteLine("Lancer de " + nombreDés + " dés");

            for (int i = 0; i < nombreDés; i++)
            {
                lancers[i] = new Dé();
                // ToDo if a modifier, là c'est pour tester
                if (hardMode)
                {
                    foreach (Dé dé in panne.DésPiégés)
                    {
                        if (dé.Valeur.Equals(lancers[i]))
                        {
                            switch (dé.Type)
                            {
                                case déType.Bléssure:
                                    lancers[i].Type = déType.Bléssure;
                                    Debug.WriteLine("Dé emplacement : " + i + " = bléssure, -1 pv pour le membre");
                                    membre.Pv--;
                                    break;
                                case déType.Stase:
                                    lancers[i].Type = déType.Stase;
                                    Debug.WriteLine("Dé emplacement : " + i + " = stase, à rendre non relancable mais utilisable pour capacité ou réparation");
                                    // Verouiller le dé mais utilisable pour capacité spéciale ou réparation
                                    break;
                                case déType.Caduc:
                                    lancers[i].Type = déType.Caduc;
                                    Debug.WriteLine("Dé emplacement : " + i + " = caduc, à rendre non relancable, non utilisable");
                                    // Vérouiller le dé
                                    break;
                            }

                        }
                    }
                }
                Debug.WriteLine("Lancer " + i + " : " + lancers[i].Valeur);
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
        private void UtiliserPouvoir(Membre membre, Module module, List<Dé> lancers)
        {

            bool is5InList = false;
            bool is6InList = false;

            foreach (Dé dé in lancers)
            {
                if (dé.Valeur.Equals(5))
                {
                    is5InList = true;
                }
                if (dé.Valeur.Equals(6))
                {
                    is6InList = true;
                }
            }

            Debug.WriteLine("Membre " + membre.Role + " utilise son pouvoir");
            Debug.WriteLine("Lancer a au moins un 5 ? " + is5InList.ToString());
            Debug.WriteLine("Lancer a au moins un 6 ? " + is5InList.ToString());
            Debug.WriteLine("Le Module actuel est il en panne ? " + module.EstEnPanne.ToString());

            // On peut récupérer l'index du dé 5 ou 6 avec lancers.IndexOf(5)
            // A revoir

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
                    // ToDo METTRE RANDOM pour le premier jour, là c'est pour test
                    pannes.Add(new Panne(1, Panne.taille.Moyenne, hardMode));
                    modules[1].Panne = pannes.Where(p => p.Id == 1).FirstOrDefault();
                    modules[1].EstEnPanne = true;
                    break;
                case 2:
                    pannes.Add(new Panne(2, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(3, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(4, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 2).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 3).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 4).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 3:
                    pannes.Add(new Panne(5, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(6, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(7, Panne.taille.Petite, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 5).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 6).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 7).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 4:
                    pannes.Add(new Panne(8, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(9, Panne.taille.Petite, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 8).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 9).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 5:
                    pannes.Add(new Panne(10, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(11, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(12, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 10).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 11).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 12).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 6:
                    pannes.Add(new Panne(13, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(14, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 13).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 14).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 7:
                    pannes.Add(new Panne(15, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(16, Panne.taille.Moyenne, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 15).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 16).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 8:
                    pannes.Add(new Panne(17, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(18, Panne.taille.Petite, hardMode));
                    pannes.Add(new Panne(19, Panne.taille.Petite, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 17).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 18).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 19).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 9:
                    pannes.Add(new Panne(20, Panne.taille.Moyenne, hardMode));
                    pannes.Add(new Panne(21, Panne.taille.Petite, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 20).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 21).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    break;
                case 10:
                    pannes.Add(new Panne(22, Panne.taille.Grosse, hardMode));
                    pannes.Add(new Panne(23, Panne.taille.Petite, hardMode));
                    random = rdm.Next(1, 8);
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
                    modules[random].Panne = pannes.Where(p => p.Id == 22).FirstOrDefault();
                    modules[random].EstEnPanne = true;
                    while (modules[random].EstEnPanne)
                        random = rdm.Next(1, 8);
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

        private void slider_TimeSemaine_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            string msg = String.Format("{0}", e.NewValue);
            this.lbl_TimeSemaine.Text = msg;
        }
    }

}