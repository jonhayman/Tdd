using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ConsoleApplication25
{
    class Program
    {
        static void Main(string[] args)
        {
        }

    }

    public class Cart  : IEnumerable<Tuple<double, int>>
    {
        List<Tuple<double, int>> m_Cart = new List<Tuple<double, int>>();

       
        public void Add(double amount, int quantity)
        {
            m_Cart.Add(Tuple.Create(amount, quantity));
        }
        double AbsoluteTotal()
        {
            return m_Cart.Aggregate(0.0, (i, tuple) => i + tuple.Item1*tuple.Item2);
        }

        public double Total()
        {
            var absTotal = AbsoluteTotal();
            if (absTotal > 200)
            {
                return 0.90 * absTotal;
            }
            else if (absTotal > 100)
            {
                return 0.95*absTotal;
            }
            else
            {
                return absTotal;
            }
        }

        public IEnumerator<Tuple<double, int>> GetEnumerator()
        {
            return m_Cart.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Cart.GetEnumerator();
        }
    }


    public class CartTests
    {
        [Test]
        public void CheckTotalForEmptyCart()
        {
            var cart = new Cart {};
            Assert.AreEqual(0, cart.Total());
        }


        [Test]
        public void CheckTotalForSingleItem()
        {
            var cart = new Cart { {10.0, 1} };
            Assert.AreEqual(10, cart.Total());
        }

        [Test]
        public void CheckTotalWith5PercentDiscount()
        {
            var cart = new Cart { { 10.0, 5 }, { 20.0, 5 } };
            Assert.AreEqual(142.5, cart.Total());
        }

        [Test]
        public void CheckTotalWith10PercentDiscount()
        {
            var cart = new Cart() {{10.0, 10}, {5.0, 40}};
            Assert.AreEqual(270, cart.Total());
        }
    }

    class PrimeTester
    {
        public bool IsPrime(int n)
        {
            if (n < 2)
            {
                return false;            
            }

            for (int i = 2; i < n; i++)
            {
                if (n%i == 0) return false;
            }

            return true;
        }
    }

    class PrimeGenerator
    {
        public IEnumerable<int> GetPrimesBelow(int max)
        {
            var notPrimeCondition = new bool[max];

            for (var i = 2; i < max; i++)
            {
                if (!notPrimeCondition[i])
                {
                    yield return i;
                }

                var k = 1;
                while (i*k < max)
                {
                    notPrimeCondition[i*k] = true;
                    k++;
                }
            }
        }
    }


    [TestFixture]
    public class PrimeTests
    {
        private PrimeTester m_PrimeTester;

        [SetUp]
        public void SetUp()
        {
            m_PrimeTester = new PrimeTester();
        }

        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(4, false)]
        public void CheckPrimeness(int n, bool isPrime)
        {
            Assert.AreEqual(isPrime, m_PrimeTester.IsPrime(n));
        }

        [Test]
        public void TestGenerator()
        {
            var generator = new PrimeGenerator();
            var primesBelow1000 = generator.GetPrimesBelow(1000);

            foreach (var n in primesBelow1000)
            {
                Assert.IsTrue(m_PrimeTester.IsPrime(n));
            }
        }

        [Test]
        public void TestGeneratorNegation()
        {
            var generator = new PrimeGenerator();
            var primesBelow1000 = new HashSet<int>(generator.GetPrimesBelow(1000));
            
            for (int i = 0; i < 1000; i++)
            {
                if (!primesBelow1000.Contains(i))
                {
                    Assert.IsFalse(m_PrimeTester.IsPrime(i));
                }
            }
        }


        [Test]
        public void TestGeneratorAll()
        {
            var generator = new PrimeGenerator();
            var primesBelow1000 = new HashSet<int>(generator.GetPrimesBelow(1000));

            for (int i = 0; i < 1000; i++)
            {
                Assert.AreEqual(m_PrimeTester.IsPrime(i), primesBelow1000.Contains(i));
            }
        }

    }
}
