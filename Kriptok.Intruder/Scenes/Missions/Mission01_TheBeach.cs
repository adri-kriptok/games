using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.Intruder.Scenes.Missions
{
    internal class Mission01_TheBeach : MissionBriefingBase
    {
        protected override string GetMissionText()
        {
            /*
             
We don't ask questions if you don't either.

             */

            return @"Mission Briefing

You know how this goes:

You're the best for the job.
And you only work alone.

blah, blah, blah....

Mission Objective: TOP SECRET

Ehm... survive for now...

Good luck!";
        }
    }
}
