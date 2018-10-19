//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using EntLibContrib.Data.IBM.DB2.iSeries.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using IBM.Data.DB2.iSeries;
using System.Collections.Generic;

namespace EntLibContrib.Data.IBM.DB2.iSeries
{
    /// <summary>
    /// <para>Represents a Db2 iSeries Server database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses DB2 .Net Provider(IBM.Data.DB2.iSeries) to connect to the database.
    /// </para>  
    /// </remarks>
    [iDB2Permission(SecurityAction.Demand)]
    [ConfigurationElementType(typeof(Db2DatabaseData))]
    public class Db2Database : Database
    {
        /// <summary>
        /// The iDB2Factory instance.
        /// </summary>
        public static readonly iDB2Factory Instance = iDB2Factory.Instance;

        private readonly IDictionary<string, ParameterTypeRegistry> _registeredParameterTypes = new Dictionary<string, ParameterTypeRegistry>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Db2Database"/> class with a connection string.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public Db2Database(string connectionString)
            : base(connectionString, Instance)
        {

        }
        
        /// <summary>
        /// <para>Adds a new instance of a <see cref="DbParameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The command to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>    
        public override void AddParameter(DbCommand command, string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            if (DbType.Guid.Equals(dbType))
            {
                object convertedValue = ConvertGuidToByteArray(value);

#pragma warning disable 612, 618
                AddParameter((iDB2Command)command, name, iDB2DbType.iDB2Binary, 16, direction, nullable, precision,
                    scale, sourceColumn, sourceVersion, convertedValue);
#pragma warning restore 612, 618

                RegisterParameterType(command, name, dbType);
            }
            else
                base.AddParameter((iDB2Command)command, name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="iDB2Parameter"/> object to the command.</para>
        /// </summary>
        /// <param name="command">The <see cref="iDB2Command"/> to add the parameter.</param>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="idb2Type"><para>One of the <see cref="iDB2DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>A value indicating whether the parameter accepts <see langword="null"/> (<b>Nothing</b> in Visual Basic) values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
#pragma warning disable 612, 618
        public void AddParameter(DbCommand command, string name, iDB2DbType idb2Type, int size,
            ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn,
            DataRowVersion sourceVersion, object value)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            iDB2Parameter param = base.CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value) as iDB2Parameter;
            if (param != null)
            {
                param.iDB2DbType = idb2Type;
                command.Parameters.Add(param);
            }
        }
#pragma warning restore 612, 618

        /// <summary>
        /// <para>Discovers parameters on the <paramref name="command"/> and assigns the values from <paramref name="parameterValues"/> to the <paramref name="command"/>s Parameters list.</para>
        /// </summary>
        /// <param name="command">The command the parameeter values will be assigned to</param>
        /// <param name="parameterValues">The parameter values that will be assigned to the command.</param>
        public override void AssignParameters(DbCommand command, object[] parameterValues)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            base.AssignParameters(command, parameterValues);
        }

        /// <summary>
        /// All data readers get wrapped in objects so that they properly manage connections.
        /// Some derived Database classes will need to create a different wrapper, so this
        /// method is provided so that they can do this.
        /// </summary>
        /// <param name="connection">Connection + refcount.</param>
        /// <param name="innerReader">The reader to wrap.</param>
        /// <returns>The new reader.</returns>
        protected override IDataReader CreateWrappedReader(DatabaseConnectionWrapper connection, IDataReader innerReader)
        {
            return new RefCountingDb2DataReaderWrapper(connection, (iDB2DataReader)innerReader);
        }

        /// <summary>
        /// Sets the RowUpdated event for the data adapter.
        /// </summary>
        /// <param name="adapter">The <see cref="DbDataAdapter"/> to set the event.</param>
        /// <remarks>The <see cref="DbDataAdapter"/> must be an <see cref="iDB2DataAdapter"/>.</remarks>
        protected override void SetUpRowUpdatedEvent(DbDataAdapter adapter)
        {
            ((iDB2DataAdapter)adapter).RowUpdated += OnIBMRowUpdated;
        }

