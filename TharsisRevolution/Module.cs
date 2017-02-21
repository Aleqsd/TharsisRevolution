﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    class Module
    {
        public enum moduleType { PostePilotage, Serre, SystemeSurvie, Maintenance, Infirmerie, Détente, Laboratoire }

        private int emplacement;
        private moduleType type;
        private bool estEnPanne = false;
        private Panne panne;
        private bool presenceMembre = false;

        public Module(moduleType type, int emplacement)
        {
            this.emplacement = emplacement;
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

        public int Emplacement
        {
            get
            {
                return emplacement;
            }

            set
            {
                emplacement = value;
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
