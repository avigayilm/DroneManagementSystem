using System;
using System.Collections.Generic;
using System.Reflection;

namespace DalApi
{
    /// <summary>
    /// Static Factory class for creating Dal tier implementation object according to
    /// configuration in file config.xml
    /// </summary>
    public static class DalFactory
    {
        //public static Idal GetDal()
        //{
        //    string dalType = DalConfig.DalName;
        //    string dalPkg = DalConfig.DalPackages[dalType];
        //    if (dalPkg == null) throw new DalConfigException($"Package {dalType} is not found in packages lilst in config");
        //    try{ Assembly.Load(dalPkg); }
        //    catch (Exception) { throw new DalConfigException("Failed to load  the congig.xml file"); }

        //    Type type = Type.GetType($"Dal.{dalPkg}, {dalPkg}");
        //    if (type == null) throw new DalConfigException($" Class {dalPkg} was not foungd in teh  {dalPkg}.dll");

        //    Idal dal = (Idal)type.GetProperty("Instance",
        //        BindingFlags.Public | BindingFlags.Static).GetValue(null);
        //    if (dal == null) throw new DalConfigException($"Class {dalPkg} is not a singleton or wrong property name for Instance");

        //    return dal;
        //}

















        /// <summary>
        /// The function creates the data layer implementation object according to the dal type project receieved by iterating through the configuration file
        /// config.xml of optional packages 
        /// each of these projects is within the 'Dal' namespace and must include singleton a singleton - designed class with the same name as the packages name
        /// each of these classes holds the sole instance of the class for all implementation and usage intents and pruposes.
        /// </summary>
        /// <returns>Dal tier implementation object</returns>

        public static Idal GetDal()
        {
            // get dal implementation name from config.xml according to <data> element
            string dalType = DalConfig.DalName;
            // bring package name (dll file name) for the dal name (above) from the list of packages in config.xml
            DalConfig.DalPackage dalPackage;
            try // get dal package info according to <dal> element value in config file
            {
                dalPackage = DalConfig.DalPackages[dalType];
            }
            catch (KeyNotFoundException ex)
            {
                // if package name is not found in the list - there is a problem in config.xml
                throw new DalConfigException($"Wrong DL type: {dalType}", ex);
            }


            try // Load into CLR the dal implementation assembly according to dll file name (taken above)
            {
                _ = Assembly.Load(dalPackage.PkgName);
            }
            catch (Exception ex)
            {
                throw new DalConfigException($"Failed loading {dalPackage.PkgName}.dll", ex);
            }

            // Get concrete Dal implementation's class metadata object
            ////////// 1st element in the list inside the string is full class name:
            //    namespace = "Dal" or as specified in the "namespace" attribute in the config file,
            //    class name = package name or as specified in the "class" attribute in the config file
            //    the last requirement (class name = package name) is not mandatory in general - but this is the way it
            //    is configured per the implementation here, otherwise we'd need to add class name in addition to package
            //    name in the config.xml file - which is clearly a good option.
            //    NB: the class may not be public - it will still be found... Our approach that the implemntation class
            //        should hold "internal" access permission (which is actually the default access permission)
            // 2nd element is the package name = assembly name (as above)
            Type type;

            try
            {
                type = Type.GetType($"{dalPackage.NameSpace}.{dalPackage.ClassName}, {dalPackage.PkgName}", true);
            }

            catch (Exception ex)
            { // If the type is not found - the implementation is not correct - it looks like the class name is wrong...
                throw new DalConfigException($"Class not found due to a wrong namespace or/and class name: {dalPackage.PkgName}:{dalPackage.NameSpace}.{dalPackage.ClassName}", ex);
            }
            // * Get concrete Dal implementation's Instance
            // Get property info for public static property named "Instance" (in the dal implementation class- taken above)
            // If the property is not found or it's not public or not static then it is not properly implemented
            // as a Singleton...
            // Get the value of the property Instance (get function is automatically called by the system)
            // Since the property is static - the object parameter is irrelevant for the GetValue() function and we can use null
            try
            {
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                Idal dal = type.GetProperty("instance", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null) as Idal;
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (dal == null)
                {
                    throw new DalConfigException($"Class {dalPackage.NameSpace}.{dalPackage.ClassName} instance is not initialized");
                }
                // now it looks like we have appropriate dal implementation instance :-)
                return dal;
            }
            catch (NullReferenceException ex)
            {
                throw new DalConfigException($"Class {dalPackage.NameSpace}.{dalPackage.ClassName} is not a singleton", ex);
            }

        }
    }
}













//using DalApi;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;


//namespace DalApi
//{



//   public class DalFactory
//    {
//        public static Idal GetIdal()
//        {
//            Type type = Type.GetType($"{"Dal"}.{"DalObject"}, {"DalObject"}", true);
//            Idal idal = type.GetProperty("Instance", System.Reflection.BindingFlags.Public
//                | System.Reflection.BindingFlags.Static).GetValue(null) as Idal;
//            return idal;
//        }
//    }
//}

