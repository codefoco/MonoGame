// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

namespace Microsoft.Xna.Framework.Content
{
#if NET
    [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.All)]
#endif
    internal class DoubleReader : ContentTypeReader<double>
    {
        public DoubleReader()
        {
        }

        protected internal override double Read(ContentReader input, double existingInstance)
        {
            return input.ReadDouble();
        }
    }
}

