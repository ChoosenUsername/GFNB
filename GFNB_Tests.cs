using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LongModularArithmetic;
using LongModArithmetic;
using NUnit.Framework;

namespace GFNB
{
    [TestFixture]
    class GFNB_Tests
    {
        Operation operation = new Operation();
        Calculator calculator = new Calculator();
        ModCalculator modcalculator = new ModCalculator();
        PrimalityAlgorithm primality = new PrimalityAlgorithm();
        Number one = new Number("1");

        [Test]
        [Description("Verifies that expansion is set correctly.")]
        [TestCase("3")]
        [TestCase("125")]
        [TestCase("AD")]
        [TestCase("5")]
        public void InitialConditionsTest(string m)
        {
            var number = new Number(m);
            var one = new Number("1");
            var three = new Number("3");
            var four = new Number("4");
            var p = calculator.LongAdd(calculator.ShiftBitsToHigh(number, 1), one);
            Assert.AreEqual(0, calculator.LongCmp(three, modcalculator.Mod(p, four)));
            Assert.AreEqual(true, primality.SolovayStrassenPrimalityTest(p, 200));
        }


        [Test]
        [Description("Verifies that constant '0' exist in current field.")]
        [TestCase(293, 11, 6, 1, 0)]
        [TestCase(163, 7, 6, 3, 0)]
        [TestCase(409, 15, 6, 1, 0)]
        [TestCase(571, 10, 5, 2, 0)]
        public void FindZeroTest(int m, int deg2, int deg3, int deg4, int deg5)
        {
            Polynomial polynomial = new Polynomial(m, deg2, deg3, deg4, deg5);
            Operation operation = new Operation();
            var result = operation.XOR(polynomial.array, polynomial.array);
            ulong[] zero = new ulong[1];
            Assert.AreEqual(0, operation.LongCmp(result, zero));
        }

        /*
        [Test]
        [Description("Verifies that squaring works correctly")]
        [TestCase("04C14E116A02694B4AF9DFD770FAEAB31EEE007EB2B770C4AFA092219030695F6E364E7FDDD6FA72431A326168747B00F865B59D35F2EA2A5255859D7797C083", 509, "1260A708B50134A5A57CEFEBB87D75598F77003F595BB86257D04910C81834AFB71B273FEEEB7D39218D1930B43A3D807C32DACE9AF97515292AC2CEBBCBE041")]
        [TestCase("1EF93452D2ED8144A741E4B96FFCFC1015FC7C0C9973", 173, "1F7C9A296976C0A253A0F25CB7FE7E080AFE3E064CB9")]
        [TestCase("451A4EFB42E394768922A79DD73857410395F7EEA9A", 173, "228D277DA171CA3B449153CEEB9C2BA081CAFBF754D")]
        [TestCase("1448A5958415BB616F2BF75E19E06D883DC622D1B040", 173, "8DF87CD9F9407E543E2164F244F46B59CD10173A82F")]
        [TestCase("000FCBE71C2637C9F90498C8078F85E143FDCCDBD38F", 173, "BE71C2637C9F90498C8078F85E143FDCCDBD38F007E")]
        [TestCase("000FCBE71C2637C9F90498C8078F85E143FDCCDBD38F", 173, "BE71C2637C9F90498C8078F85E143FDCCDBD38F007E")]
        public void SquaringTest(string hex1, int m, string expected)
        {
            Polynomial polynomial = new Polynomial(hex1);
            var square = operation.Squaring(polynomial.array, 1, m);
            polynomial.array = square;
            Assert.AreEqual(expected, polynomial.ToHexString());
        }
        */

        [Test]
        [Description("Verifies that trace works correctly")]
        [TestCase(293, 234, 162, 111, 64, 1ul)]
        [TestCase(132, 31, 22, 16, 9, 1ul)]
        [TestCase(132, 31, 3, 2, 1, 1ul)]
        [TestCase(132, 31, 0, 0, 0, 0ul)]
        public void TraceTest(int m, int deg2, int deg3, int deg4, int deg5, ulong expected)
        {
            Polynomial polynomial = new Polynomial(m, deg2, deg3, deg4, deg5);
            var trace = operation.Trace(polynomial.array);
            Assert.AreEqual(expected, trace);
        }


