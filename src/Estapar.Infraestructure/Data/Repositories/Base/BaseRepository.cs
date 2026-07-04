using Dapper;
using Estapar.Domain.Dtos.Configs;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace Estapar.Infraestructure.Data.Repositories.Base;

/// <summary>
/// Provides a base implementation for database operations, including querying and executing commands.
/// </summary>
/// <remarks>This class is designed to simplify common database interactions by providing methods for executing
/// queries, retrieving single results, and performing scalar operations. It uses Dapper for data access and supports
/// asynchronous operations.  The repository is initialized with application settings, including connection strings and
/// command timeout configurations. If no command timeout is specified, a default value of 900 seconds is
/// used.</remarks>
public class BaseRepository(IOptions<AppSettings> configuracoes)
{
    /// <summary>
    /// Gets or sets the command timeout duration, in seconds, for database operations.
    /// </summary>
    public int _commandTimeout { get; set; } = 900;

    /// <summary>
    /// Creates and returns a new database connection using the configured connection string.
    /// </summary>
    /// <remarks>The connection string is retrieved from the application's configuration settings. Ensure that
    /// the configuration contains a valid connection string before calling this method.</remarks>
    /// <returns>An <see cref="IDbConnection"/> instance representing the database connection. The caller is responsible for
    /// opening, using, and disposing of the connection.</returns>
    protected IDbConnection GerarConexaoConnect() => new NpgsqlConnection(configuracoes.Value.ConnectionStrings.DataBase);

    /// <summary>
    /// Executes an asynchronous database query and retrieves a collection of results of the specified type.
    /// </summary>
    /// <remarks>This method uses the provided database connection to execute the query asynchronously. Ensure
    /// the connection is properly opened and disposed after use. The query execution respects the configured command
    /// timeout.</remarks>
    /// <typeparam name="T">The type of the objects to be returned in the result set.</typeparam>
    /// <param name="dbCon">The database connection to use for executing the query. Must be an open and valid connection.</param>
    /// <param name="sql">The SQL query to execute. Cannot be null or empty.</param>
    /// <param name="parameters">An optional object containing parameters to be passed to the query. Can be null if no parameters are required.</param>
    /// <returns>A task representing the asynchronous operation. The result contains an <see cref="IEnumerable{T}"/> of objects
    /// of type <typeparamref name="T"/> representing the rows returned by the query. Returns an empty collection if no
    /// rows are found.</returns>
    public virtual async Task<IEnumerable<T>> DbQueryAsync<T>(IDbConnection dbCon, string sql, object parameters = null)
    {
        return await dbCon.QueryAsync<T>(sql, parameters, commandTimeout: _commandTimeout);
    }

    /// <summary>
    /// Executes a SQL query asynchronously and retrieves the first result or a default value if no results are found.
    /// </summary>
    /// <remarks>This method uses Dapper's <c>QueryFirstOrDefaultAsync</c> internally to execute the query.
    /// Ensure the <paramref name="dbCon"/> is properly opened and disposed to avoid connection issues.</remarks>
    /// <typeparam name="T">The type of the result object.</typeparam>
    /// <param name="dbCon">The database connection to use for executing the query. Must be an open and valid connection.</param>
    /// <param name="sql">The SQL query to execute. Cannot be null or empty.</param>
    /// <param name="parameters">The parameters to pass to the query. Can be null if the query does not require parameters.</param>
    /// <returns>The first result of the query as an object of type <typeparamref name="T"/>, or the default value for
    /// <typeparamref name="T"/> if no results are found.</returns>
    public virtual async Task<T> DbQuerySingleAsync<T>(IDbConnection dbCon, string sql, object parameters)
    {
        return await dbCon.QueryFirstOrDefaultAsync<T>(sql, parameters, commandTimeout: _commandTimeout);
    }

    /// <summary>
    /// Executes a database command asynchronously and returns a value indicating whether the operation was successful.
    /// </summary>
    /// <remarks>This method uses the <c>ExecuteAsync</c> method of the provided <see cref="IDbConnection"/>
    /// to execute the command. Ensure the <paramref name="dbCon"/> is properly opened and disposed to avoid
    /// connection-related issues.</remarks>
    /// <typeparam name="T">The type parameter used for the operation. This is typically not directly utilized in this method but may be
    /// relevant for derived implementations.</typeparam>
    /// <param name="dbCon">The database connection to use for executing the command. Must be an open and valid <see cref="IDbConnection"/>
    /// instance.</param>
    /// <param name="sql">The SQL query or command to execute. Cannot be null or empty.</param>
    /// <param name="parameters">The parameters to pass to the SQL command. Can be null if no parameters are required.</param>
    /// <param name="commandType">The type of the command to execute, such as <see cref="CommandType.Text"/> or <see
    /// cref="CommandType.StoredProcedure"/>. Defaults to <see cref="CommandType.Text"/>.</param>
    /// <returns><see langword="true"/> if the command affected one or more rows; otherwise, <see langword="false"/>.</returns>
    public virtual async Task<bool> DbExecuteAsync<T>(IDbConnection dbCon, string sql, object parameters, CommandType commandType = CommandType.Text)
    {
        return await dbCon.ExecuteAsync(sql, parameters, commandTimeout: _commandTimeout, commandType: commandType) > 0;
    }

    /// <summary>
    /// Executes the specified SQL query asynchronously and retrieves a single scalar value as a boolean.
    /// </summary>
    /// <remarks>This method uses the <c>ExecuteScalarAsync</c> method of the provided <paramref
    /// name="dbCon"/> to execute the query. Ensure that the connection is open before calling this method.</remarks>
    /// <param name="dbCon">The database connection to use for executing the query. Must be an open and valid connection.</param>
    /// <param name="sql">The SQL query to execute. The query should return a single scalar value.</param>
    /// <param name="parameters">The parameters to pass to the SQL query. Can be an anonymous object or a dictionary of parameter names and
    /// values.</param>
    /// <returns>A task representing the asynchronous operation. The result of the task is <see langword="true"/> if the scalar
    /// value evaluates to true; otherwise, <see langword="false"/>.</returns>
    public virtual async Task<bool> DbExecuteScalarAsync(IDbConnection dbCon, string sql, object parameters)
    {
        return await dbCon.ExecuteScalarAsync<bool>(sql, parameters, commandTimeout: _commandTimeout);
    }

    /// <summary>
    /// Executes a SQL query asynchronously and returns a single scalar value of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the scalar value to be returned.</typeparam>
    /// <param name="dbCon">The database connection to use for executing the query. Must be open and valid.</param>
    /// <param name="sql">The SQL query to execute. Cannot be null or empty.</param>
    /// <param name="parameters">An optional object containing parameters to be passed to the query. Can be null if no parameters are required.</param>
    /// <returns>A task representing the asynchronous operation. The result contains the scalar value of type <typeparamref
    /// name="T"/> returned by the query, or the default value of <typeparamref name="T"/> if the query result is null.</returns>
    public virtual async Task<T> DbExecuteScalarDynamicAsync<T>(IDbConnection dbCon, string sql, object parameters = null)
    {
        return await dbCon.ExecuteScalarAsync<T>(sql, parameters, commandTimeout: _commandTimeout);
    }
}
