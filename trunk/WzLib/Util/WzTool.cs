/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010 Snow and haha01haha01
   
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections;
using System.IO;
using MapleLib.MapleCryptoLib;

namespace MapleLib.WzLib.Util
{
	public class WzTool
	{

		public static Hashtable StringCache = new Hashtable();

		public static UInt32 RotateLeft(UInt32 x, byte n)
		{
			return (UInt32)(((x) << (n)) | ((x) >> (32 - (n))));
		}

		public static UInt32 RotateRight(UInt32 x, byte n)
		{
			return (UInt32)(((x) >> (n)) | ((x) << (32 - (n))));
		}

		public static int GetCompressedIntLength(int i)
		{
			if (i > 127 || i < -127)
				return 5;
			return 1;
		}

		public static int GetEncodedStringLength(string s)
		{
			int len = 0;
			if (string.IsNullOrEmpty(s))
				return 1;
			bool unicode = false;
			foreach (char c in s)
				if (c > 255)
					unicode = true;
			if (unicode)
			{
				if (s.Length > 126)
					len += 5;
				else
					len += 1;
				len += s.Length * 2;
			}
			else
			{
				if (s.Length > 127)
					len += 5;
				else
					len += 1;
				len += s.Length;
			}
			return len;
		}

		public static int GetWzObjectValueLength(string s, byte type)
		{
			string storeName = type + "_" + s;
			if (s.Length > 4 && StringCache.ContainsKey(storeName))
			{
				return 5;
			}
			else
			{
				StringCache[storeName] = 1;
				return 1 + GetEncodedStringLength(s);
			}
		}

		public static T StringToEnum<T>(string name)
		{
			try
			{
				return (T)Enum.Parse(typeof(T), name);
			}
			catch
			{
				return default(T);
			}
		}

        public static bool AESSelfCheck(ref string exceptionResult)
        {
            try
            {
                byte[] foo = WzKeyGenerator.GenerateWzKey(GetIvByMapleVersion(WzMapleVersion.GMS));
                return true;
            }
            catch (Exception e)
            {
                exceptionResult = e.Message;
                return false;
            }
        }

		public static byte[] GetIvByMapleVersion(WzMapleVersion ver)
		{
			switch (ver)
			{
				case WzMapleVersion.BMS:
					return CryptoConstants.WZ_MSEAIV;//?
				case WzMapleVersion.EMS:
					return CryptoConstants.WZ_MSEAIV;//?
				case WzMapleVersion.GMS:
					return CryptoConstants.WZ_GMSIV;
				case WzMapleVersion.CLASSIC:
				default:
					return new byte[4];
			}
		}
	}
}