// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Content
{
#if NET
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
#endif
    internal class UInt16Reader : ContentTypeReader<ushort>
    {
        public UInt16Reader()
        {
        }

        protected internal override ushort Read(ContentReader input, ushort existingInstance)
        {
            return input.ReadUInt16();
        }
    }
}
