using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard
{
    public class Des
    {
        
        //C# DES加解密主函数
        private static string DES(string key, string strMessage, bool isEncrypt, int mode, string strIV)
        {
            int[] spfunction1 = new int[] { 0x1010400, 0, 0x10000, 0x1010404, 0x1010004, 0x10404, 0x4, 0x10000, 0x400, 0x1010400, 0x1010404, 0x400, 0x1000404, 0x1010004, 0x1000000, 0x4, 0x404, 0x1000400, 0x1000400, 0x10400, 0x10400, 0x1010000, 0x1010000, 0x1000404, 0x10004, 0x1000004, 0x1000004, 0x10004, 0, 0x404, 0x10404, 0x1000000, 0x10000, 0x1010404, 0x4, 0x1010000, 0x1010400, 0x1000000, 0x1000000, 0x400, 0x1010004, 0x10000, 0x10400, 0x1000004, 0x400, 0x4, 0x1000404, 0x10404, 0x1010404, 0x10004, 0x1010000, 0x1000404, 0x1000004, 0x404, 0x10404, 0x1010400, 0x404, 0x1000400, 0x1000400, 0, 0x10004, 0x10400, 0, 0x1010004 };
            int[] spfunction2 = new int[] { -0x7fef7fe0, -0x7fff8000, 0x8000, 0x108020, 0x100000, 0x20, -0x7fefffe0, -0x7fff7fe0, -0x7fffffe0, -0x7fef7fe0, -0x7fef8000, -0x8000000, -0x7fff8000, 0x100000, 0x20, -0x7fefffe0, 0x108000, 0x100020, -0x7fff7fe0, 0, -0x8000000, 0x8000, 0x108020, -0x7ff00000, 0x100020, -0x7fffffe0, 0, 0x108000, 0x8020, -0x7fef8000, -0x7ff00000, 0x8020, 0, 0x108020, -0x7fefffe0, 0x100000, -0x7fff7fe0, -0x7ff00000, -0x7fef8000, 0x8000, -0x7ff00000, -0x7fff8000, 0x20, -0x7fef7fe0, 0x108020, 0x20, 0x8000, -0x8000000, 0x8020, -0x7fef8000, 0x100000, -0x7fffffe0, 0x100020, -0x7fff7fe0, -0x7fffffe0, 0x100020, 0x108000, 0, -0x7fff8000, 0x8020, -0x8000000, -0x7fefffe0, -0x7fef7fe0, 0x108000 };
            int[] spfunction3 = new int[] { 0x208, 0x8020200, 0, 0x8020008, 0x8000200, 0, 0x20208, 0x8000200, 0x20008, 0x8000008, 0x8000008, 0x20000, 0x8020208, 0x20008, 0x8020000, 0x208, 0x8000000, 0x8, 0x8020200, 0x200, 0x20200, 0x8020000, 0x8020008, 0x20208, 0x8000208, 0x20200, 0x20000, 0x8000208, 0x8, 0x8020208, 0x200, 0x8000000, 0x8020200, 0x8000000, 0x20008, 0x208, 0x20000, 0x8020200, 0x8000200, 0, 0x200, 0x20008, 0x8020208, 0x8000200, 0x8000008, 0x200, 0, 0x8020008, 0x8000208, 0x20000, 0x8000000, 0x8020208, 0x8, 0x20208, 0x20200, 0x8000008, 0x8020000, 0x8000208, 0x208, 0x8020000, 0x20208, 0x8, 0x8020008, 0x20200 };
            int[] spfunction4 = new int[] { 0x802001, 0x2081, 0x2081, 0x80, 0x802080, 0x800081, 0x800001, 0x2001, 0, 0x802000, 0x802000, 0x802081, 0x81, 0, 0x800080, 0x800001, 0x1, 0x2000, 0x800000, 0x802001, 0x80, 0x800000, 0x2001, 0x2080, 0x800081, 0x1, 0x2080, 0x800080, 0x2000, 0x802080, 0x802081, 0x81, 0x800080, 0x800001, 0x802000, 0x802081, 0x81, 0, 0, 0x802000, 0x2080, 0x800080, 0x800081, 0x1, 0x802001, 0x2081, 0x2081, 0x80, 0x802081, 0x81, 0x1, 0x2000, 0x800001, 0x2001, 0x802080, 0x800081, 0x2001, 0x2080, 0x800000, 0x802001, 0x80, 0x800000, 0x2000, 0x802080 };
            int[] spfunction5 = new int[] { 0x100, 0x2080100, 0x2080000, 0x42000100, 0x80000, 0x100, 0x40000000, 0x2080000, 0x40080100, 0x80000, 0x2000100, 0x40080100, 0x42000100, 0x42080000, 0x80100, 0x40000000, 0x2000000, 0x40080000, 0x40080000, 0, 0x40000100, 0x42080100, 0x42080100, 0x2000100, 0x42080000, 0x40000100, 0, 0x42000000, 0x2080100, 0x2000000, 0x42000000, 0x80100, 0x80000, 0x42000100, 0x100, 0x2000000, 0x40000000, 0x2080000, 0x42000100, 0x40080100, 0x2000100, 0x40000000, 0x42080000, 0x2080100, 0x40080100, 0x100, 0x2000000, 0x42080000, 0x42080100, 0x80100, 0x42000000, 0x42080100, 0x2080000, 0, 0x40080000, 0x42000000, 0x80100, 0x2000100, 0x40000100, 0x80000, 0, 0x40080000, 0x2080100, 0x40000100 };
            int[] spfunction6 = new int[] { 0x20000010, 0x20400000, 0x4000, 0x20404010, 0x20400000, 0x10, 0x20404010, 0x400000, 0x20004000, 0x404010, 0x400000, 0x20000010, 0x400010, 0x20004000, 0x20000000, 0x4010, 0, 0x400010, 0x20004010, 0x4000, 0x404000, 0x20004010, 0x10, 0x20400010, 0x20400010, 0, 0x404010, 0x20404000, 0x4010, 0x404000, 0x20404000, 0x20000000, 0x20004000, 0x10, 0x20400010, 0x404000, 0x20404010, 0x400000, 0x4010, 0x20000010, 0x400000, 0x20004000, 0x20000000, 0x4010, 0x20000010, 0x20404010, 0x404000, 0x20400000, 0x404010, 0x20404000, 0, 0x20400010, 0x10, 0x4000, 0x20400000, 0x404010, 0x4000, 0x400010, 0x20004010, 0, 0x20404000, 0x20000000, 0x400010, 0x20004010 };
            int[] spfunction7 = new int[] { 0x200000, 0x4200002, 0x4000802, 0, 0x800, 0x4000802, 0x200802, 0x4200800, 0x4200802, 0x200000, 0, 0x4000002, 0x2, 0x4000000, 0x4200002, 0x802, 0x4000800, 0x200802, 0x200002, 0x4000800, 0x4000002, 0x4200000, 0x4200800, 0x200002, 0x4200000, 0x800, 0x802, 0x4200802, 0x200800, 0x2, 0x4000000, 0x200800, 0x4000000, 0x200800, 0x200000, 0x4000802, 0x4000802, 0x4200002, 0x4200002, 0x2, 0x200002, 0x4000000, 0x4000800, 0x200000, 0x4200800, 0x802, 0x200802, 0x4200800, 0x802, 0x4000002, 0x4200802, 0x4200000, 0x200800, 0, 0x2, 0x4200802, 0, 0x200802, 0x4200000, 0x800, 0x4000002, 0x4000800, 0x800, 0x200002 };
            int[] spfunction8 = new int[] { 0x10001040, 0x1000, 0x40000, 0x10041040, 0x10000000, 0x10001040, 0x40, 0x10000000, 0x40040, 0x10040000, 0x10041040, 0x41000, 0x10041000, 0x41040, 0x1000, 0x40, 0x10040000, 0x10000040, 0x10001000, 0x1040, 0x41000, 0x40040, 0x10040040, 0x10041000, 0x1040, 0, 0, 0x10040040, 0x10000040, 0x10001000, 0x41040, 0x40000, 0x41040, 0x40000, 0x10041000, 0x1000, 0x40, 0x10040040, 0x1000, 0x41040, 0x10001000, 0x40, 0x10000040, 0x10040000, 0x10040040, 0x10000000, 0x40000, 0x10001040, 0, 0x10041040, 0x40040, 0x10000040, 0x10040000, 0x10001000, 0x10001040, 0, 0x10041040, 0x41000, 0x41000, 0x1040, 0x1040, 0x40040, 0x10000000, 0x10041000 };



            int[] keys = DES_CreateKey(key);

            int m = 0, i, j, temp, right1, right2, left, right;
            int[] looping;
            int cbcleft = 0, cbcleft2 = 0, cbcright = 0, cbcright2 = 0;
            int endloop, loopinc;
            int len = strMessage.Length;
            int chunk = 0;
            int iterations = (keys.Length == 32) ? 3 : 9;

            if (iterations == 3)
            {
                looping = isEncrypt ? new int[] { 0, 32, 2 } : new int[] { 30, -2, -2 };
            }
            else
            {
                looping = isEncrypt ? new int[] { 0, 32, 2, 62, 30, -2, 64, 96, 2 } : new int[] { 94, 62, -2, 32, 64, 2, 30, -2, -2 };
            }

            strMessage += "\0\0\0\0\0\0\0\0";

            StringBuilder result = new StringBuilder();
            StringBuilder tempresult = new StringBuilder();

            if (mode == 1)
            {
                int ivLen = strIV.Length;
                char[] civ = strIV.ToCharArray();
                int[] iv = new int[ivLen + 8];

                for (i = 0; i < ivLen; i++)
                {
                    iv[i] = Convert.ToInt32(civ[i]);
                }

                for (i = ivLen; i < (ivLen + 8); ++i)
                {
                    iv[i] = 0;
                }

                cbcleft = (iv[m++] << 24) | (iv[m++] << 16) | (iv[m++] << 8) | iv[m++];
                cbcright = (iv[m++] << 24) | (iv[m++] << 16) | (iv[m++] << 8) | iv[m++];
                m = 0;
            }

            while (m < len)
            {

                int[] message = new int[len + 8];
                char[] cm = strMessage.ToCharArray();
                for (i = 0; i < (len + 8); ++i)
                {
                    message[i] = Convert.ToInt32(cm[i]);
                }

                if (isEncrypt)
                {
                    left = (message[m++] << 16) | message[m++];
                    right = (message[m++] << 16) | message[m++];
                }
                else
                {
                    left = (message[m++] << 24) | (message[m++] << 16) | (message[m++] << 8) | message[m++];
                    right = (message[m++] << 24) | (message[m++] << 16) | (message[m++] << 8) | message[m++];
                }

                if (mode == 1)
                {
                    if (isEncrypt)
                    {
                        left ^= cbcleft;
                        right ^= cbcright;
                    }
                    else
                    {
                        cbcleft2 = cbcleft;
                        cbcright2 = cbcright;
                        cbcleft = left;
                        cbcright = right;
                    }
                }

                temp = (MoveByte(left, 4) ^ right) & 0x0f0f0f0f;
                right ^= temp;
                left ^= (temp << 4);
                temp = (MoveByte(left, 16) ^ right) & 0x0000ffff;
                right ^= temp;
                left ^= (temp << 16);
                temp = (MoveByte(right, 2) ^ left) & 0x33333333;
                left ^= temp;
                right ^= (temp << 2);
                temp = (MoveByte(right, 8) ^ left) & 0x00ff00ff;
                left ^= temp; right ^= (temp << 8);
                temp = (MoveByte(left, 1) ^ right) & 0x55555555;
                right ^= temp;
                left ^= (temp << 1);
                left = ((left << 1) | MoveByte(left, 31));
                right = ((right << 1) | MoveByte(right, 31));
                for (j = 0; j < iterations; j += 3)
                {
                    endloop = looping[j + 1];
                    loopinc = looping[j + 2];
                    for (i = looping[j]; i != endloop; i += loopinc)
                    {
                        right1 = right ^ keys[i];
                        right2 = (MoveByte(right, 4) | (right << 28)) ^ keys[i + 1];
                        temp = left;
                        left = right;
                        right = temp ^ (spfunction2[MoveByte(right1, 24) & 0x3f] | spfunction4[MoveByte(right1, 16) & 0x3f] | spfunction6[MoveByte(right1, 8) & 0x3f] | spfunction8[right1 & 0x3f] | spfunction1[MoveByte(right2, 24) & 0x3f] | spfunction3[MoveByte(right2, 16) & 0x3f] | spfunction5[MoveByte(right2, 8) & 0x3f] | spfunction7[right2 & 0x3f]);
                    }

                    temp = left;
                    left = right;
                    right = temp;
                }
                left = (MoveByte(left, 1) | (left << 31));
                right = (MoveByte(right, 1) | (right << 31));
                temp = (MoveByte(left, 1) ^ right) & 0x55555555;
                right ^= temp;
                left ^= (temp << 1);
                temp = (MoveByte(right, 8) ^ left) & 0x00ff00ff;
                left ^= temp; right ^= (temp << 8);
                temp = (MoveByte(right, 2) ^ left) & 0x33333333;
                left ^= temp;
                right ^= (temp << 2);
                temp = (MoveByte(left, 16) ^ right) & 0x0000ffff;
                right ^= temp;
                left ^= (temp << 16);
                temp = (MoveByte(left, 4) ^ right) & 0x0f0f0f0f;
                right ^= temp;
                left ^= (temp << 4);

                if (mode == 1)
                {
                    if (isEncrypt)
                    {
                        cbcleft = left;
                        cbcright = right;
                    }
                    else
                    {
                        left ^= cbcleft2;
                        right ^= cbcright2;
                    }
                }

                if (isEncrypt)
                {
                    tempresult.Append(Convert.ToChar((MoveByte(left, 24))));
                    tempresult.Append(Convert.ToChar((MoveByte(left, 16) & 0xff)));
                    tempresult.Append(Convert.ToChar((MoveByte(left, 8) & 0xff)));
                    tempresult.Append(Convert.ToChar((left & 0xff)));
                    tempresult.Append(Convert.ToChar(MoveByte(right, 24)));
                    tempresult.Append(Convert.ToChar((MoveByte(right, 16) & 0xff)));
                    tempresult.Append(Convert.ToChar((MoveByte(right, 8) & 0xff)));
                    tempresult.Append(Convert.ToChar((right & 0xff)));
                }
                else
                {
                    tempresult.Append(Convert.ToChar(((MoveByte(left, 16) & 0xffff))));
                    tempresult.Append(Convert.ToChar((left & 0xffff)));
                    tempresult.Append(Convert.ToChar((MoveByte(right, 16) & 0xffff)));
                    tempresult.Append(Convert.ToChar((right & 0xffff)));
                }

                if (isEncrypt)
                {
                    chunk += 16;
                }
                else
                {
                    chunk += 8;
                }

                if (chunk == 512)
                {
                    result.Append(tempresult.ToString());
                    tempresult.Remove(0, tempresult.Length);
                    chunk = 0;
                }
            }

            return result.ToString() + tempresult.ToString();
        }

        //密钥生成函数
        private static int[] DES_CreateKey(string strKey)
        {
            int[] pc2bytes0 = new int[] { 0, 0x4, 0x20000000, 0x20000004, 0x10000, 0x10004, 0x20010000, 0x20010004, 0x200, 0x204, 0x20000200, 0x20000204, 0x10200, 0x10204, 0x20010200, 0x20010204 };
            int[] pc2bytes1 = new int[] { 0, 0x1, 0x100000, 0x100001, 0x4000000, 0x4000001, 0x4100000, 0x4100001, 0x100, 0x101, 0x100100, 0x100101, 0x4000100, 0x4000101, 0x4100100, 0x4100101 };
            int[] pc2bytes2 = new int[] { 0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808, 0, 0x8, 0x800, 0x808, 0x1000000, 0x1000008, 0x1000800, 0x1000808 };
            int[] pc2bytes3 = new int[] { 0, 0x200000, 0x8000000, 0x8200000, 0x2000, 0x202000, 0x8002000, 0x8202000, 0x20000, 0x220000, 0x8020000, 0x8220000, 0x22000, 0x222000, 0x8022000, 0x8222000 };
            int[] pc2bytes4 = new int[] { 0, 0x40000, 0x10, 0x40010, 0, 0x40000, 0x10, 0x40010, 0x1000, 0x41000, 0x1010, 0x41010, 0x1000, 0x41000, 0x1010, 0x41010 };
            int[] pc2bytes5 = new int[] { 0, 0x400, 0x20, 0x420, 0, 0x400, 0x20, 0x420, 0x2000000, 0x2000400, 0x2000020, 0x2000420, 0x2000000, 0x2000400, 0x2000020, 0x2000420 };
            int[] pc2bytes6 = new int[] { 0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002, 0, 0x10000000, 0x80000, 0x10080000, 0x2, 0x10000002, 0x80002, 0x10080002 };
            int[] pc2bytes7 = new int[] { 0, 0x10000, 0x800, 0x10800, 0x20000000, 0x20010000, 0x20000800, 0x20010800, 0x20000, 0x30000, 0x20800, 0x30800, 0x20020000, 0x20030000, 0x20020800, 0x20030800 };
            int[] pc2bytes8 = new int[] { 0, 0x40000, 0, 0x40000, 0x2, 0x40002, 0x2, 0x40002, 0x2000000, 0x2040000, 0x2000000, 0x2040000, 0x2000002, 0x2040002, 0x2000002, 0x2040002 };
            int[] pc2bytes9 = new int[] { 0, 0x10000000, 0x8, 0x10000008, 0, 0x10000000, 0x8, 0x10000008, 0x400, 0x10000400, 0x408, 0x10000408, 0x400, 0x10000400, 0x408, 0x10000408 };
            int[] pc2bytes10 = new int[] { 0, 0x20, 0, 0x20, 0x100000, 0x100020, 0x100000, 0x100020, 0x2000, 0x2020, 0x2000, 0x2020, 0x102000, 0x102020, 0x102000, 0x102020 };
            int[] pc2bytes11 = new int[] { 0, 0x1000000, 0x200, 0x1000200, 0x200000, 0x1200000, 0x200200, 0x1200200, 0x4000000, 0x5000000, 0x4000200, 0x5000200, 0x4200000, 0x5200000, 0x4200200, 0x5200200 };
            int[] pc2bytes12 = new int[] { 0, 0x1000, 0x8000000, 0x8001000, 0x80000, 0x81000, 0x8080000, 0x8081000, 0x10, 0x1010, 0x8000010, 0x8001010, 0x80010, 0x81010, 0x8080010, 0x8081010 };
            int[] pc2bytes13 = new int[] { 0, 0x4, 0x100, 0x104, 0, 0x4, 0x100, 0x104, 0x1, 0x5, 0x101, 0x105, 0x1, 0x5, 0x101, 0x105 };

            int iterations = strKey.Length >= 24 ? 3 : 1;

            int[] keys = new int[32 * iterations];
            int[] shifts = new int[] { 0, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 };
            int lefttemp, righttemp;
            int m = 0, n = 0;
            int left, right, temp;

            char[] ckey = strKey.ToCharArray();

            int strLen = strKey.Length;
            int keyLen = strLen + iterations * 8;
            int[] key = new int[keyLen];

            for (int i = 0; i < strLen; ++i)
            {
                key[i] = Convert.ToInt32(ckey[i]);
            }

            for (int i = strLen; i < keyLen; ++i)
            {
                key[i] = 0;
            }

            for (int j = 0; j < iterations; j++)
            {
                left = (key[m++] << 24) | (key[m++] << 16) | (key[m++] << 8) | key[m++];
                right = (key[m++] << 24) | (key[m++] << 16) | (key[m++] << 8) | key[m++];
                temp = (MoveByte(left, 4) ^ right) & 0x0f0f0f0f;
                right ^= temp;
                left ^= (temp << 4);
                temp = (MoveByte(right, -16) ^ left) & 0x0000ffff;
                left ^= temp;
                right ^= (temp << -16);
                temp = (MoveByte(left, 2) ^ right) & 0x33333333;
                right ^= temp;
                left ^= (temp << 2);
                temp = (MoveByte(right, -16) ^ left) & 0x0000ffff;
                left ^= temp;
                right ^= (temp << -16);
                temp = (MoveByte(left, 1) ^ right) & 0x55555555;
                right ^= temp;
                left ^= (temp << 1);
                temp = (MoveByte(right, 8) ^ left) & 0x00ff00ff;
                left ^= temp;
                right ^= (temp << 8);
                temp = (MoveByte(left, 1) ^ right) & 0x55555555;
                right ^= temp;
                left ^= (temp << 1);
                temp = (left << 8) | (MoveByte(right, 20) & 0x000000f0);
                left = (right << 24) | ((right << 8) & 0xff0000) | (MoveByte(right, 8) & 0xff00) | (MoveByte(right, 24) & 0xf0);
                right = temp;

                int shiftLen = shifts.Length;
                for (int i = 0; i < shiftLen; i++)
                {
                    if (shifts[i] == 1)
                    {
                        left = (left << 2) | MoveByte(left, 26);
                        right = (right << 2) | MoveByte(right, 26);
                    }
                    else
                    {
                        left = (left << 1) | MoveByte(left, 27);
                        right = (right << 1) | MoveByte(right, 27);
                    }
                    left &= -0xf;
                    right &= -0xf;
                    lefttemp = pc2bytes0[MoveByte(left, 28)] | pc2bytes1[MoveByte(left, 24) & 0xf] | pc2bytes2[MoveByte(left, 20) & 0xf] | pc2bytes3[MoveByte(left, 16) & 0xf] | pc2bytes4[MoveByte(left, 12) & 0xf] | pc2bytes5[MoveByte(left, 8) & 0xf] | pc2bytes6[MoveByte(left, 4) & 0xf];
                    righttemp = pc2bytes7[MoveByte(right, 28)] | pc2bytes8[MoveByte(right, 24) & 0xf] | pc2bytes9[MoveByte(right, 20) & 0xf] | pc2bytes10[MoveByte(right, 16) & 0xf] | pc2bytes11[MoveByte(right, 12) & 0xf] | pc2bytes12[MoveByte(right, 8) & 0xf] | pc2bytes13[MoveByte(right, 4) & 0xf];
                    temp = (MoveByte(righttemp, 16) ^ lefttemp) & 0x0000ffff;
                    keys[n++] = lefttemp ^ temp;
                    keys[n++] = righttemp ^ (temp << 16);
                }
            }



            return keys;
        }
        //实现无符号右移,相当于javascript中的>>>运算符
        private static int MoveByte(int val, int pos)
        {
            string strBit = string.Empty;

            //取得二进制字符串
            strBit = Convert.ToString(val, 2);

            //转成32位长度的二进制
            if (val >= 0)
            {
                strBit = Convert.ToString(val, 2);
                int len = strBit.Length;
                len = 32 - len;

                for (int i = 0; i < len; ++i)
                {
                    strBit = "0" + strBit;
                }
            }


            //如果pos小于0,则应移pos + 32位
            pos = (pos < 0) ? pos + 32 : pos;

            for (int i = 0; i < pos; ++i)
            {
                strBit = "0" + strBit.Substring(0, 31);
            }

            return Convert.ToInt32(strBit, 2);
        }

        //将普通的字符串转换成16进制的字符串
        private static string StringToHex(string s)
        {
            StringBuilder sb = new StringBuilder();
            char[] hexs = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

            int len = s.Length;
            char[] cs = s.ToCharArray();
            for (int i = 0; i < len; ++i)
            {
                sb.Append(hexs[cs[i] >> 4]);
                sb.Append(hexs[cs[i] & 0xf]);
            }

            return sb.ToString();
        }

        //将16进制的字符串转换成普通的字符串
        private static string HexToString(string s)
        {
            StringBuilder sb = new StringBuilder();
            int len = s.Length;
            char c;
            for (int i = 0; i < len; i += 2)
            {
                c = Convert.ToChar(Convert.ToInt16("0x" + s.Substring(i, 2), 16));
                sb.Append(c);
            }
            return sb.ToString();
        }

        //C# DES加密函数
        public static string DesEncrypt(string key, string message)
        {
            return StringToHex(DES(key, message, true, 0, ""));
        }

        //C# DES解密函数
        public static string DesDecrypt(string key, string message)
        {
            return DES(key, HexToString(message), false, 0, "");
        }
    }
}
