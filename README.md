
# Yixin.Atom
基于树莓派的微型气象站设计与开发
Weather Station system based on Raspbery Pi

硬件部分：
树莓派2或3一块，8G以上内存卡一张，读卡器（或带sd卡接口的电脑）一个；
Raspberry Pi 2 or 3，8G SD card, card reader;
Dht11温湿度传感器一个，BMP180气压计一个；
DHT11 Temperature and humidity sensor, BMP180 air-gauge;
土壤湿度计一个，MQ-2烟雾气敏传感器一个，雨滴传感器一个；
tensiometer, smoke gas sensor, raindrop sensor;
公母杜邦线若干，面包板一个，micro USB线一根，电源一个；
Dupont Line , Breadborad, Micro USB, Power.;
HDMI接口显示器一台（或HDMI转其它接口）。
Monitor with HDMI.

开发部分(Developing)：
集成开发环境(IDE)： Visual Studio 2017（建议使用社区版）;
开发语言(Language)：C#;
开发平台(platform)：UWP；
通信协议(Communications Protocol)：MQTT；
树莓派操作系统(Operating system)：WIndows 10 IoT Core；

注意：本系统的SDK版本和操作系统都是15063，需要在15063以下环境使用请自行修改解决方案文件。
Notice: SDK and OS version both are 15063, if you want to run it on lower version, please modify solution config.
