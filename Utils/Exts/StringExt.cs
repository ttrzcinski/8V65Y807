﻿namespace EchoBot.Utils.Exts
{
    public static class StringExt
    {
        public static string[] SplitAfter(this string value, int lineLength)
        {
            // Check, if value has content
            if (string.IsNullOrWhiteSpace(value) || lineLength < 1) return null;//new[] {value};
            // Check, if given value is longer, than lineLength
            if (value.Length <= lineLength) return new[] { value };
            // Process further
            var numOfItems = value.Length % lineLength;
            // TODO Add loop here, because value can be longer, than two lines
            var items = new string[numOfItems];
            for (var i = 0; i < numOfItems; i++)
            {
                items[i] = value.Substring(i * lineLength, (i + 1) * lineLength);
            }
            return items;
        }
    }
}
