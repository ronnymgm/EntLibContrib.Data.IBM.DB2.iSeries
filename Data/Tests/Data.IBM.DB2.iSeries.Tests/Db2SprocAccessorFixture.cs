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
using System.Linq;
using System.Data;
using System.Data.Common;
using EntLibContrib.Data.IBM.DB2.iSeries.Tests.TestSupport;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using IBM.Data.DB2.iSeries;

namespace EntLibContrib.Data.IBM.DB2.iSeries.Tests
{
    public abstract class Db2SprocAccessorContext : ArrangeActAssert
    {
        protected ConnectionState ConnectionState;
        protected int NumberOfConnectionsCreated;
        protected string ConnectionString;
        protected Db2Database Database;

        protected override void Arrange()
        {
            EnvironmentHelper.AssertDb2ClientIsInstalled();
            DatabaseProviderFactory factory = new DatabaseProviderFactory(Db2TestConfigurationSource.CreateConfigurationSource());
            Database = (Db2Database)factory.Create("Db2Test");
            ConnectionString = Database.ConnectionString;
        }

        protected class Product
        {
            public string TenMostExpensiveProducts { get; set; }
            public decimal UnitPrice { get; set; }
        }

        private class TestableSqlDatabase : Db2Database
        {
            Db2SprocAccessorContext context;
            public TestableSqlDatabase(string connectionstring, Db2SprocAccessorContext context)
                : base(connectionstring)
            {
                this.context = context;
            }

            public override DbConnection CreateConnection()
            {
                context.NumberOfConnectionsCreated++;

                DbConnection connection = base.CreateConnection();
                connection.StateChange += (sender, args) => { context.ConnectionState = args.CurrentState; };
                return connection;
            }
        }
    }

