using System;
using IBM.Data.DB2.iSeries;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections;
using System.Globalization;

namespace EntLibContrib.Data.IBM.DB2.iSeries
{
    /// <summary>
    /// 
    /// </summary>
    public class Db2DataReaderWrapper : DataReaderWrapper, IEnumerable
    {
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerReader"></param>
        public Db2DataReaderWrapper(iDB2DataReader innerReader)
            : base(innerReader)
        {
            
        }

        /// <summary>
        /// Gets the wrapped <see cref="iDB2DataReader"/>.
        /// </summary>
        public new iDB2DataReader InnerReader => (iDB2DataReader)base.InnerReader;

        /// <summary>
        /// Gets the value of the specified field converted to a GUID.
        /// </summary>
        /// <param name="index">The index of the field to find.</param>
        /// <returns>The GUID of the specified field.</returns>
        /// <remarks>
        /// This method will cast the result data Guid data type. In iDB2 you must use that as Raw(16) so
        /// that this method can convert that to Guid properly.
        /// </remarks>        
        public override Guid GetGuid(int index)
        {
            var guidBuffer = (byte[])InnerReader[index];
            return new Guid(guidBuffer);
        }


        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerReader.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    /// Another wrapper for <see cref="iDB2DataReader"/> that adds connection
    /// reference counting.
    /// </summary>
    public class RefCountingDb2DataReaderWrapper : Db2DataReaderWrapper
    {
        private readonly DatabaseConnectionWrapper connection;

        internal RefCountingDb2DataReaderWrapper(DatabaseConnectionWrapper connection, iDB2DataReader innerReader)
            : base(innerReader)
        {
            this.connection = connection;
            this.connection.AddRef();
        }

        /// <summary>
        /// Closes the <see cref="T:System.Data.IDataReader"/> Object.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Close()
        {
            if (!IsClosed)
            {
                base.Close();
                connection.Dispose();
            }
        }

        /// <summary>
        /// Close the contained data reader when disposing and releases the connection
        /// if it's not used anymore.
        /// </summary>
        /// <param name="disposing">True if called from Dispose method, false if called from finalizer. Since
        /// this class doesn't have a finalizer, this will always be true.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!IsClosed)
                {
                    base.Dispose(true);
                    connection.Dispose();
                }
            }
        }
    }
}
