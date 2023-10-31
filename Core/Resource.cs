using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloriosa.Core
{
    public class Resource
    {
        public string name;
        public object resource;

        // Gérer resource local ou globale.
        public Resource(string _name, object _res, bool global=true)
        {
            name = _name;
            resource = _res;
            if (global)
                GPOOL.Add(this);
            else
                CURVIEW.lPOOL.Add(this); // Créer ça dans la CURVIEW.
        }

        /// <summary>
        /// Finds a resource and returns it.
        /// </summary>
        /// <param name="resourceName">A string used for defining a resource.</param>
        /// <returns>The resource associated with the name. Null if it doesn't exist.</returns>
        public static T? FindResource<T>(string resourceName, bool global=true)
        {
            // Si global est true, chercher dans la pool globale, sinon dans la pool de CURVIEW.
            if (global)
            {
                var res = GPOOL.Find(x => x.name == resourceName && x.resource is T);
                return (T?)(res?.resource);
            }
            else
            {
                var res = CURVIEW.lPOOL.Find(x => x.name == resourceName && x.resource is T);
                return (T?)(res?.resource);
            }
        }
    }
}
