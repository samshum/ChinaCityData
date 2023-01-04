./data/province_index.json  JSON文件URL：
	http://xzqh.mca.gov.cn/map 中的源文件json数据
	
./data/corecity_geo.json JSON文件URL：
	http://xzqh.mca.gov.cn/data/quanguo_Point.geojson


一、数据集归属关系：

    省列表(province_index.json) -> 市数据(provinces) -> 区县数据(City)
                                       |                    |
                                       V                    V
                                      GEO                  GEO
                                    Magnetic             Magnetic

二、geos & magnetics 包括省440000和市440100的数据
       