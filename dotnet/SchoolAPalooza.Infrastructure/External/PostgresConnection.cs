using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Npgsql;

namespace SchoolAPalooza.Infrastructure.External
{
    public interface IPostgresConnection
    {
        IEnumerable<T> ReadData<T>(string statement, object parameters = null);
        void WriteData(string statement, object parameters = null);
    }

    public class PostgresConnection : IPostgresConnection
    {
        readonly string connectionString;

        public PostgresConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IEnumerable<T> ReadData<T>(string statement, object parameters = null)
        {
            IDbConnection connection = null;
            try
            {
                connection = OpenConnection();
                var result = connection.Query<T>(statement, parameters);

                return result;
            }
            catch (Exception ex)
            {
                // TODO: logging
                throw;
            }
            finally
            {
                connection?.Dispose();
            }
        }

        IDbConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public void WriteData(string statement, object parameters = null)
        {
            IDbConnection connection = null;
            try
            {
                connection = OpenConnection();
                connection.Execute(statement, parameters);
            }
            catch (Exception ex)
            {
                // TODO: logging
                throw;
            }
            finally
            {
                connection?.Dispose();
            }
        }
    }
}