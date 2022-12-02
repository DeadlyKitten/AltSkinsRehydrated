namespace AltSkinsRehydrated.Data
{
    class OnlineSkinData : IFastSerializable, IFastWritable, IFastReadable
    {
        public string SkinID;

        public OnlineSkinData(string skinID)
        {
            SkinID = skinID;
        }

        public void Read(FastInStream reader)
        {
            SkinID = reader.ReadAscii();
        }

        public void Write(FastOutStream writer)
        {
            writer.WriteAscii(SkinID);
        }
    }
}
