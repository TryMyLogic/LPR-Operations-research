using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPR381_Windows_Project.Algorithms
{
    public interface ISolver
    {
        void Solve(LinearModel model, out string output);
    }
}
