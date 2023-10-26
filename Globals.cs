using Gloriosa;
using Gloriosa.Core;
using Gloriosa.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Globals
{
    public static AppFrame APP { get; set; }
    public static List<World> WORLDS { get; set; }
    public static GameObjectPool TPOOL { get; set; }
    public static List<Resource> RPOOL { get; set; }
    public static Scoredata SCORE { get; set; }
}
