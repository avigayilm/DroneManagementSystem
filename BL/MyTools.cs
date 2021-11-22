using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using DAL;

namespace BL
{

    public static class DeepCopy
    {
        //public static void CopyFields<S, T>(this S source, T target)
        //{
        //    foreach (PropertyInfo targetProp in target.GetType().GetProperties())
        //    {
        //        PropertyInfo sourceProp = typeof(S).GetProperty(targetProp.Name);//will search for each property from target in source and if it doesnt exist wil return a null
        //        if (sourceProp != null)
        //        {


        //            dynamic val = sourceProp.GetValue(source, null);//will copy hte property's value from source object to value
        //            if (val is ValueType || val is string)
        //            {
        //                targetProp.SetValue(target, val);
        //            }
        //        }
        //        else
        //        {
        //            //BindingFlags flags = BindingFlags.GetProperty;
        //            foreach (PropertyInfo prop in source.GetType().GetProperties())
        //            {
        //                Type t = prop.GetType();
        //                if (prop.GetType().IsClass)
        //                    //sourceProp = typeof(prop).GetProperty(targetProp.Name);
        //                    prop.CopyPropertiesToNew
        //            }
        //        }
        //    }
        //}

        public static void CopyPropertiestoIDAL<Source, Target>(this Source source, Target target)//from bl to idal
        {
            foreach (var targetProp in target.GetType().GetProperties())//goes over all source props
            {
                bool isMatched = source.GetType().GetProperties().Any(sourceProp => sourceProp.Name == targetProp.Name && sourceProp.GetType() == targetProp.GetType());
                if (isMatched)
                {
                    PropertyInfo propInf = typeof(Source).GetProperty(targetProp.Name);//get my wanted property
                    var value = propInf.GetValue(source);//get the value of this property
                    //PropertyInfo propertyInfo = target.GetType().GetProperty(targetProp.Name);
                    propInf.SetValue(target, value);
                }
                else
                {
                    foreach (PropertyInfo sProp in source.GetType().GetProperties())//each propret thats a class
                    { //isMatched = sProp.GetType().IsClass;

                        if (/*isMatched*/sProp.GetType().IsClass)//is a class
                        {
                            //object obj = tProp.GetValue();
                            //var value = sProp.GetValue(source);
                            //object obj = tProp;
                            //var value = sProp.GetValue(source);
                            PropertyInfo propInf = sProp.GetType().GetProperty(targetProp.Name);//the wanted property within the inner class
                            var value = propInf.GetValue(sProp);//get the value from inner class insource
                            propInf.SetValue(target, value);
                        }
                    }

                }
            }
            //return target;
        }

        /// <summary>
        /// copies all fields from an idal object to a bl object in deep copy
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        public static void CopyPropertiestoIBL<Source, Target>(this Source source, Target target)//from idal to bl
        {
            PropertyInfo[] propertyInfos = target.GetType().GetProperties();
            foreach (var sProp in propertyInfos)//goes over all source props
            {

                //bool isMatched = target.GetType().GetProperties().Any(targetProp => targetProp!= null && targetProp.Name == sProp.Name && targetProp.GetType() == sProp.GetType());
                PropertyInfo propInf = typeof(Source).GetProperty(sProp.Name);//get my wanted property - has the same name in sprop and targetprop so will work anyhow

                if (propInf != null)//such a propety does indeed exist in target
                {
                    
                    object value = propInf.GetValue(source);//get the value of this property
                    //PropertyInfo propertyInfo = target.GetType().GetProperty(targetProp.Name);
                    if (value is ValueType || value is string)
                        sProp.SetValue(target, value);//copy it to target
                }

             
                else //doesnt exist
                {
                    PropertyInfo[] propertyInfos1 = target.GetType().GetProperties();
                    foreach (PropertyInfo tProp in propertyInfos1)//each propret thats a class
                    { //isMatched = sProp.GetType().IsClass;

                        if (/*isMatched*/tProp.GetType().IsClass)//is a class
                        {
                            PropertyInfo propInf2 = tProp.GetType().GetProperty(sProp.Name);//the wanted property 
                            if (propInf2 == null)//if not found in this class
                                continue;
                            var value = propInf2.GetValue(source);//get the value from inner class insource
                            propInf2.SetValue(tProp, value);
                            break;
                        }
                    }
                }
            }
            //return target;
        }
        /// <summary>
        /// converts a DalList to a BlList
        /// </summary>
        /// <typeparam name="Source"></typeparam>
        /// <typeparam name="Target"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static  void CopyPropertyListtoIBLList<Source, Target>(this IEnumerable<Source> source, List<Target> target)
            where Target : new()//from idal to bl 
        {
            Target T;
            foreach(Source idalElement in source)
            {
                T = new();
                idalElement.CopyPropertiestoIBL(T);
                target.Add(T);
            }
            
        }

        public static void CopyPropertyListtoIBLList1<Source, Target>(this IEnumerable<Source> source, List<Target> target)
           where Target : struct//from idal to bl 
        {
            Target T = new();
            foreach (Source idalElement in source)
            {
                idalElement.CopyPropertiestoIBL(T);
                target.Add(T);
            }
        }


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


//    public static Drone ToBLDrone(this IDAL.DO.Drone dro)
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
//    public static IEnumerable<Drone> ToBLDroneList(this List<IDAL.DO.Drone> dr)
//    {
//        List<Drone> BLdrones = new();
//        foreach (IDAL.DO.Drone dro in dr)
//        {
//            BLdrones.Add(dro.ToBLDrone());//adds to the list all drones from idal list converted
//        }
//        return BLdrones;
//    }

//    public static IDAL.DO.Station StationToDal(this Station st)
//    {
//        IDAL.DO.Station dalStation = new()
//        {
//            id = st.id,
//            name = st.name,
//            chargeSlots = st.chargeslots,
//            longitude = st.loc.longitude,
//            latitude = st.loc.latitude
//        };
//        return dalStation;
//    }
//    /// <summary>
//    /// converts bl parcel to dal parcel
//    /// </summary>
//    /// <param name="par"></param>
//    /// <returns></returns>
//    public static IDAL.DO.Parcel ParcelToDal(this Parcel par)
//    {
//        IDAL.DO.Parcel dalParcel = new()
//        {
//            id = par.id,
//            senderid = par.sender.id,
//            targetid = par.receiver.id,
//            weight = (IDAL.DO.WeightCategories)par.weight,
//            priority = (IDAL.DO.Priorities)par.priority,
//            requested = par.requested,
//            scheduled = par.scheduled,
//            pickedUp = par.pickedUp,
//            delivered = par.delivered,
//            droneId = par.dr.id
//        };
//        return dalParcel;
//    }

//    public static IDAL.DO.Customer CustomerToDal(this Customer cus)
//    {
//        IDAL.DO.Customer dalCustomer = new()
//        {
//            id = cus.id,
//            latitude = cus.loc.latitude,
//            longitude = cus.loc.longitude,
//            name = cus.name,
//            phone = cus.phoneNumber
//        };
//        return dalCustomer;
//    }
//}
