using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public class CurrentParameters
    {
        private List<Membre> membres;
        private List<Module> modules;
        private int indexCurrentMembre;
        private int indexCurrentModule;
        private bool hardMode;
        private Vaisseau vaisseau;
        private int numeroSemaine;

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

        public CurrentParameters(List<Membre> membres, List<Module> modules, int indexCurrentMembre, int indexCurrentModule, bool hardMode, Vaisseau vaisseau, int numeroSemaine)
        {
            this.membres = membres;
            this.modules = modules;
            this.indexCurrentMembre = indexCurrentMembre;
            this.indexCurrentModule = indexCurrentModule;
            this.hardMode = hardMode;
            this.vaisseau = vaisseau;
            this.numeroSemaine = numeroSemaine;
        }
    }
}
