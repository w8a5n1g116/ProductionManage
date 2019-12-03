using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPLib.Model;

namespace SAPLib
{
    public class SAPReader
    {
        DataFromSapEntities db = new DataFromSapEntities();

        public double GetProductWeightCount(string factoryCode,DateTime yearMonth)
        {
            double weightCount = 0;

            var weight1 = db.Database.SqlQuery<double?>("SELECT SUM(CAST(mm.MENGE as FLOAT)*CAST(mm.NTGEW as FLOAT)) as a FROM [dbo].[T_SAP_PP_MaterialMove] as mm ,MBEW as mb where WERKS = '"+ factoryCode + "' and mm.MATNR = mb.MATNR and mb.ADD1 = 'Z140' and (BWART = 'Z01' or BWART = '101') and Year(mm.BUDAT_MKPF) = "+yearMonth.Year+" and MONTH(mm.BUDAT_MKPF) = "+yearMonth.Month+";");

            var weight2 = db.Database.SqlQuery<double?>("SELECT SUM(CAST(mm.MENGE as FLOAT)*CAST(mm.NTGEW as FLOAT)) as a FROM [dbo].[T_SAP_PP_MaterialMove] as mm ,MBEW as mb where WERKS = '" + factoryCode + "' and mm.MATNR = mb.MATNR and mb.ADD1 = 'Z140' and (BWART = 'Z02' or BWART = '102') and Year(mm.BUDAT_MKPF) = " + yearMonth.Year + " and MONTH(mm.BUDAT_MKPF) = " + yearMonth.Month + ";");


            double w1 = weight1.FirstOrDefault() != null ? (double)weight1.FirstOrDefault() : 0;
            double w2 = weight2.FirstOrDefault() != null ? (double)weight2.FirstOrDefault() : 0;
            weightCount = w1 - w2;

            if (weightCount != 0)
                return weightCount / 1000;
            else
                return 0;
            
        }

        public double GetProductHourCount(string factoryCode, DateTime yearMonth)
        {
            //double weightCount = 0;

            //if(!string.IsNullOrEmpty(factoryCode))
            //{
            //    var weight1 = db.Database.SqlQuery<double?>("select "
            //    + "SUM(case when d.STZHL = '00000001' then - 1 * cast(VGW01 as float) else cast(VGW01 as float) end) 指标值 "
            //    + "from "
            //    + "[DataFromSap].[dbo].T_SAP_PP_ProductOrderRote as a "
            //    + "inner join[DataFromSap].[dbo].MBEW as b on a.MATNR = b.MATNR and a.WERKS = b.BWKEY "
            //    + "inner join[DataFromSap].[dbo].T_SAP_PP_ProductOrderHeader as c on a.AUFNR = c.AUFNR "
            //    + "inner join[DataFromSap].[dbo].[T_SAP_PP_QualityBI] as d  on a.AUFNR = d.AUFNR "
            //    + "where "
            //    + "b.ADD1 = 'Z140' "
            //    + "and(c.ADD3 like  '%CLSD%' or  c.ADD3 like  '%CNF%' or  c.ADD3 like  '%DLV%' or c.ADD3 like  '%PCNF%'  or c.ADD3 like '%TECO%') "
            //    + "and a.WERKS = " + factoryCode + " "
            //    + "and a.VORNR = d.VORNR "
            //    + "and YEAR(d.BUDAT) = " + yearMonth.Year + " "
            //    + "and MONTH(d.BUDAT) = " + yearMonth.Month);

            //    if (weight1.FirstOrDefault() != null)
            //    {
            //        weightCount = (double)weight1.FirstOrDefault();

            //        return weightCount;
            //    }
            //    else
            //    {
            //        return 0;
            //    }
            //}else
            //{
            //    return 0;
            //}

            switch (factoryCode)
            {
                case "1300":
                    return 5721;
                case "1410":
                    return 4872;
                case "1420":
                    return 7200;
                case "1430":
                    return 0;
                case "2000":
                    return 500;
                default:
                    return 0;
            }

        }
    }
}
