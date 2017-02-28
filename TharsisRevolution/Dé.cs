using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public enum déType { Bléssure, Stase, Caduc }

    class Dé
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
            int temp = RandomNumber(1, 4);
            switch (temp)
            {
                case 1:
                    this.type = déType.Bléssure;
                    break;
                case 2:
                    this.type = déType.Caduc;
                    break;
                case 3:
                    this.type = déType.Stase;
                    break;
            }
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
