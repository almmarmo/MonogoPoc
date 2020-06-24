using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonogoPoc
{
    public static class BsonExtensions
    {
        public static bool ConvertField<T, TVoid>(this BsonValue owner, string fieldName, string oldFieldName, Func<string, T> parse)
        {
            bool converted = false;
            if (owner[fieldName].GetType() != typeof(TVoid))
            {
                string fieldValue = owner[fieldName].AsString;
                owner[fieldName] = BsonValue.Create(parse.Invoke(fieldValue));
                if(oldFieldName != null)
                    owner[oldFieldName] = fieldValue;
                Console.WriteLine($"Converting {fieldName}:{fieldValue} => {owner[fieldName]}.");
                converted = true;
            }
            else
            {
                Console.WriteLine($"Clause with DateTime {fieldName}={owner[fieldName]} ignored.");
            }

            return converted;
        }
    }
}
