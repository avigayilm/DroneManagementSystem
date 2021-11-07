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
            List<Parcel> undeliveredParcel = (List<Parcel>)idal1.UndeliveredParcels();
            foreach(Parcel p in undeliveredParcel)
            {
                if(p.delivered==DateTime.MinValue)// package is not yet delivered
                {
                        int DroneId = p.droneId;
                        int droneIndex = tempDroneList.FindIndex(d => d.id == DroneId);
                    if (droneIndex >= 0)
                    {
                        var temp = tempDroneList[droneIndex];
                        //status of drone
                        temp.status = st;
                        //location of drone
                        if (p.requested != DateTime.MinValue && p.pickedUp == DateTime.MinValue)//if assigned but not yet collected)
                        {
                            //location of deone will be in  station closed to the sender
                            temp.


                            }
                        if (p.requested != DateTime.MinValue && p.pickedUp != DateTime.MinValue)// if pakage is assigned and picked up
                        {
                            //location wull be at the sender

                        }
                        // battery will be random between mnim and 100 so it can deliver
                    }
                }
                else// if the drone is not delivering
                {

                }
            }
            foreach(Drone d in tempDroneList)
            {

            }

        }

        //functions used in the main menu



    }
}
