﻿using NUnit.Framework;
using System;
using LanguageExt;
using static LanguageExt.List;
using static LanguageExt.Prelude;

namespace LanguageExtTests
{
    [TestFixture]
    public class MemoTests
    {
        [Test]
        public void MemoTest1()
        {
            var saved = DateTime.Now;
            var date = saved;

            var f = Prelude.memo(() => date.ToString());

            var res1 = f();

            date = DateTime.Now.AddDays(1);

            var res2 = f();

            Assert.IsTrue(res1 == res2);
        }

        [Test]
        public void MemoTest2()
        {
            var fix = 0;
            var count = 100;

            Func<int, int> fn = x => x + fix;

            var m = fn.memoUnsafe();

            var nums1 = map(range(0, count), i => m(i));

            fix = 1000;

            var nums2 = map(range(0, count), i => m(i));

            Assert.IsTrue(
                length(filter(zip(nums1, nums2, (a, b) => a == b), v => v)) == count
                );
        }

        [Test]
        public void MemoTest3()
        {
            GC.Collect();

            var fix = 0;
            var count = 1000;

            Func<int, int> fn = x => x + fix;

            var m = fn.memo();

            var nums1 = freeze(map(range(0, count), i => m(i)));

            fix = 1000;

            var nums2 = freeze(map(range(0, count), i => m(i)));

            Assert.IsTrue(
                length(filter(zip(nums1, nums2, (a, b) => a == b), v => v)) == count
                );
        }

        /*      
            Uncomment this if you have time on your hands

        [Test]
        public void MemoMemoryTest()
        {
            var mbStart = GC.GetTotalMemory(false) / 1048576L;

            Func<int, string> fn = x => x.ToString();
            var m = fn.memo();

            range(0, Int32.MaxValue).Iter(i => m(i));

            var mbFinish = GC.GetTotalMemory(false) / 1048576L;

            Assert.IsTrue(mbFinish - mbStart < 30);
        }
            */
    }
}
