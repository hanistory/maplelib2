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

using System.IO;
using System;
using MapleLib.WzLib.Util;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that contains data for an MP3 file
	/// </summary>
	public class WzSoundProperty : IExtended
	{
		#region Fields
		internal string name;
		internal byte[] mp3bytes = null;
		internal IWzObject parent;
        internal int len_ms;
		internal WzImage imgParent;
        internal WzBinaryReader wzReader;
        internal long offs;
        public static readonly byte[] soundHeaderMask = new byte[] { 0x02, 0x83, 0xEB, 0x36, 0xE4, 0x4F, 0x52, 0xCE, 0x11, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70, 0x8B, 0xEB, 0x36, 0xE4, 0x4F, 0x52, 0xCE, 0x11, 0x9F, 0x53, 0x00, 0x20, 0xAF, 0x0B, 0xA7, 0x70, 0x00, 0x01, 0x81, 0x9F, 0x58, 0x05, 0x56, 0xC3, 0xCE, 0x11, 0xBF, 0x01, 0x00, 0xAA, 0x00, 0x55, 0x59, 0x5A, 0x1E, 0x55, 0x00, 0x02, 0x00,/*FRQ 56*/0xAA, 0xBB, 0xCC, 0xDD/*/FRQ 59*/, 0x10, 0x27, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x01, 0x00, 0x02, 0x00, 0x00, 0x00, 0x0A, 0x02, 0x01, 0x00, 0x00, 0x00 };
		#endregion

		#region Inherited Members
        public override void SetValue(object value)
        {
            if (value is byte[]) SetDataUnsafe((byte[])value);
            else SetDataUnsafe(CreateCustomProperty("foo", (string)value).GetBytes(false));
        }

        public override IWzImageProperty DeepClone()
        {
            WzSoundProperty clone = (WzSoundProperty)MemberwiseClone();
            return clone;
        }

		public override object WzValue { get { return GetBytes(false); } }
		/// <summary>
		/// The parent of the object
		/// </summary>
		public override IWzObject Parent { get { return parent; } internal set { parent = value; } }
		/// <summary>
		/// The image that this property is contained in
		/// </summary>
		public override WzImage ParentImage { get { return imgParent; } internal set { imgParent = value; } }
		/// <summary>
		/// The name of the property
		/// </summary>
		public override string Name { get { return name; } set { name = value; } }
		/// <summary>
		/// The WzPropertyType of the property
		/// </summary>
		public override WzPropertyType PropertyType { get { return WzPropertyType.Sound; } }
		public override void WriteValue(WzBinaryWriter writer)
		{
            byte[] data = GetBytes(false);
			writer.WriteStringValue("Sound_DX8", 0x73, 0x1B);
			writer.Write((byte)0);
			writer.WriteCompressedInt(data.Length);
			writer.WriteCompressedInt(0);
			writer.Write(data);
		}
		public override void ExportXml(StreamWriter writer, int level)
		{
			writer.WriteLine(XmlUtil.Indentation(level) + XmlUtil.EmptyNamedTag("WzSound", this.Name));
		}
		/// <summary>
		/// Disposes the object
		/// </summary>
		public override void Dispose()
		{
			name = null;
			mp3bytes = null;
		}
		#endregion

		#region Custom Members
		/// <summary>
		/// The data of the mp3 file
		/// </summary>
        [Obsolete("To enable more control over memory usage, this property was superseded by the GetBytes method and will be removed in the future")]
		public byte[] SoundData { get { return GetBytes(false); } }
        /// <summary>
        /// Length of the mp3 file in milliseconds
        /// </summary>
        public int Length { get { return len_ms; } }
		/// <summary>
		/// Creates a blank WzSoundProperty
		/// </summary>
		public WzSoundProperty() { }
		/// <summary>
		/// Creates a WzSoundProperty with the specified name
		/// </summary>
		/// <param name="name">The name of the property</param>
		public WzSoundProperty(string name)
		{
			this.name = name;
		}

        private static byte[] Combine(byte[] a, byte[] b)
        {
            byte[] result = new byte[a.Length + b.Length];
            Array.Copy(a, 0, result, 0, a.Length);
            Array.Copy(b, 0, result, a.Length, b.Length);
            return result;
        }

        public void SetDataUnsafe(byte[] data)
        {
            this.mp3bytes = data;
        }

        public static WzSoundProperty CreateCustomProperty(string name, string file)
        {
            WzSoundProperty newProp = new WzSoundProperty(name);
            MP3Header header = new MP3Header();
            header.ReadMP3Information(file);
            newProp.len_ms = header.intLength * 1000;
            byte[] frequencyBytes = BitConverter.GetBytes(header.intFrequency);
            byte[] headerBytes = new byte[soundHeaderMask.Length];
            Array.Copy(soundHeaderMask, headerBytes, headerBytes.Length);
            for (int i = 0; i < 4; i++) { headerBytes[56 + i] = frequencyBytes[i]; }
            newProp.mp3bytes = Combine(headerBytes, File.ReadAllBytes(file));
            return newProp;
        }
        #endregion

        #region Parsing Methods
        internal void ParseSound(WzBinaryReader reader)
		{
			reader.BaseStream.Position++;
            offs = reader.BaseStream.Position;
			int soundDataLen = reader.ReadCompressedInt();
			len_ms = reader.ReadCompressedInt();
			//mp3bytes = reader.ReadBytes(soundDataLen);
            reader.BaseStream.Position += soundDataLen;
            wzReader = reader;
		}

        public byte[] GetBytes(bool saveInMemory)
        {
            if (mp3bytes != null)
                return mp3bytes;
            else
            {
                if (wzReader == null) return null;
                long currentPos = wzReader.BaseStream.Position;
                wzReader.BaseStream.Position = offs;
                int soundDataLen = wzReader.ReadCompressedInt();
                wzReader.ReadCompressedInt();
                //wzReader.BaseStream.Position += 82;
                mp3bytes = wzReader.ReadBytes(soundDataLen);
                wzReader.BaseStream.Position = currentPos;
                if (saveInMemory)
                    return mp3bytes;
                else
                {
                    byte[] result = mp3bytes;
                    mp3bytes = null;
                    return result;
                }
            }
        }

        public void SaveToFile(string file)
        {
            File.WriteAllBytes(file, GetBytes(false));
        }
		#endregion

        #region Cast Values
        internal override byte[] ToBytes(byte[] def)
        {
            return GetBytes(false);
        }
        #endregion
	}
}