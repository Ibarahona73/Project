﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Project.Views
{
	public class ByteArrayConverter : JsonConverter<byte[]>
	{
		public override byte[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException();
			}

			return Convert.FromBase64String(reader.GetString());
		}

		public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(Convert.ToBase64String(value));
		}
	}
}
