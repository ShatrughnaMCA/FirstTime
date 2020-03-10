using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Entity
{
    public class ModelHelper
    {
        //encrypt optional parameter in url
        public static string Encode(string encodeVal)
        {
            byte[] encoded = Encoding.UTF8.GetBytes(encodeVal);
            return Convert.ToBase64String(encoded);
        }
        //decrypt optional parameter in uirl
        public static string Decode(string decodeVal)
        {
            byte[] encoded = Convert.FromBase64String(decodeVal);
            return Encoding.UTF8.GetString(encoded);
        }
    }
}
