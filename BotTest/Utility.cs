using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Utility {
    public static Random random;
    static Utility() {
        random = new Random(DateTime.Now.Millisecond);
    }
    public Utility() {

    }
}