using Kriptok.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kriptok.JAEditor.Data
{
    public class Merc
    {
        private readonly byte[] bytes;

        /// <summary>
        /// Ubicación del mercenario en el archivo.
        /// </summary>
        private readonly int location;

        public Merc(int location, byte[] bytes)
        {
            this.bytes = bytes;
            this.location = location;

#if DEBUG
            //var mercData = bytes.Select(p => ItemsEnumHelper.Find(p)).ToArray();
#endif
            Right = new ItemSlot(0, bytes, false);            
            Left = new ItemSlot(12, bytes, false);

            Head = new ItemSlot(24, bytes, true);
            Face = new ItemSlot(36, bytes, true);
            Ears = new ItemSlot(48, bytes, true);
            Body = new ItemSlot(60, bytes, true);
            Vest = new ItemSlot(72, bytes, true);
            Pocket1 = new ItemSlot(84, bytes, false);
            Pocket2 = new ItemSlot(96, bytes, false);
            Pocket3 = new ItemSlot(108, bytes, false);
            Pocket4 = new ItemSlot(120, bytes, false);
            Pocket5 = new ItemSlot(132, bytes, false);            
        }

        public override string ToString() => $"[Merc] {Name}";        

        /// <summary>
        /// 6 caracteres.
        /// </summary>
        public string Name
        {
            get
            {
                var n = bytes.Skip(366).Take(6).ToArray();
                if (n[0] == '\0')
                {
                    return null;
                }
                else
                {
                    return string.Join(string.Empty, Encoding.ASCII.GetChars(n).Select(p => p.ToString())).Split('\0').First().Trim();
                }
            }
        }

        public ItemSlot Right { get; }

        public ItemSlot Left { get; }

        public ItemSlot Head { get; }
        public ItemSlot Face { get; }
        public ItemSlot Ears { get; }

        public ItemSlot Body { get; }
        public ItemSlot Vest { get; }

        public ItemSlot Pocket1 { get; }
        public ItemSlot Pocket2 { get; }
        public ItemSlot Pocket3 { get; }
        public ItemSlot Pocket4 { get; }
        public ItemSlot Pocket5 { get; }

        public byte Health1
        {
            get => bytes[387];
            set => bytes[387] = value;
        }

        public byte Health2
        {
            get => bytes[388];
            set => bytes[388] = value;
        }

        public byte Agility
        {
            get => bytes[395];
            set => bytes[395] = value;
        }

        public byte Dexterity
        {
            get => bytes[396];
            set => bytes[396] = value;
        }

        public byte Wisdom
        {
            get => bytes[397];
            set => bytes[397] = value;
        }

        public byte Medic
        {
            get => bytes[398];
            set => bytes[398] = value;
        }

        public byte Explosives
        {
            get => bytes[399];
            set => bytes[399] = value;
        }

        public byte Mechanic
        {
            get => bytes[400];
            set => bytes[400] = value;
        }

        public byte Marksmanship
        {
            get => bytes[401];
            set => bytes[401] = value;
        }

        public byte ExperienceLevel
        {
            get => bytes[402];
            set => bytes[402] = value;
        }

        public bool Camouflaged
        {
            get => bytes[433] == byte.MaxValue;
            set => bytes[433] = byte.MaxValue;
        }

        internal void SaveTo(byte[] bytes)
        {
            foreach (var itemSlot in new ItemSlot[]
            {
                Right,Left,Head,Face,Ears , Body ,Vest,
                Pocket1,Pocket2,Pocket3,Pocket4,Pocket5
            })
            {                
                itemSlot.SaveTo(this.bytes);
            }

            // Copio los bytes.
            for(int i = 0; i < this.bytes.Length; i++)
            {
                bytes[location + i] = this.bytes[i];
            }
        }
    }
}
