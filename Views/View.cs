using Gloriosa.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Views
{
    public enum ViewTypes
    {
        Menu,
        Stage
    }

    public class View
    {
        public readonly ViewTypes viewType;

        public List<World> worlds;
        public GameObjectPool gOP;
        public List<Resource> lPOOL;

        public View(ViewTypes type)
        {
            if (CURVIEW != null)
            {
                // Clear l'ancienne view, libérer les resources locales, del les GameObject, et juste libérer l'intégralité de la View précédente quoi.
            }
            viewType = type;
            CURVIEW = this;
        }

        private void Update()
        {

        }

        private void Render()
        {

        }
    }
}
