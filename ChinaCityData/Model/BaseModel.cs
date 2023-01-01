using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChinaCityData
{
    /// <summary>
    /// 基本模型
    /// </summary>
    internal class BaseModel
    {

        /// <summary>
        /// 区号大码: 110000
        /// </summary>
        public string quHuaDaiMa { get; set; }

        /// <summary>
        /// 区号
        /// </summary>
        public string quhao { get; set; }

        /// <summary>
        /// 省级： 北京市（京）
        /// </summary>
        public string shengji { get; set; }

        /// <summary>
        /// 地级
        /// </summary>
        public string diji { get; set; }

        /// <summary>
        /// 县级
        /// </summary>
        public string xianji { get; set; }

        /// <summary>
        /// 子集合
        /// </summary>
        public Object[] children { get; set; }

    }
}
