using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public class Panne
    {
        public enum taille { Petite, Moyenne, Grosse }

        private static readonly Random rdm = new Random();
        private static readonly object syncLock = new object();
        public static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            {
                return rdm.Next(min, max);
            }
        }

        private int dégat;
        private int id;
        private taille taillePanne;
        private bool hardMode;
        private int nombreDésPiégés = 0;
        private List<Dé> désPiégés;

        public Panne(int id, taille taillePanne, bool hardMode)
        {
            this.id = id;
            this.taillePanne = taillePanne;
            if (TaillePanne.Equals(taille.Petite))
                dégat = RandomNumber(1, 12);
            if (TaillePanne.Equals(taille.Moyenne))
                dégat = RandomNumber(12, 24);
            if (TaillePanne.Equals(taille.Grosse))
                dégat = RandomNumber(24, 35);

            if (hardMode)
            {
                DésPiégés = new List<Dé>();
                NombreDésPiégés = RandomNumber(1, 4);
                for (int i = 0; i < NombreDésPiégés; i++)
                    DésPiégés.Add(new Dé());
            }
        }

        public int Dégat
        {
            get
            {
                return dégat;
            }

            set
            {
                dégat = value;
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

        internal taille TaillePanne
        {
            get
            {
                return taillePanne;
            }

            set
            {
                taillePanne = value;
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

        public int NombreDésPiégés
        {
            get
            {
                return nombreDésPiégés;
            }

            set
            {
                nombreDésPiégés = value;
            }
        }

        internal List<Dé> DésPiégés
        {
            get
            {
                return désPiégés;
            }

            set
            {
                désPiégés = value;
            }
        }
    }
}
