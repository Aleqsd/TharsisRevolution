using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    /// <summary>
    /// Classe permettant le transfert des objets en paramètres d'une page à l'autre par la fonction NavigateTo().
    /// </summary>
    public class CurrentParameters
    {
        private List<Membre> membres;
        private List<Module> modules;
        private int indexCurrentMembre;
        private int indexCurrentModule;
        private bool hardMode;
        private Vaisseau vaisseau;
        private int numeroSemaine;
        private bool gameStarted;

        public List<Membre> Membres
        {
            get
            {
                return membres;
            }

            set
            {
                membres = value;
            }
        }

        public List<Module> Modules
        {
            get
            {
                return modules;
            }

            set
            {
                modules = value;
            }
        }

        public int IndexCurrentMembre
        {
            get
            {
                return indexCurrentMembre;
            }

            set
            {
                indexCurrentMembre = value;
            }
        }

        public int IndexCurrentModule
        {
            get
            {
                return indexCurrentModule;
            }

            set
            {
                indexCurrentModule = value;
            }
        }

        public bool HardMode
        {
            get
            {
                return hardMode;
            }

            set
            {
                hardMode = value;
            }
        }

        internal Vaisseau Vaisseau
        {
            get
            {
                return vaisseau;
            }

            set
            {
                vaisseau = value;
            }
        }

        public int NumeroSemaine
        {
            get
            {
                return numeroSemaine;
            }

            set
            {
                numeroSemaine = value;
            }
        }

        public bool GameStarted
        {
            get
            {
                return gameStarted;
            }

            set
            {
                gameStarted = value;
            }
        }

        public CurrentParameters()
        {
            // Constructeur vide
        }

        public CurrentParameters(List<Membre> membres, List<Module> modules, int indexCurrentMembre, int indexCurrentModule, bool hardMode, Vaisseau vaisseau, int numeroSemaine, bool gameStarted)
        {
            this.membres = membres;
            this.modules = modules;
            this.indexCurrentMembre = indexCurrentMembre;
            this.indexCurrentModule = indexCurrentModule;
            this.hardMode = hardMode;
            this.vaisseau = vaisseau;
            this.numeroSemaine = numeroSemaine;
            this.gameStarted = gameStarted;
        }
    }
}
