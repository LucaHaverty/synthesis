﻿using Newtonsoft.Json;
using SynthesisAPI.VirtualFileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynthesisAPI.AssetManager
{
    public class JSONAsset : Asset
    {
        public JSONAsset(string name, Guid owner, Permissions perm)
        {
            Init(name, owner, perm);
        }

        public long Seek(long offset, SeekOrigin loc = SeekOrigin.Begin)
        {
            return stream.Seek(offset, loc);
        }

        public TObject Deserialize<TObject>(long offset = long.MaxValue, SeekOrigin loc = SeekOrigin.Begin, bool retainPosition = true)
        {
            long? returnPosition = null;
            if (offset != long.MaxValue)
            {
                if (retainPosition)
                {
                    returnPosition = stream.Position;
                }
                Seek(offset, loc);
            }

            TObject obj = JsonConvert.DeserializeObject<TObject>(reader.ReadToEnd());

            if (returnPosition != null)
            {
                Seek(returnPosition.Value);
            }
            return obj;
        }

        public void Delete()
        {
        }

        public override IResource Load(byte[] data)
        {
            stream = new MemoryStream(data, false);
            reader = new StreamReader(stream);

            return this;
        }

        private StreamReader reader;
        private MemoryStream stream;
    }
}
