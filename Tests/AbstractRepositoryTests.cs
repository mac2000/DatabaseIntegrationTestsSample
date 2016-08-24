using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    public abstract class AbstractRepositoryTests<T>
    {
        private TransactionScope _trans;
        protected T Repository;

        public abstract T InitializeRepository();

        [TestInitialize]
        public void InitializeTest()
        {
            Repository = InitializeRepository();
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _trans.Dispose();
        }
    }
}
