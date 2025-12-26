// Copyright (c) Nexai.
// The Nexai licenses this file to you under the MIT license.
// Produce by Nexai & community

namespace Nexai.Toolbox.Serializations.Converters
{
    using Nexai.Toolbox;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using System;

    /// <summary>
    /// Meta converter as proxy to <see cref="IElvexJsonObjectConverter"/>
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonConverter" />
    internal sealed class NewtownJsonMetaConverter : JsonConverter
    {
        #region Fields

        private readonly IReadOnlyCollection<IElvexJsonObjectConverter> _converters;

        #endregion

        #region Ctor

        /// <summary>
        /// Prevents a default instance of the <see cref="NewtownJsonMetaConverter"/> class from being created.
        /// </summary>
        internal NewtownJsonMetaConverter(IEnumerable<IElvexJsonObjectConverter> converters)
        {
            this._converters = converters?.ToArray() ?? EnumerableHelper<IElvexJsonObjectConverter>.ReadOnly;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return this._converters.Any(c => c.CanConvert(objectType));
        }

        /// <inheritdoc />
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var initialObjectType = objectType;

            var converter = this._converters.FirstOrDefault(c => c.CanConvert(objectType) && c.CanRead);

            if (converter is not null)
            {
                var obj = JObject.Load(reader);

                var success = converter.ReadJson((prop) =>
                {
                    var exist = obj.TryGetValue(prop, out var value);

                    object? propValue = null;
                    if (value is JProperty jprop)
                        propValue = jprop.Value.Value<object>();
                    else if (value is JValue jvalue)
                        propValue = jvalue.Value;
                    else
                        propValue = value?.Value<object>();

                    return Tuple.Create(exist, propValue);
                },
                ref objectType,
                ref existingValue);

                if (success)
                    return existingValue;

                var objReader = obj.CreateReader();
                return serializer.Deserialize(objReader, objectType);
            }

            if (initialObjectType == objectType)
                throw new InvalidOperationException("Could not deserialize item type " + objectType + " Path " + reader.Path);

            return serializer.Deserialize(reader, objectType);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException("NewtownJsonMetaConverter should only be used while deserializing.");
        }

        #endregion
    }
}
