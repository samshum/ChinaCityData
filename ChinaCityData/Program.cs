using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChinaCityData
{
    internal static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            initnal();

            bool isShowMenu = true;
            while (isShowMenu)
            {
                Console.WriteLine("根据序号选择操作菜单： (注意：第一次运行程序需按序号小到大执行!)");
                Console.WriteLine("1. 获取全国各个省的数据 ");
                Console.WriteLine("2. 获取省各个市的数据 ");
                Console.WriteLine("3. 获取省各个市的GEO定位数据 ");
                Console.WriteLine("0. 退出应用 ");
                String getOption = Console.ReadLine();

                switch (getOption)
                {
                    case "1":
                        Console.WriteLine("开始获取全国各个省的数据.........");
                        getProvincesData();
                        Console.WriteLine("全国各个省的数据，获取完成！");
                        break;
                    case "2":
                        Console.WriteLine("开始获取省各个市的数据.........");
                        getCitysData();
                        Console.WriteLine("省各个市的数据，获取完成！");
                        break;
                    case "3":
                        Console.WriteLine("开始获取省各个市的GEO数据.........");
                        getCitysPoint();
                        Console.WriteLine("省各个市的GEO数据，获取完成！");
                        break;
                    case "0":
                        isShowMenu = false;
                        break;
                }
            }
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        private static void initnal() {
            string[] dirs = { "./data/provinces/", "./data/citys/", "./data/geos/" };
            IO.CheckDir(dirs);
        }

        private static List<BaseModel> _provinces;
        /// <summary>
        /// 获取各个省级数据集
        /// </summary>
        private static List<BaseModel> provinceList
        {
            get {
                if (_provinces == null) {
                    string provinceData = IO.Read("./data/province_index.json");
                    _provinces = JsonConvert.DeserializeObject<List<BaseModel>>(provinceData);
                }
                return _provinces;
            }
        }

        /// <summary>
        /// 获取各个省数据
        /// </summary>
        private static void getProvincesData() {

            provinceList.ForEach(p =>
            {
                new Task(() =>
                {
                    string cityData = Http.Post("http://xzqh.mca.gov.cn/selectJson", "shengji=" + p.shengji, "http://xzqh.mca.gov.cn/map");
                    cityData = cityData.Replace("\"shengji\":\"\"", "\"shengji\":\"" + p.shengji + "\"");
                    IO.Write(cityData, "./data/provinces/" + p.quHuaDaiMa + ".json");
                    Console.WriteLine(p.shengji + ", 数据执行完成！");
                }).Start();
                Thread.Sleep(100);
            });
        }

        /// <summary>
        /// 获取省的各个市的数据
        /// </summary>
        private static void getCitysData()
        {
            List<BaseModel> getAllCitys = new List<BaseModel>();
            provinceList.ForEach(p =>
            {
                string citysData = IO.Read("./data/provinces/" + p.quHuaDaiMa + ".json");
                List<BaseModel> currentCity = JsonConvert.DeserializeObject<List<BaseModel>>(citysData);
                currentCity.ForEach(c =>
                {
                    c.shengji = p.shengji;
                    getAllCitys.Add(c);
                });
            });

            //Console.WriteLine(JsonConvert.SerializeObject(getAllCitys));
            //shengji":"北京市（京）","diji":"北京市"
            //string cityData = Http.Post("http://xzqh.mca.gov.cn/selectJson", "shengji=北京市（京）&diji=北京市", "http://xzqh.mca.gov.cn/map");
            //Console.WriteLine(cityData);

            getAllCitys.ForEach(c =>
            {
                new Task(() =>
                {
                    string cityData = Http.Post("http://xzqh.mca.gov.cn/selectJson", "shengji=" + c.shengji + "&diji=" + c.diji, "http://xzqh.mca.gov.cn/map");
                    cityData = cityData.Replace("\"shengji\":\"\"", "\"shengji\":\"" + c.shengji + "\"");
                    cityData = cityData.Replace("\"diji\":\"\"", "\"diji\":\"" + c.diji + "\"");
                    // 处理如果当前的编码为空，需要下属区的编码加工处理
                    if (c.quHuaDaiMa.Length == 0)
                    {
                        List<BaseModel> currentCity = JsonConvert.DeserializeObject<List<BaseModel>>(cityData);
                        if (currentCity != null && currentCity.Count > 0)
                        {
                            string getPer5 = currentCity[0].quHuaDaiMa.Substring(0, 5) + "0";
                            cityData = cityData.Replace("\"quHuaDaiMa\":\"\"", "\"quHuaDaiMa\":\"" + getPer5 + "\"");
                            IO.Write(cityData, "./data/citys/" + getPer5 + ".json");
                        }
                    }
                    else
                    {
                        IO.Write(cityData, "./data/citys/" + c.quHuaDaiMa + ".json");
                    }
                    Console.WriteLine(c.shengji + c.diji + ", 数据执行完成！");
                }).Start();
                Thread.Sleep(100);
            });
            IO.Write(JsonConvert.SerializeObject(getAllCitys), "./data/citys_index.json");
        }


        /// <summary>
        /// 获取城市的GEO定位数据
        /// </summary>
        private static void getCitysPoint()
        {
            //http://xzqh.mca.gov.cn/data/440600_Point.geojson
            string citysData = IO.Read("./data/citys_index.json");
            List<BaseModel> citys = JsonConvert.DeserializeObject<List<BaseModel>>(citysData);
            citys.ForEach(c => {
                //Console.WriteLine(city.diji + " / " + city.quHuaDaiMa);
                if (c.quHuaDaiMa.Length > 0 && c.quhao.Length > 0)
                {
                    new Task(() =>
                    {
                        string uri = "http://xzqh.mca.gov.cn/data/" + c.quHuaDaiMa + "_Point.geojson";
                        string geoResult = Http.Get(uri);
                        if(geoResult != null && geoResult.Length > 0)
                        IO.Write(geoResult, "./data/geos/" + c.quHuaDaiMa + ".json");
                        Console.WriteLine(c.shengji + c.diji + ", GEO 数据执行完成！");
                    }).Start();
                }
            });
        }

    }
}
