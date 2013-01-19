using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

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

        public async Task<object> Load(Type valueType, string user, string name, Type storageType)
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

                command.Parameters.AddWithValue("@Type", valueType.FullName);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                return await command.ExecuteScalarAsync();
            }
        }

        public async Task Save(Type valueType, string user, string name, object value)
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
                command.Parameters.AddWithValue("@Type", valueType.FullName);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Version", valueType.Assembly.GetName().Version.ToString());

                var updatedRows = command.ExecuteNonQuery();
                if (updatedRows == 0)
                {
                    command.CommandText = sqlInsert;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> Exists(Type valueType, string user, string name)
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

                command.Parameters.AddWithValue("@Type", valueType.FullName);
                command.Parameters.AddWithValue("@User", (object)user ?? DBNull.Value);
                command.Parameters.AddWithValue("@Name", (object)name ?? DBNull.Value);

                var rowCount = (int)await command.ExecuteScalarAsync();

                return rowCount > 0;
            }
        }

        public async Task<Setting[]> LoadAll(Type storageType)
        {
            const string sql = "SELECT [Type], [User], [Name], [Value] FROM [dbo].[Settings]";

            using (var connection = CreateConnection())
            {
                var command = new SqlCommand(sql, connection);
                var result = new List<Setting>();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new Setting
                        {
                            Type = Type.GetType((string)reader["Type"]),
                            User = (string)reader["User"],
                            Name = (string)reader["Name"],
                            Value = reader["Value"],
                            Version = new Version((string)reader["Version"])
                        });
                    }
                }

                return result.ToArray();
            }
        }

        private void VerifySettingsTableExists()
        {
            const string sqlCheck = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'Settings'";
            const string sqlCreate = "CREATE TABLE [dbo].[Settings] " +
                                     "( " +
                                     "  [Type] NVARCHAR(250) NOT NULL, " +
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