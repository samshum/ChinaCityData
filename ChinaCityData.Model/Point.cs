using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ChinaCityData.Model
{
    /// <summary>
    /// 区域数据模型
    /// </summary>
    [Serializable]
    public class Point
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; } 

        /// <summary>
        /// GEO定位信息
        /// </summary>
        public double[] Geo { get; set; }

        /// <summary>
        /// 地磁偏角值 
        /// </summary>
        public double Declination { get; set; }

        /// <summary>
        /// 下属节点
        /// </summary>
        public List<Point> Childens { get;set; }
    }
}
