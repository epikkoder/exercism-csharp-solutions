public static class TelemetryBuffer
{
    public static byte[] ToBuffer(long reading)
    {
        byte[] buffer = new byte[9];
        byte[] payload;

        if (reading is >= ushort.MinValue and <= ushort.MaxValue) // ushort, 2 bytes unsigned
        {
            payload = BitConverter.GetBytes((ushort)reading);
            buffer[0] = 2;
        }
        else if (reading is >= short.MinValue and <= -1) // short, 2 bytes signed
        {
            payload = BitConverter.GetBytes((short)reading);
            buffer[0] = 256 - 2;
        }
        else if (reading is >= 2147483648 and <= uint.MaxValue) // uint, 4 bytes unsigned
        {
            payload = BitConverter.GetBytes((uint)reading);
            buffer[0] = 4;
        }
        else if ((reading is >= 65536 and <= int.MaxValue)
            || (reading is >= int.MinValue and <= -32769)) // int, 4 bytes signed
        {
            payload = BitConverter.GetBytes((int)reading);
            buffer[0] = 256 - 4;
        }
        else
        {
            payload = BitConverter.GetBytes(reading); // long, 8 bytes signed
            buffer[0] = 256 - 8;
        }

        Array.Copy(payload, 0, buffer, 1, payload.Length);

        return buffer;
    }

    public static long FromBuffer(byte[] buffer)
    {
        long bufferValue;

        switch (buffer[0])
        {
            case 2: // ushort, 2 bytes unsigned
                bufferValue = BitConverter.ToUInt16(buffer, 1);
                break;
            case 254: // short, 2 bytes signed
                bufferValue = BitConverter.ToInt16(buffer, 1);
                break;
            case 4: // uint, 4 bytes usigned
                bufferValue = BitConverter.ToUInt32(buffer, 1);
                break;
            case 252: // int, 4 bytes signed
                bufferValue = BitConverter.ToInt32(buffer, 1);
                break;
            case 248: // long, 8 bytes signed
                bufferValue = BitConverter.ToInt64(buffer, 1);
                break;
            default:
                bufferValue = 0;
                break;
        }

        return bufferValue;
    }
}