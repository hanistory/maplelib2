﻿/*  MapleLib - A general-purpose MapleStory library
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

using System.Collections.Generic;
using System.IO;
using MapleLib.WzLib.Util;
using MapleLib.WzLib.WzProperties;

namespace MapleLib.WzLib
{
	/// <summary>
	/// A class that parses and contains the data of a wz list file
	/// </summary>
	public class WzListFile : IWzFile
	{
		#region Fields
		internal byte[] wzFileBytes;
        internal List<WzListEntry> listEntries = new List<WzListEntry>();
		internal string name = "";
		internal byte[] WzIv;
        internal WzMapleVersion version;
        internal string filePath;
		#endregion

		/// <summary>
		/// Name of the WzListFile
		/// </summary>
		public override string Name { get { return name; } set { name = value; } }
		/// <summary>
		/// The entries in the list wz file
		/// </summary>
		public List<WzListEntry> WzListEntries { get { return listEntries; } }
		/// <summary>
		/// The WzObjectType of the file
		/// </summary>
		public override WzObjectType ObjectType { get { return WzObjectType.File; } }
		public override IWzObject Parent { get { return null; } internal set { } }
        public override string FilePath { get { return filePath; } }
        public override WzMapleVersion MapleVersion { get { return version; } set { version = value; } }
        public override IWzFile WzFileParent { get { return this; } }
		public override void Dispose()
		{
			wzFileBytes = null;
			name = null;
			listEntries.Clear();
			listEntries = null;
		}

        
        public WzListEntry this[string name]
        {
            get
            {
                foreach (WzListEntry entry in listEntries) if (entry.Name == name) return entry;
                return null;
            }
        }

		/// <summary>
		/// Open a wz list file from a file on the disk
		/// </summary>
		/// <param name="filePath">Path to the wz file</param>
		public WzListFile(string filePath, WzMapleVersion version)
		{
            this.filePath = filePath;
			name = Path.GetFileName(filePath);
            wzFileBytes = File.ReadAllBytes(filePath);
            this.version = version;
            this.WzIv = WzTool.GetIvByMapleVersion(version);
		}

        public WzListFile(WzMapleVersion version, string name)
        {
            this.name = name;
            this.version = version;
            this.WzIv = WzTool.GetIvByMapleVersion(version);
        }

		/// <summary>
		/// Parses the wz list file
		/// </summary>
		public void ParseWzFile()
		{
			WzBinaryReader wzParser = new WzBinaryReader(new MemoryStream(wzFileBytes), WzIv);
			while (wzParser.PeekChar() != -1)
			{
				int Len = wzParser.ReadInt32();
				char[] List = new char[Len];
                for (int i = 0; i < Len; i++)
                {
                    List[i] = (char)wzParser.ReadInt16();
                }
				wzParser.ReadUInt16(); //encrypted null
				string Decrypted = wzParser.DecryptString(List);
				if (wzParser.PeekChar() == -1)
					if (Decrypted[Decrypted.Length - 1] == '/')
						Decrypted = Decrypted.TrimEnd("/".ToCharArray()) + "g"; // Last char should always be a g (.img)
				listEntries.Add(new WzListEntry(Decrypted));
			}
			wzParser.Close();
		}

		public override void SaveToDisk(string path)
		{
            WzBinaryWriter wzWriter = new WzBinaryWriter(File.Create(path), WzIv);
            for (int i = 0; i < listEntries.Count - 1; i++)
            {
                SaveListString(listEntries[i].Name, wzWriter);
            }
            string lastEntry = listEntries[listEntries.Count].Name;
            SaveListString(lastEntry.Substring(0, lastEntry.Length - 1) + "/", wzWriter);
		}

        private void SaveListString(string s, WzBinaryWriter wzWriter)
        {
            wzWriter.WriteCompressedInt(s.Length);
            char[] encryptedChars = wzWriter.EncryptString(s + (char)0);
            for (int i = 0; i < encryptedChars.Length; i++)
            {
                wzWriter.Write((short)encryptedChars[i]);
            }
        }

        public override void Remove()
        {
            Dispose();
        }
    }
}