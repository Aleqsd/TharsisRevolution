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
    public class Membre
    {
        private static readonly Random rdm = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return rdm.Next(min, max);
            }
        }

        public enum roleMembre { Docteur, Mécanicien, Capitaine, Commandant }
        public int id;
        public int pv;
        public int nombreDeDés;
        public roleMembre role;
        public bool aJoué = false;
        public Module position;

        public Membre(roleMembre role, int id)
        {
            this.role = role;
            pv = RandomNumber(2, 5);
            nombreDeDés = RandomNumber(2, 5);
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

