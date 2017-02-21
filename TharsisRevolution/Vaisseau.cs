using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    class Vaisseau
    {
        private int pv;

        public Vaisseau()
        {
            Random rdm = new Random();
            Pv = rdm.Next(2, 6);
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
