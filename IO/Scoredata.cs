using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Gloriosa.IO
{
    [Serializable]
    public abstract class Scoredata
    {
        [XmlIgnore, JsonIgnore]
        private string path;

        /// <summary>
        /// Creates a score file.
        /// Only supports XML for the moment.
        /// </summary>
        /// <param name="format">Json by defaut.</param>
        public Scoredata(string filePath)
        {
            path = filePath;
        }

        public void WriteToFile()
        {

        }

        public static Scoredata LoadScoreFile(string filePath)
        {
            return null;
        }


    }
}
