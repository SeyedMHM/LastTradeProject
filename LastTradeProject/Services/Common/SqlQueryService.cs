using System.Data.Common;
using System.Data.SqlClient;

namespace LastTradeProject.Services.Common
{
    public class SqlQueryService : ISqlQueryService
    {
        private string GetConnectionString()
        {
            return "Server=.; Database=TradeDb; Integrated Security=True;";
        }


        public async Task<List<TResult>> GetRowsAsync<TResult>(string command, CancellationToken cancellationToken)
        {
            List<TResult> results = new List<TResult>();

            SqlDataReader reader = null;

            SqlConnection connection = new SqlConnection(GetConnectionString());

            SqlCommand sqlCommand = new SqlCommand(command, connection);

            try
            {
                await connection.OpenAsync(cancellationToken);

                reader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                List<string> propertyNames = GetDynamicPropertyFromResult(reader, typeof(TResult)).ToList();

                while (await reader.ReadAsync(cancellationToken))
                {
                    MapToObject(results, reader, propertyNames);
                }

                return results;
            }
            finally
            {
                if (reader != null)
                {
                    await reader.CloseAsync();
                }
                if (connection != null && reader != null)
                {
                    await reader.CloseAsync();
                }
            }
        }


        private void MapToObject<TResult>(List<TResult> results, SqlDataReader reader, List<string> propertyNames)
        {
            var dto = Activator.CreateInstance(typeof(TResult));

            var propertiesWithValue = SetPropertiesValue(propertyNames, reader);

            foreach (var property in typeof(TResult).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (string)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(int))
                {
                    property.SetValue(dto, (int)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(int?))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (int?)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(bool))
                {
                    property.SetValue(dto, (bool)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(long))
                {
                    property.SetValue(dto, (long)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(decimal))
                {
                    property.SetValue(dto, (decimal)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(decimal?))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (decimal?)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(float))
                {
                    property.SetValue(dto, (float)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(float?))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (float?)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(double))
                {
                    property.SetValue(dto, (double)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(double?))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (double?)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(DateTime))
                {
                    property.SetValue(dto, (DateTime)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    property.SetValue(dto, Convert.IsDBNull(reader[property.Name]) ? null : (DateTime?)reader[property.Name]);
                }
                else if (property.PropertyType == typeof(List<KeyValuePair<string, object>>))
                {
                    property.SetValue(dto, propertiesWithValue);
                }
            }

            results.Add((TResult)dto);
        }


        /// <summary>
        /// گاهی اوقات امکان دارد هنگام مپ کردن خروجی دیتابیس به کلاس مورد نظر، بعضی از پراپرتی های کلاس در خروجی کوئری ما موجود نباشد
        /// بهمین دلیل در این متد ما فقط پراپرتی هایی از کلاس را مقدار دهی می کنیم که معادل آن در دیتابیس وجود دارد، 
        /// در غیر اینصورت باشد خطا خواهد داد
        /// </summary>
        /// <param name="result">نام فیلدهای خروجی از دیتابیس</param>
        /// <param name="classTypeName">پراپرتی های از کلاس که قرار است نتیجه کوئری در آن ریخته شود</param>
        private IEnumerable<string> GetDynamicPropertyFromResult(DbDataReader result, Type classTypeName)
        {
            var fieldsOfQueryResult = GetFieldsOfQueryResult(result);

            var propertiesOfClass = classTypeName.GetProperties().Select(q => q.Name);

            return fieldsOfQueryResult.Intersect(propertiesOfClass).ToList();
        }


        private IEnumerable<string> GetFieldsOfQueryResult(DbDataReader result)
        {
            var propertyNames = new List<string>();

            for (var col = 0; col < result.FieldCount; col++)
            {
                propertyNames.Add(result.GetName(col));
            }

            return propertyNames;
        }


        /// <summary>
        /// لیست از نام پراپرتی ها کلاس مورد نظر را می گیرد و مقادیر آن را از خروجی کوئری ما پر می کند
        /// </summary>
        /// <param name="propertiesName">لیست پراپرتی های کلاس خروجی ما</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, object>> SetPropertiesValue(List<string> propertiesName, DbDataReader reader)
        {
            var properties = new List<KeyValuePair<string, object>>();

            foreach (var filed in propertiesName)
            {
                var property = new KeyValuePair<string, object>(filed, reader[filed]);

                properties.Add(property);
            }

            return properties;
        }

    }
}