        [Test]
        [Description("Verifies that ROL works correctly")]
        [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", 64, "10A34FC2E191C48429DB512ABA45540185B1FCFC02C5")]
        [TestCase("1A89E7A9C35309B56845CF28CDF7B418DF330361B0FA", 34, "D4C26D5A1173CA337DED0637CCC0D86C3EB513CF538")]
        [TestCase("1AA53CFFB0C76C9799F2A17D00F825009F206A1C3764", 9, "A79FF618ED92F33E542FA01F04A013E40D4386EC9AA")]
        [TestCase("000FCBE71C2637C9F90498C8078F85E143FDCCDBD38F", 16, "BE71C2637C9F90498C8078F85E143FDCCDBD38F007E")]
        public void ROL(string hex1, int shift, string expected)
        {
            Polynomial a = new Polynomial(hex1);
            var result = operation.ROL(a.array, shift, 173);
            a.array = result;
            Assert.AreEqual(expected, a.ToHexString());
        }


        [Test]
        [Description("Verifies that ShiftBitsToHigh works correctly")]
        [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", 16, "A8030B63F9F8058B0A34FC2E191C48429DB512ABA450000")]
        [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", 64, "A8030B63F9F8058B0A34FC2E191C48429DB512ABA450000000000000000")]
        [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", 63, "540185B1FCFC02C5851A7E170C8E24214EDA8955D228000000000000000")]
        [TestCase("ABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEF0000000000000000000000", 328, "ABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEFABCDEF00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase("2", 0, "2")]
        [TestCase("2", 0, "2")]
        [TestCase("1", 1, "2")]
        [TestCase("1", 2, "4")]
        [TestCase("F", 4, "F0")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 32, "FFFFFFFFFFFFFFFF00000000")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 64, "FFFFFFFFFFFFFFFF0000000000000000")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 1, "1FFFFFFFFFFFFFFFE")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 4, "FFFFFFFFFFFFFFFF0")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 64, "FFFFFFFFFFFFFFFF0000000000000000")]
        [TestCase("0000000000000000FFFFFFFFFFFFFFFF", 63, "7FFFFFFFFFFFFFFF8000000000000000")]
        [TestCase("00000000000000000000000000000000FFFFFFFFFFFFFFFF", 128, "FFFFFFFFFFFFFFFF00000000000000000000000000000000")]
        [TestCase("8B9D4EC6", 23, "45CEA763000000")]
        [TestCase("8B9D4EC6", 1, "1173A9D8C")]
        [TestCase("8B9D4EC6", 64, "8B9D4EC60000000000000000")]
        [TestCase("8B9D4EC6", 128, "8B9D4EC600000000000000000000000000000000")]
        [TestCase("8B9D4EC6", 256, "8B9D4EC60000000000000000000000000000000000000000000000000000000000000000")]
        [TestCase("A73A29E935944B953354055780BC770CA4A4EB7873E0CEA660C9F8881875288340C04B295C364539F0FEDB82C176C38F4AA9FA4CB2A7CDC8AF609E3DEC35937A7EC4E4FB234F3BC55B88455F7005BD7F3EE0B066D96E6C00F90EEE4ECF4E21A6405F9854D0C2DF19432104EAC2F7C1934DC031379AAD4DE5AF685D5994022C37", 23, "539D14F49ACA25CA99AA02ABC05E3B86525275BC39F067533064FC440C3A9441A0602594AE1B229CF87F6DC160BB61C7A554FD265953E6E457B04F1EF61AC9BD3F62727D91A79DE2ADC422AFB802DEBF9F7058336CB736007C87772767A710D3202FCC2A68616F8CA1908275617BE0C9A6E0189BCD56A6F2D7B42EACCA01161B800000")]
        [TestCase("8B9D4EC6", 2, "22E753B18")]
        [TestCase("8B9D4EC6", 3, "45CEA7630")]
        [TestCase("8B9D4EC6", 23, "45CEA763000000")]
        [TestCase("8B9D4EC6", 321, "1173A9D8C00000000000000000000000000000000000000000000000000000000000000000000000000000000")]
        public void ShiftBitsToHigh(string hex1, int shift, string expected)
        {
            Polynomial a = new Polynomial(hex1);
            var result = operation.ShiftBitsToHigh(a.array, shift);
            a.array = result;
            Assert.AreEqual(expected, a.ToHexString());
        }

        [Test]
        [Description("Verifies that ShiftBitsToLow works correctly.")]
        [TestCase("FFAAA242432FFFFFFF12352565897", 45, "7FD551212197FFFFFF")]
        [TestCase("FFFFFFFF", 1, "7FFFFFFF")]
        [TestCase("AADDDDDDFFFF3128898923482954398923479827FFFFF", 123, "155BBBBBBFFFE62")]
        [TestCase("F423FFFFFCE22", 4, "F423FFFFFCE2")]
        [TestCase("F423FFFFFCE22", 109, "0")]
        [TestCase("118427B3B467203A97DE75CF815074515C3E3D1944A5192", 64, "118427B3B467203A97DE75CF8150745")]
        [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", 109, "540185B1FCFC02C5")]
        public void TestShiftBitsToLow(string hex1, int shift, string expected)
        {
            Polynomial a = new Polynomial(hex1);
            var result = operation.ShiftBitsToLow(a.array, shift);
            a.array = result;
            Assert.AreEqual(expected, a.ToHexString());
        }



        private static readonly IEnumerable<TestCaseData> _testMatrix = GetMatrixCases();

        private static IEnumerable<TestCaseData> GetMatrixCases()
        {
            yield return new TestCaseData(
                new ulong[,]
                {
                    { 0, 1, 0 },
                    { 1, 0, 1 },
                    { 0, 1, 1 },
                }, 3, "3").SetName("GF(2^3) Lambda Matrix");


            yield return new TestCaseData(
                new ulong[,]
                {
                    { 0, 1, 0, 0, 0 },
                    { 1, 0, 0, 1, 0 },
                    { 0, 0, 0, 1, 1 },
                    { 0, 1, 1, 0, 0 },
                    { 0, 0, 1, 0, 1 },
                }, 5, "5").SetName("GF(2^5) Lambda Matrix");
        }
        [TestCaseSource(nameof(_testMatrix))]
        public void TestMatrix(ulong[,] matrix, int m, string sm)
        {

            Number p = new Number(sm);
            p = calculator.LongAdd(calculator.ShiftBitsToHigh(p, 1), one);
            var result = operation.LambdaMatrix(m, p);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    CollectionAssert.AreEqual(matrix, result);
                }
            }
        }


