using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmileParameter
{
    internal class ThreadWork
    {
        public void Tx1(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            var a = "";

            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                if (classloopcount == 1 || classloopcount == xx)
                {
                    if (classloopcount == 1)
                    {
                        a = "Start";
                    };

                    if (classloopcount == xx)
                    {
                        a = "End";
                    };

                    Console.Write("\r");
                    Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})   {6}       "
                        , currentParameterId
                        , paramLoopCount.ToString()
                        , paramCount.ToString()
                        , threadId.ToString()
                        , classloopcount.ToString()
                        , xx.ToString()
                        , a);
                }

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx2(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx3(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx4(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx5(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx6(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx7(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx8(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx9(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx10(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx11(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx12(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx13(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx14(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx15(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx16(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx17(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx18(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx19(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }

        public void Tx20(int threadId, int scheduleId, string ShortName, int currentParameterId, int paramLoopCount, int paramCount, List<string> lst)
        {
            SmileParameterEntities db = new SmileParameterEntities();

            var classloopcount = 0;
            var xx = lst.Count;
            foreach (var c in lst)
            {
                classloopcount += 1;
                //  currentParameterId = param.ParameterId.Value;
                // var  currentReferenceId = c;

                Console.Write("\r");

                Console.WriteLine("\rParam: {0} ({1} of {2}) |Thread {3}    Object ({4} of {5})          "
                    , currentParameterId
                    , paramLoopCount.ToString()
                    , paramCount.ToString()
                    , threadId.ToString()
                    , classloopcount.ToString()
                    , xx.ToString());

                //Thread.Sleep(1000);

                db.sp_ExecCalculateSP(ShortName, scheduleId.ToString(), c);
            }
        }
    }
}