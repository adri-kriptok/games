using System.Linq;

namespace Kriptok.JAEditor.Data
{
    public class ItemSlot
    {
        private readonly int location;
        private readonly byte[] bytes;
        private readonly bool forceOneUnit;

        public ItemSlot(int location, byte[] bytes, bool forceOneUnit)
            : this(bytes.Skip(location).Take(12).ToArray(), location, forceOneUnit)
        {
        }

        private ItemSlot(byte[] bytes, int location, bool forceOneUnit)
        {
            this.location = location;
            this.bytes = bytes;
            this.forceOneUnit = forceOneUnit;
        }

        public ItemsEnum Object
        {
            get => ItemsEnumHelper.Find(bytes[0]);
            set => bytes[0] = (byte)(((int)value) & 0xFF);
        }

        public ItemsEnum Attachment
        {
            get => ItemsEnumHelper.Find(bytes[3]);
            set => bytes[3] = (byte)(((int)value) & 0xFF);
        }

        /// <summary>
        /// En el caso de las armas, son las balas.
        /// </summary>
        public byte Quantity
        {
            get => bytes[1];
            set => bytes[1] = value;
        }

        /// <summary>
        /// En el caso de las armas, son las balas.
        /// </summary>
        public byte AttachmentQuantity
        {
            get => bytes[4];
            set => bytes[4] = value;
        }

        /// <summary>
        /// Estado del ítem principal.
        /// </summary>
        public byte State
        {
            get => bytes[2];
            set => bytes[2] = value;
        }

        /// <summary>
        /// Estado del ítem principal.
        /// </summary>
        public byte AttachmentState
        {
            get => bytes[5];
            set => bytes[5] = value;
        }

        public byte State2
        {
            get => bytes[8];
            set => bytes[8] = value;
        }

        public byte State3
        {
            get => bytes[9];
            set => bytes[9] = value;
        }

        public byte State4
        {
            get => bytes[10];
            set => bytes[10] = value;
        }

        public byte State5
        {
            get => bytes[11];
            set => bytes[11] = value;
        }

        public override string ToString()
        {
            if (Object == ItemsEnum.Nothing)
            {
                return string.Empty;
            }
            return $"{Object} x ({Quantity})";
        }

        internal int GetMax() => Object.GetMax();

        internal int GetMin() => Object.GetMin();

        internal int GetVal() => Quantity;

        internal void SaveTo(byte[] bytes)
        {
            if (Object != ItemsEnum.Nothing)
            {
                State = 100;
                
                if (GetMax() == GetMin())
                {
                    Quantity = 99;
                }
                else
                {
                    if (forceOneUnit)
                    {                            
                        Quantity = 1;
                    }
                    else if ((Object & ItemsEnum.HasBullets) == ItemsEnum.HasBullets)
                    {
                    }
                    else if (Quantity > 1)
                    {
                        State2 = 100;
                        if (Quantity > 2)
                        {
                            State3 = 100;
                            if (Quantity > 3)
                            {
                                State4 = 100;
                                if (Quantity > 4)
                                {
                                    State5 = 100;
                                }
                            }
                        }
                    }
                }
                
                if (Attachment != ItemsEnum.Nothing)
                {
                    AttachmentState = 100;
                    AttachmentQuantity = 1;
                }
            }           

            // Copio los bytes.
            for (int i = 0; i < this.bytes.Length; i++)
            {
                bytes[location + i] = this.bytes[i];
            }
        }
    }
}
