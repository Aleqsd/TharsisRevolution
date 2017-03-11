using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public class GameParameters
    {
        private bool hardMode;

        public GameParameters(bool hardMode)
        {
            this.hardMode = hardMode;
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
    }
}
