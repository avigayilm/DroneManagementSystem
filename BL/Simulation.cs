using System;

namespace BL
{
    internal class Simulation
    {
        private BL bL;
        private int droneId;
        private Func<bool> func;
        private Action reportProgress;

        public Simulation(BL bL, int droneId, Func<bool> func, Action reportProgress)
        {
            this.bL = bL;
            this.droneId = droneId;
            this.func = func;
            this.reportProgress = reportProgress;
        }
    }
}