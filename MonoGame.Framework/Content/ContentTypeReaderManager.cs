// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using MonoGame.Framework.Utilities;

namespace Microsoft.Xna.Framework.Content
{
    public sealed class ContentTypeReaderManager
    {
        private static readonly object _locker;

        private static readonly Dictionary<Type, ContentTypeReader> _contentReadersCache;

        private Dictionary<Type, ContentTypeReader> _contentReaders;

        private static readonly string _assemblyName;

#if NET6_0_OR_GREATER
        private static readonly bool _isRunningOnNetCore = true;
#else
        private static readonly bool _isRunningOnNetCore = Type.GetType("System.Private.CoreLib") != null;
#endif

        static ContentTypeReaderManager()
        {
            _locker = new object();
            _contentReadersCache = new Dictionary<Type, ContentTypeReader>(255);
            _assemblyName = ReflectionHelpers.GetAssembly(typeof(ContentTypeReaderManager)).FullName;

            // cache the most common type readers to avoid the most common AOT/trimming related issues
            // (especially on MonoGame standard content types)
            AddTypeCreator(typeof(AlphaTestEffectReader).FullName, () => new AlphaTestEffectReader());
            AddTypeCreator(typeof(ArrayReader<int>).FullName, () => new ArrayReader<int>());
            AddTypeCreator(typeof(ArrayReader<float>).FullName, () => new ArrayReader<float>());
            AddTypeCreator(typeof(ArrayReader<char>).FullName, () => new ArrayReader<char>());
            AddTypeCreator(typeof(ArrayReader<string>).FullName, () => new ArrayReader<string>());
            AddTypeCreator(typeof(ArrayReader<Point>).FullName, () => new ArrayReader<Point>());
            AddTypeCreator(typeof(ArrayReader<Vector2>).FullName, () => new ArrayReader<Vector2>());
            AddTypeCreator(typeof(ArrayReader<Vector3>).FullName, () => new ArrayReader<Vector3>());
            AddTypeCreator(typeof(ArrayReader<Matrix>).FullName, () => new ArrayReader<Matrix>());
            AddTypeCreator(typeof(ArrayReader<Rectangle>).FullName, () => new ArrayReader<Rectangle>());
            AddTypeCreator(typeof(ArrayReader<Color>).FullName, () => new ArrayReader<Color>());
            AddTypeCreator(typeof(ArrayReader<StringReader>).FullName, () => new ArrayReader<StringReader>());
            AddTypeCreator(typeof(BasicEffectReader).FullName, () => new BasicEffectReader());
            AddTypeCreator(typeof(BooleanReader).FullName, () => new BooleanReader());
            AddTypeCreator(typeof(BoundingBoxReader).FullName, () => new BoundingBoxReader());
            AddTypeCreator(typeof(BoundingFrustumReader).FullName, () => new BoundingFrustumReader());
            AddTypeCreator(typeof(BoundingSphereReader).FullName, () => new BoundingSphereReader());
            AddTypeCreator(typeof(ByteReader).FullName, () => new ByteReader());
            AddTypeCreator(typeof(CharReader).FullName, () => new CharReader());
            AddTypeCreator(typeof(ColorReader).FullName, () => new ColorReader());
            AddTypeCreator(typeof(CurveReader).FullName, () => new CurveReader());
            AddTypeCreator(typeof(DateTimeReader).FullName, () => new DateTimeReader());
            AddTypeCreator(typeof(DecimalReader).FullName, () => new DecimalReader());
            // DictionaryReader<TKey, TValue>
            AddTypeCreator(typeof(DoubleReader).FullName, () => new DoubleReader());
            AddTypeCreator(typeof(DualTextureEffectReader).FullName, () => new DualTextureEffectReader());
            AddTypeCreator(typeof(EffectMaterialReader).FullName, () => new EffectMaterialReader());
            AddTypeCreator(typeof(EffectReader).FullName + ", " + _assemblyName, () => new EffectReader());
            AddTypeCreator(typeof(EnumReader<Graphics.SpriteEffects>).FullName, () => new EnumReader<Graphics.SpriteEffects>());
            AddTypeCreator(typeof(EnumReader<Graphics.Blend>).FullName, () => new EnumReader<Graphics.Blend>());
            AddTypeCreator(typeof(EnvironmentMapEffectReader).FullName, () => new EnvironmentMapEffectReader());
            AddTypeCreator(typeof(ExternalReferenceReader).FullName + ", " + _assemblyName, () => new ExternalReferenceReader());
            AddTypeCreator(typeof(IndexBufferReader).FullName + ", " + _assemblyName, () => new IndexBufferReader());
            AddTypeCreator(typeof(Int16Reader).FullName, () => new Int16Reader());
            AddTypeCreator(typeof(Int32Reader).FullName, () => new Int32Reader());
            AddTypeCreator(typeof(Int64Reader).FullName, () => new Int64Reader());
            AddTypeCreator(typeof(ListReader<int>).FullName, () => new ListReader<int>());
            AddTypeCreator(typeof(ListReader<float>).FullName, () => new ListReader<float>());
            AddTypeCreator(typeof(ListReader<char>).FullName, () => new ListReader<char>());
            AddTypeCreator(typeof(ListReader<string>).FullName, () => new ListReader<string>());
            AddTypeCreator(typeof(ListReader<Point>).FullName, () => new ListReader<Point>());
            AddTypeCreator(typeof(ListReader<Vector2>).FullName, () => new ListReader<Vector2>());
            AddTypeCreator(typeof(ListReader<Vector3>).FullName, () => new ListReader<Vector3>());
            AddTypeCreator(typeof(ListReader<Matrix>).FullName, () => new ListReader<Matrix>());
            AddTypeCreator(typeof(ListReader<Rectangle>).FullName, () => new ListReader<Rectangle>());
            AddTypeCreator(typeof(ListReader<Color>).FullName, () => new ListReader<Color>());
            AddTypeCreator(typeof(ListReader<StringReader>).FullName, () => new ListReader<StringReader>());
            AddTypeCreator(typeof(MatrixReader).FullName, () => new MatrixReader());
            AddTypeCreator(typeof(ModelReader).FullName, () => new ModelReader());
            // MultiArrayReader<T>
            AddTypeCreator(typeof(NullableReader<Rectangle>).FullName, () => new NullableReader<Rectangle>());
            AddTypeCreator(typeof(PlaneReader).FullName, () => new PlaneReader());
            AddTypeCreator(typeof(PointReader).FullName, () => new PointReader());
            AddTypeCreator(typeof(QuaternionReader).FullName, () => new QuaternionReader());
            AddTypeCreator(typeof(RayReader).FullName, () => new RayReader());
            AddTypeCreator(typeof(RectangleReader).FullName, () => new RectangleReader());
            // ReflectiveReader<T>
            AddTypeCreator(typeof(SByteReader).FullName, () => new SByteReader());
            AddTypeCreator(typeof(SingleReader).FullName, () => new SingleReader());
            AddTypeCreator(typeof(SkinnedEffectReader).FullName, () => new SkinnedEffectReader());
            AddTypeCreator(typeof(SongReader).FullName, () => new SongReader());
            AddTypeCreator(typeof(SoundEffectReader).FullName, () => new SoundEffectReader());
            AddTypeCreator(typeof(SpriteFontReader).FullName + ", " + _assemblyName, () => new SpriteFontReader());
            AddTypeCreator(typeof(StringReader).FullName, () => new StringReader());
            AddTypeCreator(typeof(Texture2DReader).FullName, () => new Texture2DReader());
            AddTypeCreator(typeof(Texture3DReader).FullName, () => new Texture3DReader());
            AddTypeCreator(typeof(TextureCubeReader).FullName, () => new TextureCubeReader());
            AddTypeCreator(typeof(TextureReader).FullName, () => new TextureReader());
            AddTypeCreator(typeof(TimeSpanReader).FullName, () => new TimeSpanReader());
            AddTypeCreator(typeof(UInt16Reader).FullName, () => new UInt16Reader());
            AddTypeCreator(typeof(UInt32Reader).FullName, () => new UInt32Reader());
            AddTypeCreator(typeof(UInt64Reader).FullName, () => new UInt64Reader());
            AddTypeCreator(typeof(Vector2Reader).FullName, () => new Vector2Reader());
            AddTypeCreator(typeof(Vector3Reader).FullName, () => new Vector3Reader());
            AddTypeCreator(typeof(Vector4Reader).FullName, () => new Vector4Reader());
            AddTypeCreator(typeof(VertexBufferReader).FullName, () => new VertexBufferReader());
            AddTypeCreator(typeof(VertexDeclarationReader).FullName, () => new VertexDeclarationReader());
            AddTypeCreator(typeof(VideoReader).FullName, () => new VideoReader());
        }

