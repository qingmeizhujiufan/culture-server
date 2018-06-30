using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class EnumData
    {
        // 树的个数拆分率
        static public float[] EnumTreeSplit = new float[]
        {
            0,
            0.0002F,
            0.0004F,
            0.0006F,
            0.0008F,
            0.001F,
            0.0012F,
            0.0014F,
            0.0016F,
            0.0018F,
            0.0020F,
            0.0022F,
            0.0024F,
            0.0026F,
            0.0028F,
            0.0030F
        };
        // 树种植果实数
        static public int[] EnumTreeTypePlantSum = new int[3]
        {
            300,
            1000,
            5000
        };
        // 树最多收益数
        static public int[] EnumTreeTypeIncomeSum = new int[3]
        {
            900,
            3000,
            15000
        };
        // 树上最多放置桃子数
        static public int[] EnumTreeTypeSum = new int[3]
        {
            2000,
            20000,
            200000
        };
    }

    //
    
}
