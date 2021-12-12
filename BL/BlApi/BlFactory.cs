using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public class BlFactory
    {
        public static Ibl GetBl()
        {
           
            try
            {
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                Ibl bl =  BL.BL.Instance as Ibl;
                // If the instance property is not initialized (i.e. it does not hold a real instance reference)...
                if (bl == null)
                {
                    throw new BlConfigException($"instance is not initialized");
                }
                // now it looks like we have appropriate dal implementation instance :-)
                return bl;
            }
            catch (NullReferenceException ex)
            {
                throw new BlConfigException($"Class BL is not a singleton", ex);
            }

        }
    }


    [Serializable]
    public class BlConfigException : Exception
    {
        public BlConfigException(string message) : base(message) { }
        public BlConfigException(string message, Exception inner) : base(message, inner) { }
    }

}
