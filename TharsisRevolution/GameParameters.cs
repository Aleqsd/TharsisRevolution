using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TharsisRevolution
{
    /// <summary>
    /// Classe permettant le transfert du bool hardMode à la sélection du niveau de difficulté dans la page d'accueil.
    /// </summary>
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
