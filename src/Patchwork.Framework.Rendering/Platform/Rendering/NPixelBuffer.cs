#region Usings
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Shin.Framework.Extensions;
using Shin.Framework.Runtime;
using Shin.Framework.Threading;
#endregion

namespace Patchwork.Framework.Platform.Rendering
{
    public class NPixelBuffer : NMemoryChunk, IEquatable<NPixelBuffer>
    {
        #region Members
        private int m_height;
        private int m_rowBytes;
        private int m_width;
        #endregion

        #region Properties
        public int Height
        {
            get { return m_height; }
        }

        public int Width
        {
            get { return m_width; }
        }
        #endregion
        public NPixelBuffer(int width, int height, ICollection<byte> contents) : base(contents)
        {
            //EnsureSize(width, height);
            //SetContent(contents.ToArray());
            m_width = width; 
            m_height = height;
        }


        public NPixelBuffer(int width, int height)
        {
            EnsureSize(width, height);
        }

        internal NPixelBuffer(IntPtr handle, int width, int height, int rowBytes, int length)
            : base(handle, length)
        {
            m_width = width;
            m_height = height;
            m_rowBytes = rowBytes;
        }

        public NPixelBuffer(int width, int height, int rowBytes, int length) : base(length)
        {
            m_width = width;
            m_height = height;
            m_rowBytes = rowBytes;
        }

        #region Methods
        public bool CheckSize(int width, int height)
        {
            return (width <= 0 && height <= 0) || 
                   (width == m_width && height == m_height);
        }

        public void EnsureSize(int width, int height)
        {
            if (CheckSize(width, height))
                return;
            Resize(width, height);
        }

        public void Resize(int width, int height)
        {
            if (m_isReadOnly)
                return;

            m_lock.TryEnter(SynchronizationAccess.Write);
            var length = 0;
            try
            {
                var stride = 4 * ((width * 32 + 31) / 32);
                length = height * stride;
                if (!m_buffer.IsInvalid  && (length == (int)m_buffer.ByteLength))
                    return;

                m_buffer = new SafeMemoryBuffer(length);
                m_handle = new NHandle(m_buffer.DangerousGetHandle());
                m_rowBytes = stride;
                m_width = width;
                m_height = height;
                GC.AddMemoryPressure(length);
            }
            finally
            {
                m_lock.TryExit(SynchronizationAccess.Write);
            }

            Resize(length);
        }

        public new static NPixelBuffer Empty
        {
            get
            {
                return  new NPixelBuffer(1,1);
            }
        }

        // Set a pixel's value.
        public void SetPixel(int x, int y, byte red, byte green, byte blue, byte alpha, ref byte[] pixels)
        {
            var index = y * m_rowBytes + x * 4;
            //pixels = new byte[m_rowBytes];
            pixels[index++] = blue;
            pixels[index++] = green;
            pixels[index++] = red;
            pixels[index++] = alpha;
        }

        public NPixelBuffer Copy()
        {

            if (m_buffer.ByteLength <= 0)
                return Empty;

            m_lock.TryEnter(SynchronizationAccess.Write);
            try
            {
                //if (m_hasLock)
                //    return null;

                //if (!m_spin.IsHeldByCurrentThread)
                //m_spin.TryEnter(ref m_hasLock);
                //m_hasLock = m_semaphore.Wait(m_lockTimeout);
                //if (!m_hasLock)
                //    return null;

                //Monitor.Enter(m_lock);
                //lock (m_lock)
                //{
                    NPixelBuffer pb;
                    //lock (m_buffer)
                    //{
                        //IntPtr ptr = IntPtr.Zero;
                        //ptr = Marshal.AllocHGlobal(m_length);
                        //GC.AddMemoryPressure(m_length);
                        //var arry = new byte[m_length];

                        //Marshal.Copy(m_handle.Pointer, arry, 0, m_length);
                        //Marshal.Copy(arry, 0, ptr, m_length);
                        if (m_buffer.ByteLength <= 0)
                            return Empty;

                        var tmpBuffer = new NPixelBuffer(m_width, m_height, m_rowBytes, (int)m_buffer.ByteLength);
                //unsafe
                //{
                //    var ptr = (byte*)(IntPtr)(m_handle as NHandle);
                //    var des = (byte*)(IntPtr)(tmpBuffer.Handle as NHandle);
                //    Buffer.MemoryCopy(ptr,
                //                      des,
                //                      m_buffer.ByteLength,
                //                      m_buffer.ByteLength);
                //}
                //tmpBuffer = new NPixelBuffer(m_width, m_height, m_rowBytes, (int)m_buffer.ByteLength);
                tmpBuffer.SetContent(GetContent().ToArray());
                //tmpBuffer.Contents = arry;
                //tmpBuffer.m_isReadOnly = false;
                //}

                //Monitor.Exit(m_lock);
                return tmpBuffer;
                //}
            }
            catch (Exception ex)
            {
                Core.Logger.LogException(ex);
                throw;
            }
            finally
            {
                m_lock.TryExit(SynchronizationAccess.Write);
                //if (m_hasWriteLock)
                //{
                    //if (m_spin.IsHeldByCurrentThread)
                    //    m_spin.Exit();
                    //Monitor.Exit(m_lock);
                    //m_semaphore.Release();
                //    m_lockSlim.ExitWriteLock();
                //    m_hasWriteLock = false;
                //}
            }
        }

