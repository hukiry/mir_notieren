using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEditor.Android;
using UnityEngine;

//全面解决无法选择指定Gradle Version的问题,指定 导出的安卓工程 Gradle Version ; 
//by unity2020.3.33f1； 放Unity工程目录Editor文件夹下
#if UNITY_2022_1_OR_NEWER
public class GradleAndroidProject : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get; } = 0;
    public void OnPostGenerateGradleAndroidProject(string basePath)
    {
        //单纯导出aab或apk 输出的AS工程输出的 原路径 Temp\gradleOut\unityLibrary
        string[] paths = basePath.Split('\\');
        string rootDirPath = basePath.Replace(paths[paths.Length - 1], "");

#region gradle-wrapper.properties
        string wrapperPath = rootDirPath + "gradle/wrapper";
        Debug.Log("wrapperPath:" + wrapperPath);
        if (!Directory.Exists(wrapperPath)) Directory.CreateDirectory(wrapperPath);

        string gradleWrapperProperties = wrapperPath + "/gradle-wrapper.properties";
        if (File.Exists(gradleWrapperProperties)) File.Delete(gradleWrapperProperties);
        List<string> lins = new List<string>();
        lins.Add("#Tue Jun 25 18:26:02 CST 2024");
        lins.Add("distributionBase=GRADLE_USER_HOME");
        lins.Add("distributionUrl=https\\://services.gradle.org/distributions/gradle-6.7.1-bin.zip");
        lins.Add("distributionPath=wrapper/dists");
        lins.Add("zipStorePath=wrapper/dists");
        lins.Add("zipStoreBase=GRADLE_USER_HOME");
        File.WriteAllLines(gradleWrapperProperties, lins);
#endregion

#region gradle.properties
        string propertiesPath = rootDirPath + "/gradle.properties";
        if (File.Exists(propertiesPath)) File.Delete(propertiesPath);
        List<string> propertiesLines = new List<string>();
        propertiesLines.Add("#gradle.properties");
        propertiesLines.Add("org.gradle.jvmargs=-Xmx4096M");
        propertiesLines.Add("org.gradle.parallel=true");
        propertiesLines.Add("unityStreamingAssets =.unity3d, UnityServicesProjectConfiguration.json, ota / Android, .manifest, ota / challengemap, ota / halloween_2018, ota / house_1, ota / house_2, ota / house_common, ota / localize_ch, ota / localize_chs, ota / localize_common, ota / localize_de, ota / localize_du, ota / localize_en, ota / localize_es, ota / localize_fr, ota / localize_in, ota / localize_it, ota / localize_jp, ota / localize_kr, ota / localize_pt, ota / localize_ru, ota / localize_th, ota / localize_tr, ota / music, ota / room1, ota / room10, ota / room10001, ota / room10001_candles_1, ota / room10001_candles_2, ota / room10001_candles_3, ota / room10001_holiday_decor_1_1, ota / room10001_holiday_decor_1_2, ota / room10001_holiday_decor_1_3, ota / room10001_holiday_decor_2_1, ota / room10001_holiday_decor_2_2, ota / room10001_holiday_decor_2_3, ota / room10001_party_food_1_1, ota / room10001_party_food_1_2, ota / room10001_party_food_1_3, ota / room10001_party_food_2_1, ota / room10001_party_food_2_2, ota / room10001_party_food_2_3, ota / room10001_party_table_2_1, ota / room10001_party_table_2_2, ota / room10001_party_table_2_3, ota / room10001_spooky_lamp_1_1, ota / room10001_spooky_lamp_1_2, ota / room10001_spooky_lamp_1_3, ota / room10001_spooky_lamp_2_1, ota / room10001_spooky_lamp_2_2, ota / room10001_spooky_lamp_2_3, ota / room10001_stage_decor_1_1, ota / room10001_stage_decor_1_2, ota / room10001_stage_decor_1_3, ota / room10001_stage_decor_2_1, ota / room10001_stage_decor_2_2, ota / room10001_stage_decor_2_3, ota / room10001_stage_decor_3_1, ota / room10001_stage_decor_3_2, ota / room10001_stage_decor_3_3, ota / room10001_stage_decor_4_1, ota / room10001_stage_decor_4_2, ota / room10001_stage_decor_4_3, ota / room10001_stage_decor_5_1, ota / room10001_stage_decor_5_2, ota / room10001_stage_decor_5_3, ota / room101, ota / room101_counter_chair_1, ota / room101_counter_chair_2, ota / room101_counter_chair_3, ota / room101_counter_decor_1, ota / room101_counter_decor_2, ota / room101_counter_decor_3, ota / room101_refrigerator_1, ota / room101_refrigerator_2, ota / room101_refrigerator_3, ota / room10_cabinet_4_1, ota / room10_cabinet_4_2, ota / room10_cabinet_4_3, ota / room10_cabinet_decor_1_1, ota / room10_cabinet_decor_1_2, ota / room10_cabinet_decor_1_3, ota / room10_cabinet_decor_2_1, ota / room10_cabinet_decor_2_2, ota / room10_cabinet_decor_2_3, ota / room10_cabinet_decor_3_1, ota / room10_cabinet_decor_3_2, ota / room10_cabinet_decor_3_3, ota / room10_decor_1, ota / room10_decor_2, ota / room10_decor_3, ota / room10_kitchen_table_decor_1, ota / room10_kitchen_table_decor_2, ota / room10_kitchen_table_decor_3, ota / room11, ota / room11_curtains_1, ota / room11_curtains_2, ota / room11_curtains_3, ota / room11_flowers_1, ota / room11_flowers_2, ota / room11_flowers_3, ota / room11_paintings_1, ota / room11_paintings_2, ota / room11_paintings_3, ota / room11_plants_1, ota / room11_plants_2, ota / room11_plants_3, ota / room11_table_decor_1, ota / room11_table_decor_2, ota / room11_table_decor_3, ota / room11_tile_omament_1, ota / room11_tile_omament_2, ota / room11_tile_omament_3, ota / room11_wall_decor_1, ota / room11_wall_decor_2, ota / room11_wall_decor_3, ota / room11_wall_light_1, ota / room11_wall_light_2, ota / room11_wall_light_3, ota / room2, ota / room201, ota / room201_bench_decor_1, ota / room201_bench_decor_2, ota / room201_bench_decor_3, ota / room201_bench_pad_1, ota / room201_bench_pad_2, ota / room201_bench_pad_3, ota / room201_lounge_chair_1, ota / room201_lounge_chair_2, ota / room201_lounge_chair_3, ota / room201_stool_1, ota / room201_stool_2, ota / room201_stool_3, ota / room2_bedside_lamp1_1, ota / room2_bedside_lamp1_2, ota / room2_bedside_lamp1_3, ota / room2_bedside_table1_1, ota / room2_bedside_table1_2, ota / room2_bedside_table1_3, ota / room2_bedside_table1_4, ota / room2_bedside_table_decoration1_1, ota / room2_bedside_table_decoration1_2, ota / room2_bedside_table_decoration1_3, ota / room2_bed_stool1_1, ota / room2_bed_stool1_2, ota / room2_bed_stool1_3, ota / room2_carpet1_1, ota / room2_carpet1_2, ota / room2_carpet1_3, ota / room2_curtain1_1, ota / room2_curtain1_2, ota / room2_curtain1_3, ota / room2_fitness1_1, ota / room2_fitness1_2, ota / room2_fitness1_3, ota / room2_table1_1, ota / room2_table1_2, ota / room2_table1_3, ota / room2_wall_cabinet1_1, ota / room2_wall_cabinet1_2, ota / room2_wall_cabinet1_3, ota / room2_wall_decoration1_1, ota / room2_wall_decoration1_2, ota / room2_wall_decoration1_3, ota / room3, ota / room301, ota / room301_hedges_1_1, ota / room301_hedges_1_2, ota / room301_hedges_1_3, ota / room301_hedges_2_1, ota / room301_hedges_2_2, ota / room301_hedges_2_3, ota / room301_lawn_chair_1, ota / room301_lawn_chair_2, ota / room301_lawn_chair_3, ota / room301_porch_swing_1, ota / room301_porch_swing_2, ota / room301_porch_swing_3, ota / room301_tree_1, ota / room301_tree_2, ota / room301_tree_3, ota / room3_carpet1_1, ota / room3_carpet1_2, ota / room3_carpet1_3, ota / room3_carpet1_4, ota / room3_curtain1_1, ota / room3_curtain1_2, ota / room3_curtain1_3, ota / room3_curtain1_4, ota / room3_floor_lamp1_1, ota / room3_floor_lamp1_2, ota / room3_floor_lamp1_3, ota / room3_painting1_1, ota / room3_painting1_2, ota / room3_painting1_3, ota / room3_plant1_1, ota / room3_plant1_2, ota / room3_plant1_3, ota / room3_side_table1_1, ota / room3_side_table1_2, ota / room3_side_table1_3, ota / room3_side_table_decoration1_1, ota / room3_side_table_decoration1_2, ota / room3_side_table_decoration1_3, ota / room3_side_table_decoration1_4, ota / room3_soft_stool1_1, ota / room3_soft_stool1_2, ota / room3_soft_stool1_3, ota / room3_television1_1, ota / room3_television1_2, ota / room3_television1_3, ota / room3_wall_cabinet1_1, ota / room3_wall_cabinet1_2, ota / room3_wall_cabinet1_3, ota / room3_wall_cabinet_decoration1_1, ota / room3_wall_cabinet_decoration1_2, ota / room3_wall_cabinet_decoration1_3, ota / room4, ota / room4_bottles1_1, ota / room4_bottles1_2, ota / room4_bottles1_3, ota / room4_cabinet_appliance1_1, ota / room4_cabinet_appliance1_2, ota / room4_cabinet_appliance1_3, ota / room4_chair1_1, ota / room4_chair1_2, ota / room4_chair1_3, ota / room4_cup_holder1_1, ota / room4_cup_holder1_2, ota / room4_cup_holder1_3, ota / room4_fridge1_1, ota / room4_fridge1_2, ota / room4_fridge1_3, ota / room4_range_hood1_1, ota / room4_range_hood1_2, ota / room4_range_hood1_3, ota / room4_side_closet_back1_1, ota / room4_side_closet_back1_2, ota / room4_side_closet_back1_3, ota / room4_side_closet_front1_1, ota / room4_side_closet_front1_2, ota / room4_side_closet_front1_3, ota / room4_table1_1, ota / room4_table1_2, ota / room4_table1_3, ota / room4_table_decoration1_1, ota / room4_table_decoration1_2, ota / room4_table_decoration1_3, ota / room5, ota / room5_basket1_1, ota / room5_basket1_2, ota / room5_basket1_3, ota / room5_carpet1_1, ota / room5_carpet1_2, ota / room5_carpet1_3, ota / room5_chair1_1, ota / room5_chair1_2, ota / room5_chair1_3, ota / room5_chandelier1_1, ota / room5_chandelier1_2, ota / room5_chandelier1_3, ota / room5_closet1_1, ota / room5_closet1_2, ota / room5_closet1_3, ota / room5_sundries_rack1_1, ota / room5_sundries_rack1_2, ota / room5_sundries_rack1_3, ota / room5_towel_rack1_1, ota / room5_towel_rack1_2, ota / room5_towel_rack1_3, ota / room5_washing_machine1_1, ota / room5_washing_machine1_2, ota / room5_washing_machine1_3, ota / room6, ota / room6_armchair_1, ota / room6_armchair_2, ota / room6_armchair_3, ota / room6_bed_bench_1, ota / room6_bed_bench_2, ota / room6_bed_bench_3, ota / room6_bed_linens_1, ota / room6_bed_linens_2, ota / room6_bed_linens_3, ota / room6_carpet_1, ota / room6_carpet_2, ota / room6_carpet_3, ota / room6_dresser_1, ota / room6_dresser_2, ota / room6_dresser_3, ota / room6_floor_lamp_1, ota / room6_floor_lamp_2, ota / room6_floor_lamp_3, ota / room6_plant_1, ota / room6_plant_2, ota / room6_plant_3, ota / room6_vanity_1, ota / room6_vanity_2, ota / room6_vanity_3, ota / room6_wall_decoration_1, ota / room6_wall_decoration_2, ota / room6_wall_decoration_3, ota / room7, ota / room7_end_table_decor_1, ota / room7_end_table_decor_2, ota / room7_end_table_decor_3, ota / room7_floor_lamp_1, ota / room7_floor_lamp_2, ota / room7_floor_lamp_3, ota / room7_plant_1, ota / room7_plant_2, ota / room7_plant_3, ota / room7_rug_1, ota / room7_rug_2, ota / room7_rug_3, ota / room7_rug_4, ota / room7_stool_1, ota / room7_stool_2, ota / room7_stool_3, ota / room7_wall_light_1, ota / room7_wall_light_2, ota / room7_wall_light_3, ota / room8, ota / room8_curtains_1, ota / room8_curtains_2, ota / room8_curtains_3, ota / room8_desk_supplies_1, ota / room8_desk_supplies_2, ota / room8_desk_supplies_3, ota / room8_floor_lamp_1, ota / room8_floor_lamp_2, ota / room8_floor_lamp_3, ota / room8_paintings_1, ota / room8_paintings_2, ota / room8_paintings_3, ota / room8_plant_1, ota / room8_plant_2, ota / room8_plant_3, ota / room8_small_rug_1, ota / room8_small_rug_2, ota / room8_small_rug_3, ota / room9, ota / room9_coffee_table_1, ota / room9_coffee_table_2, ota / room9_coffee_table_3, ota / room9_lounge_chairs_1, ota / room9_lounge_chairs_2, ota / room9_lounge_chairs_3, ota / room9_painting_1, ota / room9_painting_2, ota / room9_painting_3, ota / room9_plant_1, ota / room9_plant_2, ota / room9_plant_3, ota / room9_rug_1, ota / room9_rug_2, ota / room9_rug_3, ota / room9_sofa_1, ota / room9_sofa_2, ota / room9_sofa_3, ota / room9_wall_lamp_1, ota / room9_wall_lamp_2, ota / room9_wall_lamp_3, ota / rooms_common, ota / share_bg, ota / sound, ota / table");
        propertiesLines.Add("android.enableJetifier=true");
        propertiesLines.Add("# android.useAndroidX=true");
        propertiesLines.Add("android.useAndroidX=true");
        File.WriteAllLines(propertiesPath, propertiesLines);
#endregion

//        #region build.gradle
//        string gradlePath = basePath + "/build.gradle";
//        var list = File.ReadLines(gradlePath).ToList();
//        bool isOk = false;
//        for (int i = 0; i < list.Count; i++)
//        {
//            if (list[i].Trim().StartsWith("implementation(name: 'billing-5.2.1'") || list[i].Trim().StartsWith("implementation(name: 'common'"))
//            {
//                list[i] = " //" + list[i];
//                if (!isOk)
//                {
//                    isOk = true;
//                    list.Insert(i, "implementation 'androidx.fragment:fragment:1.3.6'");
//#if UNITY_2019_1_OR_NEWER
//                    list.Insert(i, "implementation 'com.android.billingclient:billing:7.1.1'");//审核要求：google内购库升级 6.0.1。
//#elif UNITY_2020_1_OR_NEWER
//                    list.Insert(i, "implementation 'com.android.billingclient:billing:6.2.1'");//审核要求：google内购库升级 6.0.1。
//#endif
//                }
//            }
//        }
//        File.WriteAllLines(gradlePath, list); 
//        #endregion

    }
}
#endif