using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
                Console.WriteLine("1. 获取全国各个省的数据");
                Console.WriteLine("2. 获取省各个市的数据");
                Console.WriteLine("3. 获取省和各个市GEO定位数据");
                Console.WriteLine("4. 获取市、县的Magnetics数据");
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
                        Console.WriteLine("市数据，获取完成！");
                        break;
                    case "3":
                        Console.WriteLine("开始获取省各个市的GEO数据.........");
                        getCitysPoint();
                        Console.WriteLine("市GEO数据，获取完成！");
                        break;
                    case "4":
                        Console.WriteLine("开始获取各个市、县的Magnetics数据.........");
                        getCitysMagnetic();
                        Console.WriteLine("市、县Magnetics数据，获取完成！");
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
            string[] dirs = { "./data/provinces/", "./data/citys/", "./data/geos/", "./data/magnetics/" };
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
        /// 1 获取各个省数据
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
        /// 2 获取省的各个市的数据
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
        /// 3 获取省和城市的GEO定位数据
        /// </summary>
        private static void getCitysPoint()
        {
            //http://xzqh.mca.gov.cn/data/440600_Point.geojson
            string citysData = IO.Read("./data/citys_index.json");
            List<BaseModel> citys = JsonConvert.DeserializeObject<List<BaseModel>>(citysData);

            string[] getDirFiles = IO.DirFiles("./data/provinces", true);
            foreach (string pid in getDirFiles) {
                citys.Add(new BaseModel() { quHuaDaiMa = pid });
            }

            citys.ForEach(c => {
                //Console.WriteLine(city.diji + " / " + city.quHuaDaiMa);
                if (c.quHuaDaiMa.Length > 0)
                {
                    new Task(() =>
                    {
                        string uri = "http://xzqh.mca.gov.cn/data/" + c.quHuaDaiMa + "_Point.geojson";
                        string geoResult = Http.Get(uri);
                        IO.Write(geoResult, "./data/geos/" + c.quHuaDaiMa + ".json");
                        Console.WriteLine(c.shengji + c.diji + ", GEO 数据执行完成！");
                    }).Start();
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// 4 获取市、县的Magnetics数据
        /// </summary>
        private static void getCitysMagnetic() {
            string[] geoFiles = IO.DirFiles("./data/geos/", true);
            List<Magnetic> locals = new List<Magnetic>();
            foreach (string geo in geoFiles)
            {
                string magneticData = IO.Read("./data/geos/" + geo + ".json");
                if (magneticData != null)
                {
                    Magnetic magnetic = JsonConvert.DeserializeObject<Magnetic>(magneticData);
                    if (magnetic.features.Length >= 1 && magnetic.features[0].geometry.coordinates.Length == 2)
                        locals.Add(magnetic);
                }

            }

            // 市、县Magnetics数据
            locals.ForEach(l =>
            {
                if(l.features.Length > 0) {
                    foreach(Feature f in l.features)
                    {
                        new Task(() =>
                        {
                            string dataNow = DateTime.Now.ToString("yyyy-M-d");
                            string year = dataNow.Split('-')[0];
                            string month = dataNow.Split('-')[1];
                            string day = dataNow.Split('-')[2];

                            string dqCode = f.properties.QUHUADAIMA;
                            double[] geo = f.geometry.coordinates;

                            string uri = "https://www.ngdc.noaa.gov/geomag-web/calculators/calculateIgrfwmm?browserRequest=true&key=EAU2y&lat1=" + geo[1] + "&lat1Hemisphere=N&lon1=" + geo[0] + "&lon1Hemisphere=E&coordinateSystem=M&elevation=0&elevationUnits=K&model=WMM&startYear="+ year + "&startMonth="+ month + "&startDay="+ day + "&endYear="+ year + "&endMonth="+ month + "&endDay="+ day + "&dateStepSize=1.0&resultFormat=json";
                            string refererUri = "https://www.ngdc.noaa.gov/geomag/calculators/magcalc.shtml";

                            string geoResult = Http.Get(uri, refererUri);
                            if (geoResult != null && geoResult.Length > 0)
                                IO.Write(geoResult, "./data/magnetics/"+ dqCode + ".json");

                            Console.WriteLine(dqCode + ", 市、县的Magnetics数据执行完成！");

                        }).Start();
                        Thread.Sleep(100);
                    }
                }
            });
        }

    }
}
