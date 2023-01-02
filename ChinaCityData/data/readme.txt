./data/province_index.json  JSON文件URL：
	http://xzqh.mca.gov.cn/map 中的源文件json数据
	
./data/corecity_geo.json JSON文件URL：
	http://xzqh.mca.gov.cn/data/quanguo_Point.geojson


数据集归属关系：

省列表(province_index.json) -> 市数据(provinces) -> 区县数据(City)
                                   |                    |
                                   V                    V
                                  GEO                  GEO
                                Magnetic             Magnetic


                                     