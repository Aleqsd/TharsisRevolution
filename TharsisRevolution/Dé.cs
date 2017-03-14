using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public enum déType {Normal, Bléssure, Stase, Caduc, Highlight, Grisé, BléssureHighlight, StaseHighlight}

    public class Dé
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

        private int valeur;
        private déType type;

        public Dé()
        {
            this.valeur = RandomNumber(1, 7);
            this.type = déType.Normal;

        }

        public int Valeur
        {
            get
            {
                return valeur;
            }

            set
            {
                valeur = value;
            }
        }

        public déType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }
    }
}
