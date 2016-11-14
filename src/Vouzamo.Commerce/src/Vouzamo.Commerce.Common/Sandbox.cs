using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vouzamo.Commerce.Common.Extensions;
using Vouzamo.Commerce.Common.Models;

namespace Vouzamo.Commerce.Common
{
    public static class Sandbox
    {
        public static void Play()
        {
            var each = new UnitOfMeasure("Each", "ea");
            var ml = new UnitOfMeasure("Millilitres", "ml");

            var source = new Source("supplier");

            var glassBottle = new Material("glass bottle");
            var perfume = new Material("perfume");

            var bom = new Dictionary<Guid, int>
            {
                { glassBottle.Id, 1},
                { perfume.Id, 50}
            };

            var bottledPerfume = new Material("bottled perfume", bom);

            var caseOfPerfume = bottledPerfume.Receive(each.Measure(20));
            var schedule = caseOfPerfume.SourceFrom(source, 0.5M);

            var actualize = schedule.
        }
    }
}