        public ContentTypeReader GetTypeReader(Type targetType)
        {
            if (targetType.IsArray && targetType.GetArrayRank() > 1)
                targetType = typeof(Array);

            ContentTypeReader reader;
            if (_contentReaders.TryGetValue(targetType, out reader))
                return reader;

            return null;
        }

        // Trick to prevent the linker removing the code, but not actually execute the code

        internal ContentTypeReader[] LoadAssetReaders(ContentReader reader)
        {
#pragma warning disable 0219, 0649
            // Trick to prevent the linker removing the code, but not actually execute the code
            if (Environment.GetEnvironmentVariable("DUMMYVAR") == "DUMMYVAR")
            {
                // Dummy variables required for it to work on iDevices ** DO NOT DELETE ** 
                // This forces the classes not to be optimized out when deploying to iDevices
                var hByteReader = new ByteReader();
                var hSByteReader = new SByteReader();
                var hDateTimeReader = new DateTimeReader();
                var hDecimalReader = new DecimalReader();
                var hBoundingSphereReader = new BoundingSphereReader();
                var hBoundingFrustumReader = new BoundingFrustumReader();
                var hRayReader = new RayReader();
                var hCharListReader = new ListReader<char>();
                var hRectangleListReader = new ListReader<Rectangle>();
                var hRectangleArrayReader = new ArrayReader<Rectangle>();
                var hVector3ListReader = new ListReader<Vector3>();
                var hStringListReader = new ListReader<StringReader>();
                var hIntListReader = new ListReader<int>();
                var hShortListReader = new ListReader<short>();
                var hSpriteFontReader = new SpriteFontReader();
                var hTexture2DReader = new Texture2DReader();
                var hCharReader = new CharReader();
                var hRectangleReader = new RectangleReader();
                var hStringReader = new StringReader();
                var hVector2Reader = new Vector2Reader();
                var hVector3Reader = new Vector3Reader();
                var hVector4Reader = new Vector4Reader();
                var hCurveReader = new CurveReader();
                var hIndexBufferReader = new IndexBufferReader();
                var hBoundingBoxReader = new BoundingBoxReader();
                var hMatrixReader = new MatrixReader();
                var hBasicEffectReader = new BasicEffectReader();
                var hVertexBufferReader = new VertexBufferReader();
                var hAlphaTestEffectReader = new AlphaTestEffectReader();
                var hEnumSpriteEffectsReader = new EnumReader<Graphics.SpriteEffects>();
                var hArrayFloatReader = new ArrayReader<float>();
                var hArrayVector2Reader = new ArrayReader<Vector2>();
                var hListVector2Reader = new ListReader<Vector2>();
                var hArrayMatrixReader = new ArrayReader<Matrix>();
                var hEnumBlendReader = new EnumReader<Graphics.Blend>();
                var hNullableRectReader = new NullableReader<Rectangle>();
                var hEffectMaterialReader = new EffectMaterialReader();
                var hExternalReferenceReader = new ExternalReferenceReader();
                var hSoundEffectReader = new SoundEffectReader();
                var hSongReader = new SongReader();
                var hModelReader = new ModelReader();
                var hInt32Reader = new Int32Reader();
                var hInt16Reader = new Int16Reader();
                var hEffectReader = new EffectReader();
                var hSingleReader = new SingleReader();

                // At the moment the Video class doesn't exist
                // on all platforms... Allow it to compile anyway.
#if ANDROID || (IOS && !TVOS) || MONOMAC || (WINDOWS && !OPENGL) || WINDOWS_UAP
                var hVideoReader = new VideoReader();
#endif
                string types = hByteReader.GetType() + " " +
                hSByteReader.GetType() + " " +
                hDateTimeReader.GetType() + " " +
                hDecimalReader.GetType() + " " +
                hBoundingSphereReader.GetType() + " " +
                hBoundingFrustumReader.GetType() + " " +
                hRayReader.GetType() + " " +
                hCharListReader.GetType() + " " +
                hRectangleListReader.GetType() + " " +
                hRectangleArrayReader.GetType() + " " +
                hVector3ListReader.GetType() + " " +
                hStringListReader.GetType() + " " +
                hIntListReader.GetType() + " " +
                hShortListReader.GetType() + " " +
                hSpriteFontReader.GetType() + " " +
                hTexture2DReader.GetType() + " " +
                hCharReader.GetType() + " " +
                hRectangleReader.GetType() + " " +
                hStringReader.GetType() + " " +
                hVector2Reader.GetType() + " " +
                hVector3Reader.GetType() + " " +
                hVector4Reader.GetType() + " " +
                hCurveReader.GetType() + " " +
                hIndexBufferReader.GetType() + " " +
                hBoundingBoxReader.GetType() + " " +
                hMatrixReader.GetType() + " " +
                hBasicEffectReader.GetType() + " " +
                hVertexBufferReader.GetType() + " " +
                hAlphaTestEffectReader.GetType() + " " +
                hEnumSpriteEffectsReader.GetType() + " " +
                hArrayFloatReader.GetType() + " " +
                hArrayVector2Reader.GetType() + " " +
                hListVector2Reader.GetType() + " " +
                hArrayMatrixReader.GetType() + " " +
                hEnumBlendReader.GetType() + " " +
                hNullableRectReader.GetType() + " " +
                hEffectMaterialReader.GetType() + " " +
                hExternalReferenceReader.GetType() + " " +
                hSoundEffectReader.GetType() + " " +
                hSongReader.GetType() + " " +
                hModelReader.GetType() + " " +
                hInt32Reader.GetType() + " " +
                hInt16Reader.GetType() + " " +
                hEffectReader.GetType() + " " +

#if ANDROID || (IOS && !TVOS) || MONOMAC || (WINDOWS && !OPENGL) || WINDOWS_UAP
                hVideoReader.GetType() + " " +
#endif
                hSingleReader.GetType();

                Console.WriteLine(types);
            }
#pragma warning restore 0219, 0649

            // The first content byte i read tells me the number of content readers in this XNB file
            var numberOfReaders = reader.Read7BitEncodedInt();
            var contentReaders = new ContentTypeReader[numberOfReaders];
            var needsInitialize = new BitArray(numberOfReaders);
            _contentReaders = new Dictionary<Type, ContentTypeReader>(numberOfReaders);

            // Lock until we're done allocating and initializing any new
            // content type readers...  this ensures we can load content
            // from multiple threads and still cache the readers.
            lock (_locker)
            {
                // For each reader in the file, we read out the length of the string which contains the type of the reader,
                // then we read out the string. Finally we instantiate an instance of that reader using reflection
                for (var i = 0; i < numberOfReaders; i++)
                {
                    // This string tells us what reader we need to decode the following data
                    string originalReaderTypeString = reader.ReadString();

                    Func<ContentTypeReader> readerFunc;
                    if (typeCreators.TryGetValue(originalReaderTypeString, out readerFunc))
                    {
                        contentReaders[i] = readerFunc();
                        needsInitialize[i] = true;
                    }
                    else
                    {
                        // Need to resolve namespace differences
                        string readerTypeString = originalReaderTypeString;

                        readerTypeString = PrepareType(readerTypeString);

                        Type l_readerType = null;
                        try
                        {
                            // this might fail in AOT context and we need to properly warn the user on what to do if it happens
#pragma warning disable IL2057
                            l_readerType = Type.GetType(readerTypeString);
#pragma warning restore IL2057
                        }
                        catch (NotSupportedException e)
                        {
                            throw new NotSupportedException("It seems that you are using PublishAot and trying to load assets with a reflection-based serializer (which is not natively supported). To work around this error, call ContentTypeReaderManager.AddTypeCreator() in your Game constructor with the type mentionned in the following message: " + e.Message);
                        }

                        if (l_readerType != null)
                        {
                            ContentTypeReader typeReader;
                            if (!_contentReadersCache.TryGetValue(l_readerType, out typeReader))
                            {
                                try
                                {
                                    typeReader = l_readerType.GetDefaultConstructor().Invoke(null) as ContentTypeReader;
                                }
                                catch (TargetInvocationException ex)
                                {
                                    // If you are getting here, the Mono runtime is most likely not able to JIT the type.
                                    // In particular, MonoTouch needs help instantiating types that are only defined in strings in Xnb files. 
                                    throw new InvalidOperationException(
                                        "Failed to get default constructor for ContentTypeReader. To work around, add a creation function to ContentTypeReaderManager.AddTypeCreator() " +
                                        "with the following failed type string: " + originalReaderTypeString, ex);
                                }
                                // Catching non-CLS compliant exceptions to catch native exceptions like Access Violations on NativeAOT
                                catch
                                {
                                    throw new NotSupportedException("It seems that you are using PublishAot and trying to load assets with a reflection-based serializer (which is not natively supported). To work around this error, call ContentTypeReaderManager.AddTypeCreator() in your Game constructor with the following type: " + originalReaderTypeString);
                                }

                                needsInitialize[i] = true;

                                _contentReadersCache.Add(l_readerType, typeReader);
                            }

                            contentReaders[i] = typeReader;
                        }
                        else
                            throw new ContentLoadException(
                                    "Could not find ContentTypeReader Type. Please ensure the name of the Assembly that contains the Type matches the assembly in the full type name: " +
                                    originalReaderTypeString + " (" + readerTypeString + ")");
                    }

                    var targetType = contentReaders[i].TargetType;
                    if (targetType != null)
                        if (!_contentReaders.ContainsKey(targetType))
                            _contentReaders.Add(targetType, contentReaders[i]);

                    // I think the next 4 bytes refer to the "Version" of the type reader,
                    // although it always seems to be zero
                    reader.ReadInt32();
                }

                // Initialize any new readers.
                for (var i = 0; i < contentReaders.Length; i++)
                {
                    if (needsInitialize.Get(i))
                        contentReaders[i].Initialize(this);
                }

            } // lock (_locker)

            return contentReaders;
        }