                [Test]
                [Description("Verifies that multiplication works correctly")]
                [TestCase("0A8030B63F9F8058B0A34FC2E191C48429DB512ABA45", "17635B5A9CA68AC4C2DCE9D73C4E0DBA889646BD2AA1", 173, "AD", "11971E3E08783AAC1D08C2E518BDB1FD1E036D1F0CA4")]
                [TestCase("3DEDB8E696F3C466E5C4DC8593EB6F7A64211B4AA45", "1F6C62EA8B0A2B4BC820D41AA7F70A65A4681BF3634D", 173, "AD", "1EF915C38EAA28ACD1AD088AA287D7A0D745DCC3413")]
                [TestCase("0D7B00F897A96838F1F49AC0726F2D64E92D74A0595F", "15A20BF57A54C15C70BEE451A49E4C699A824438736F", 173, "AD", "1DDDFE1144ACD8DE10418D2CA05513E52E5AA5AC19F7")]
                [TestCase("0549AC5BC76BA711E7D67D4A250DD9F477E3047B5920", "1D8D8B062FE231629DA6025DAB0982CACC7F3AFC92DD", 173, "AD", "1FF294EF66E5E4146AF9B7CC2AE7F12F247A2DF7D75D")]
                [TestCase("01BAC18AD05FB13C30356B8C84EC6EBCA118EE3C715C", "1A81EC6F2F025E244C9E244F6AF071D48DC0A1FE6D94", 173, "AD", "1E930D08C33D7B1CD4961C49B9EE34C7C94A49FEE5BE")]
                [TestCase("10EE3CECB43B11C2DCA0C88D66F0E5044C1E1ADF8D20", "0B8499D7E4D557D72871FE351ECDBB01683B94E2AA99", 173, "AD", "11FDF452AE11FB215F53FE53326EDE753FB28380EFE2")]
                [TestCase("1563FE441FA9C04F8BE698C88CE710E53FD07FAC34AD", "003AB348CB5DA433BAE93E0612B081F7D3A198DDE8B3", 173, "AD", "1352DDF734B294D8C6168F661A67FDFF77CFF706A50")]
                [TestCase("0B2AAF3CC492524295BB537A142FAB2529AD388E267B", "02BE3B7D8DE252119F09EA0F0F3890BDAE372EBDA270", 173, "AD", "18D2CD2E93F422E7705A8DB134934697E4CD37906DA3")]
                [TestCase("01A295583135022540A7B7A0A864CCD794EAEBEB6073", "10D5D23ABD30760062F5AAF7128FC291B4BFF82D2750", 173, "AD", "9C437E8F843FE6B87E44D460E79E71A255EF8B7F788")]
                [TestCase("1C5BBEA4458C7C205BEDA727D90E68C48D3F44C8097B", "1DF9B978CD8981E8D807C4D8FAD367FA6DD0B3E9F422", 173, "AD", "131775370B5D59BEDDD652F8D3EE8ADB6582D12F4938")]
                [TestCase("0E03ADAE0802D1F81B1CF64F9B7629D06F7219E4857B", "0FDBED1D07A0C325CF83A1375A367DEDFFEAE4A07AED", 173, "AD", "7BFFBDCB44577C52326CF481182FD33938F0AAA3FEE")]
                [TestCase("0AC4FB21982B9114275BD42E89241E03C991BD8D1F28", "00CF0000A0D89D708D72B7127C06DF380EE2ABC88850", 173, "AD", "1AE277AAD71028CA496C664397EC70AA020A0B95C2CD")]
                [TestCase("3", "5", 3, "3", "4")]
                [TestCase("3", "7", 3, "3", "3")]
                [TestCase("3", "4", 3, "3", "6")]
                public void MultiplicationTest(string a, string b, int m, string sm, string expected)
                {
                    Polynomial mul1 = new Polynomial(a);
                    Polynomial mul2 = new Polynomial(b);
                    Number p = new Number(sm);
                    p = calculator.LongAdd(calculator.ShiftBitsToHigh(p, 1), one);
                    var lambda = operation.LambdaMatrix(m, p);
                    var r = operation.Multiplication(lambda, mul1.array, mul2.array, m);
                    Polynomial result = new Polynomial("1");
                    result.array = r;
                    Assert.AreEqual(expected, result.ToHexString());
                }

            


        
        [Test]
        [Description("Verifies that power works correctly")]
        [TestCase("16AEBEC919B89AFE2613286027037ABD8D183A6EC4AC", "1D7B15356D23C976F0AD2C838E07BD0BD8887FD51644", 173, "AD", "BDE6F95CE99F9D044BF9D74A1BCC785415B86D14018")]
        [TestCase("018351E69532E52B9CCB77BF10C0D3F80BAB140406EF", "04BD84187BD97CD8067BB4923E88A4D4A2DB4DCC8FDD", 173, "AD", "96A8FA6DC48892197277E583BE04C47B2889A05632B")]
        [TestCase("0A521E75D88273EDDF7BC814A4DBEB1FFD7B219F83B5", "1C382386DADC3D324E93D9005C7EC6D8D59A7CE1641E", 173, "AD", "2F996951EC9CEAB66B4B1BBDE017C91F6D00352C46F")]
        [TestCase("3", "11", 3, "3", "4")]
        [TestCase("16AECC6D8A46B5B6FBD5AB870A74E56318A3B38709CF", "010899D0F3D6A1759F8BD2781D9F2E7A36BA33AB69B2", 173, "AD", "4FD3E830C0F230E9DAB61D226733E450DF1D46D034F")]
        public void Gorner(string hex1, string hex2, int m, string sm, string expected)
        {
            Number a = new Number(hex1);
            Number b = new Number(hex2);

            Number p = new Number(sm);
            p = calculator.LongAdd(calculator.ShiftBitsToHigh(p, 1), one);

            var lambda = operation.LambdaMatrix(m, p);

            var result = operation.Gorner(a.array, b.array, m, lambda);
            Polynomial r = new Polynomial(1);
            r.array = result;
            Assert.AreEqual(expected, r.ToHexString());
        }
        
   


        
        
