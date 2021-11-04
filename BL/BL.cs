using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL;
using IDAL.DO;
using IDAL;

namespace BL
{
    partial class BL:IBL.IBL
    {
        BL()
        {
            //DAL.DalObject dal2 = new();
            IDal idal1 = new DAL.DalObject();
            double[]tempArray= idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];
            List<Drone> tempDroneList = (List<Drone>)idal1.GetAllDrones();

            foreach()
            tempDroneList.ForEach(Drone d in tempDroneList)
            {
                if()
                    d.
            }

        }

        Station smallestDistance(Station st,Customer cs)
        {
            int distanc
            List<Drone> tempStationList= (List<Station>)idal1.GetAllStation();


        }
    }
}
