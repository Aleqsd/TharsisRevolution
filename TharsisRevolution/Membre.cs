using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    /// <summary>
    /// Classe du Membre d'équipage
    /// </summary>
    class Membre
    {
        public enum roleMembre { Docteur, Mécanicien, Capitaine, Commandant }
        private int id;
        private int pv;
        private int nombreDeDés;
        private roleMembre role;
        private bool aJoué = false;
        private Module position;

        public Membre(roleMembre role, int id)
        {
            this.role = role;
            Random rnd = new Random();
            pv = rnd.Next(2, 5);
            nombreDeDés = rnd.Next(2, 5);
            this.Id = id;
        }

        public int Pv
        {
            get
            {
                return pv;
            }

            set
            {
                pv = value;
            }
        }

        public int NombreDeDés
        {
            get
            {
                return nombreDeDés;
            }

            set
            {
                nombreDeDés = value;
            }
        }

        public bool AJoué
        {
            get
            {
                return aJoué;
            }

            set
            {
                aJoué = value;
            }
        }

        internal roleMembre Role
        {
            get
            {
                return role;
            }

            set
            {
                role = value;
            }
        }

        internal Module Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }
    }
}

