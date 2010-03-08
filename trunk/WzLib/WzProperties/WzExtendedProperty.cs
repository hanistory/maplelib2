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
/*
using System.IO;
using MapleLib.WzLib.Util;

namespace MapleLib.WzLib.WzProperties
{*/
	/// <summary>
	/// A wz property that extends to a different property
	/// </summary>
	/*public class WzExtendedPropertyT : IWzImageProperty
	{
		#region Fields
		internal IWzImageProperty extendedProperty;
		internal int endOfBlock = 0;
		internal uint offset = 0;
		internal string name;
		internal WzBinaryReader reader;
		internal IWzObject parent;
		internal WzImage imgParent;
		#endregion

		#region Inherited Members
        public override IWzImageProperty DeepClone()
        {
            /*WzExtendedProperty clone = new WzExtendedProperty(offset,endOfBlock, name);
            clone.parent = parent;
            clone.imgParent = imgParent;
            clone.reader = reader;
            WzExtendedPropertyT clone = (WzExtendedPropertyT)MemberwiseClone();
            clone.extendedProperty = extendedProperty.DeepClone();
            return clone;
        }

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
		public override WzPropertyType PropertyType { get { return WzPropertyType.ByteFloat; } }*/
		/*public override void WriteValue(WzBinaryWriter writer)
		{
			writer.Write((byte)9);
			long beforePos = writer.BaseStream.Position;
			writer.Write(0); // Placeholder
			ExtendedProperty.WriteValue(writer);
			int len = (int)(writer.BaseStream.Position - beforePos);
			long newPos = writer.BaseStream.Position;
			writer.BaseStream.Position = beforePos;
			writer.Write(len - 4);
			writer.BaseStream.Position = newPos;
		}*/
		/*/// <summary>
		/// Dispose the object
		/// </summary>
		public override void Dispose()
		{
			name = null;
			extendedProperty.Dispose();
			reader = null;
		}
		#endregion

		#region Custom Members
		/// <summary>
		/// The property that this WzExtendedProperty extends to
		/// </summary>
		public IWzImageProperty ExtendedProperty { get { return extendedProperty; } set { extendedProperty = value; } }
		/// <summary>
		/// Creates a blank WzEntendedProperty
		/// </summary>
		public WzExtendedPropertyT() { }
		/// <summary>
		/// Creates a WzExtendedProperty with the specified name
		/// </summary>
		/// <param name="name">The name of the property</param>
		public WzExtendedPropertyT(string name)
		{
			this.name = name;
		}
		internal WzExtendedPropertyT(uint offset, string name)
		{
			this.name = name;
			this.offset = offset;
		}
        internal WzExtendedPropertyT(uint offset, int eob, string name)
		{
			endOfBlock = eob;
			this.name = name;
			this.offset = offset;
		}
		#endregion

		#region Parsing Methods





		/// <summary>
		/// Parses the extended property
		/// </summary>
		/// <param name="reader">The current BinaryReader that's reading the wz file</param>
		internal void ParseExtendedProperty(WzBinaryReader reader)
		{
			this.reader = reader;
			DumpBlock(endOfBlock, name);
		}
		internal void DumpBlock(int endOfBlock, string name)
		{
			switch (reader.ReadByte())
			{
				case 0x1B:
					ExtractMore(endOfBlock, name, reader.ReadStringAtOffset(offset + reader.ReadInt32()));
					return;
				case 0x73:
					ExtractMore(endOfBlock, name, "");
					return;
			}
		}
		internal void ExtractMore(int eob, string name, string iname)
		{
			if (iname == "")
				iname = reader.ReadString();
			switch (iname)
			{
				case "Property":
					WzSubProperty subProp = new WzSubProperty(name) { Parent = parent, ParentImage = imgParent };
					reader.BaseStream.Position += 2;
					subProp.AddProperties(IWzImageProperty.ParsePropertyList(offset, reader, subProp, imgParent));
					extendedProperty = subProp;
					break;
				case "Canvas":
					WzCanvasProperty canvasProp = new WzCanvasProperty(name) { Parent = parent, ParentImage = imgParent };
					reader.BaseStream.Position++;
					if (reader.ReadByte() == 1)
					{
						reader.BaseStream.Position += 2;
						canvasProp.AddProperties(IWzImageProperty.ParsePropertyList(offset, reader, canvasProp, imgParent));
					}
					canvasProp.PngProperty = new WzPngProperty(reader) { Parent = canvasProp, ParentImage = imgParent };
					extendedProperty = canvasProp;
					break;
				case "Shape2D#Vector2D":
					WzVectorProperty vecProp = new WzVectorProperty(name) { Parent = parent, ParentImage = imgParent };
					vecProp.X = new WzCompressedIntProperty("X", reader.ReadCompressedInt()) { Parent = vecProp, ParentImage = imgParent };
					vecProp.Y = new WzCompressedIntProperty("Y", reader.ReadCompressedInt()) { Parent = vecProp, ParentImage = imgParent };
					extendedProperty = vecProp;
					break;
				case "Shape2D#Convex2D":
					WzConvexProperty convexProp = new WzConvexProperty(name) { Parent = parent, ParentImage = imgParent };
					int convexEntryCount = reader.ReadCompressedInt();
					for (int i = 0; i < convexEntryCount; i++)
					{
                        WzExtendedPropertyT exProp = new WzExtendedPropertyT(offset, name) { Parent = convexProp, ParentImage = imgParent };
						exProp.ParseExtendedProperty(reader);
						convexProp.AddProperty(exProp);
					}
					extendedProperty = convexProp;
					break;
				case "Sound_DX8":
					WzSoundProperty soundProp = new WzSoundProperty(name) { Parent = parent, ParentImage = imgParent };
					soundProp.ParseSound(reader);
					extendedProperty = soundProp;
					break;
				case "UOL":
					reader.BaseStream.Position++;
					switch (reader.ReadByte())
					{
						case 0:
							extendedProperty = new WzUOLProperty(name, reader.ReadString()) { Parent = parent, ParentImage = imgParent };
							break;
						case 1:
							extendedProperty = new WzUOLProperty(name, reader.ReadStringAtOffset(offset + reader.ReadInt32())) { Parent = parent, ParentImage = imgParent };
							break;
					}
					break;
			}
		}
		#endregion
	}
}*/