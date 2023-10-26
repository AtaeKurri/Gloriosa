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

        public Resource(string _name, object _res)
        {
            name = _name;
            resource = _res;
            RPOOL.Add(this);
        }

        /// <summary>
        /// Finds a resource and returns it.
        /// </summary>
        /// <param name="resourceName">A string used for defining a resource.</param>
        /// <returns>The resource associated with the name. Null if it doesn't exist.</returns>
        public static T? FindResource<T>(string resourceName)
        {
            var res = RPOOL.Find(x => x.name == resourceName && x.resource is T);
            return (T?)(res?.resource);
        }
    }
}
