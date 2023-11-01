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
                CURVIEW.lPOOL.Add(this);
        }

        /// <summary>
        /// Finds a resource and returns it. Searches into the local view pool and then the global one if the resource is not found within the local pool.
        /// </summary>
        /// <param name="resourceName">A string used for defining a resource.</param>
        /// <returns>The resource associated with the name. Null if it doesn't exist.</returns>
        public static T? FindResource<T>(string resourceName)//, bool global=true)
        {
            // Chercher dans les deux si elle est pas trouvée dans la locale d'abord, et ensuite la globale.
            var res = CURVIEW.lPOOL.Find(x => x.name == resourceName && x.resource is T);
            if (res == null)
                res = GPOOL.Find(x => x.name == resourceName && x.resource is T);
            return (T?)(res?.resource);
        }
    }
}
