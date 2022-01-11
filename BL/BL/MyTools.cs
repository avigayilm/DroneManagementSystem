using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BL
{

    public static class DeepCopy
    {

        /// <summary>
        /// copies all fields from an idal object to a bl object in deep copy
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>

        public static Target CopyProperties<Source, Target>(this Source source, Target target)//from idal to bl
        {
            PropertyInfo[] propertyInfos = target.GetType().GetProperties();
            foreach (var targetProp in propertyInfos)//goes over all source props
            {

                //bool isMatched = target.GetType().GetProperties().Any(targetProp => targetProp!= null && targetProp.Name == sProp.Name && targetProp.GetType() == sProp.GetType());
                // get wanted property will return null if not found
                PropertyInfo propInf = typeof(Source).GetProperty(targetProp.Name);

                if (propInf != null)//such a propety does indeed exist in target
                {
                    object value = propInf.GetValue(source);//get the value of this property
                    if (value is ValueType || value is string)
                        targetProp.SetValue(target, value);//copy it to target
                }
                //else
                //     if (targetProp.GetType().IsSubclassOf(typeof(object)))
                //           CopyProperties(source, targetProp);

            }
            return target;
        }

        /// <summary>
        /// converts a DalList to a BlList
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static void CopyPropertyListtoIBLList<Source, Target>(this IEnumerable<Source> source, List<Target> target)
            where Target : new()//from idal to bl 
        {
            Target T ;
            foreach (Source idalElement in source)
            {
                T = new Target();
                idalElement.CopyProperties(T);
                target.Add(T);
            }

        }


        //public static void CopyProperties<Source, Target>(this Source source, Target target)//from bl to idal
        //{
        //    PropertyInfo[] propertyInfos = target.GetType().GetProperties();

        //    foreach (var targetProp in propertyInfos)//goes over all source props
        //    {
        //        //bool isMatched = source.GetType().GetProperties().Any(sourceProp => sourceProp.Name == targetProp.Name && sourceProp.GetType() == targetProp.GetType());
        //        PropertyInfo propInf = typeof(Source).GetProperty(targetProp.Name);
        //        if (propInf != null)
        //        {
        //            //PropertyInfo propInf = typeof(Source).GetProperty(targetProp.Name);//get my wanted property
        //            object value = propInf.GetValue(source);//get the value of this property
        //                                                    //PropertyInfo propertyInfo = target.GetType().GetProperty(targetProp.Name);
        //            if (value is ValueType || value is string)
        //                targetProp.SetValue(target, value);
        //        }

        //    else//
        //    {
        //        PropertyInfo[] propertyInfos1 = source.GetType().GetProperties();
        //        foreach (PropertyInfo sProp in propertyInfos1)//each propret thats a class
        //        { //isMatched = sProp.GetType().IsClass;

        //            if (sProp.GetType().IsClass)//is a class
        //            {
        //                PropertyInfo propInf2 = sProp.GetType().GetProperty(targetProp.Name);//the wanted property within the inner class
        //                if (propInf2 == null)//if not found in this class
        //                    continue;
        //                var value = propInf2.GetValue(sProp);//get the value from inner class insource
        //                sProp.SetValue(target, value);
        //            }
        //        }

        //    }
        //}
        //return target;
        // }

      

        //public static void CopyPropertyListtoIBLList1<Source, Target>(this IEnumerable<Source> source, List<Target> target)
        //   where Target : struct//from idal to bl 
        //{
        //    Target T = new();
        //    foreach (Source idalElement in source)
        //    {
        //        idalElement.CopyPropertiestoIBL(T);
        //        target.Add(T);
        //    }
        //}


        //public static T SmallestDistance<T>(this Location current, List<T> toFindIn)
        //{
        //    double minDis = double.MaxValue;
        //    foreach(T cur in toFindIn )
        //    {
        //        Location temp = typeof(T).GetProperty(typeof(Location).Name).GetValue(cur);
        //    }
        //}
    }
}

//        public static void CopyPropertiesToIEnumerable<T, S>(this IEnumerable<S> from, List<T> to)
//        where T : new()
//        {
//            foreach (S s in from)
//            {
//                T t = new();
//                s.CopyFields(t);
//                to.Add(t);
//            }
//        }
//        public static object CopyPropertiesToNew<S>(this S from, Type type)
//        {
//            object to = Activator.CreateInstance(type); // new object of Type
//            from.CopyFields(to);
//            return to;
//        }
//    }
//}

//static class MyBLTools
//{

/// <summary>
/// function converts a drone object from idal to a Bl drone
/// </summary>
/// <param name="dro"></param>
/// <returns></returns>


//    public static Drone ToBLDrone(this DO.Drone dro)
//    {
//        Drone BLDrone = new()
//        {
//            id = dro.id,
//            model = dro.model,
//            weight = (WeightCategories)dro.maxWeight,
//        };
//        return BLDrone;
//    }
//    /// <summary>
//    /// function converts IDAL droneList to BL droneList
//    /// </summary>
//    /// <param name="dr"></param>
//    /// <returns></returns>
//    public static IEnumerable<Drone> ToBLDroneList(this List<DO.Drone> dr)
//    {
//        List<Drone> BLdrones = new();
//        foreach (DO.Drone dro in dr)
//        {
//            BLdrones.Add(dro.ToBLDrone());//adds to the list all drones from idal list converted
//        }
//        return BLdrones;
//    }

//    public static DO.Station StationToDal(this Station st)
//    {
//        DO.Station dalStation = new()
//        {
//            id = st.id,
//            name = st.name,
//            chargeSlots = st.chargeslots,
//            Longitude = st.loc.Longitude,
//            latitude = st.loc.latitude
//        };
//        return dalStation;
//    }
//    /// <summary>
//    /// converts bl parcel to dal parcel
//    /// </summary>
//    /// <param name="par"></param>
//    /// <returns></returns>
//    public static DO.Parcel ParcelToDal(this Parcel par)
//    {
//        DO.Parcel dalParcel = new()
//        {
//            id = par.id,
//            senderid = par.sender.id,
//            targetid = par.receiver.id,
//            weight = (DO.WeightCategories)par.weight,
//            priority = (DO.Priorities)par.priority,
//            requested = par.requested,
//            scheduled = par.scheduled,
//            pickedUp = par.pickedUp,
//            delivered = par.delivered,
//            droneId = par.dr.id
//        };
//        return dalParcel;
//    }

//    public static DO.Customer CustomerToDal(this Customer cus)
//    {
//        DO.Customer dalCustomer = new()
//        {
//            id = cus.id,
//            latitude = cus.loc.latitude,
//            Longitude = cus.loc.Longitude,
//            name = cus.name,
//            phone = cus.phoneNumber
//        };
//        return dalCustomer;
//    }
//}
