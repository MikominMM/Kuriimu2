﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using Kontract.Interfaces.Common;

namespace Kontract.Interfaces.Image
{
    /// <summary>
    /// This is the image adapter interface for creating image format plugins.
    /// </summary>
    public interface IImageAdapter : IPlugin
    {
        /// <summary>
        /// The list of images provided by the image adapter to the UI.
        /// </summary>
        IList<BitmapInfo> BitmapInfos { get; }

        /// <summary>
        /// The list of formats provided by the image adapter to change encoding
        /// </summary>
        IList<FormatInfo> FormatInfos { get; }

        /// <summary>
        /// Instructs the plugin to encode the bitmaps and report progress as it goes.
        /// </summary>
        /// <param name="bitmapInfo">The BitmapInfo to re-encode</param>
        /// <param name="formatInfo">The FormatInfo to encode into</param>
        /// <param name="progress">The progress object to report progress through.</param>
        /// <returns>True if the bitmaps were successfully encoded, False otherwise.</returns>
        Task<bool> Encode(BitmapInfo bitmapInfo, FormatInfo formatInfo, IProgress<ProgressReport> progress);
    }

    /// <summary>
    /// The base bitmap info class.
    /// </summary>
    public class BitmapInfo
    {
        public BitmapInfo(Bitmap image, FormatInfo formatInfo)
        {
            Image = image;
            FormatInfo = formatInfo;
        }

        /// <summary>
        /// The main image data.
        /// </summary>
        [Browsable(false)]
        public Bitmap Image { get; set; }

        /// <summary>
        /// The list of all mipmap data.
        /// </summary>
        [Browsable(false)]
        public List<Bitmap> MipMaps { get; set; }

        /// <summary>
        /// The number of mipmaps that this BitmapInfo has.
        /// </summary>
        [Category("Properties")]
        [ReadOnly(true)]
        public virtual int MipMapCount => MipMaps?.Count ?? 0;

        /// <summary>
        /// The name of the main image.
        /// </summary>
        [Category("Properties")]
        [ReadOnly(true)]
        public string Name { get; set; }

        /// <summary>
        /// Returns the dimensions of the main iamge.
        /// </summary>
        [Category("Properties")]
        [Description("The dimensions of the image.")]
        public Size Size => Image?.Size ?? new Size();

        /// <summary>
        /// The image format information for encoding and decoding purposes
        /// </summary>
        [Browsable(false)]
        public FormatInfo FormatInfo { get; set; }
    }

    /// <summary>
    /// The base class for format information
    /// </summary>
    public class FormatInfo
    {
        public FormatInfo(int formatIndex, string formatName)
        {
            FormatIndex = formatIndex;
            FormatName = formatName;
        }

        /// <summary>
        /// The unique index into a format list, specific to the adapter
        /// </summary>
        public int FormatIndex { get; }

        /// <summary>
        /// The name of the format used; Doesn't need to be unique
        /// </summary>
        public string FormatName { get; }
    }
}
