using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    public class Module
    {
        public enum moduleType { PostePilotage, Serre, SystemeSurvie, Maintenance, Infirmerie, Détente, Laboratoire }

        private int emplacementX, emplacementY; //modif thomas
        private moduleType type;
        private bool estEnPanne = false;
        private Panne panne;
        private bool presenceMembre = false;

        public Module(moduleType type, int emplacementX, int emplacementY)//modif thomas
        {
            this.emplacementX = emplacementX; //modif thomas
            this.emplacementY = emplacementY;//modif thomas
            this.type = type;
        }

        public bool EstEnPanne
        {
            get
            {
                return estEnPanne;
            }

            set
            {
                estEnPanne = value;
            }
        }

        public bool PresenceMembre
        {
            get
            {
                return presenceMembre;
            }

            set
            {
                presenceMembre = value;
            }
        }

        internal Panne Panne
        {
            get
            {
                return panne;
            }

            set
            {
                panne = value;
            }
        }

        public int EmplacementX
        {
            get
            {
                return emplacementX;
            }

            set
            {
                emplacementX = value;
            }
        }

        public int EmplacementY
        {
            get
            {
                return emplacementY;
            }

            set
            {
                emplacementY = value;
            }
        }

        internal moduleType Type
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
