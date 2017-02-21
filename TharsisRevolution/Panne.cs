using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    class Panne
    {
        public enum taille { Petite, Moyenne, Grosse }

        private int dégat;
        private int id;
        private taille taillePanne;

        public Panne(int id, taille taillePanne)
        {
            this.id = id;
            this.taillePanne = taillePanne;
            Random rnd = new Random();
            if (TaillePanne.Equals(taille.Petite))
                dégat = rnd.Next(1, 12);
            if (TaillePanne.Equals(taille.Moyenne))
                dégat = rnd.Next(12, 24);
            if (TaillePanne.Equals(taille.Grosse))
                dégat = rnd.Next(24, 35);
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
    }
}
