using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.WzLib;
using MapleLib.WzLib.WzProperties;
//using HaCreator.MapEditor;
using MapleLib.WzLib.WzStructure;

namespace MapleLib.WzLib.WzStructure
{
    public static class InfoTool
    {
        public static string GetString(IWzImageProperty source)
        {
            return ((WzStringProperty)source).Value;
        }

        public static double GetDouble(IWzImageProperty source)
        {
            return ((WzDoubleProperty)source).Value;
        }

        public static int GetInt(IWzImageProperty source)
        {
            return ((WzCompressedIntProperty)source).Value;
        }

        public static int? GetOptionalInt(IWzImageProperty source)
        {
            return source == null ? (int?)null : ((WzCompressedIntProperty)source).Value;
        }


        public static MapleBool GetOptionalBool(IWzImageProperty source)
        {
            if (source == null) return MapleBool.NotExist;
            else return ((WzCompressedIntProperty)source).Value == 1;
        }

        public static bool GetBool(IWzImageProperty source)
        {
            return ((WzCompressedIntProperty)source).Value == 1;
        }

        public static float GetFloat(IWzImageProperty source)
        {
            return ((WzByteFloatProperty)source).Value;
        }

        public static int? GetOptionalTranslatedInt(IWzImageProperty source)
        {
            string str = InfoTool.GetOptionalString(source);
            if (str == null) return null;
            return int.Parse(str);
        }

        public static string GetOptionalString(IWzImageProperty source)
        {
            return source == null ? null : ((WzStringProperty)source).Value;
        }
    }
}
