using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TransitionSystemChecker.PathFormulas;

namespace TransitionSystemChecker.StateFormulas
{
    class SExists : StateFormula
    {
        public PathFormula path;

        public SExists(PathFormula path)
        {
            this.path = path;
        }

        public override StateFormula existentialNormalForm()
        {

            if(path.GetType() == typeof(PWeakUntil)) {
                StateFormula left = ((PWeakUntil) path).left;
                StateFormula right = ((PWeakUntil) path).right;

                //left = left.existentialNormalForm();
                //right = right.existentialNormalForm();

                left = new SNot(right);
                right = new SAnd(new SNot(left), new SNot(right));

                
                PathFormula until = new PUntil(new SNot(right), new SAnd(new SNot(left), new SNot(right)));
                PathFormula globally = new PGlobally(new SNot(right));

                StateFormula e_until = new SExists(until);
                StateFormula e_globally = new SExists(globally);

                StateFormula or = new SOr(e_until, e_globally);

                return or.existentialNormalForm();

            }


            PathFormula e_path = path.existentialNormalForm();

            return new SExists(e_path);
        }

    }
}
