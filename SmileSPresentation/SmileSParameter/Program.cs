using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SmileParameter.SMSService;

namespace SmileParameter
{
    internal class Program
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="args">
        /// Arguments ( ScheduleID PlanTemplateId IsStampDCR)
        /// Schedule ID (0 = Create New / id (int))
        /// Plan Template ID
        /// Is Stamp DCR 0/1
        /// </param>
        private static void Main(string[] args)
        {
            int scheduleId;
            int planTemplateId;
            int IsStampDCR;
            int currentParameterId;
            string currentReferenceId;

            try
            {
                DateTime start;
                DateTime stop;

                //set default arg
                if (args.Length == 0)
                {
                    //get schedule
                    Console.WriteLine("Please enter schedule id (enter 0 for create new)");
                    scheduleId = Convert.ToInt32(Console.ReadLine());
                    //get template id
                    Console.WriteLine("Please enter template id");
                    planTemplateId = Convert.ToInt32(Console.ReadLine());
                    //Get IsStamp DCR
                    Console.WriteLine("Please Enter IsStampDCR (0/1)");
                    IsStampDCR = Convert.ToInt32(Console.ReadLine());
                }
                else
                {
                    //Get Schedule Id
                    scheduleId = Convert.ToInt32(args[0]);
                    //Get Plan Template Id
                    planTemplateId = Convert.ToInt32(args[1]);
                    //Get Is StampDCR
                    IsStampDCR = Convert.ToInt32(args[2]);
                }

                //Create Schedule If ID = 0
                if (scheduleId == 0)
                {
                    scheduleId = new Program().CreateNewSchedule(IsStampDCR);
                }

                //Get Database Object
                SmileParameterEntities db = new SmileParameterEntities();

                //Set Time
                start = DateTime.Now;

                //Clear Old Value
                db.usp_ClearParameterSchedule(Convert.ToInt32(scheduleId), Convert.ToInt32(planTemplateId));

                var lstParam = db.usp_GetSchedulePlan(Convert.ToInt32(planTemplateId)).ToList();

                var paramLoopCount = 0;

                var paramCount = lstParam.Count;

                foreach (var param in lstParam)
                {
                    paramLoopCount += 1;

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    //var lstClass = db.usp_GetStgRefId(param.ClassId).ToList();

                    //var lstClassCount = lstClass.Count;

                    //var classloopcount = 0;

                    //foreach (var c in lstClass)
                    //{
                    //    classloopcount += 1;
                    //    currentParameterId = param.ParameterId.Value;
                    //    currentReferenceId = c;
                    //    Console.Write("\r");
                    //    Console.Write("\rParam: {0} ({1} of {2}) | Object ({3} of {4})          "
                    //        , currentParameterId
                    //        , paramLoopCount.ToString()
                    //        , paramCount.ToString()
                    //        , classloopcount.ToString()
                    //        , lstClassCount.ToString());
                    //    db.sp_ExecCalculateSP(param.ShortName, scheduleId.ToString(), c);
                    //}

                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    ////01/////

                    List<usp_GetStgRefId_V2_Result> lstRefId = new List<usp_GetStgRefId_V2_Result>();

                    lstRefId = db.usp_GetStgRefId_V2(param.ClassId).ToList();

                    List<string> lst01 = new List<string>();
                    List<string> lst02 = new List<string>();
                    List<string> lst03 = new List<string>();
                    List<string> lst04 = new List<string>();
                    List<string> lst05 = new List<string>();
                    List<string> lst06 = new List<string>();
                    List<string> lst07 = new List<string>();
                    List<string> lst08 = new List<string>();
                    List<string> lst09 = new List<string>();
                    List<string> lst10 = new List<string>();

                    //List<string> lst11 = new List<string>();
                    //List<string> lst12 = new List<string>();
                    //List<string> lst13 = new List<string>();
                    //List<string> lst14 = new List<string>();
                    //List<string> lst15 = new List<string>();
                    //List<string> lst16 = new List<string>();
                    //List<string> lst17 = new List<string>();
                    //List<string> lst18 = new List<string>();
                    //List<string> lst19 = new List<string>();
                    //List<string> lst20 = new List<string>();

                    var xi = (lstRefId.Count / 10) + 1;

                    var i01 = (xi * 0) + 1;
                    var i02 = (xi * 1) + 1;
                    var i03 = (xi * 2) + 1;
                    var i04 = (xi * 3) + 1;
                    var i05 = (xi * 4) + 1;
                    var i06 = (xi * 5) + 1;
                    var i07 = (xi * 6) + 1;
                    var i08 = (xi * 7) + 1;
                    var i09 = (xi * 8) + 1;
                    var i10 = (xi * 9) + 1;
                    var i11 = (xi * 10) + 1;

                    //var i12 = (xi * 11) + 1;
                    //var i13 = (xi * 12) + 1;
                    //var i14 = (xi * 13) + 1;
                    //var i15 = (xi * 14) + 1;
                    //var i16 = (xi * 15) + 1;
                    //var i17 = (xi * 16) + 1;
                    //var i18 = (xi * 17) + 1;
                    //var i19 = (xi * 18) + 1;
                    //var i20 = (xi * 19) + 1;
                    //var i21 = (xi * 20) + 1;

                    lst01 = lstRefId.Where(x => x.Id >= i01 && x.Id < i02).Select(a => a.RefId).ToList();
                    lst02 = lstRefId.Where(x => x.Id >= i02 && x.Id < i03).Select(a => a.RefId).ToList();
                    lst03 = lstRefId.Where(x => x.Id >= i03 && x.Id < i04).Select(a => a.RefId).ToList();
                    lst04 = lstRefId.Where(x => x.Id >= i04 && x.Id < i05).Select(a => a.RefId).ToList();
                    lst05 = lstRefId.Where(x => x.Id >= i05 && x.Id < i06).Select(a => a.RefId).ToList();
                    lst06 = lstRefId.Where(x => x.Id >= i06 && x.Id < i07).Select(a => a.RefId).ToList();
                    lst07 = lstRefId.Where(x => x.Id >= i07 && x.Id < i08).Select(a => a.RefId).ToList();
                    lst08 = lstRefId.Where(x => x.Id >= i08 && x.Id < i09).Select(a => a.RefId).ToList();
                    lst09 = lstRefId.Where(x => x.Id >= i09 && x.Id < i10).Select(a => a.RefId).ToList();
                    lst10 = lstRefId.Where(x => x.Id >= i10 && x.Id < i11).Select(a => a.RefId).ToList();

                    //lst11 = lstRefId.Where(x => x.Id >= i11 && x.Id < i12).Select(a => a.RefId).ToList();
                    //lst12 = lstRefId.Where(x => x.Id >= i12 && x.Id < i13).Select(a => a.RefId).ToList();
                    //lst13 = lstRefId.Where(x => x.Id >= i13 && x.Id < i14).Select(a => a.RefId).ToList();
                    //lst14 = lstRefId.Where(x => x.Id >= i14 && x.Id < i15).Select(a => a.RefId).ToList();
                    //lst15 = lstRefId.Where(x => x.Id >= i15 && x.Id < i16).Select(a => a.RefId).ToList();
                    //lst16 = lstRefId.Where(x => x.Id >= i16 && x.Id < i17).Select(a => a.RefId).ToList();
                    //lst17 = lstRefId.Where(x => x.Id >= i17 && x.Id < i18).Select(a => a.RefId).ToList();
                    //lst18 = lstRefId.Where(x => x.Id >= i18 && x.Id < i19).Select(a => a.RefId).ToList();
                    //lst19 = lstRefId.Where(x => x.Id >= i19 && x.Id < i20).Select(a => a.RefId).ToList();
                    //lst20 = lstRefId.Where(x => x.Id >= i20 && x.Id < i21).Select(a => a.RefId).ToList();

                    ThreadWork xxxx = new ThreadWork();

                    Thread p01 = new Thread(() => xxxx.Tx1(1, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst01.ToList()));
                    Thread p02 = new Thread(() => xxxx.Tx1(2, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst02.ToList()));
                    Thread p03 = new Thread(() => xxxx.Tx1(3, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst03.ToList()));
                    Thread p04 = new Thread(() => xxxx.Tx1(4, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst04.ToList()));
                    Thread p05 = new Thread(() => xxxx.Tx1(5, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst05.ToList()));
                    Thread p06 = new Thread(() => xxxx.Tx1(6, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst06.ToList()));
                    Thread p07 = new Thread(() => xxxx.Tx1(7, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst07.ToList()));
                    Thread p08 = new Thread(() => xxxx.Tx1(8, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst08.ToList()));
                    Thread p09 = new Thread(() => xxxx.Tx1(9, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst09.ToList()));
                    Thread p10 = new Thread(() => xxxx.Tx1(10, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst10.ToList()));

                    //Thread p11 = new Thread(() => xxxx.Tx1(11, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst11.ToList()));
                    //Thread p12 = new Thread(() => xxxx.Tx1(12, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst12.ToList()));
                    //Thread p13 = new Thread(() => xxxx.Tx1(13, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst13.ToList()));
                    //Thread p14 = new Thread(() => xxxx.Tx1(14, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst14.ToList()));
                    //Thread p15 = new Thread(() => xxxx.Tx1(15, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst15.ToList()));
                    //Thread p16 = new Thread(() => xxxx.Tx1(16, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst16.ToList()));
                    //Thread p17 = new Thread(() => xxxx.Tx1(17, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst17.ToList()));
                    //Thread p18 = new Thread(() => xxxx.Tx1(18, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst18.ToList()));
                    //Thread p19 = new Thread(() => xxxx.Tx1(19, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst19.ToList()));
                    //Thread p20 = new Thread(() => xxxx.Tx1(20, scheduleId, param.ShortName, param.ParameterId.Value, paramLoopCount, paramCount, lst20.ToList()));

                    //if (lst01.Count() > 0) { p01.Start(); }
                    //if (lst02.Count() > 0) { p02.Start(); }
                    //if (lst03.Count() > 0) { p03.Start(); }
                    //if (lst04.Count() > 0) { p04.Start(); }
                    //if (lst05.Count() > 0) { p05.Start(); }
                    //if (lst06.Count() > 0) { p06.Start(); }
                    //if (lst07.Count() > 0) { p07.Start(); }
                    //if (lst08.Count() > 0) { p08.Start(); }
                    //if (lst09.Count() > 0) { p09.Start(); }
                    //if (lst10.Count() > 0) { p10.Start(); }

                    ////var x1  = lst01.Count();
                    ////var x2  = lst02.Count();
                    ////var x3  = lst03.Count();
                    ////var x4  = lst04.Count();
                    ////var x5  = lst05.Count();
                    ////var x6  = lst06.Count();
                    ////var x7  = lst07.Count();
                    ////var x8  = lst08.Count();
                    ////var x9  = lst09.Count();
                    ////var x10 = lst10.Count();
                    ////var x11 = lst11.Count();
                    ////var x12 = lst12.Count();
                    ////var x13 = lst13.Count();
                    ////var x14 = lst14.Count();
                    ////var x15 = lst15.Count();
                    ////var x16 = lst16.Count();
                    ////var x17 = lst17.Count();
                    ////var x18 = lst18.Count();
                    ////var x19 = lst19.Count();
                    ////var x20 = lst20.Count();

                    p01.Start();
                    p02.Start();
                    p03.Start();
                    p04.Start();
                    p05.Start();
                    p06.Start();
                    p07.Start();
                    p08.Start();
                    p09.Start();
                    p10.Start();

                    //p11.Start();
                    //p12.Start();
                    //p13.Start();
                    //p14.Start();
                    //p15.Start();
                    //p16.Start();
                    //p17.Start();
                    //p18.Start();
                    //p19.Start();
                    //p20.Start();

                    p01.Join();
                    p02.Join();
                    p03.Join();
                    p04.Join();
                    p05.Join();
                    p06.Join();
                    p07.Join();
                    p08.Join();
                    p09.Join();
                    p10.Join();

                    //p11.Join();
                    //p12.Join();
                    //p13.Join();
                    //p14.Join();
                    //p15.Join();
                    //p16.Join();
                    //p17.Join();
                    //p18.Join();
                    //p19.Join();
                    //p20.Join();
                }

                stop = DateTime.Now;
                db.usp_Schedule_UpdateTime(scheduleId, start, stop);
                Console.WriteLine("");
                Console.WriteLine("Start : {0}   Stop : {1}", start.ToString(), stop.ToString());
                Console.ReadLine();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Create new schedule
        /// </summary>
        /// <returns></returns>
        public int CreateNewSchedule(int IsStampDCR)
        {
            int result;
            SmileParameterEntities db = new SmileParameterEntities();
            result = db.usp_Schedule_Create(IsStampDCR).FirstOrDefault().ID;

            return result;
        }
    }
}