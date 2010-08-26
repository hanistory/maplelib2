using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MapleLib.WzLib.WzStructure
{
    public struct MapleBool //I know I could have used the nullable bool.
    {
        public static readonly byte NotExist = 0;
        public static readonly byte False = 1;
        public static readonly byte True = 2;

        private byte val { get; set; }
        public static implicit operator MapleBool(byte value)
        {
            return new MapleBool
            {
                val = value
            };
        }

        public static implicit operator MapleBool(bool? value)
        {
            return new MapleBool
            {
                val = value == null ? MapleBool.NotExist : (bool)value ? MapleBool.True : MapleBool.False
            };
        }

        public static implicit operator bool(MapleBool value)
        {
            return value == MapleBool.True;
        }

        public override bool Equals(object obj)
        {
            return obj is MapleBool ? ((MapleBool)obj).val.Equals(val) : false;
        }

        public override int GetHashCode()
        {
            return val.GetHashCode();
        }

        public static bool operator ==(MapleBool a, MapleBool b)
        {
            return a.val.Equals(b.val);
        }

        public static bool operator ==(MapleBool a, bool b)
        {
            return (b && (a.val == MapleBool.True)) || (!b && (a.val == MapleBool.False));
        }

        public static bool operator !=(MapleBool a, MapleBool b)
        {
            return !a.val.Equals(b.val);
        }

        public static bool operator !=(MapleBool a, bool b)
        {
            return (b && (a.val != MapleBool.True)) || (!b && (a.val != MapleBool.False));
        }
    }
}