        /// <summary>
        /// Gets a parameter value.
        /// </summary>
        /// <param name="command">The command that contains the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The value of the parameter.</returns>
        public override object GetParameterValue(DbCommand command, string parameterName)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            object convertedValue = base.GetParameterValue(command, parameterName);

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);

                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertByteArrayToGuid(convertedValue);
                    }
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// Sets a parameter value.
        /// </summary>
        /// <param name="command">The command with the parameter.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The parameter value.</param>
        public override void SetParameterValue(DbCommand command, string parameterName, object value)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            object convertedValue = value;

            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry != null)
            {
                if (registry.HasRegisteredParameterType(parameterName))
                {
                    DbType dbType = registry.GetRegisteredParameterType(parameterName);
                    if (DbType.Guid == dbType)
                    {
                        convertedValue = ConvertGuidToByteArray(value);
                    }
                }
            }

            base.SetParameterValue(command, parameterName, convertedValue);
        }

        private ParameterTypeRegistry GetParameterTypeRegistry(string commandText)
        {
            ParameterTypeRegistry registry;
            _registeredParameterTypes.TryGetValue(commandText, out registry);
            return registry;
        }

        private void RegisterParameterType(IDbCommand command, string parameterName, DbType dbType)
        {
            ParameterTypeRegistry registry = GetParameterTypeRegistry(command.CommandText);
            if (registry == null)
            {
                registry = new ParameterTypeRegistry();
                _registeredParameterTypes.Add(command.CommandText, registry);
            }

            registry.RegisterParameterType(parameterName, dbType);
        }

        /// <summary>
        /// Converts a GUID object to a byte array or DBNull if null.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of a Guid to iDB2Binary</remarks>
        /// <param name="value">The Guid value.</param>
        /// <returns>byte array or DBNull</returns>
        private static object ConvertGuidToByteArray(object value)
        {
            return value is DBNull || (value == null) ? Convert.DBNull : ((Guid)value).ToByteArray();
        }

        /// <summary>
        /// Converts a byte array object to GUID.
        /// </summary>
        /// <remarks>This is specifically used in the conversion of iDB2Binary back to Guid</remarks>
        /// <param name="value">The value.</param>
        /// <returns>Guid object or DBNull</returns>
        private static object ConvertByteArrayToGuid(object value)
        {
            byte[] buffer;

            if (value is iDB2Binary)
                buffer = (iDB2Binary)value;
            else
                buffer = (byte[])value;

            if (buffer.Length == 0)
            {
                return DBNull.Value;
            }
            return new Guid(buffer);
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnIBMRowUpdated(object sender, iDB2RowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                if (args.Errors != null)
                {
                    args.Row.RowError = "Failed to update row";
                    args.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }

        /// <summary>
        /// Does this <see cref='Database'/> object support parameter discovery?
        /// </summary>
        /// <value>true.</value>
        public override bool SupportsParemeterDiscovery => true;

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the <see cref="DbCommand"/> and populates the Parameters collection of the specified <see cref="DbCommand"/> object. 
        /// </summary>
        /// <param name="discoveryCommand">The <see cref="DbCommand"/> to do the discovery.</param>
        /// <remarks>The <see cref="DbCommand"/> must be a <see cref="DbCommand"/> instance.</remarks>
        protected override void DeriveParameters(System.Data.Common.DbCommand discoveryCommand)
        {
            iDB2CommandBuilder.DeriveParameters((iDB2Command)discoveryCommand);
        }

        /// <summary>
        /// <para>Creates a <see cref="DbCommand"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="DbCommand"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>        
        public override DbCommand GetStoredProcCommand(string storedProcedureName, params object[] parameterValues)
        {
            return base.GetStoredProcCommand(storedProcedureName, parameterValues);
        }

        /// <summary>
        /// Returns the starting index for parameters in a command.
        /// </summary>
        /// <returns>The starting index for parameters in a command.</returns>
        protected override int UserParametersStartIndex()
        {
            return 0;
        }

        /// <summary>
        /// Builds a value parameter name for the current database.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formated parameter name.</returns>
        public override string BuildParameterName(string name)
        {
            return name;
        }

        /// <summary>
        /// Determines if the number of parameters in the command matches the array of parameter values.
        /// </summary>
        /// <param name="command">The <see cref="DbCommand"/> containing the parameters.</param>
        /// <param name="values">The array of parameter values.</param>
        /// <returns><see langword="true"/> if the number of parameters and values match; otherwise, <see langword="false"/>.</returns>
        protected override bool SameNumberOfParametersAndValues(System.Data.Common.DbCommand command, object[] values)
        {
            int returnParameterCount = 0;
            int numberOfParametersToStoredProcedure = command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = values.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor<TResult>(procedureName).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor<TResult>(procedureName, parameterMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor(procedureName, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor(procedureName, parameterMapper, rowMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor(procedureName, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Executes a stored procedure and returns the result as an enumerable of <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The element type that will be returned when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that will be executed.</param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="parameterValues">Parameter values passsed to the stored procedure.</param>
        /// <returns>An enumerable of <typeparamref name="TResult"/>.</returns>
        public IEnumerable<TResult> ExecuteDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper, params object[] parameterValues)
            where TResult : new()
        {
            return CreateDb2SprocAccessor(procedureName, parameterMapper, resultSetMapper).Execute(parameterValues);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateDb2SprocAccessor(procedureName, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// The conversion from <see cref="IDataRecord"/> to <typeparamref name="TResult"/> will be done for each property based on matching property name to column name.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper)
            where TResult : new()
        {
            IRowMapper<TResult> defaultRowMapper = MapBuilder<TResult>.BuildAllProperties();

            return CreateDb2SprocAccessor(procedureName, parameterMapper, defaultRowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new Db2SprocAccessor<TResult>(this, procedureName, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="rowMapper">The <see cref="IRowMapper&lt;TResult&gt;"/> that will be used to convert the returned data to clr type <typeparamref name="TResult"/>.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IRowMapper<TResult> rowMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new Db2SprocAccessor<TResult>(this, procedureName, parameterMapper, rowMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new Db2SprocAccessor<TResult>(this, procedureName, resultSetMapper);
        }

        /// <summary>
        /// Creates a <see cref="SprocAccessor&lt;TResult&gt;"/> for the given stored procedure.
        /// </summary>
        /// <typeparam name="TResult">The type the <see cref="SprocAccessor&lt;TResult&gt;"/> should return when executing.</typeparam>
        /// <param name="resultSetMapper">The <see cref="IResultSetMapper&lt;TResult&gt;"/> that will be used to convert the returned set to an enumerable of clr type <typeparamref name="TResult"/>.</param>
        /// <param name="procedureName">The name of the stored procedure that should be executed by the <see cref="SprocAccessor&lt;TResult&gt;"/>. </param>
        /// <param name="parameterMapper">The <see cref="IParameterMapper"/> that will be used to interpret the parameters passed to the Execute method.</param>
        /// <returns>A new instance of <see cref="SprocAccessor&lt;TResult&gt;"/>.</returns>
        public DataAccessor<TResult> CreateDb2SprocAccessor<TResult>(string procedureName, IParameterMapper parameterMapper, IResultSetMapper<TResult> resultSetMapper)
        {
            if (string.IsNullOrEmpty(procedureName)) throw new ArgumentException();

            return new Db2SprocAccessor<TResult>(this, procedureName, parameterMapper, resultSetMapper);
        }


    }

    /// <summary>
    /// Holds in memory a collection of commands with corresponding parameter names and types
    /// </summary>
    internal sealed class ParameterTypeRegistry
    {
        private readonly IDictionary<string, DbType> _parameterTypes;

        internal ParameterTypeRegistry()
        {
            _parameterTypes = new Dictionary<string, DbType>();
        }

        internal void RegisterParameterType(string parameterName, DbType parameterType)
        {
            _parameterTypes[parameterName] = parameterType;
        }

        internal bool HasRegisteredParameterType(string parameterName)
        {
            return _parameterTypes.ContainsKey(parameterName);
        }

        internal DbType GetRegisteredParameterType(string parameterName)
        {
            return _parameterTypes[parameterName];
        }
    }
}
