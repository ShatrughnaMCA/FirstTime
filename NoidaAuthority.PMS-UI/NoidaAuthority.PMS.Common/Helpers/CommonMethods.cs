using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoidaAuthority.PMS.Common
{
    public static class CommonMethods
    {
        /// <summary>
        /// GetRandomPassword
        /// </summary>
        /// <returns></returns>
        public static String GetRandomPassword()
        {
            string allowedChars = "";
            allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";
            allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";
            allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";
            char[] sep = { ',' };
            string[] arr = allowedChars.Split(sep);
            string passwordString = "";
            string temp = "";
            Random rand = new Random();
            for (int i = 0; i < 7; i++)
            {
                temp = arr[rand.Next(0, arr.Length)];
                passwordString += temp;
            }
            passwordString += GetRandomInteger();
            return passwordString;
        }

        /// <summary>
        /// GetRandomPassword
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public static String GetRandomPassword(string firstName, string lastName)
        {
            string passwordString = string.Empty;
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {


                var firstTwo = firstName.Substring(0, 2);
                var lastTwo = lastName.Substring(0, 2);
                var ranChars = GetRandomString(2);
                var ranNum = GetRandomNumber();

                passwordString = firstTwo + lastTwo + ranChars + Convert.ToInt32(ranNum);
            }
            return passwordString;
        }


        /// <summary>
        /// GetRandomString
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static String GetRandomString(int length)
        {
            var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            var len = length;

            var chars = new char[len];
            var rd = new Random();

            for (var i = 0; i < len; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new String(chars);
        }

        /// <summary>
        /// GetRandomNumber
        /// </summary>
        /// <returns></returns>
        private static int GetRandomNumber()
        {
            int min = 10;
            int max = 99;
            Random random = new Random((int)DateTime.Now.Ticks);

            return random.Next(min, max);
        }

        /// <summary>
        /// CombineUrlPath
        /// </summary>
        /// <param name="uri1"></param>
        /// <param name="uri2"></param>
        /// <returns></returns>
        public static string CombineUrlPath(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
        private static int GetRandomInteger()
        {
            int seed = 345;
            int min = 1;
            int max = 9;
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(min, max);
        }
    }

    public static class CommonHelper
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
