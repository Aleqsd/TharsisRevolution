using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public class Vaisseau
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

        private int pv;

        public Vaisseau()
        {
            Pv = RandomNumber(2, 6);
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
    }
}