        public static NFrameBuffer GenerateBuffer(int width, int height)
        {
            var b = new NFrameBuffer(width, height);
            var tmpb = new byte[b.PixelBuffer.Length];
            // Set the pixel colors.
            double[,] values = new double[width, height];
            for (int ix = 0; ix < width; ix++)
            {
                double x = 0 + ix;
                for (int iz = 0; iz < height; iz++)
                {
                    double z = 0 + iz;
                    values[ix, iz] = F(x, z);
                }
            }

            for (int ix = 0; ix < width; ix++)
            {
                for (int iz = 0; iz < height; iz++)
                {
                    byte red, green, blue;
                    MapRainbowColor(values[ix, iz],
                                    0,
                                    height,
                                    out red,
                                    out green,
                                    out blue);
                    b.PixelBuffer.SetPixel(ix, iz, red, green, blue, 255, ref tmpb);
                }
            }

            b.PixelBuffer.SetContent(tmpb);
            return b;
        }

        private static void MapRainbowColor(double value,
                                     double min_value,
                                     double max_value,
                                     out byte red,
                                     out byte green,
                                     out byte blue)
        {
            // Convert into a value between 0 and 1023.
            int int_value = (int)(1023 * (value - min_value) / (max_value - min_value));

            // Map different color bands.
            if (int_value < 256)
            {
                // Red to yellow. (255, 0, 0) to (255, 255, 0).
                red = 255;
                green = (byte)int_value;
                blue = 0;
            }
            else if (int_value < 512)
            {
                // Yellow to green. (255, 255, 0) to (0, 255, 0).
                int_value -= 256;
                red = (byte)(255 - int_value);
                green = 255;
                blue = 0;
            }
            else if (int_value < 768)
            {
                // Green to aqua. (0, 255, 0) to (0, 255, 255).
                int_value -= 512;
                red = 0;
                green = 255;
                blue = (byte)int_value;
            }
            else
            {
                // Aqua to blue. (0, 255, 255) to (0, 0, 255).
                int_value -= 768;
                red = 0;
                green = (byte)(255 - int_value);
                blue = 255;
            }
        }

        // The function that defines the surface we are drawing.
        private static double F(double x, double z)
        {
            const double two_pi = 2 * 3.14159265;
            double r2 = x * x + z * z;
            double r = Math.Sqrt(r2);
            double theta = Math.Atan2(z, x);
            return Math.Exp(-r2) * Math.Sin(two_pi * r) * Math.Cos(3 * theta);
        }

        /// <inheritdoc />
        protected override void DisposeUnmanagedResources()
        {
            base.DisposeUnmanagedResources();
        }
        #endregion

        /// <inheritdoc />
        public bool Equals(NPixelBuffer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            //if (Contents.SequenceCompareTo(other.Contents) == 0)
            if (Contents.SequenceEqual(other.Contents))
                return true;
            return base.Equals(other) && m_height == other.m_height && m_rowBytes == other.m_rowBytes && m_width == other.m_width;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NPixelBuffer)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), m_height, m_rowBytes, m_width);
        }

        public static bool operator ==(NPixelBuffer left, NPixelBuffer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NPixelBuffer left, NPixelBuffer right)
        {
            return !Equals(left, right);
        }
    }
}