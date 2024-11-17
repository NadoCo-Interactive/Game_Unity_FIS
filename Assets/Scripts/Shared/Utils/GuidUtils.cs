using System;

public static class GuidUtils
{
    public static ulong ToUlong(this Guid guid)
    {
        var idULong = BitConverter.ToUInt64(guid.ToByteArray());
        return idULong;
    }
}