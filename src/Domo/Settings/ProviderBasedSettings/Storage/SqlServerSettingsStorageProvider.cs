using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Domo.Settings.ProviderBasedSettings.Storage
{
    public class SqlServerSettingsStorageProvider : ISettingsStorageProvider
    {
        private readonly string _connectionString;

        public SqlServerSettingsStorageProvider(string sqlServerSettingsConnectionString)
        {
            _connectionString = sqlServerSettingsConnectionString;

            VerifySettingsTableExists();
        }

        public bool SupportsSerializationType(Type storageType)
        {
            return storageType == typeof(string);
        }

        public object Load(Type valueType, string user, string name, Type storageType)
        {
            const string sql =
                "SELECT [Value] " +
                "FROM [dbo].[Settings] " +
                "WHERE [Type] = @Type " +
                "  AND (@User IS NULL OR [User] = @User) " +
                "  AND (@Name IS NULL OR [Name] = @Name)";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Type", valueType.GUID);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                return command.ExecuteScalar();
            }
        }

        public void Save(Type valueType, string user, string name, object value)
        {
            const string sqlInsert = "INSERT [dbo].[Settings] VALUES(@Type, @User, @Name, @Value, @Version)";
            const string sqlUpdate =
                "UPDATE [dbo].[Settings] " +
                "SET [Value] = @Value, " +
                "    [Version] = @Version " +
                "WHERE [Type] = @Type " +
                "  AND (@User IS NULL OR [User] = @User) " +
                "  AND (@Name IS NULL OR [Name] = @Name)";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sqlUpdate, connection);

                command.Parameters.AddWithValue("@Value", value);
                command.Parameters.AddWithValue("@Type", valueType.GUID);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
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
            const string sql =
                "SELECT COUNT(*) " +
                "FROM [dbo].[Settings] " +
                "WHERE [Type] = @Type " +
                "  AND (@User IS NULL OR [User] = @User) " +
                "  AND (@Name IS NULL OR [Name] = @Name)";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Type", valueType.GUID);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                var rowCount = (int)command.ExecuteScalar();

                return rowCount > 0;
            }
        }

        public IEnumerable<Setting> LoadAll(Type storageType)
        {
            const string sql = "SELECT [Type], [User], [Name], [Value] FROM [dbo].[Settings]";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return new Setting
                        {
                            Type = Type.GetTypeFromCLSID((Guid)reader["Type"]),
                            User = (string)reader["User"],
                            Name = (string)reader["Name"],
                            Value = reader["Value"],
                            Version = new Version((string)reader["Version"])
                        };
                    }
                }
            }
        }

        private void VerifySettingsTableExists()
        {
            const string sqlCheck = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Settings'";
            const string sqlCreate = "CREATE TABLE [dbo].[Settings] " +
                                     "( " +
                                     "  [Type] UNIQUEIDENTIFIER NOT NULL, " +
                                     "  [User] NVARCHAR(100) NULL, " +
                                     "  [Name] NVARCHAR(100) NULL, " +
                                     "  [Value] NTEXT NULL, " +
                                     "  [Version] NVARCHAR(20) NOT NULL, " +
                                     ") " +
                                     "CREATE CLUSTERED INDEX [IX_Settings] ON [dbo].[Settings] ([Type]) " +
                                     "CREATE UNIQUE INDEX [IX_Settings_Lookup] ON [dbo].[Settings] ([Type], [User], [Name])";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sqlCheck, connection);
                var exists = (int)command.ExecuteScalar() == 1;
                if (exists)
                    return;

                command.CommandText = sqlCreate;
                command.ExecuteNonQuery();
            }
        }

        private SqlConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}