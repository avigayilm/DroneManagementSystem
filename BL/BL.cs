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
    public partial class BL : Ibl
    {
        internal static Random rand = new Random();
        internal static List<IBL.BO.Drone> droneBL = new();

        public BL()
        {
            //DAL.DalObject dal2 = new();
            IDal idal1 = new DAL.DalObject();
            double[] tempArray = idal1.DronePwrUsg();
            double pwrUsgEmpty = tempArray[0];
            double pwrUsgLight = tempArray[1];
            double pwrUsgMedium = tempArray[2];
            double pwrUsgHeavy = tempArray[3];
            double chargePH = tempArray[4];
            List<Drone> tempDroneList = (List<Drone>)idal1.GetAllDrones();// what type of drone is it using...

            List<Parcel> undeliveredParcel = (List<Parcel>)idal1.UndeliveredParcels();
            foreach (Parcel p in undeliveredParcel)
            {
                int DroneId = p.droneId;
                int droneIndex = tempDroneList.FindIndex(d => d.id == DroneId);
                if (droneIndex == -1)// if there is no drone assigned to the parcel
                {
                    //throw exceptionnnn
                }
                else if (droneIndex > 0)// if there is a drown assigned to the parcel
                {
                    var temp = tempDroneList[droneIndex];
                    IBL.BO.Drone tempBl = new();
                    tempBl.id = droneIndex;
                    tempBl.model = temp.model;
                    // weight= (WeightCategories)temp.maxWeight,
                    //status of drone
                    tempBl.status = IBL.BO.DroneStatuses.Delivery;
                    //location of drone
                    if (p.requested != DateTime.MinValue && p.pickedUp == DateTime.MinValue)//if assigned but not yet collected)
                    {
                        var stationTemp = idal1.smallestDistanceStation(p.senderid);
                        //location of deone will be in  station closed to the sender
                        tempBl.loc.longitude = stationTemp.longitude;
                        tempBl.loc.latitude = stationTemp.latitude;
                    }
                    if (p.requested != DateTime.MinValue && p.pickedUp != DateTime.MinValue)// if pakage is assigned and picked up
                    {
                        //location wull be at the sender
                        List<Customer> tempCustomerList = (List<Customer>)idal1.GetAllCustomers();
                        int customerIndex = tempCustomerList.FindIndex(c => c.id == p.senderid);
                        if (customerIndex == -1)
                        {//throw
                        }
                        else
                        {
                            tempBl.loc.longitude = tempCustomerList[customerIndex].longitude;
                            tempBl.loc.latitude = tempCustomerList[customerIndex].latitude;
                        }
                    }
                    tempBl.battery = rand.Next(40, 100);// random battery level so that the drone can still fly
                }
            }

                        // battery will be random between mnim and 100 so it can deliver


            else// if the drone is not delivering
            {

            }

            foreach (Drone d in tempDroneList)
            {

            }
            droneBL.Add(tempBl)
         }
        public void AddStation(Station stat)
        {
            int index = DataSource.parcelList.FindIndex(s => s.id == stat.id);
            if (index != -1)
            {
                throw new ParcelException("Id exists already\n");
            }
            else
            {
                DataSource.stationList.Add(stat);
            }
        }
    }
    //functions used in the main menu



}