    [TestClass]
    public class WhenExecutingSproc : Db2SprocAccessorContext
    {
        [TestMethod]
        public void ThenConvertsResultInObjects()
        {
            var x = Database.ExecuteDb2SprocAccessor<Product>("TenMostExpensiveProducts");
            Assert.AreEqual(10, x.Count());
            Assert.IsNotNull(x.First().TenMostExpensiveProducts);
            Assert.AreNotEqual(0, x.First().UnitPrice);
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingRowMapper : Db2SprocAccessorContext
    {
        IRowMapper<Product> rowMapper;

        protected override void Arrange()
        {
            base.Arrange();

            rowMapper = new RowMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteDb2SprocAccessor<Product>("TenMostExpensiveProducts", rowMapper);
            Assert.AreEqual(10, x.Count());
            Assert.AreEqual("pname", x.First().TenMostExpensiveProducts);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class RowMapper : IRowMapper<Product>
        {
            public Product MapRow(IDataRecord row)
            {
                return new Product
                {
                    TenMostExpensiveProducts = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingResultSetMapper : Db2SprocAccessorContext
    {
        IResultSetMapper<Product> resultSetMapper;

        protected override void Arrange()
        {
            base.Arrange();

            resultSetMapper = new ResultSetMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteDb2SprocAccessor<Product>("TenMostExpensiveProducts", resultSetMapper);
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().TenMostExpensiveProducts);
            Assert.AreEqual(23, x.First().UnitPrice);
        }

        private class ResultSetMapper : IResultSetMapper<Product>
        {
            public IEnumerable<Product> MapSet(IDataReader reader)
            {
                yield return new Product
                {
                    TenMostExpensiveProducts = "pname",
                    UnitPrice = 23
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapper : Db2SprocAccessorContext
    {
        IParameterMapper parameterMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteDb2SprocAccessor<ProductSales>("ProductSalesByYear", parameterMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual("Chai", x.First().ProductName);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new iDB2Parameter("vProdName", "Chai"));
                command.Parameters.Add(new iDB2Parameter("vOrdYear", "1996"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapperAndRowMapper : Db2SprocAccessorContext
    {

        IParameterMapper parameterMapper;
        IRowMapper<ProductSales> rowMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
            rowMapper = new RowMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteDb2SprocAccessor<ProductSales>("ProductSalesByYear", parameterMapper, rowMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(12, x.First().TotalPurchase);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new iDB2Parameter("vProdName", "Chai"));
                command.Parameters.Add(new iDB2Parameter("vOrdYear", "1996"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }

        private class RowMapper : IRowMapper<ProductSales>
        {
            public ProductSales MapRow(IDataRecord row)
            {
                return new ProductSales
                {
                    ProductName = "pname",
                    TotalPurchase = 12
                };
            }
        }
    }

    [TestClass]
    public class WhenExecutingSprocPassingParameterMapperAndResultSetMapper : Db2SprocAccessorContext
    {
        IParameterMapper parameterMapper;
        IResultSetMapper<ProductSales> resultSetMapper;

        protected override void Arrange()
        {
            base.Arrange();

            parameterMapper = new ParameterMapper();
            resultSetMapper = new ResultSetMapper();
        }

        [TestMethod]
        public void ThenConvertsResultInObjectsUsingRowMapper()
        {
            var x = Database.ExecuteDb2SprocAccessor<ProductSales>("ProductSalesByYear", parameterMapper, resultSetMapper);
            Assert.IsNotNull(x);
            Assert.AreEqual(1, x.Count());
            Assert.AreEqual("pname", x.First().ProductName);
            Assert.AreEqual(12, x.First().TotalPurchase);
        }

        private class ParameterMapper : IParameterMapper
        {
            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                command.Parameters.Add(new iDB2Parameter("vProdName", "Chai"));
                command.Parameters.Add(new iDB2Parameter("vOrdYear", "1996"));
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }

        private class ResultSetMapper : IResultSetMapper<ProductSales>
        {
            public IEnumerable<ProductSales> MapSet(IDataReader reader)
            {
                yield return new ProductSales
                {
                    ProductName = "pname",
                    TotalPurchase = 12
                };
            }
        }
    }

    [TestClass]
    public class WhenCreatingSprocAccessor : Db2SprocAccessorContext
    {
        [TestMethod]
        public void ThenCanCreateSprocAccessor()
        {
            var sprocAccessor = Database.CreateDb2SprocAccessor<Product>("TenMostExpensiveProducts");
            Assert.IsNotNull(sprocAccessor);
        }

        [TestMethod]
        public void ThenCanCreateSprocAccessorWithRowMapper()
        {
            var sprocAccessor = Database.CreateDb2SprocAccessor<Product>("TenMostExpensiveProducts", MapBuilder<Product>.MapNoProperties().Build());
            Assert.IsNotNull(sprocAccessor);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullMapperThrows()
        {
            Database.CreateDb2SprocAccessor<Product>("prodedure name", (IRowMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullResultSetMapperThrows()
        {
            Database.CreateDb2SprocAccessor<Product>("prodedure name", (IResultSetMapper<Product>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullParameterMapperThrows()
        {
            Database.CreateDb2SprocAccessor<Product>("prodedure name", null, MapBuilder<Product>.BuildAllProperties());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThenCreateSprocAccessorWithNullDatabaseThrows()
        {
            new SprocAccessor<Product>(null, "procedure name", MapBuilder<Product>.BuildAllProperties());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ThenCreateSprocAccessorWithEmptyArgThrowsArgumentException()
        {
            Database.CreateDb2SprocAccessor<Product>(string.Empty);
        }
    }

    [TestClass]
    public class WhenExecutingSprocAccessor : Db2SprocAccessorContext
    {
        private DataAccessor<Product> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateDb2SprocAccessor<Product>("TenMostExpensiveProducts");
        }

        [TestMethod]
        public void ThenReturnsResultsAsEnumerable()
        {
            Assert.IsNotNull(accessor.Execute());
        }

        [TestMethod]
        public void ThenClosesConnectionAfterResultsAreEnumerated()
        {
            var result = accessor.Execute();
            Assert.AreEqual(10, result.Count());

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionEvenThoughEnumerationIsntFinished()
        {
            var result = accessor.Execute();
            var foo = result.First();

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenClosesConnectionAfterIteratingPartially()
        {
            var resultSet = accessor.Execute();

            int i = 0;
            foreach (var result in resultSet)
            {
                i++;
                if (i == 3) break;
            }

            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenConnectionIsClosedAfterExecuting()
        {
            var result = accessor.Execute().ToList();
            Assert.AreEqual(ConnectionState.Closed, base.ConnectionState);
        }

        [TestMethod]
        public void ThenSetsPropertiesBasedOnPropertyName()
        {
            var result = accessor.Execute();
            Product firstProduct = result.First();
            Assert.IsNotNull(firstProduct.TenMostExpensiveProducts);
            Assert.AreNotEqual(0d, firstProduct.UnitPrice);
        }
    }


    [TestClass]
    public class WhenParameterizedSprocAccessorIsCreated : Db2SprocAccessorContext
    {
        private DataAccessor<ProductSales> accessor;

        protected override void Arrange()
        {
            base.Arrange();

            accessor = Database.CreateDb2SprocAccessor<ProductSales>("ProductSalesByYear");
        }

        [TestMethod]
        public void ThenCanPassParameterInExecute()
        {
            var result = accessor.Execute("Chai", "1998");
            Assert.IsNotNull(result);
            var enumerared = result.ToList();
        }


        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingCustomRowMapper : Db2SprocAccessorContext
    {
        private DataAccessor<Product> accessor;
        private CustomMapper mapper;

        protected override void Arrange()
        {
            base.Arrange();

            mapper = new CustomMapper();
            accessor = Database.CreateDb2SprocAccessor<Product>("TenMostExpensiveProducts", mapper);
        }

        [TestMethod]
        public void ThenMapperIsCalledForEveryRow()
        {
            accessor.Execute().ToList();
            Assert.AreEqual(10, mapper.MapRowCallCount);
        }

        private class CustomMapper : IRowMapper<Product>
        {
            public int MapRowCallCount = 0;

            public Product MapRow(IDataRecord row)
            {
                MapRowCallCount++;
                return new Product();
            }
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingCustomResultSetMapper : Db2SprocAccessorContext
    {
        private DataAccessor<Product> accessor;
        private CustomMapper mapper;

        protected override void Arrange()
        {
            base.Arrange();

            mapper = new CustomMapper();
            accessor = Database.CreateDb2SprocAccessor<Product>("TenMostExpensiveProducts", mapper);
        }

        [TestMethod]
        public void ThenMapperIsCalledOncePerExecute()
        {
            accessor.Execute().ToList();
            Assert.AreEqual(1, mapper.MapSetCallCount);
        }

        private class CustomMapper : IResultSetMapper<Product>
        {
            public int MapSetCallCount = 0;

            public IEnumerable<Product> MapSet(IDataReader reader)
            {
                MapSetCallCount++;
                return Enumerable.Empty<Product>();
            }
        }
    }

    [TestClass]
    public class WhenSprocAccessorIsCreatedPassingParameterMapper : Db2SprocAccessorContext
    {
        private DataAccessor<ProductSales> accessor;
        private SqlParameterMapper parameterMapper;

        protected override void Arrange()
        {
            base.Arrange();
            parameterMapper = new SqlParameterMapper();

            accessor = Database.CreateDb2SprocAccessor<ProductSales>("ProductSalesByYear", parameterMapper);
        }

        [TestMethod]
        public void ThenParameterMapperIsCalledOnceOnExecute()
        {
            var result = accessor.Execute("Chai", "1996").ToList();
            Assert.AreEqual(2, parameterMapper.AssignParametersCallCount);
        }

        [TestMethod]
        public void ThenParameterMapperOutputIsUsedToExecuteSproc()
        {
            var result = accessor.Execute("Chai", "1996");
            Assert.IsNotNull(result);
            Assert.AreEqual("Chai", result.First().ProductName);
        }

        private class SqlParameterMapper : IParameterMapper
        {
            public int AssignParametersCallCount = 0;

            public void AssignParameters(DbCommand command, object[] parameterValues)
            {
                AssignParametersCallCount++;

                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = "ProdName";
                parameter.Value = parameterValues.First();

                command.Parameters.Add(parameter);

                AssignParametersCallCount++;

                DbParameter parameter2 = command.CreateParameter();
                parameter2.ParameterName = "OrdYear";
                parameter2.Value = parameterValues.ElementAt(1);

                command.Parameters.Add(parameter2);
            }
        }

        private class ProductSales
        {
            public string ProductName { get; set; }
            public double TotalPurchase { get; set; }
        }
    }
}
