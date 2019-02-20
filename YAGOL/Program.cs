using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAGOL
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var instance = new Yagol())
                instance.Run(15.0);
        }
    }
}