        /// <summary>
        /// Removes Version, Culture and PublicKeyToken from a type string.
        /// </summary>
        /// <remarks>
        /// Supports multiple generic types (e.g. Dictionary&lt;TKey,TValue&gt;) and nested generic types (e.g. List&lt;List&lt;int&gt;&gt;).
        /// </remarks> 
        /// <param name="type">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public static string PrepareType(string type)
        {
            //Needed to support nested types
            int count = type.Split(new[] { "[[" }, StringSplitOptions.None).Length - 1;

            string preparedType = type;

            for (int i = 0; i < count; i++)
            {
                preparedType = Regex.Replace(preparedType, @"\[(.+?), Version=.+?\]", "[$1]");
            }

            //Handle non generic types
            if (preparedType.Contains("PublicKeyToken"))
                preparedType = Regex.Replace(preparedType, @"(.+?), Version=.+?$", "$1");

            // TODO: For WinRT this is most likely broken!
            preparedType = preparedType.Replace(", Microsoft.Xna.Framework.Graphics", string.Format(", {0}", _assemblyName));
            preparedType = preparedType.Replace(", Microsoft.Xna.Framework.Video", string.Format(", {0}", _assemblyName));
            preparedType = preparedType.Replace(", Microsoft.Xna.Framework", string.Format(", {0}", _assemblyName));

            if (_isRunningOnNetCore)
                preparedType = preparedType.Replace("mscorlib", "System.Private.CoreLib");
            else
                preparedType = preparedType.Replace("System.Private.CoreLib", "mscorlib");

            return preparedType;
        }

        // Static map of type names to creation functions. Required as iOS requires all types at compile time
        private static Dictionary<string, Func<ContentTypeReader>> typeCreators = new Dictionary<string, Func<ContentTypeReader>>();

        /// <summary>
        /// Adds the type creator.
        /// </summary>
        /// <param name='typeString'>
        /// Type string.
        /// </param>
        /// <param name='createFunction'>
        /// Create function.
        /// </param>
        public static void AddTypeCreator(string typeString, Func<ContentTypeReader> createFunction)
        {
            if (!typeCreators.ContainsKey(typeString))
                typeCreators.Add(typeString, createFunction);
        }

        public static void ClearTypeCreators()
        {
            typeCreators.Clear();
        }

    }
}
