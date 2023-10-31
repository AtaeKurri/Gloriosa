using Gloriosa;
using Gloriosa.Core;
using Gloriosa.IO;
using Gloriosa.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Globals
{
    public static AppFrame APP { get; set; }
    public static View? CURVIEW { get; set; }
    public static List<Resource> GPOOL { get; set; }
    public static Scoredata SCORE { get; set; }
}
