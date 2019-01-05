using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;


namespace game
{
    public class Str2Enum
    {
        public static bool To<T>(string str, out T v)
        {
            try
            {
                v = (T)System.Enum.Parse(typeof(T), str, true);
                return true;
            }
            catch (System.Exception)
            {
                v = default(T);
                return false;
            }
        }

        public static T To<T>(string str, T def)
        {
            try
            {
                def = (T)System.Enum.Parse(typeof(T), str, true);
            }
            catch (System.Exception)
            {

            }

            return def;
        }
    }

}
