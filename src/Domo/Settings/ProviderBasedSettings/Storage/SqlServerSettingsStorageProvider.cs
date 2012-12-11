using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public class SqlServerSettingsStorageProvider : ISettingsStorageProvider
    {
        private readonly string _settingsConnectionString;

        public SqlServerSettingsStorageProvider(string settingsConnectionString)
        {
            _settingsConnectionString = settingsConnectionString;
        }

        public bool SupportsSerializationType(Type storageType)
        {
            return
                storageType == typeof(string) ||
                storageType == typeof(byte[]);
        }

        public object Load(Type valueType, string user, string name, Type storageType)
        {
            const string sql = "SELECT [Value] FROM [Settings] WHERE [Type] = @Type AND [User] = @User AND [Name] = @Name";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Type", valueType.Name);
                command.Parameters.AddWithValue("@User", user);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                return command.ExecuteScalar();
            }
        }

        public void Save(Type valueType, string user, string name, object value)
        {
            const string sqlInsert = "INSERT [Settings] VALUES(@Type, @User, @Name, @Value, @Version)";
            const string sqlUpdate = "UPDATE [Settings] SET [Value] = @Value, [Version] = @Version WHERE [Type] = @Type AND [User] = @User AND [Name] = @Name";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sqlUpdate, connection);

                command.Parameters.AddWithValue("@Value", value);
                command.Parameters.AddWithValue("@Type", valueType.Name);
                command.Parameters.AddWithValue("@User", user);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Version", valueType.Assembly.GetName().Version.ToString());

                var updatedRows = command.ExecuteNonQuery();
                if (updatedRows == 0)
                {
                    command.CommandText = sqlInsert;
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool Exists(Type valueType, string user, string name)
        {
            const string sql = "SELECT COUNT(*) FROM [Settings] WHERE [Type] = @Type AND [User] = @User AND [Name] = @Name";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Type", valueType.Name);
                command.Parameters.AddWithValue("@User", user);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                var rowCount = (int)command.ExecuteScalar();

                return rowCount > 0;
            }
        }

        public IEnumerable<Setting> LoadAll(Type storageType)
        {
            const string sql = "SELECT [Type], [User], [Name], [Value] FROM [Settings]";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new Setting
                        {
                            Name = (string)reader["Name"],
                            Type = Type.GetType((string)reader["Type"]),
                            User = (string)reader["User"],
                            Value = reader["Value"],
                            Version = new Version((string)reader["Version"])
                        };
                    }
                }
            }
        }

        private SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(_settingsConnectionString);
            connection.Open();

            return connection;
        }
    }
}