        [Test]
        [TestCase("3DEDB8E696F3C466E5C4DC8593EB6F7A64211B4AA45", 173, "AD", "142692BF5BD695FE303B3C455055E4E69B731DA61A46")]
        [TestCase("1D4B3D21810BB4D211A41A513844CF83B3E97C911E39", 173, "AD", "1CA42CE0C74779689DABA30BA995430262218D9D7EEB")]
        [TestCase("10C4DBBBF71C7BECFE7C9A11C4547B35DF7C89B72DA9", 173, "AD", "195FA44F625C0A814F9E8730475EAD0AC08E2E573B30")]
        [TestCase("1A0A6C28D1E9C4F51D28F25462BC7CD2CC1E0CB3628D", 173, "AD", "17156EDE84E44753893DC2A9144ABA82DF454679EFF4")]
        [TestCase("3", 3, "3", "2")]
        public void InversedElementTest(string hex1, int m, string sm, string expected)
        {
            Number a = new Number(hex1);
            
            ulong[] One = new ulong[] { 1 };
            ulong[] Two = new ulong[] { 2 };
            Number p = new Number(sm);
            p = calculator.LongAdd(calculator.ShiftBitsToHigh(p, 1), one);

            var lambda = operation.LambdaMatrix(m, p);

            var power = operation.LongSub(operation.ShiftBitsToHigh(One, m), Two);
            
            var inverse = operation.Gorner(a.array, power, m, lambda);

            Polynomial result = new Polynomial(1);
            result.array = inverse;
            Assert.AreEqual(expected, result.ToHexString());

        }
        
    }
}

