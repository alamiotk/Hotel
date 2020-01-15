
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uwp_App;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {

        //sprawdzanie czy po podaniu -1 jako numer statusu usterki pojawi się wartość False

        [TestMethod]
        public void TestMethod1()
        {
            Usterka usterka = new Usterka();
            Assert.IsFalse(usterka.SprawdzStatus(-1));
            
        }

        //sprawdzanie czy po podaniu wartości -1000 do metody SprawdźSy tatus pojawi się wartość False

       [TestMethod]
        public void TestMethod2()
        {
            Rezerwacja rezerwacja = new Rezerwacja();
            Assert.IsFalse(rezerwacja.SprawdzRezerwacje(-1000));

        }

        //sprawdzanie czy po podaniu Stringa, który nie ma praktycznie szans się tam pojawić zwrócona zostanie wartość False

        [TestMethod]
        public void TestMethod3()
        {
            Atrakcja atrakcja = new Atrakcja();
            Assert.IsFalse(atrakcja.Sprawdz("MojWlasnyRodzajString"));

        }
    }
}
