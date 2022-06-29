using System;

/// <summary>
/// Summary description for Dec2Hex
/// </summary>
private string Dec2Hex(int dec)
{
    const char[] hex = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14','15' };

    string temp = "";

    while(dec > 0)
    {
        temp = hex[dec % 16] + temp;
        dec /= 16;
    }

    return temp;
}
