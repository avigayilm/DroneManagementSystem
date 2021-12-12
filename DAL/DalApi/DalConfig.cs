using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DalApi
{
    internal static class DalConfig
    {
        public class DalPackage
        {
            public string Name;
            public string PkgName;
            public string NameSpace;
            public string ClassName;
        }

        internal static string DalName;
        internal static Dictionary<string, DalPackage> DalPackages;

        /// <summary>
        /// Static constructor extracts Dal packages list and Dal type from
        /// Dal configuration file config.xml
        /// </summary>
        static DalConfig()
        {
            XElement dalConfig = XElement.Load(@"config.xml");
            DalName = dalConfig.Element("dal").Value;
            DalPackages = (from pkg in dalConfig.Element("dal-packages").Elements()
                           let tmp1 = pkg.Attribute("namespace")
                           let nameSpace = tmp1 == null ? "Dal" : tmp1.Value
                           let tmp2 = pkg.Attribute("class")
                           let className = tmp2 == null ? pkg.Value : tmp2.Value
                           select new DalPackage()
                           {
                               Name = "" + pkg.Name,
                               PkgName = pkg.Value,
                               NameSpace = nameSpace,
                               ClassName = className
                           })
                           .ToDictionary(p => "" + p.Name, p => p);
        }
    }
}
   