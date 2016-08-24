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
        private T _repository;

        public abstract T InitializeRepository();

        [TestInitialize]
        public void InitializeTest()
        {
            _repository = InitializeRepository();
            _trans = new TransactionScope();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _trans.Dispose();
        }
    }
}
