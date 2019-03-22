﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Komponent.IO;
using Kontract;
using Kontract.Attributes;
using Kontract.Interfaces;
using Kontract.Interfaces.Common;
using Kontract.Interfaces.Image;

namespace Kore.SamplePlugins
{
    [Export(typeof(MtTexAdapter))]
    [Export(typeof(IImageAdapter))]
    //[Export(typeof(IIdentifyFiles))]
    [Export(typeof(ICreateFiles))]
    //[Export(typeof(ILoadFiles))]
    [Export(typeof(ISaveFiles))]
    [Export(typeof(IMtFrameworkTextureAdapter))]
    [PluginInfo("5D5B51A3-7280-4E90-B02E-E0ABD7C1F005", "MT Framework Texture", "MTTEX", "IcySon55", "", "This is the MTTEX image adapter for Kuriimu.")]
    [PluginExtensionInfo("*.tex")]
    public sealed class MtTexAdapter : IImageAdapter, /*IIdentifyFiles,*/ ICreateFiles, /*ILoadFiles,*/ /*ISaveFiles,*/ IMtFrameworkTextureAdapter
    {
        private MTTEX _format;
        private List<BitmapInfo> _bitmapInfos;

        #region Properties

        [FormFieldIgnore]
        public IList<BitmapInfo> BitmapInfos => _bitmapInfos;

        public IList<FormatInfo> FormatInfos => _format.HeaderInfo.Version == MTTEX.Version._Switchv1 ?
            MTTEX.SwitchFormats.Select(x => new FormatInfo(x.Key, x.Value.FormatName)).ToList()
            : MTTEX.Formats.Select(x => new FormatInfo(x.Key, x.Value.FormatName)).ToList();

        #endregion

        public bool Identify(string filename)
        {
            var result = true;

            try
            {
                using (var br = new BinaryReaderX(File.OpenRead(filename)))
                {
                    var magic = br.ReadString(4, Encoding.ASCII);
                    if (magic != "TEX\0" && magic != "\0XET")
                        result = false;
                }
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        public void Create()
        {
            //_format = new MTTEX();
        }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                _format = new MTTEX(File.OpenRead(filename));
                var formatInfo = new FormatInfo(_format.HeaderInfo.Format, _format.HeaderInfo.Version == MTTEX.Version._Switchv1 ?
                        MTTEX.SwitchFormats[_format.HeaderInfo.Format].FormatName :
                        MTTEX.Formats[_format.HeaderInfo.Format].FormatName);
                _bitmapInfos = new List<BitmapInfo>() { new BitmapInfo(_format.Bitmaps.FirstOrDefault(), formatInfo) { MipMaps = _format.Bitmaps.Skip(1).ToList(), Name = "0" } };
            }
        }

        public async Task<bool> Encode(BitmapInfo bitmapInfo, FormatInfo formatInfo, IProgress<ProgressReport> progress)
        {
            // TODO: Get the Kanvas to encode the image and update the UI with it.
            return false;
        }

        public void Save(string filename, int versionIndex = 0)
        {
            _format.Save(File.Create(filename));
        }

        public void Dispose() { }
    }
}
