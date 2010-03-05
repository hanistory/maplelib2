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

//uncomment to enable UOL resolving, comment to disable it
#define UOLRES

using System.IO;
using MapleLib.WzLib.Util;

namespace MapleLib.WzLib.WzProperties
{
	/// <summary>
	/// A property that's value is a string
	/// </summary>
	public class WzUOLProperty : IWzImageProperty
	{
		#region Fields
		internal string name, val;
		internal IWzObject parent;
		internal WzImage imgParent;
		internal IWzNonDirectoryProperty linkVal;
		#endregion

		#region Inherited Members
        public override IWzImageProperty DeepClone()
        {
            /*WzUOLProperty clone = new WzUOLProperty(name, val);
            clone.parent = parent;
            clone.imgParent = imgParent;*/
            WzUOLProperty clone = (WzUOLProperty)MemberwiseClone();
            return clone;
        }

		public override object WzValue
        {
            get
            {
#if UOLRES
                return LinkValue;
#else
                return this;
#endif
            }
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

		public override IWzImageProperty[] WzProperties
		{
			get
			{
#if UOLRES
				return LinkValue.WzProperties;
#else
                return new IWzImageProperty[0];
#endif
			}
		}

		public override IWzImageProperty this[string name]
		{
			get
			{
#if UOLRES
				return LinkValue[name];
#else
                return null;
#endif
			}
		}

		public override IWzImageProperty GetFromPath(string path)
		{
#if UOLRES
    		return LinkValue.GetFromPath(path);
#else
            return null;
#endif
		}
		/// <summary>
		/// The WzPropertyType of the property
		/// </summary>
		public override WzPropertyType PropertyType { get { return WzPropertyType.UOL; } }

		public override void WriteValue(MapleLib.WzLib.Util.WzBinaryWriter writer)
		{
			writer.WriteStringValue("UOL", 0x73, 0x1B);
			writer.Write((byte)0);
			writer.WriteStringValue(Value, 0, 1);
		}

		public override void ExportXml(StreamWriter writer, int level)
		{
			writer.WriteLine(XmlUtil.Indentation(level) + XmlUtil.EmptyNamedValuePair("WzUOL", this.Name, this.Value));
		}

		/// <summary>
		/// Disposes the object
		/// </summary>
		public override void Dispose()
		{
			name = null;
			val = null;
		}
		#endregion

		#region Custom Members
		/// <summary>
		/// The value of the property
		/// </summary>
		public string Value { get { return val; } set { val = value; } }

#if UOLRES
        public IWzNonDirectoryProperty LinkValue
		{
			get
			{
				if (linkVal == null)
				{
					string[] paths = val.Split('/');
                    linkVal = (IWzNonDirectoryProperty)parent;
                    string asdf = parent.FullPath;
					foreach (string path in paths)
					{
						if (path == "..")
						{
                            linkVal = (IWzNonDirectoryProperty)linkVal.Parent;
						}
						else
						{
							linkVal = linkVal[path];
						}
					}
				}
				return linkVal;
			}
		}
#endif

		/// <summary>
		/// Creates a blank WzUOLProperty
		/// </summary>
		public WzUOLProperty() { }

		/// <summary>
		/// Creates a WzUOLProperty with the specified name
		/// </summary>
		/// <param name="name">The name of the property</param>
		public WzUOLProperty(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Creates a WzUOLProperty with the specified name and value
		/// </summary>
		/// <param name="name">The name of the property</param>
		/// <param name="value">The value of the property</param>
		public WzUOLProperty(string name, string value)
		{
			this.name = name;
			this.val = value;
		}
		#endregion
	}
}