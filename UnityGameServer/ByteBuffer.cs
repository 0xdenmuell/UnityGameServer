﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * This ByteBuffer class converts all TCP outcoming requests from regular dataytypes (string, int, ...) to byte or byte[].
 * Also it converts incoming TCP requests from byte or byte[] to regular dataytypes (string, int, ...).
 */

namespace UnityGameServer
{
    public class ByteBuffer : IDisposable
    {


        private List<byte> Buff;
        private byte[] readBuff;
        private int readPos;
        private bool buffUpdated = false;

        public ByteBuffer()
        {
            Buff = new List<byte>();
            readPos = 0;
        }

        public int GetReadPos()
        {
            return readPos;
        }
        public byte[] ToArray()
        {
            return Buff.ToArray();
        }
        public int Count()
        {
            return Buff.Count();
        }
        public int Length()
        {
            return Count() - readPos;
        }
        public void Clear()
        {
            Buff.Clear();
            readPos = 0;
        }

        public void WriteByte(byte input)
        {
            Buff.Add(input);
            buffUpdated = true;
        }
        public void WriteBytes(byte[] input)
        {
            Buff.AddRange(input);
            buffUpdated = true;
        }
        public void WriteShort(short input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void WriteInteger(int input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void WriteLong(long input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void WriteFloat(float input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void WriteBool(bool input)
        {
            Buff.AddRange(BitConverter.GetBytes(input));
            buffUpdated = true;
        }
        public void WriteString(string input)
        {
            Buff.AddRange(BitConverter.GetBytes(input.Length));
            Buff.AddRange(Encoding.ASCII.GetBytes(input));
            buffUpdated = true;
        }


        public byte ReadByte(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                byte value = readBuff[readPos];
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 1;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'BYTE'");
            }


        }
        public byte[] ReadBytes(int Length, bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                byte[] value = Buff.GetRange(readPos, Length).ToArray();
                if (Peek)
                {
                    readPos += Length;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'BYTE[]'");
            }


        }
        public short ReadShorts(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                short value = BitConverter.ToInt16(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 2;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'SHORT'");
            }


        }
        public int ReadInteger(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                int value = BitConverter.ToInt32(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 4;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'INT");
            }


        }
        public long ReadLong(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                long value = BitConverter.ToInt64(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 8;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'LONG'");
            }


        }
        public float ReadFloat(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                float value = BitConverter.ToSingle(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 4;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'FLOAT'");
            }


        }
        public bool ReadBool(bool Peek = true)
        {
            if (Buff.Count > readPos)
            {
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                bool value = BitConverter.ToBoolean(readBuff, readPos);
                if (Peek & Buff.Count > readPos)
                {
                    readPos += 1;
                }

                return value;
            }

            else
            {
                throw new Exception("You are not trying to read out a 'BOOL'");
            }


        }
        public string ReadString(bool Peek = true)
        {
            try
            {
                int length = ReadInteger(true);
                if (buffUpdated)
                {
                    readBuff = Buff.ToArray();
                    buffUpdated = false;
                }

                string value = Encoding.ASCII.GetString(readBuff, readPos, length);
                if (Peek & Buff.Count > readPos)
                {
                    if (value.Length > 0)
                    {
                        readPos += length;
                    }
                }

                return value;
            }
            catch (Exception)
            {
                throw new Exception("You are not trying to read out a 'STRING'");
            }
        }


        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    Buff.Clear();
                    readPos = 0;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
